import { apiRequest } from "./api.js";
import { clearSession, setSession } from "./session.js";

export async function login(identifier, password) {
  const data = await apiRequest("/auth/login", { method: "POST", body: { identifier, password } });

  setSession(data.accessToken, data.user);
  return data.user;
}

export async function register(form) {
  const data = await apiRequest("/auth/register", { method: "POST", body: form });

  setSession(data.accessToken, data.user);
  return data.user;
}

/**
 * Recupera la sesión al abrir o recargar la página.
 * El backend lee la cookie de renovación y responde con un token nuevo.
 * Devuelve el usuario si había sesión, o null si no la había.
 */
export async function restoreSession() {
  try {
    const data = await apiRequest("/auth/refresh", { method: "POST", retryOnExpiredToken: false });

    setSession(data.accessToken, data.user ?? null);

    // Si el backend solo devuelve el token, pedimos el usuario aparte.
    const user = data.user ?? (await apiRequest("/auth/me"));
    setSession(data.accessToken, user);
    return user;
  } catch {
    clearSession();
    return null;
  }
}

export async function logout() {
  try {
    await apiRequest("/auth/logout", { method: "POST" });
  } finally {
    clearSession();
  }
}
