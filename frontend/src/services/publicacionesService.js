import { USE_MOCKS } from "../config.js";
import { apiRequest } from "./api.js";
import {
  mockActualizarPublicacion,
  mockBorrarPublicacion,
  mockCrearPublicacion,
  mockListarPublicaciones,
} from "./mockPublicacionesApi.js";

export function listarPublicaciones(pagina = 1) {
  return USE_MOCKS
    ? mockListarPublicaciones()
    : apiRequest(`/feed?pagina=${pagina}&tamanoPagina=20`);
}

export function crearPublicacion(contenido) {
  return USE_MOCKS
    ? mockCrearPublicacion(contenido)
    : apiRequest("/feed", { method: "POST", body: { contenido } });
}

export function actualizarPublicacion(id, contenido) {
  return USE_MOCKS
    ? mockActualizarPublicacion(id, contenido)
    : apiRequest(`/feed/${id}`, { method: "PUT", body: { contenido } });
}

export function borrarPublicacion(id) {
  return USE_MOCKS
    ? mockBorrarPublicacion(id)
    : apiRequest(`/feed/${id}`, { method: "DELETE" });
}