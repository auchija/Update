import { TEST_USERS } from "../data/users.js";
import { ApiError } from "./api.js";

const REGISTERED_USERS_STORAGE_KEY = "test-registered-users";
// Imita la cookie httpOnly de renovación que usará el backend real.
const MOCK_REFRESH_SESSION_KEY = "mock-refresh-session";
const MAX_FAILED_ATTEMPTS = 5;
const MIN_DELAY_MS = 500;
const MAX_EXTRA_DELAY_MS = 700;
const RESERVED_HANDLES = ["login", "registro", "explorar", "feed", "admin", "api", "ajustes"];

const failedAttemptsByUserId = {};

export async function mockLogin(identifier, password) {
  await simulateNetworkDelay();

  const cleanIdentifier = identifier.trim().toLowerCase().replace(/^@/, "");
  const user = getAllUsers().find(
    (candidate) => candidate.email === cleanIdentifier || candidate.handle === cleanIdentifier,
  );

  if (user && failedAttemptsByUserId[user.id] >= MAX_FAILED_ATTEMPTS) {
    throw new ApiError(
      423,
      "Tu cuenta está bloqueada por demasiados intentos. Inténtalo de nuevo en 15 minutos.",
    );
  }

  if (!user || user.password !== password) {
    if (user) {
      failedAttemptsByUserId[user.id] = (failedAttemptsByUserId[user.id] ?? 0) + 1;
    }
    throw new ApiError(401, "Los datos no coinciden. Revisa tu correo o usuario y tu contraseña.");
  }

  failedAttemptsByUserId[user.id] = 0;
  return startSession(user);
}

export async function mockRegister({
  displayName,
  handle,
  email,
  password,
  personas,
  termsAccepted,
}) {
  await simulateNetworkDelay();

  if (!termsAccepted) {
    throw new ApiError(422, "Debes aceptar los términos.", {
      termsAccepted: "Debes aceptar los términos.",
    });
  }

  const cleanEmail = email.trim().toLowerCase();
  const cleanHandle = handle.trim().toLowerCase();

  const takenFieldErrors = findTakenFields(cleanEmail, cleanHandle);
  if (Object.keys(takenFieldErrors).length > 0) {
    throw new ApiError(409, "Algunos datos ya están en uso.", takenFieldErrors);
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
  saveRegisteredUser(newUser);

  return startSession(newUser);
}

export async function mockRefresh() {
  await simulateNetworkDelay();

  const userId = readStorage(MOCK_REFRESH_SESSION_KEY);
  const user = userId && getAllUsers().find((candidate) => candidate.id === userId);

  if (!user) {
    removeStorage(MOCK_REFRESH_SESSION_KEY);
    throw new ApiError(401, "No hay una sesión activa.");
  }

  return createSessionResponse(user);
}

export async function mockLogout() {
  await simulateNetworkDelay();
  removeStorage(MOCK_REFRESH_SESSION_KEY);
}

function startSession(user) {
  writeStorage(MOCK_REFRESH_SESSION_KEY, user.id);
  return createSessionResponse(user);
}

function createSessionResponse(user) {
  const { password: _password, ...publicUser } = user;
  return { accessToken: `token-de-prueba-${user.id}`, user: publicUser };
}

function findTakenFields(email, handle) {
  const users = getAllUsers();
  const errors = {};

  if (users.some((user) => user.email === email)) {
    errors.email = "Ese correo ya está registrado. ¿Quieres iniciar sesión?";
  }
  if (users.some((user) => user.handle === handle) || RESERVED_HANDLES.includes(handle)) {
    errors.handle = "Ese usuario no está disponible. Prueba con otro.";
  }

  return errors;
}

function getAllUsers() {
  return [...TEST_USERS, ...readRegisteredUsers()];
}

function readRegisteredUsers() {
  try {
    return JSON.parse(readStorage(REGISTERED_USERS_STORAGE_KEY) ?? "[]");
  } catch {
    return [];
  }
}

function saveRegisteredUser(user) {
  const users = [...readRegisteredUsers(), user];
  writeStorage(REGISTERED_USERS_STORAGE_KEY, JSON.stringify(users));
}

// localStorage puede fallar (modo privado, almacenamiento bloqueado):
// en ese caso el mock sigue funcionando, solo que sin recordar la sesión.
function readStorage(key) {
  try {
    return localStorage.getItem(key);
  } catch {
    return null;
  }
}

function writeStorage(key, value) {
  try {
    localStorage.setItem(key, value);
  } catch {
    // Sin almacenamiento disponible: se ignora.
  }
}

function removeStorage(key) {
  try {
    localStorage.removeItem(key);
  } catch {
    // Sin almacenamiento disponible: se ignora.
  }
}

function simulateNetworkDelay() {
  const delay = MIN_DELAY_MS + Math.random() * MAX_EXTRA_DELAY_MS;
  return new Promise((resolve) => setTimeout(resolve, delay));
}
