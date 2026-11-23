// Datos de demostración: dos cuentas, cada una con una publicación y un emprendimiento.
// Pasa por la API (no escribe en la base directo), así que respeta todas las validaciones.
// Se puede correr varias veces: lo que ya existe no se duplica.
//
// Uso (con la API encendida y las migraciones aplicadas):
//   node backend/demo/sembrar.mjs
// Variables opcionales: UPDATE_API (por defecto http://localhost:5195),
// UPDATE_DEMO_CLAVE_1 y UPDATE_DEMO_CLAVE_2 (si no están, se piden por consola).
import { stdin, stdout } from "node:process";
import { createInterface } from "node:readline/promises";

const base = (process.env.UPDATE_API ?? "http://localhost:5195").replace(/\/$/, "");
if (!["localhost", "127.0.0.1", "[::1]"].includes(new URL(base).hostname)) {
  console.error("Este script solo trabaja con una API local. Usa una base de pruebas.");
  process.exit(1);
}

async function llamar(ruta, { datos, token } = {}) {
  const headers = { "Content-Type": "application/json" };
  if (token) headers.Authorization = `Bearer ${token}`;
  const respuesta = await fetch(base + ruta, {
    method: datos === undefined ? "GET" : "POST",
    headers,
    body: datos === undefined ? undefined : JSON.stringify(datos),
  });
  if (!respuesta.ok) {
    const error = new Error(`HTTP ${respuesta.status} en ${ruta}: ${await respuesta.text()}`);
    error.status = respuesta.status;
    throw error;
  }
  return respuesta.json();
}

async function* paginas(ruta, token) {
  for (let pagina = 1; ; pagina++) {
    const lote = await llamar(`${ruta}?pagina=${pagina}&tamanoPagina=100`, { token });
    yield* lote;
    if (lote.length < 100) return;
  }
}

async function buscar(ruta, token, condicion) {
  for await (const elemento of paginas(ruta, token)) if (condicion(elemento)) return elemento;
  return null;
}

async function pedirClave(handle) {
  const consola = createInterface({ input: stdin, output: stdout });
  try {
    return await consola.question(`Clave de prueba de ${handle} (no uses una clave real): `);
  } finally {
    consola.close();
  }
}

for (const indice of [1, 2]) {
  const handle = `demo_backend2_${indice}`;
  const correo = `${handle}@example.test`;
  const clave = process.env[`UPDATE_DEMO_CLAVE_${indice}`] ?? (await pedirClave(handle));
  if (clave.length < 8 || !/[a-zA-Z]/.test(clave) || !/\d/.test(clave)) {
    console.error("Usa al menos ocho caracteres con letras y números.");
    process.exit(1);
  }

  let sesion;
  try {
    sesion = await llamar("/api/auth/login", { datos: { identifier: correo, password: clave } });
  } catch (error) {
    if (error.status !== 401) throw error;
    sesion = await llamar("/api/auth/register", {
      datos: {
        displayName: `Demo Backend ${indice}`,
        handle,
        email: correo,
        password: clave,
        personas: ["entrepreneur"],
        termsAccepted: true,
      },
    });
  }
  const token = sesion.accessToken;

  // Las categorías base las crea la migración IndicesHistorialYCategorias.
  const categoria = await buscar("/api/mis-emprendimientos/categorias", token, () => true);
  if (!categoria) {
    console.error("No hay categorías. Aplica las migraciones: dotnet ef database update.");
    process.exit(1);
  }

  const contenido = `Primera publicacion de ${handle}`;
  if (!(await buscar("/api/feed/mias", token, (p) => p.contenido === contenido))) {
    await llamar("/api/feed", { datos: { contenido }, token });
  }

  const nombre = `Proyecto demo ${indice}`;
  if (!(await buscar("/api/mis-emprendimientos", token, (e) => e.nombre === nombre))) {
    await llamar("/api/mis-emprendimientos", {
      datos: { nombre, categoriaId: categoria.id, etapa: "idea" },
      token,
    });
  }

  console.log(`Lista la cuenta ${correo}, con su publicación y su emprendimiento.`);
}
