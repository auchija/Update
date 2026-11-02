import { USE_MOCKS } from "../config.js";
import { apiRequest } from "./api.js";
import { mockActualizarMiPerfil, mockGetMiPerfil, mockSubirFoto } from "./mockPerfilApi.js";

export function getMiPerfil() {
  return USE_MOCKS ? mockGetMiPerfil() : apiRequest("/perfil/yo");
}

export function actualizarMiPerfil(datos) {
  return USE_MOCKS
    ? mockActualizarMiPerfil(datos)
    : apiRequest("/perfil/yo", { method: "PUT", body: datos });
}

export function subirFotoPerfil(archivo) {
  if (USE_MOCKS) return mockSubirFoto(archivo);

  const body = new FormData();
  body.append("archivo", archivo);
  return apiRequest("/archivos/subir", { method: "POST", body });
}
