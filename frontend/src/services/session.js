let accessToken = null;
let currentUser = null;

export function setSession(token, user) {
  accessToken = token;
  currentUser = user ? {
    ...user,
    email: user.email ?? user.correo,
    displayName: user.displayName ?? user.correo?.split("@")[0] ?? "Mi perfil",
  } : null;
}

export function getAccessToken() {
  return accessToken;
}

export function getCurrentUser() {
  return currentUser;
}

export function clearSession() {
  accessToken = null;
  currentUser = null;
}
