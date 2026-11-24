import assert from "node:assert/strict";
import { stdin, stdout } from "node:process";
import { createInterface } from "node:readline/promises";

const base = (process.env.UPDATE_API ?? "http://localhost:5195").replace(/\/$/, "");
if (!["localhost", "127.0.0.1", "[::1]"].includes(new URL(base).hostname)) {
  console.error("Usa una API local de pruebas.");
  process.exit(1);
}

async function pedir(metodo, ruta, token, { json, archivo } = {}) {
  const url = /^https?:\/\//.test(ruta) ? ruta : base + ruta;
  const headers = token ? { Authorization: `Bearer ${token}` } : {};
  let body;
  if (json !== undefined) {
    headers["Content-Type"] = "application/json";
    body = JSON.stringify(json);
  } else if (archivo) {
    body = new FormData();
    body.append("proposito", archivo.proposito);
    body.append("archivo", new Blob([archivo.datos]), archivo.nombre);
  }
  const respuesta = await fetch(url, { method: metodo, headers, body });
  const contenido = Buffer.from(await respuesta.arrayBuffer());
  return { status: respuesta.status, url, contenido, json: () => JSON.parse(contenido.toString()) };
}

async function esperar(status, peticion) {
  const respuesta = await peticion;
  if (respuesta.status !== status) {
    throw new Error(`${respuesta.url}: esperaba ${status}, recibí ${respuesta.status}: ${respuesta.contenido.toString().slice(0, 300)}`);
  }
  return respuesta;
}

async function login(indice) {
  const handle = `demo_backend2_${indice}`;
  let clave = process.env[`UPDATE_DEMO_CLAVE_${indice}`];
  if (!clave) {
    const consola = createInterface({ input: stdin, output: stdout });
    clave = await consola.question(`Clave de prueba de ${handle}: `);
    consola.close();
  }
  const respuesta = await esperar(200, pedir("POST", "/api/auth/login", null, { json: { identifier: handle, password: clave } }));
  return respuesta.json().accessToken;
}

const a = await login(1);
const b = await login(2);

const categorias = (await esperar(200, pedir("GET", "/api/mis-emprendimientos/categorias", a))).json();
const datos = { nombre: "Prueba temporal", categoriaId: categorias[0].id, etapa: "validacion" };
const emprendimiento = (await esperar(201, pedir("POST", "/api/mis-emprendimientos", a, { json: datos }))).json();
const rutaEmprendimiento = `/api/mis-emprendimientos/${emprendimiento.id}`;
datos.nombre = "Editado";
await esperar(404, pedir("GET", rutaEmprendimiento, b));
await esperar(404, pedir("PUT", rutaEmprendimiento, b, { json: datos }));
await esperar(200, pedir("PUT", rutaEmprendimiento, a, { json: datos }));
await esperar(204, pedir("DELETE", rutaEmprendimiento, a));
await esperar(404, pedir("GET", rutaEmprendimiento, a));

for (const [token, otro] of [[a, "_2"], [b, "_1"]]) {
  const propias = (await esperar(200, pedir("GET", "/api/feed/mias", token))).json();
  assert.ok(propias.length > 0 && propias.every((p) => p.esMia));
  assert.ok(!propias.some((p) => p.contenido.includes(`demo_backend2${otro}`)));
}
assert.ok((await esperar(200, pedir("GET", "/api/feed", a))).json().length > 0);
await esperar(401, pedir("GET", "/api/feed/mias", null));

