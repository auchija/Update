import { USE_MOCKS } from "../config.js";
import { apiRequest } from "./api.js";
import { mockLogin, mockRegister } from "./mockAuthApi.js";
import { clearSession, setSession } from "./session.js";

export async function login(identifier, password) {
  const data = USE_MOCKS
    ? await mockLogin(identifier, password)
    : await apiRequest("/auth/login", { method: "POST", body: { identifier, password } });

  setSession(data.accessToken, data.user);
  return data.user;
}

export async function register(form) {
  const data = USE_MOCKS
    ? await mockRegister(form)
    : await apiRequest("/auth/register", { method: "POST", body: form });

  setSession(data.accessToken, data.user);
  return data.user;
}

export async function logout() {
  try {
    if (!USE_MOCKS) {
      await apiRequest("/auth/logout", { method: "POST" });
    }
  } finally {
    clearSession();
  }
}
