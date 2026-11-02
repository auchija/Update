import { apiRequest } from "./api.js";

/**
 * Sube una imagen al servidor; devuelve { id, url, tipoMime, tamanoBytes }.
 * proposito: "foto_perfil" o "publicacion".
 */
export function subirImagen(archivo, proposito) {
  const body = new FormData();
  body.append("archivo", archivo);
  body.append("proposito", proposito);
  return apiRequest("/archivos/subir", { method: "POST", body });
}
