import { apiRequest } from "./api.js";
import { subirImagen } from "./archivosService.js";

export function getMiPerfil() {
  return apiRequest("/perfil/yo");
}

export function actualizarMiPerfil(datos) {
  return apiRequest("/perfil/yo", { method: "PUT", body: datos });
}

export function subirFotoPerfil(archivo) {
  return subirImagen(archivo, "foto_perfil");
}
