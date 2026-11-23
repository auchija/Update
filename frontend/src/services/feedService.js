import { apiRequest } from "./api.js";
import { subirImagen } from "./archivosService.js";

export const FEED_PAGE_SIZE = 20;
export const COMMENTS_PAGE_SIZE = 50;

export function getFeed(pagina = 1) {
  return apiRequest(`/feed?pagina=${pagina}&tamanoPagina=${FEED_PAGE_SIZE}`);
}

export function getPublicacion(id) {
  return apiRequest(`/feed/${id}`);
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

export function getMisPublicaciones(pagina = 1) {
  return apiRequest(`/feed/mias?pagina=${pagina}&tamanoPagina=${FEED_PAGE_SIZE}`);
}

export function subirArchivoPublicacion(archivo) {
  return subirImagen(archivo, "publicacion");
}

/** Da o quita el "me gusta"; devuelve { meGusta, totalReacciones }. */
export function marcarMeGusta(id, meGusta) {
  return apiRequest(`/feed/${id}/me-gusta`, { method: meGusta ? "POST" : "DELETE" });
}

export function getComentarios(id) {
  return apiRequest(`/feed/${id}/comentarios?pagina=1&tamanoPagina=${COMMENTS_PAGE_SIZE}`);
}

export function comentar(id, contenido) {
  return apiRequest(`/feed/${id}/comentarios`, { method: "POST", body: { contenido } });
}

export function eliminarComentario(id, comentarioId) {
  return apiRequest(`/feed/${id}/comentarios/${comentarioId}`, { method: "DELETE" });
}
