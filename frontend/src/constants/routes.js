export const ROUTES = {
  home: "/",
  login: "/login",
  register: "/registro",
  feed: "/feed",
  explore: "/explorar",
  myPosts: "/mis-publicaciones",
  myFiles: "/mis-archivos",
  requests: "/solicitudes",
  profile: "/perfil",
  userProfile: "/u/:handle",
  post: "/p/:id",
};

/** Perfil público de un usuario: /u/handle. */
export function userProfilePath(handle) {
  return `/u/${encodeURIComponent(handle)}`;
}

/** Página de una publicación: /p/id. */
export function postPath(id) {
  return `/p/${id}`;
}

// Página a la que se llega después de iniciar sesión o registrarse.
export const DEFAULT_PRIVATE_ROUTE = ROUTES.feed;
