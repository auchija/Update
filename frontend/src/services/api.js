// ÚNICO archivo que hace peticiones (fetch) al backend. Se encarga de:
// - la URL base (VITE_API_URL) y enviar JSON,
// - adjuntar el access token (Authorization: Bearer ...),
// - enviar la cookie del refresh token (credentials: "include"),
// - renovar el access token una vez si vence (respuesta 401),
// - y convertir CUALQUIER error al mismo formato: ApiError { status, message, fieldErrors }.
import { API_URL } from "../config.js";
import { clearSession, getAccessToken, getCurrentUser, setSession } from "./session.js";

// Error con el formato común. status es 0 cuando no hubo respuesta (sin internet, servidor caído).
export class ApiError extends Error {
  constructor(status, message, fieldErrors = {}) {
    super(message);
    this.status = status;
    this.fieldErrors = fieldErrors;
  }
}

// Mensajes por defecto si el backend no envía uno. Coinciden con los códigos del backend.
const DEFAULT_MESSAGES = {
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

function defaultMessage(status) {
  return DEFAULT_MESSAGES[status] || "Algo falló en el servidor. Inténtalo de nuevo más tarde.";
}

/**
 * Hace una petición a la API y devuelve el JSON de la respuesta.
 * Ejemplo: apiRequest("/auth/login", { method: "POST", body: { identifier, password } })
 */
export async function apiRequest(path, { method = "GET", body, retry = true } = {}) {
  const headers = { "Content-Type": "application/json" };
  const token = getAccessToken();
  if (token) headers.Authorization = `Bearer ${token}`;

  let response;
  try {
    response = await fetch(API_URL + path, {
      method,
      headers,
      body: body ? JSON.stringify(body) : undefined,
      credentials: "include", // envía y recibe la cookie httpOnly del refresh token
    });
  } catch {
    throw new ApiError(
      0,
      "No pudimos conectar con el servidor. Revisa tu conexión e inténtalo de nuevo.",
    );
  }

  // El access token dura 15 minutos. Si vence, pedimos uno nuevo y repetimos la petición UNA vez.
  // (Las rutas /auth/... no se reintentan: ahí un 401 significa credenciales incorrectas.)
  if (response.status === 401 && retry && token && !path.startsWith("/auth/")) {
    const renewed = await refreshAccessToken();
    if (renewed) return apiRequest(path, { method, body, retry: false });
  }

  // Si la respuesta no es JSON (por ejemplo, 204 sin contenido), data queda en null.
  const data = await response.json().catch(() => null);

  if (!response.ok) {
    throw new ApiError(
      response.status,
      data?.message || defaultMessage(response.status),
      data?.errors || {},
    );
  }

  return data;
}

/**
 * Pide un access token nuevo usando la cookie del refresh token.
 * Devuelve true si lo consiguió. Si la cookie venció o fue revocada, cierra la sesión.
 */
export async function refreshAccessToken() {
  try {
    const data = await apiRequest("/auth/refresh", { method: "POST", retry: false });
    setSession(data.accessToken, data.user ?? getCurrentUser());
    return true;
  } catch {
    clearSession();
    return false;
  }
}
