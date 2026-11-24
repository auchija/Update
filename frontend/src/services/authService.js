import { apiRequest, isTemporaryError } from "./api.js";
import { clearSession, setSession } from "./session.js";

const RESTORE_RETRY_DELAYS_MS = [1000, 2000, 4000, 8000, 15000, 15000, 15000, 15000];

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

export async function restoreSession() {
  for (let attempt = 0; ; attempt++) {
    try {
      const data = await apiRequest("/auth/refresh", {
        method: "POST",
        retryOnExpiredToken: false,
      });

      setSession(data.accessToken, null);

      const user = await apiRequest("/auth/me");
      setSession(data.accessToken, user);
      return user;
    } catch (error) {
      clearSession();
      if (!isTemporaryError(error)) return null;
      if (attempt >= RESTORE_RETRY_DELAYS_MS.length) throw error;
      await new Promise((resolve) => setTimeout(resolve, RESTORE_RETRY_DELAYS_MS[attempt]));
    }
  }
}

export async function logout() {
  try {
    await apiRequest("/auth/logout", { method: "POST" });
  } finally {
    clearSession();
  }
}
