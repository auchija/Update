import { PGlite } from '@electric-sql/pglite';
import { PGLiteSocketServer } from '@electric-sql/pglite-socket';
import { spawn, spawnSync } from 'node:child_process';
import { randomBytes } from 'node:crypto';
import fs from 'node:fs';
import os from 'node:os';
import path from 'node:path';
import { fileURLToPath } from 'node:url';

const raiz = fileURLToPath(new URL('../../', import.meta.url)).replace(/[\\/]$/, '');
const dotnetWindows = 'C:\\Program Files\\dotnet\\dotnet.exe';
const dotnet = process.env.DOTNET_UPDATE ?? (fs.existsSync(dotnetWindows) ? dotnetWindows : 'dotnet');
const temporal = fs.mkdtempSync(path.join(os.tmpdir(), 'update-pruebas-'));
const esquema = path.join(temporal, 'esquema.sql');
const clave = randomBytes(48).toString('base64url');
const puertoBase = 55434;
const base = 'http://127.0.0.1:5090';

function ejecutar(argumentos) {
  const r = spawnSync(dotnet, argumentos, { cwd: raiz, encoding: 'utf8', env: { ...process.env, Jwt__Clave: clave } });
  if (r.status !== 0) {
    console.error(r.stdout, r.stderr);
    console.error('Si el error es de archivos bloqueados, detén el backend y vuelve a correr las pruebas.');
    process.exit(1);
  }
}

console.log('Preparando (compila el backend; debe estar detenido)...');
ejecutar(['tool', 'restore']);
ejecutar(['tool', 'run', 'dotnet-ef', 'dbcontext', 'script', '--project', 'update.Infrastructure/update.Infrastructure.csproj', '--startup-project', 'update.API/update.API.csproj', '--context', 'ApplicationDbContext', '--output', esquema]);

const sql = fs.readFileSync(esquema, 'utf8')
  .replace(/^\uFEFF/, '')
  .replace(/,?\s*CONSTRAINT ck_usuarios_tipo_perfil(_enum)? CHECK \([^\n]*?\)\)?(?=,?\r?\n)/g, '')
  .replace(/,(\s*\n\);)/g, '$1');

const db = await PGlite.create();
await db.exec(sql);
const servidor = new PGLiteSocketServer({ db, host: '127.0.0.1', port: puertoBase });
await servidor.start();

const app = spawn(dotnet, [path.join(raiz, 'update.API/bin/Debug/net10.0/update.API.dll')], {
  cwd: temporal,
  env: {
    ...process.env,
    ASPNETCORE_ENVIRONMENT: 'Development',
    ASPNETCORE_URLS: base,
    Jwt__Clave: clave,
    ConnectionStrings__DefaultConnection: `Host=127.0.0.1;Port=${puertoBase};Database=postgres;Username=postgres;SSL Mode=Disable;Pooling=false;Timeout=10;Command Timeout=20`,
  },
});
let registroApp = '';
app.stdout.on('data', (d) => { registroApp += d; });
app.stderr.on('data', (d) => { registroApp += d; });

for (let i = 0; i < 80; i++) {
  try {
    if ((await fetch(`${base}/estado`)).ok) break;
  } catch {
    await new Promise((r) => setTimeout(r, 500));
  }
}

let correctas = 0;
const fallidas = [];
const prueba = (nombre, condicion, detalle = '') => {
  if (condicion) correctas++;
  else fallidas.push(nombre);
  console.log(`${condicion ? 'OK  ' : 'FAIL'} ${nombre}${condicion ? '' : ` -> ${detalle}`}`);
};
const seccion = (titulo) => console.log(`\n== ${titulo}`);
const leer = async (r) => {
  const t = await r.text();
  try { return JSON.parse(t); } catch { return t; }
};
const enviar = (ruta, cuerpo, metodo = 'POST', cabeceras = {}) => fetch(base + ruta, {
  method: metodo,
  headers: { 'Content-Type': 'application/json', ...cabeceras },
  body: JSON.stringify(cuerpo),
});
const estado = async (promesa) => (await promesa).status;
const bearer = (t) => ({ Authorization: `Bearer ${t}` });
const registro = (handle, email, cambios = {}) => ({
  displayName: 'Persona Prueba',
  handle,
  email,
  password: 'Clave12345',
  personas: ['mentor'],
  termsAccepted: true,
  ...cambios,
});
const login = (identifier, password) => enviar('/api/auth/login', { identifier, password });
const ponerPerfil = (token, cuerpo) => enviar('/api/perfil/yo', cuerpo, 'PUT', bearer(token));
const subir = (token, bytes, nombre) => {
  const f = new FormData();
  f.append('archivo', new Blob([bytes]), nombre);
  return fetch(`${base}/api/archivos/subir`, { method: 'POST', headers: token ? bearer(token) : {}, body: f });
};
const PNG = Buffer.from('iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mNkYPhfDwAChwGA60e6kgAAAABJRU5ErkJggg==', 'base64');

