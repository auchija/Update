import { apiRequest } from "./api.js";
import { FEED_PAGE_SIZE } from "./feedService.js";

const enRuta = (handle) => encodeURIComponent(handle);

export function getPerfilPublico(handle) {
  return apiRequest(`/comunidad/perfiles/${enRuta(handle)}`);
}

export function getPublicacionesDePerfil(handle, pagina = 1) {
  return apiRequest(
    `/comunidad/perfiles/${enRuta(handle)}/publicaciones?pagina=${pagina}&tamanoPagina=${FEED_PAGE_SIZE}`,
  );
}

/** Sigue o deja de seguir; devuelve { loSigo, totalSeguidores }. */
export function seguirPerfil(handle, seguir) {
  return apiRequest(`/comunidad/perfiles/${enRuta(handle)}/seguir`, {
    method: seguir ? "POST" : "DELETE",
  });
}

export function getSugerencias(cantidad = 10) {
  return apiRequest(`/comunidad/sugerencias?cantidad=${cantidad}`);
}
