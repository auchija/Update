import { apiRequest } from "./api.js";

export function listarPublicaciones(pagina = 1) {
  return apiRequest(`/feed?pagina=${pagina}&tamanoPagina=20`);
}

export function crearPublicacion(contenido) {
  return apiRequest("/feed", { method: "POST", body: { contenido } });
}

export function actualizarPublicacion(id, contenido) {
  return apiRequest(`/feed/${id}`, { method: "PUT", body: { contenido } });
}

export function borrarPublicacion(id) {
  return apiRequest(`/feed/${id}`, { method: "DELETE" });
}
