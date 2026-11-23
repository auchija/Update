import { API_URL } from "../config.js";
import { apiRequest } from "./api.js";

export const ARCHIVOS_PAGE_SIZE = 20;

/**
 * Sube un archivo; devuelve { id, url, tipoMime, tamanoBytes }.
 * proposito: "foto_perfil", "publicacion", "producto" o "emprendimiento".
 */
export function subirImagen(archivo, proposito) {
  const body = new FormData();
  body.append("archivo", archivo);
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
    // El backend responde 409 si el archivo sigue usado en una publicación o perfil.
    if (error.status === 409) {
      error.message = "Este archivo está en uso. Quítalo primero de la publicación o del perfil.";
    }
    throw error;
  }
}

/** /archivos/mios devuelve rutas relativas ("/uploads/..."): las completa con el host de la API. */
export function getArchivoUrl(url) {
  if (!url || /^https?:\/\//.test(url) || !API_URL) return url;
  return new URL(url, API_URL).href;
}