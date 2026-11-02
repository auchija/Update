// Guarda las publicaciones en memoria (se reinician al recargar la página).
let publicaciones = [
  {
    id: "mock-1",
    autor: "Equipo UpDate",
    contenido: "Bienvenido a UpDate. Comparte tu proyecto con la comunidad.",
    creadoEn: new Date().toISOString(),
    actualizadoEn: null,
    esPropia: false,
  },
];

let contador = 2;
const esperar = (ms = 300) => new Promise((resolve) => setTimeout(resolve, ms));

export async function mockListarPublicaciones() {
  await esperar();
  return [...publicaciones];
}

export async function mockCrearPublicacion(contenido) {
  await esperar();
  const nueva = {
    id: `mock-${contador++}`,
    autor: "Tú",
    contenido,
    creadoEn: new Date().toISOString(),
    actualizadoEn: null,
    esPropia: true,
  };
  publicaciones = [nueva, ...publicaciones];
  return nueva;
}

export async function mockActualizarPublicacion(id, contenido) {
  await esperar();
  const actual = publicaciones.find((p) => p.id === id);
  if (!actual) throw new Error("La publicacion ya no existe.");
  const actualizada = { ...actual, contenido, actualizadoEn: new Date().toISOString() };
  publicaciones = publicaciones.map((p) => (p.id === id ? actualizada : p));
  return actualizada;
}

export async function mockBorrarPublicacion(id) {
  await esperar();
  publicaciones = publicaciones.filter((p) => p.id !== id);
}