// Sesión del usuario, tal como la define el backend (docs/contrato-api.md):
//
// - El ACCESS TOKEN (dura 15 minutos) se guarda solo EN MEMORIA, en esta variable.
//   Nunca en localStorage: así un script malicioso (XSS) no puede robarlo.
// - El REFRESH TOKEN (dura 30 días) lo guarda el backend en una cookie httpOnly.
//   JavaScript no puede leerla; el navegador la envía sola en cada petición.
//
// Al recargar la página la memoria se borra, pero la cookie sigue ahí: el frontend
// pide un token nuevo con restoreSession() (en services/authService.js).

let accessToken = null;
let currentUser = null;

export function setSession(token, user) {
  accessToken = token;
  currentUser = user;
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
