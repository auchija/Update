import { API_URL } from "../config.js";
import { clearSession, getAccessToken, getCurrentUser, setSession } from "./session.js";

const NO_CONNECTION_STATUS = 0;

const DEFAULT_ERROR_MESSAGES = {
  400: "Revisa los datos e inténtalo de nuevo.",
  401: "Tu sesión terminó. Inicia sesión de nuevo.",
  403: "No tienes permiso para hacer esto.",
  404: "No encontramos lo que buscabas.",
  409: "Ya existe un registro con esos datos.",
  413: "El archivo es demasiado pesado.",
  422: "Revisa los datos e inténtalo de nuevo.",
  423: "Tu cuenta está bloqueada por demasiados intentos. Inténtalo de nuevo en 15 minutos.",
  429: "Hiciste demasiadas peticiones. Espera un momento.",
};

const GENERIC_ERROR_MESSAGE = "Algo falló en el servidor. Inténtalo de nuevo más tarde.";

export class ApiError extends Error {
  constructor(status, message, fieldErrors = {}) {
    super(message);
    this.status = status;
    this.fieldErrors = fieldErrors;
  }
}

export async function apiRequest(path, { method = "GET", body, retryOnExpiredToken = true } = {}) {
  const accessToken = getAccessToken();
  const response = await sendRequest(path, { method, body, accessToken });

  const tokenExpired = response.status === 401 && accessToken && !path.startsWith("/auth/");
  if (tokenExpired && retryOnExpiredToken && (await refreshAccessToken())) {
    return apiRequest(path, { method, body, retryOnExpiredToken: false });
  }

  const data = await response.json().catch(() => null);

  if (!response.ok) {
    throw new ApiError(
      response.status,
      data?.message || DEFAULT_ERROR_MESSAGES[response.status] || GENERIC_ERROR_MESSAGE,
      data?.errors || {},
    );
  }

  return data;
}

async function sendRequest(path, { method, body, accessToken }) {
  const headers = { "Content-Type": "application/json" };
  if (accessToken) {
    headers.Authorization = `Bearer ${accessToken}`;
  }

  try {
    return await fetch(API_URL + path, {
      method,
      headers,
      body: body ? JSON.stringify(body) : undefined,
      credentials: "include",
    });
  } catch {
    throw new ApiError(
      NO_CONNECTION_STATUS,
      "No pudimos conectar con el servidor. Revisa tu conexión e inténtalo de nuevo.",
    );
  }
}

async function refreshAccessToken() {
  try {
    const data = await apiRequest("/auth/refresh", {
      method: "POST",
      retryOnExpiredToken: false,
    });
    setSession(data.accessToken, data.user ?? getCurrentUser());
    return true;
  } catch {
    clearSession();
    return false;
  }
}
