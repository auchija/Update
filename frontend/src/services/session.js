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