const pdf = Buffer.from("%PDF-1.4\n%%EOF");
const subir = (nombre, proposito, contenido = pdf) => pedir("POST", "/api/archivos/subir", a, { archivo: { nombre, proposito, datos: contenido } });
await esperar(400, subir("x.jpg", "publicacion"));
await esperar(400, subir("x.pdf", "foto_perfil"));
await esperar(400, subir("x.pdf", "publicacion", Buffer.alloc(5 * 1024 * 1024 + 1, "x")));
const documento = (await esperar(201, subir("demo.pdf", "publicacion"))).json();
assert.ok(/^https?:\/\//.test(documento.url), "la URL del archivo debe ser completa");
assert.deepEqual((await esperar(200, pedir("GET", documento.url))).contenido, pdf);

const misArchivos = (await esperar(200, pedir("GET", "/api/archivos/mios", a))).json();
const enHistorial = misArchivos.find((x) => x.id === documento.id);
assert.ok(enHistorial && /^https?:\/\//.test(enHistorial.url), "el historial debe traer la URL completa");
assert.ok(!(await esperar(200, pedir("GET", "/api/archivos/mios", b))).json().some((x) => x.id === documento.id));

const entrada = { contenido: "Con adjunto", imagenArchivoId: documento.id };
const publicacion = (await esperar(201, pedir("POST", "/api/feed", a, { json: entrada }))).json();
assert.equal(publicacion.adjuntoTipoMime, "application/pdf");
const rutaPublicacion = `/api/feed/${publicacion.id}`;
const borrarDocumento = `/api/archivos/mios/${documento.id}`;
await esperar(403, pedir("DELETE", borrarDocumento, b));
await esperar(409, pedir("DELETE", borrarDocumento, a));
await esperar(403, pedir("PUT", rutaPublicacion, b, { json: entrada }));
await esperar(403, pedir("DELETE", rutaPublicacion, b));
await esperar(200, pedir("PUT", rutaPublicacion, a, { json: { contenido: "Sin adjunto" } }));
await esperar(204, pedir("DELETE", borrarDocumento, a));
await esperar(404, pedir("GET", documento.url));
await esperar(204, pedir("DELETE", rutaPublicacion, a));
await esperar(404, pedir("GET", rutaPublicacion, a));
console.log("OK: emprendimientos, dos usuarios, historiales, límite, extensiones y ciclo del PDF.");

const png = Buffer.from("iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAQAAAC1HAwCAAAAC0lEQVR42mP8/x8AAusB9Wl2YgAAAABJRU5ErkJggg==", "base64");
const imagen = async (proposito) => (await esperar(201, subir("demo.png", proposito, png))).json();

const foto = await imagen("foto_perfil");
const perfil = (await esperar(200, pedir("GET", "/api/perfil/yo", a))).json();
const campos = ["nombreVisible", "nombreUsuario", "titular", "biografia", "sitioWeb", "fotoArchivoId"];
const anterior = Object.fromEntries(campos.map((campo) => [campo, perfil[campo] ?? null]));
await esperar(200, pedir("PUT", "/api/perfil/yo", a, { json: { ...anterior, fotoArchivoId: foto.id } }));
assert.equal((await esperar(200, pedir("GET", "/api/perfil/yo", a))).json().fotoArchivoId, foto.id);
await esperar(200, pedir("PUT", "/api/perfil/yo", a, { json: anterior }));
await esperar(204, pedir("DELETE", `/api/archivos/mios/${foto.id}`, a));

const portada = await imagen("emprendimiento");
const conPortada = (await esperar(201, pedir("POST", "/api/mis-emprendimientos", a, { json: { ...datos, portadaArchivoId: portada.id } }))).json();
assert.ok(/^https?:\/\//.test(conPortada.portadaUrl), "la URL de la portada debe ser completa");
assert.deepEqual((await esperar(200, pedir("GET", conPortada.portadaUrl))).contenido, png);
await esperar(204, pedir("DELETE", `/api/mis-emprendimientos/${conPortada.id}`, a));
await esperar(204, pedir("DELETE", `/api/archivos/mios/${portada.id}`, a));
console.log("OK: foto de perfil y portada de emprendimiento.");

const productoId = process.env.UPDATE_DEMO_PRODUCTO_ID;
if (productoId) {
  const adjunto = await imagen("producto");
  const enlace = `/api/mis-productos/${productoId}/archivos/${adjunto.id}`;
  await esperar(403, pedir("PUT", enlace, b));
  await esperar(204, pedir("PUT", enlace, a));
  await esperar(409, pedir("DELETE", `/api/archivos/mios/${adjunto.id}`, a));
  await esperar(204, pedir("DELETE", enlace, a));
  await esperar(204, pedir("DELETE", `/api/archivos/mios/${adjunto.id}`, a));
  console.log("OK: archivo de producto.");
} else {
  console.log("Producto sin probar. Ejecuta producto.sql y define UPDATE_DEMO_PRODUCTO_ID.");
}
