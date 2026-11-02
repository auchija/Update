import { apiRequest } from "./api.js";
import { subirImagen } from "./archivosService.js";

export const FEED_PAGE_SIZE = 20;

export function getFeed(pagina = 1) {
  return apiRequest(`/feed?pagina=${pagina}&tamanoPagina=${FEED_PAGE_SIZE}`);
}

export function crearPublicacion(datos) {
  return apiRequest("/feed", { method: "POST", body: datos });
}

export function actualizarPublicacion(id, datos) {
  return apiRequest(`/feed/${id}`, { method: "PUT", body: datos });
}

export function eliminarPublicacion(id) {
  return apiRequest(`/feed/${id}`, { method: "DELETE" });
}

export function subirImagenPublicacion(archivo) {
  return subirImagen(archivo, "publicacion");
}
