import { PGlite } from '@electric-sql/pglite';
import fs from 'node:fs/promises';
import { fileURLToPath } from 'node:url';

const raiz = fileURLToPath(new URL('../../', import.meta.url)).replace(/[\\/]$/, '');
const leer = async nombre => (await fs.readFile(`${raiz}/migraciones/${nombre}.sql`, 'utf8')).replace(/^\uFEFF/, '');
const [completo, inicial, transicion, revertir] = await Promise.all([
  leer('ImplementarModeloUpDate'), leer('InicialUsuarios'), leer('TransicionUsuarios'), leer('RevertirModelo')
]);
const resultados = [];
function exigir(nombre, valor) {
  if (!valor) throw new Error(nombre);
  resultados.push(nombre);
}
async function contar(db) {
  const tablas = (await db.query("SELECT count(*)::int AS n FROM information_schema.tables WHERE table_schema='public' AND table_type='BASE TABLE' AND table_name <> '__EFMigrationsHistory'")).rows[0].n;
  const columnas = (await db.query("SELECT count(*)::int AS n FROM information_schema.columns WHERE table_schema='public' AND table_name <> '__EFMigrationsHistory'")).rows[0].n;
  const relaciones = (await db.query("SELECT count(*)::int AS n FROM information_schema.table_constraints WHERE table_schema='public' AND constraint_type='FOREIGN KEY'")).rows[0].n;
  const historial = (await db.query('SELECT count(*)::int AS n FROM "__EFMigrationsHistory"')).rows[0].n;
  return { tablas, columnas, relaciones, historial };
}

try {
  let db = await PGlite.create();
  try {
    await db.exec(completo);
    await db.exec(completo);
    const n = await contar(db);
    exigir('Base vacía: 56 tablas, 451 columnas, 123 FK y 2 migraciones; SQL idempotente', n.tablas === 56 && n.columnas === 451 && n.relaciones === 123 && n.historial === 2);
    const identidades = (await db.query("SELECT count(*)::int AS n FROM information_schema.columns WHERE table_schema='public' AND identity_generation='ALWAYS'")).rows[0].n;
    exigir('Las seis identidades ALWAYS coinciden con el SQL del proyecto', identidades === 6);
  } finally { await db.close(); }

  db = await PGlite.create();
  try {
    await db.exec(inicial);
    await db.exec(`INSERT INTO "Usuarios" ("Id", "TipoPerfil", "Correo", "HashContrasena", "RolPlataforma", "TerminosAceptadosEn", "CreadoEn", "ActualizadoEn", "IntentosFallidos") VALUES
      ('55555555-5555-4555-8555-555555555555', 0, 'Anterior@ejemplo.com', 'hash_anterior_1', 2, '2026-09-15T12:00:00Z', '2026-09-15T12:00:00Z', NULL, 4),
      ('66666666-6666-4666-8666-666666666666', 0, 'otro@ejemplo.com', 'hash_anterior_2', 0, '2026-09-16T12:00:00Z', '2026-09-16T12:00:00Z', '2026-09-17T12:00:00Z', 0);`);
    await db.exec(transicion);
    await db.exec(transicion);
    const n = await contar(db);
    exigir('Transición de Usuarios original: modelo completo e historial sin duplicar', n.tablas === 56 && n.columnas === 451 && n.relaciones === 123 && n.historial === 2);
    const usuarios = (await db.query('SELECT id, correo, hash_contrasena, rol_plataforma, intentos_fallidos, creado_en, actualizado_en FROM usuarios ORDER BY id')).rows;
    exigir('Se conservan UUID, correo, hash, rol administrador y contador de intentos', usuarios[0].id === '55555555-5555-4555-8555-555555555555' && usuarios[0].correo === 'Anterior@ejemplo.com' && usuarios[0].hash_contrasena === 'hash_anterior_1' && usuarios[0].rol_plataforma === 'administrador' && usuarios[0].intentos_fallidos === 4);
    exigir('Rol cero se convierte a usuario y se conserva la fecha de actualización existente', usuarios[1].rol_plataforma === 'usuario' && usuarios[1].actualizado_en.toISOString() === '2026-09-17T12:00:00.000Z');
    exigir('Fecha de actualización antes nula se completa con la fecha de creación', usuarios[0].creado_en.toISOString() === '2026-09-15T12:00:00.000Z' && usuarios[0].actualizado_en.toISOString() === '2026-09-15T12:00:00.000Z');
    const perfiles = (await db.query('SELECT id, tipo, nombre_usuario, creado_en FROM perfiles ORDER BY id')).rows;
    exigir('Se crean perfiles de persona con los UUID originales y nombres únicos', perfiles.length === 2 && perfiles[0].id === usuarios[0].id && perfiles[1].id === usuarios[1].id && perfiles.every(p => p.tipo === 'persona') && perfiles[0].nombre_usuario !== perfiles[1].nombre_usuario);
    await db.exec(revertir);
    const originales = (await db.query('SELECT "Id", "Correo", "HashContrasena", "RolPlataforma" FROM "Usuarios" ORDER BY "Id"')).rows;
    exigir('La reversión conserva usuarios y recupera los enums numéricos', originales.length === 2 && originales[0].RolPlataforma === 2 && originales[0].HashContrasena === 'hash_anterior_1');
    await db.exec(transicion);
    exigir('La transición se puede aplicar después de revertirla', (await contar(db)).tablas === 56);
  } finally { await db.close(); }

  db = await PGlite.create();
  try {
    await db.exec(inicial);
    await db.exec(`INSERT INTO "Usuarios" ("Id", "TipoPerfil", "Correo", "HashContrasena", "RolPlataforma", "TerminosAceptadosEn", "CreadoEn") VALUES ('77777777-7777-4777-8777-777777777777', 1, 'incompatible@ejemplo.com', 'hash_prueba', 0, now(), now());`);
    let rechazado = false;
    try { await db.exec(transicion); }
    catch (e) { rechazado = e.message.includes('perfiles de persona'); await db.exec('ROLLBACK'); }
    const fila = (await db.query('SELECT "TipoPerfil" FROM "Usuarios"')).rows[0];
    exigir('Datos incompatibles detienen la migración y conservan la fila original', rechazado && fila.TipoPerfil === 1);
  } finally { await db.close(); }

  const resumen = { estado: 'correcto', tablas: 56, columnas: 451, relaciones: 123, migracionesAplicadas: 2, usuariosAnterioresProbados: 2, verificaciones: resultados };
  await fs.writeFile(`${raiz}/verificacion/resultados/migracion.json`, JSON.stringify(resumen, null, 2));
  console.log(JSON.stringify(resumen));
} catch (error) {
  console.error(error.message);
  process.exitCode = 1;
}
