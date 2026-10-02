import { PGlite } from '@electric-sql/pglite';
import { PGLiteSocketServer } from '@electric-sql/pglite-socket';
import fs from 'node:fs/promises';
import { spawn } from 'node:child_process';
import { createHmac, randomBytes } from 'node:crypto';

import { fileURLToPath } from 'node:url';
const raiz = fileURLToPath(new URL('../../', import.meta.url)).replace(/[\\/]$/, '');
const db = await PGlite.create();
const esquema = await fs.readFile(`${raiz}/verificacion/resultados/crear_esquema.sql`, 'utf8');
await db.exec(esquema);
const version = (await db.query('select version() as version')).rows[0].version;
const conteo = (await db.query("select count(*)::int as total from information_schema.tables where table_schema='public' and table_type='BASE TABLE'")).rows[0].total;
if (conteo !== 56) throw new Error(`Se esperaban 56 tablas, hay ${conteo}.`);
await db.exec(`
INSERT INTO monedas (codigo, nombre, simbolo) VALUES ('COP', 'Peso colombiano', '$');
INSERT INTO categorias (slug,nombre) VALUES ('pruebas','Pruebas');
INSERT INTO perfiles (id,tipo,nombre_usuario,nombre_visible) VALUES
('11111111-1111-4111-8111-111111111111','persona','persona_prueba','Persona prueba'),
('22222222-2222-4222-8222-222222222222','emprendimiento','empresa_prueba','Empresa prueba');
INSERT INTO usuarios (id,correo,terminos_aceptados_en,hash_contrasena) VALUES
('11111111-1111-4111-8111-111111111111','prueba@example.com',now(),'hash_de_prueba');
INSERT INTO emprendimientos (id,categoria_id,creado_por_usuario_id) VALUES
('22222222-2222-4222-8222-222222222222',1,'11111111-1111-4111-8111-111111111111');
`);
const server = new PGLiteSocketServer({ db, host: '127.0.0.1', port: 55432 });
await server.start();
const clave = randomBytes(48).toString('base64url');
const codificar = value => Buffer.from(JSON.stringify(value)).toString('base64url');
const firmar = rol => {
  const ahora = Math.floor(Date.now()/1000);
  const cuerpo = codificar({alg:'HS256',typ:'JWT'})+'.'+codificar({iss:'UpDate',aud:'UpDate.API',sub:'prueba',rol,iat:ahora,nbf:ahora-1,exp:ahora+600});
  return cuerpo+'.'+createHmac('sha256',clave).update(cuerpo).digest('base64url');
};
const token = firmar('administrador');
const app = spawn(process.env.DOTNET_UPDATE ?? 'dotnet', [`${raiz}/update.API/bin/Debug/net10.0/update.API.dll`], {
  cwd:`${raiz}/update.API`,
  env:{...process.env,ASPNETCORE_ENVIRONMENT:'Development',ASPNETCORE_URLS:'http://127.0.0.1:5089',Jwt__Clave:clave,
       ConnectionStrings__DefaultConnection:'Host=127.0.0.1;Port=55432;Database=postgres;Username=postgres;SSL Mode=Disable;Pooling=false;Timeout=10;Command Timeout=20'}
});
let registro='';
app.stdout.on('data', d=>registro+=d.toString());
app.stderr.on('data', d=>registro+=d.toString());
const resultados=[];
const verificar=(nombre,valor)=>{ if(!valor) throw new Error(nombre); resultados.push(nombre); };
async function llamada(metodo,ruta,datos,esperado,autorizacion=token) {
  const r=await fetch('http://127.0.0.1:5089'+ruta, {method:metodo,headers:{'Content-Type':'application/json',...(autorizacion?{Authorization:'Bearer '+autorizacion}:{})},...(datos!==undefined?{body:JSON.stringify(datos)}:{})});
  const t=await r.text(); let body; try {body=JSON.parse(t)} catch {body=t}
  if(r.status!==esperado) throw new Error(`${metodo} ${ruta}: esperado ${esperado}, recibido ${r.status}. ${t}`);
  resultados.push(`${metodo} ${ruta}: ${r.status}`);
  return body;
}
try {
  let listo=false;
  for(let i=0;i<60;i++) {
    try { if((await fetch('http://127.0.0.1:5089/estado')).ok) {listo=true;break;} } catch {}
    await new Promise(r=>setTimeout(r,250));
  }
  verificar('API inicia en .NET 10',listo);
  const salud = await llamada('GET', '/health', undefined, 200, null);
  verificar('Se conserva health con PostgreSQL Healthy', salud.status === 'Healthy');
  await llamada('GET', '/health/live', undefined, 200, null);
  await llamada('GET', '/health/ready', undefined, 200, null);
  const swagger = await llamada('GET', '/swagger/v1/swagger.json', undefined, 200, null);
  verificar('Swagger incluye CRUD y autenticación bearer', Object.hasOwn(swagger.paths, '/api/usuarios') && Object.hasOwn(swagger.components.securitySchemes, 'bearer'));
  const cors = await fetch('http://127.0.0.1:5089/api/categorias', { method: 'OPTIONS', headers: { Origin: 'http://localhost:5173', 'Access-Control-Request-Method': 'GET', 'Access-Control-Request-Headers': 'authorization' } });
  verificar('CORS admite el frontend con credentials include', cors.status === 204 && cors.headers.get('Access-Control-Allow-Origin') === 'http://localhost:5173' && cors.headers.get('Access-Control-Allow-Credentials') === 'true');
  await llamada('GET','/api/categorias',undefined,401,null);
  await llamada('GET','/api/categorias',undefined,403,firmar('usuario'));
  const cat=await llamada('POST','/api/categorias',{slug:'consulta',nombre:'Consulta',activo:false},201);
  verificar('PK integer generada y false explícito conservado',Number.isInteger(cat.id)&&cat.activo===false);
  await llamada('POST','/api/categorias',{slug:'consulta',nombre:'Duplicada'},409);
  await llamada('POST','/api/categorias',{slug:'sin_nombre',nombre:''},400);
  await llamada('GET','/api/categorias?pagina=0',undefined,400);
  await llamada('GET',`/api/categorias/${cat.id}`,undefined,200);
  await llamada('PUT',`/api/categorias/${cat.id}`,{slug:'consulta',nombre:'Actualizada',activo:false},204);
  await llamada('DELETE',`/api/categorias/${cat.id}`,undefined,204);
  await llamada('GET',`/api/categorias/${cat.id}`,undefined,404);
  const u='11111111-1111-4111-8111-111111111111';
  const e='22222222-2222-4222-8222-222222222222';
  const usuario=await llamada('GET',`/api/usuarios/${u}`,undefined,200);
  await llamada('GET', `/api/Users/${u}`, undefined, 200);
  const correo = await llamada('GET', '/api/Users/correo/PRUEBA@example.com', undefined, 200);
  verificar('Se conserva búsqueda de correo sin distinción de mayúsculas', correo.id === u);
  verificar('Respuesta de usuario omite hash de contraseña',!Object.hasOwn(usuario,'hashContrasena'));
  await llamada('POST','/api/miembros_emprendimiento',{emprendimientoId:e,usuarioId:u,rol:'propietario'},201);
  const miembro=await llamada('GET',`/api/miembros_emprendimiento/${e}/${u}`,undefined,200);
  verificar('Enum cero propietario respeta asignación explícita',miembro.rol==='propietario');
  await llamada('POST','/api/permisos_rol',{rol:'administrador',permiso:'prueba'},201);
  await llamada('GET','/api/permisos_rol/administrador/prueba',undefined,200);
  await llamada('POST','/api/tipos_persona',{codigo:'estudiante',nombre:'Estudiante'},201);
  await llamada('GET','/api/tipos_persona/estudiante',undefined,200);
  const campos={subidoPorUsuarioId:u,tipo:'imagen',proposito:'foto_perfil',bucket:'pruebas',claveObjeto:'imagen/prueba.png',tipoMime:'image/png',extension:'png',tamanoBytes:50};
  const archivo=await llamada('POST','/api/archivos',campos,201);
  verificar('UUID, timestamps UTC y defaults de archivo',typeof archivo.id==='string'&&archivo.estado==='pendiente'&&archivo.proveedorAlmacenamiento==='r2'&&archivo.creadoEn.endsWith('Z'));
  await llamada('PUT',`/api/archivos/${archivo.id}`,{...campos,estado:'listo'},204);
  const cambiado=await llamada('GET',`/api/archivos/${archivo.id}`,undefined,200);
  verificar('PUT conserva creación y cambia estado',cambiado.creadoEn===archivo.creadoEn&&cambiado.estado==='listo');
  await llamada('POST','/api/archivos',{...campos,claveObjeto:'otro.png',tipo:'invalido'},400);
  await llamada('POST','/api/archivos',{...campos,claveObjeto:'tercero.png',subidoPorUsuarioId:'33333333-3333-4333-8333-333333333333'},409);
  await llamada('DELETE',`/api/archivos/${archivo.id}`,undefined,204);
  await llamada('GET',`/api/archivos/${archivo.id}`,undefined,404);
  const oculto=(await db.query('SELECT eliminado_en,estado FROM archivos WHERE id=$1',[archivo.id])).rows[0];
  verificar('DELETE lógico conserva fila y marca eliminado',oculto.eliminado_en!==null&&oculto.estado==='eliminado');
  const lista=await llamada('GET','/api/archivos',undefined,200);
  verificar('GET listado excluye filas eliminadas',lista.length===0);
  const fkCount=(await db.query("SELECT count(*)::int as total FROM information_schema.table_constraints WHERE table_schema='public' AND constraint_type='FOREIGN KEY'")).rows[0].total;
  verificar('123 claves foráneas creadas en PostgreSQL',fkCount===123);
  const resultado={estado:'correcto',motor:version,tablas:conteo,columnasModelo:451,relaciones:fkCount,verificaciones:resultados};
  await fs.writeFile(`${raiz}/verificacion/resultados/integracion.json`,JSON.stringify(resultado,null,2));
  console.log(JSON.stringify(resultado));
} catch(error) {
  await fs.writeFile(`${raiz}/verificacion/resultados/api-error.log`,registro);
  console.error(error.message);
  process.exitCode=1;
} finally {
  app.kill('SIGTERM');
  await new Promise(r=>setTimeout(r,500));
  await server.stop();
  await db.close();
}
