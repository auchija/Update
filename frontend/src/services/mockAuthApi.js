import { TEST_USERS } from "../data/users.js";
import { ApiError } from "./api.js";

const REGISTERED_USERS_STORAGE_KEY = "test-registered-users";
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
  return createSession(user);
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

  return createSession(newUser);
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
  return JSON.parse(localStorage.getItem(REGISTERED_USERS_STORAGE_KEY) ?? "[]");
}

function saveRegisteredUser(user) {
  const users = [...readRegisteredUsers(), user];
  localStorage.setItem(REGISTERED_USERS_STORAGE_KEY, JSON.stringify(users));
}

function createSession(user) {
  const { password: _password, ...publicUser } = user;
  return { accessToken: `token-de-prueba-${user.id}`, user: publicUser };
}

function simulateNetworkDelay() {
  const delay = MIN_DELAY_MS + Math.random() * MAX_EXTRA_DELAY_MS;
  return new Promise((resolve) => setTimeout(resolve, delay));
}
