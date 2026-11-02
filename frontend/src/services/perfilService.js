import { apiRequest } from "./api.js";

export function getMiPerfil() {
  return apiRequest("/perfil/yo");
}

export function actualizarMiPerfil(datos) {
  return apiRequest("/perfil/yo", { method: "PUT", body: datos });
}

export function subirFotoPerfil(archivo) {
  const body = new FormData();
  body.append("archivo", archivo);
  return apiRequest("/archivos/subir", { method: "POST", body });
}
