const EMAIL_PATTERN = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
const HANDLE_PATTERN = /^[a-z0-9._]{3,30}$/;
const DISPLAY_NAME_MIN_LENGTH = 3;
const DISPLAY_NAME_MAX_LENGTH = 60;
const PASSWORD_MIN_LENGTH = 8;

export function normalizeHandle(text) {
  return text
    .toLowerCase()
    .normalize("NFD")
    .replace(/\p{Mark}/gu, "")
    .replace(/[^a-z0-9._]/g, "");
}

export function validateLogin({ identifier, password }) {
  const errors = {};

  if (!identifier.trim()) {
    errors.identifier = "Escribe tu correo o tu nombre de usuario.";
  }
  if (!password) {
    errors.password = "Escribe tu contraseña.";
  }

  return errors;
}

export function validateRegistration({
  displayName,
  handle,
  email,
  password,
  personas,
  termsAccepted,
}) {
  const errors = {};

  const nameLength = displayName.trim().length;
  if (nameLength < DISPLAY_NAME_MIN_LENGTH || nameLength > DISPLAY_NAME_MAX_LENGTH) {
    errors.displayName = `Escribe tu nombre completo (entre ${DISPLAY_NAME_MIN_LENGTH} y ${DISPLAY_NAME_MAX_LENGTH} caracteres).`;
  }

  if (!HANDLE_PATTERN.test(handle)) {
    errors.handle = "Usa de 3 a 30 caracteres: minúsculas, números, punto (.) o guion bajo (_).";
  }

  if (!EMAIL_PATTERN.test(email.trim())) {
    errors.email = "Escribe un correo válido, por ejemplo nombre@dominio.com.";
  }

  const passwordError = validatePassword(password);
  if (passwordError) {
    errors.password = passwordError;
  }

  if (personas.length === 0) {
    errors.personas = "Elige al menos una opción.";
  }

  if (!termsAccepted) {
    errors.termsAccepted = "Debes aceptar los términos para crear tu cuenta.";
  }

  return errors;
}

function validatePassword(password) {
  if (password.length < PASSWORD_MIN_LENGTH) {
    return `Usa al menos ${PASSWORD_MIN_LENGTH} caracteres.`;
  }
  if (!/[a-zA-Z]/.test(password) || !/[0-9]/.test(password)) {
    return "Incluye al menos una letra y un número.";
  }
  return null;
}
