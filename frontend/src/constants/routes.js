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

export function userProfilePath(handle) {
  return `/u/${encodeURIComponent(handle)}`;
}

export function postPath(id) {
  return `/p/${id}`;
}

export const DEFAULT_PRIVATE_ROUTE = ROUTES.feed;
