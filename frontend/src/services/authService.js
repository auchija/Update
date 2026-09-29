// Registro, inicio y cierre de sesión.
// Con VITE_USE_MOCKS=true usa los usuarios de prueba (src/data/users.js) y simula al
// backend: tarda un poco, aplica las mismas reglas y devuelve los mismos errores.
// Con VITE_USE_MOCKS=false llama a la API real (ver docs/contrato-api.md).
//
// Este es el ÚNICO archivo que conoce las rutas /auth/... y la forma de su JSON.
// Si el backend cambia un nombre de campo, se ajusta aquí.
import { USE_MOCKS } from "../config.js";
import { TEST_USERS } from "../data/users.js";
import { ApiError, apiRequest, refreshAccessToken } from "./api.js";
import { clearSession, getCurrentUser, setSession } from "./session.js";

/**
 * Inicia sesión con el correo o el @usuario.
 * Devuelve el usuario. Si falla lanza un ApiError:
 * 401 (datos incorrectos) o 423 (cuenta bloqueada 15 minutos tras 5 intentos fallidos).
 */
export async function login(identifier, password) {
  const data = USE_MOCKS
    ? await fakeLogin(identifier, password)
    : await apiRequest("/auth/login", { method: "POST", body: { identifier, password } });

  setSession(data.accessToken, data.user);
  return data.user;
}

/**
 * Crea la cuenta e inicia sesión.
 * `form` = { displayName, handle, email, password, personas: ["entrepreneur", ...], termsAccepted }
 * Devuelve el usuario. Si el correo o el @usuario ya existen lanza un ApiError 409.
 */
export async function register(form) {
  const data = USE_MOCKS
    ? await fakeRegister(form)
    : await apiRequest("/auth/register", { method: "POST", body: form });

  setSession(data.accessToken, data.user);
  return data.user;
}

/** Cierra la sesión en el servidor (revoca el refresh token) y en el navegador. */
export async function logout() {
  try {
    if (USE_MOCKS) fakeLogout();
    else await apiRequest("/auth/logout", { method: "POST" });
  } finally {
    clearSession(); // aunque el servidor falle, en este navegador la sesión se cierra
  }
}

/**
 * Al recargar la página el access token se pierde (vive en memoria).
 * Esta función pide uno nuevo con la cookie del refresh token.
 * Devuelve el usuario si había sesión, o null si no.
 */
export async function restoreSession() {
  if (USE_MOCKS) {
    const data = fakeRefresh();
    if (data) setSession(data.accessToken, data.user);
    return data?.user ?? null;
  }
  const ok = await refreshAccessToken();
  return ok ? getCurrentUser() : null;
}

// ---------------------------------------------------------------------------
// Backend simulado (solo se usa con VITE_USE_MOCKS=true)
// ---------------------------------------------------------------------------

const REGISTERED_KEY = "test-registered-users";
// Simula la cookie httpOnly del refresh token. En el backend real es una cookie
// que JavaScript NO puede leer; aquí usamos localStorage solo para la demostración.
const FAKE_COOKIE_KEY = "test-refresh-cookie";
const MAX_FAILED_ATTEMPTS = 5;
const failedAttempts = {}; // intentos fallidos por usuario (id → número)

function getAllUsers() {
  const registered = JSON.parse(localStorage.getItem(REGISTERED_KEY) || "[]");
  return [...TEST_USERS, ...registered];
}

// Espera entre 0,5 y 1,2 segundos, como una petición real.
function wait() {
  return new Promise((resolve) => setTimeout(resolve, 500 + Math.random() * 700));
}

// La API real nunca devuelve la contraseña.
function withoutPassword(user) {
  const { password: _password, ...publicUser } = user;
  return publicUser;
}

function startFakeSession(user) {
  localStorage.setItem(FAKE_COOKIE_KEY, user.id);
  return { accessToken: `token-de-prueba-${user.id}`, user: withoutPassword(user) };
}

async function fakeLogin(identifier, password) {
  await wait();
  // Se puede entrar con el correo o con el @usuario (con o sin la @).
  const clean = identifier.trim().toLowerCase().replace(/^@/, "");
  const user = getAllUsers().find((u) => u.email === clean || u.handle === clean);

  if (user && failedAttempts[user.id] >= MAX_FAILED_ATTEMPTS) {
    throw new ApiError(
      423,
      "Tu cuenta está bloqueada por demasiados intentos. Inténtalo de nuevo en 15 minutos.",
    );
  }

  if (!user || user.password !== password) {
    if (user) failedAttempts[user.id] = (failedAttempts[user.id] || 0) + 1;
    throw new ApiError(401, "Los datos no coinciden. Revisa tu correo o usuario y tu contraseña.");
  }

  failedAttempts[user.id] = 0;
  return startFakeSession(user);
}

async function fakeRegister({ displayName, handle, email, password, personas, termsAccepted }) {
  await wait();
  const users = getAllUsers();
  const cleanEmail = email.trim().toLowerCase();
  const cleanHandle = handle.trim().toLowerCase();

  // El backend valida por su cuenta aunque el formulario ya lo haya hecho.
  if (!termsAccepted) {
    throw new ApiError(422, "Debes aceptar los términos.", {
      termsAccepted: "Debes aceptar los términos.",
    });
  }

  // Como la base de datos (índices únicos), avisa si el correo o el @usuario ya existen.
  const errors = {};
  if (users.some((u) => u.email === cleanEmail)) {
    errors.email = "Ese correo ya está registrado. ¿Quieres iniciar sesión?";
  }
  if (users.some((u) => u.handle === cleanHandle) || RESERVED_HANDLES.includes(cleanHandle)) {
    errors.handle = "Ese usuario no está disponible. Prueba con otro.";
  }
  if (Object.keys(errors).length > 0) {
    throw new ApiError(409, "Algunos datos ya están en uso.", errors);
  }

  const newUser = {
    id: crypto.randomUUID(),
    handle: cleanHandle,
    displayName: displayName.trim(),
    email: cleanEmail,
    password,
    avatarUrl: null,
    personas,
  };
  const registered = JSON.parse(localStorage.getItem(REGISTERED_KEY) || "[]");
  localStorage.setItem(REGISTERED_KEY, JSON.stringify([...registered, newUser]));

  return startFakeSession(newUser);
}

function fakeLogout() {
  localStorage.removeItem(FAKE_COOKIE_KEY);
}

function fakeRefresh() {
  const userId = localStorage.getItem(FAKE_COOKIE_KEY);
  const user = getAllUsers().find((u) => u.id === userId);
  return user ? startFakeSession(user) : null;
}

// Algunos @usuarios que el backend reserva (tabla reserved_handles) porque chocan con rutas.
const RESERVED_HANDLES = ["login", "registro", "explorar", "feed", "admin", "api", "ajustes"];
