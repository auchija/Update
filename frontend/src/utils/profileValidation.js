const HANDLE_PATTERN = /^[a-z0-9._]{3,30}$/;
const DISPLAY_NAME_MIN_LENGTH = 3;
const DISPLAY_NAME_MAX_LENGTH = 60;
const HEADLINE_MAX_LENGTH = 255;
export const BIO_MAX_LENGTH = 2000;
export const PHOTO_MAX_BYTES = 5 * 1024 * 1024;
export const PHOTO_ACCEPT = ".jpg,.jpeg,.png,.webp";
const PHOTO_TYPES = ["image/jpeg", "image/png", "image/webp"];

export function validateProfile({ nombreUsuario, nombreVisible, titular, biografia, sitioWeb }) {
  const errors = {};

  const nameLength = nombreVisible.trim().length;
  if (nameLength < DISPLAY_NAME_MIN_LENGTH || nameLength > DISPLAY_NAME_MAX_LENGTH) {
    errors.nombreVisible = `Escribe tu nombre (entre ${DISPLAY_NAME_MIN_LENGTH} y ${DISPLAY_NAME_MAX_LENGTH} caracteres).`;
  }
  if (!HANDLE_PATTERN.test(nombreUsuario)) {
    errors.nombreUsuario =
      "Usa de 3 a 30 caracteres: minúsculas, números, punto (.) o guion bajo (_).";
  }
  if (titular.length > HEADLINE_MAX_LENGTH) {
    errors.titular = `Usa máximo ${HEADLINE_MAX_LENGTH} caracteres.`;
  }
  if (biografia.length > BIO_MAX_LENGTH) {
    errors.biografia = `Usa máximo ${BIO_MAX_LENGTH} caracteres.`;
  }
  if (sitioWeb.trim() && !isHttpUrl(sitioWeb.trim())) {
    errors.sitioWeb = "Escribe una dirección completa, por ejemplo https://miempresa.com.";
  }

  return errors;
}

export function validatePhoto(file) {
  if (!PHOTO_TYPES.includes(file.type)) return "Elige una imagen JPG, PNG o WEBP.";
  if (file.size > PHOTO_MAX_BYTES) return "La foto no puede pesar más de 5 MB.";
  return "";
}

function isHttpUrl(text) {
  try {
    const { protocol } = new URL(text);
    return protocol === "http:" || protocol === "https:";
  } catch {
    return false;
  }
}
