import assert from 'node:assert/strict';
import { test } from 'node:test';
import { mkdtemp, readFile, writeFile, rm } from 'node:fs/promises';
import { tmpdir } from 'node:os';
import { join } from 'node:path';
import { pathToFileURL } from 'node:url';

test('El cliente envia el JWT, espera al servidor y propaga los errores', async () => {
  const carpeta = await mkdtemp(join(tmpdir(), 'update-publicaciones-'));
  const fetchOriginal = globalThis.fetch;
  try {
    await writeFile(join(carpeta, 'package.json'), '{"type":"module"}');
    await writeFile(join(carpeta, 'config.js'), 'export const API_URL = "https://prueba.invalid/api";');
    for (const nombre of ['api.js', 'session.js', 'publicacionesService.js']) {
      const fuente = await readFile(new URL(`../src/services/${nombre}`, import.meta.url), 'utf8');
      await writeFile(join(carpeta, nombre), fuente.replace('../config.js', './config.js'));
    }
    const cliente = await import(pathToFileURL(join(carpeta, 'publicacionesService.js')));
    const sesion = await import(pathToFileURL(join(carpeta, 'session.js')));
    sesion.setSession('token-prueba', { id: 'usuario', correo: 'ana@example.com' });
    assert.equal(sesion.getCurrentUser().displayName, 'ana');
    const solicitudes = [];
    let estado = 200;
    let respuesta = [];
    globalThis.fetch = async (url, opciones) => {
      solicitudes.push({ url, opciones });
      return new Response(estado === 204 ? null : JSON.stringify(respuesta), { status: estado });
    };
    await cliente.listarPublicaciones(2);
    assert.equal(solicitudes.at(-1).url, 'https://prueba.invalid/api/feed?pagina=2&tamanoPagina=20');
    respuesta = { id: 'id-del-servidor', contenido: 'Hola', esPropia: true };
    estado = 201;
    assert.deepEqual(await cliente.crearPublicacion('Hola'), respuesta);
    assert.deepEqual(JSON.parse(solicitudes.at(-1).opciones.body), { contenido: 'Hola' });
    assert.equal(solicitudes.at(-1).opciones.headers.Authorization, 'Bearer token-prueba');
    assert.equal(solicitudes.at(-1).opciones.credentials, 'include');
    estado = 200;
    await cliente.actualizarPublicacion('id-del-servidor', 'Editado');
    assert.equal(solicitudes.at(-1).opciones.method, 'PUT');
    estado = 403;
    respuesta = {};
    await assert.rejects(cliente.borrarPublicacion('ajena'), (error) => error.status === 403);
    estado = 204;
    assert.equal(await cliente.borrarPublicacion('propia'), null);
    globalThis.fetch = async () => { throw new Error('Sin conexion'); };
    await assert.rejects(cliente.crearPublicacion('Conservar texto'), (error) => error.status === 0);
  } finally {
    globalThis.fetch = fetchOriginal;
    await rm(carpeta, { recursive: true, force: true });
  }
});