try {
  seccion('POST /api/auth/register');
  let r = await enviar('/api/auth/register', registro('ana.dev', 'ana@example.com'));
  let cuerpo = await leer(r);
  const tokenAna = cuerpo.accessToken;
  const cookie = (r.headers.get('set-cookie') ?? '').split(';')[0];
  prueba('datos válidos = 201', r.status === 201, JSON.stringify(cuerpo));
  prueba('devuelve displayName y handle', cuerpo.user?.displayName === 'Persona Prueba' && cuerpo.user?.handle === 'ana.dev');
  prueba('crea el perfil', (await db.query(`select 1 from perfiles where nombre_usuario='ana.dev'`)).rows.length === 1);
  prueba('correo inválido = 400', (await estado(enviar('/api/auth/register', registro('otro.uno', 'correo')))) === 400);
  prueba('contraseña débil = 400', (await estado(enviar('/api/auth/register', registro('otro.uno', 'otro1@example.com', { password: '123' })))) === 400);
  prueba('sin aceptar términos = 400', (await estado(enviar('/api/auth/register', registro('otro.uno', 'otro1@example.com', { termsAccepted: false })))) === 400);
  prueba('correo repetido = 409', (await estado(enviar('/api/auth/register', registro('otro.uno', 'ana@example.com')))) === 409);
  prueba('usuario repetido = 409', (await estado(enviar('/api/auth/register', registro('ana.dev', 'otro1@example.com')))) === 409);

  seccion('POST /api/auth/login');
  prueba('con correo = 200', (await estado(login('ana@example.com', 'Clave12345'))) === 200);
  prueba('con @usuario = 200', (await estado(login('@ana.dev', 'Clave12345'))) === 200);
  prueba('contraseña incorrecta = 401', (await estado(login('ana.dev', 'Mala12345'))) === 401);
  prueba('usuario inexistente = 401', (await estado(login('nadie.aqui', 'Mala12345'))) === 401);
  prueba('campos vacíos = 400', (await estado(login('', ''))) === 400);
  const beto = await leer(await enviar('/api/auth/register', registro('beto.ruiz', 'beto@example.com')));
  const carla = await leer(await enviar('/api/auth/register', registro('carla.paz', 'carla@example.com')));
  const codigos = [];
  for (let i = 0; i < 5; i++) codigos.push(await estado(login('carla.paz', 'Mala12345')));
  prueba('bloquea al quinto intento = 423', codigos[4] === 423 && codigos[3] === 401, codigos.join());
  prueba('la cuenta creada para el bloqueo existe', !!carla.accessToken);

  seccion('GET /api/auth/me');
  prueba('sin token = 401', (await estado(fetch(`${base}/api/auth/me`))) === 401);
  prueba('con token = 200', (await estado(fetch(`${base}/api/auth/me`, { headers: bearer(tokenAna) }))) === 200);

  seccion('POST /api/auth/refresh y /logout');
  prueba('refresh sin cookie = 401', (await estado(fetch(`${base}/api/auth/refresh`, { method: 'POST' }))) === 401);
  r = await fetch(`${base}/api/auth/refresh`, { method: 'POST', headers: { Cookie: cookie } });
  const cookieNueva = (r.headers.get('set-cookie') ?? '').split(';')[0];
  prueba('refresh con cookie = 200', r.status === 200);
  prueba('logout = 204', (await estado(fetch(`${base}/api/auth/logout`, { method: 'POST', headers: { Cookie: cookieNueva } }))) === 204);
  prueba('refresh después de logout = 401', (await estado(fetch(`${base}/api/auth/refresh`, { method: 'POST', headers: { Cookie: cookieNueva } }))) === 401);

  seccion('GET y PUT /api/perfil/yo');
  prueba('GET sin token = 401', (await estado(fetch(`${base}/api/perfil/yo`))) === 401);
  r = await fetch(`${base}/api/perfil/yo`, { headers: bearer(tokenAna) });
  prueba('GET con token = 200', r.status === 200 && (await leer(r)).nombreUsuario === 'ana.dev');
  r = await ponerPerfil(tokenAna, { nombreUsuario: 'ana.dev', nombreVisible: 'Ana Dev', titular: 'Fundadora', biografia: 'Hola', sitioWeb: 'https://ana.dev', totalSeguidores: 999 });
  cuerpo = await leer(r);
  prueba('PUT válido = 200', r.status === 200 && cuerpo.titular === 'Fundadora', JSON.stringify(cuerpo));
  prueba('PUT no cambia contadores', cuerpo.totalSeguidores === 0);
  prueba('PUT sin token = 401', (await estado(enviar('/api/perfil/yo', {}, 'PUT'))) === 401);
  prueba('usuario inválido = 400', (await estado(ponerPerfil(tokenAna, { nombreUsuario: 'A B', nombreVisible: 'Ana Dev' }))) === 400);
  prueba('sitio javascript: = 400', (await estado(ponerPerfil(tokenAna, { nombreUsuario: 'ana.dev', nombreVisible: 'Ana Dev', sitioWeb: 'javascript:alert(1)' }))) === 400);
  prueba('usuario ocupado = 409', (await estado(ponerPerfil(tokenAna, { nombreUsuario: 'beto.ruiz', nombreVisible: 'Ana Dev' }))) === 409);

  seccion('POST /api/archivos/subir');
  prueba('sin token = 401', (await estado(subir(null, PNG, 'a.png'))) === 401);
  r = await subir(tokenAna, PNG, 'foto.png');
  const foto = await leer(r);
  prueba('imagen válida = 201', r.status === 201, JSON.stringify(foto));
  const nombreDisco = new URL(foto.url).pathname.split('/').pop();
  prueba('guarda en disco con nombre UUID', /^[0-9a-f]{32}\.png$/.test(nombreDisco) && fs.existsSync(path.join(temporal, 'uploads', nombreDisco)), nombreDisco);
  const meta = (await db.query('select proposito, estado, tamano_bytes t from archivos where id=$1', [foto.id])).rows[0];
  prueba('registra los metadatos', meta?.proposito === 'foto_perfil' && meta?.estado === 'listo' && Number(meta?.t) === PNG.length, JSON.stringify(meta));
  prueba('se puede abrir por su URL', (await estado(fetch(foto.url))) === 200);
  prueba('archivo falso = 400', (await estado(subir(tokenAna, Buffer.from('no soy imagen'), 'x.png'))) === 400);
  prueba('extensión no permitida = 400', (await estado(subir(tokenAna, PNG, 'x.exe'))) === 400);
  prueba('más de 5 MB = 400', (await estado(subir(tokenAna, Buffer.concat([PNG, Buffer.alloc(5 * 1024 * 1024)]), 'g.png'))) === 400);

  seccion('Foto en el perfil y permisos');
  r = await ponerPerfil(tokenAna, { nombreUsuario: 'ana.dev', nombreVisible: 'Ana Dev', fotoArchivoId: foto.id });
  prueba('asignar mi foto = 200 con fotoUrl', r.status === 200 && (await leer(r)).fotoUrl === foto.url);
  prueba('foto de otra persona = 400', (await estado(ponerPerfil(beto.accessToken, { nombreUsuario: 'beto.ruiz', nombreVisible: 'Beto Ruiz', fotoArchivoId: foto.id }))) === 400);
  prueba('usuario normal en ruta de administrador = 403', (await estado(fetch(`${base}/api/perfiles`, { headers: bearer(tokenAna) }))) === 403);
} catch (error) {
  fallidas.push(`Error inesperado: ${error.message}`);
  console.error(error);
} finally {
  console.log(`\n${correctas} correctas, ${fallidas.length} fallidas`);
  if (fallidas.length) console.log(registroApp.split('\n').filter((l) => /^fail|Exception/.test(l)).slice(-10).join('\n'));
  app.kill();
  await servidor.stop();
  await db.close();
  fs.rmSync(temporal, { recursive: true, force: true, maxRetries: 5, retryDelay: 300 });
}
process.exit(fallidas.length ? 1 : 0);
