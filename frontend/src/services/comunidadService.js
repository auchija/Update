import { apiRequest } from "./api.js";
import { FEED_PAGE_SIZE } from "./feedService.js";

export const PEOPLE_PAGE_SIZE = 30;

const enRuta = (handle) => encodeURIComponent(handle);

export function getPerfilPublico(handle) {
  return apiRequest(`/comunidad/perfiles/${enRuta(handle)}`);
}

export function getPublicacionesDePerfil(handle, pagina = 1) {
  return apiRequest(
    `/comunidad/perfiles/${enRuta(handle)}/publicaciones?pagina=${pagina}&tamanoPagina=${FEED_PAGE_SIZE}`,
  );
}

export function getSeguidores(handle, pagina = 1) {
  return apiRequest(
    `/comunidad/perfiles/${enRuta(handle)}/seguidores?pagina=${pagina}&tamanoPagina=${PEOPLE_PAGE_SIZE}`,
  );
}

export function getSeguidos(handle, pagina = 1) {
  return apiRequest(
    `/comunidad/perfiles/${enRuta(handle)}/seguidos?pagina=${pagina}&tamanoPagina=${PEOPLE_PAGE_SIZE}`,
  );
}

export function getRelacionados(handle, cantidad = 8) {
  return apiRequest(`/comunidad/perfiles/${enRuta(handle)}/relacionados?cantidad=${cantidad}`);
}

/** Sigue (o envía solicitud) o deja de seguir; devuelve { loSigo, solicitudPendiente, totalSeguidores }. */
export function seguirPerfil(handle, seguir) {
  return apiRequest(`/comunidad/perfiles/${enRuta(handle)}/seguir`, {
    method: seguir ? "POST" : "DELETE",
  });
}

export function getSugerencias(cantidad = 10) {
  return apiRequest(`/comunidad/sugerencias?cantidad=${cantidad}`);
}

/** Solicitudes para seguir mi cuenta: [{ perfil, creadoEn }]. */
export function getSolicitudes() {
  return apiRequest("/comunidad/solicitudes");
}

export function aceptarSolicitud(handle) {
  return apiRequest(`/comunidad/solicitudes/${enRuta(handle)}/aceptar`, { method: "POST" });
}

export function rechazarSolicitud(handle) {
  return apiRequest(`/comunidad/solicitudes/${enRuta(handle)}`, { method: "DELETE" });
}
