import { TEST_USERS } from "../data/users.js";
import { ApiError } from "./api.js";
import { getCurrentUser } from "./session.js";

const DELAY_MS = 500;
const fotosSubidas = new Map();

export async function mockGetMiPerfil() {
  await simulateDelay();
  return leerPerfil();
}

export async function mockActualizarMiPerfil(datos) {
  await simulateDelay();

  const yo = getCurrentUser();
  const ocupado = TEST_USERS.some(
    (user) => user.handle === datos.nombreUsuario && user.id !== yo.id,
  );
  if (ocupado) {
    throw new ApiError(409, "Revisa los datos marcados.", {
      nombreUsuario: "Ese usuario no está disponible.",
    });
  }

  const actual = leerPerfil();
  const perfil = {
    ...actual,
    ...datos,
    fotoUrl: fotosSubidas.get(datos.fotoArchivoId) ?? actual.fotoUrl,
  };
  try {
    localStorage.setItem(storageKey(yo.id), JSON.stringify(perfil));
  } catch {
    return perfil;
  }
  return perfil;
}

export async function mockSubirFoto(archivo) {
  await simulateDelay();

  const id = crypto.randomUUID();
  const url = await leerComoDataUrl(archivo);
  fotosSubidas.set(id, url);
  return { id, url, tipoMime: archivo.type, tamanoBytes: archivo.size };
}

function leerPerfil() {
  const yo = getCurrentUser();
  const inicial = {
    id: yo.id,
    nombreUsuario: yo.handle,
    nombreVisible: yo.displayName,
    titular: null,
    biografia: null,
    sitioWeb: null,
    fotoArchivoId: null,
    fotoUrl: yo.avatarUrl,
    totalSeguidores: 0,
    totalSeguidos: 0,
    totalPublicaciones: 0,
  };

  try {
    return { ...inicial, ...JSON.parse(localStorage.getItem(storageKey(yo.id)) ?? "{}") };
  } catch {
    return inicial;
  }
}

function storageKey(userId) {
  return `mock-perfil-${userId}`;
}

function leerComoDataUrl(archivo) {
  return new Promise((resolve, reject) => {
    const reader = new FileReader();
    reader.onload = () => resolve(reader.result);
    reader.onerror = () => reject(new ApiError(400, "No pudimos leer la imagen."));
    reader.readAsDataURL(archivo);
  });
}

function simulateDelay() {
  return new Promise((resolve) => setTimeout(resolve, DELAY_MS));
}
