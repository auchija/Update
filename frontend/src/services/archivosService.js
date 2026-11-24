import { API_URL } from "../config.js";
import { optimizeImage } from "../utils/imageOptimization.js";
import { apiRequest } from "./api.js";

export const ARCHIVOS_PAGE_SIZE = 20;

export async function subirImagen(archivo, proposito) {
  const body = new FormData();
  body.append("archivo", await optimizeImage(archivo));
  body.append("proposito", proposito);
  return apiRequest("/archivos/subir", { method: "POST", body });
}

export function getMisArchivos(pagina = 1) {
  return apiRequest(`/archivos/mios?pagina=${pagina}&tamanoPagina=${ARCHIVOS_PAGE_SIZE}`);
}

export async function eliminarArchivo(id) {
  try {
    await apiRequest(`/archivos/mios/${id}`, { method: "DELETE" });
  } catch (error) {
    if (error.status === 409) {
      error.message = "Este archivo está en uso. Quítalo primero de la publicación o del perfil.";
    }
    throw error;
  }
}

export function getArchivoUrl(url) {
  const isAbsolute = (value) => /^https?:\/\//.test(value ?? "");
  if (!url || isAbsolute(url) || !isAbsolute(API_URL)) return url;
  return new URL(url, API_URL).href;
}
