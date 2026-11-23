// Mismas reglas que el backend: el front avisa antes de enviar
// y el backend vuelve a validar (nunca confiar solo en el front).
export const MAX_FILE_SIZE_MB = 5;
export const MAX_FILE_SIZE_BYTES = MAX_FILE_SIZE_MB * 1024 * 1024;

const ALLOWED_TYPES = {
  "image/jpeg": ["jpg", "jpeg"],
  "image/png": ["png"],
  "image/webp": ["webp"],
  "application/pdf": ["pdf"],
};

export const FILE_ACCEPT = ".jpg,.jpeg,.png,.webp,.pdf";

/** Devuelve el mensaje de error o "" si el archivo es válido. */
export function validateFile(file) {
  if (!file) return "";

  const extension = getExtension(file.name);
  const allowedExtensions = ALLOWED_TYPES[file.type];
  if (!allowedExtensions || !allowedExtensions.includes(extension)) {
    return "Solo se permiten archivos JPG, PNG, WEBP o PDF.";
  }
  if (file.size === 0) {
    return "El archivo está vacío.";
  }
  if (file.size > MAX_FILE_SIZE_BYTES) {
    return `El archivo pesa ${formatFileSize(file.size)}. El máximo es ${MAX_FILE_SIZE_MB} MB.`;
  }
  return "";
}

export function isImage(mimeType) {
  return typeof mimeType === "string" && mimeType.startsWith("image/");
}

export function formatFileSize(bytes) {
  if (bytes < 1024) return `${bytes} B`;
  if (bytes < 1024 * 1024) return `${(bytes / 1024).toFixed(0)} KB`;
  return `${(bytes / (1024 * 1024)).toFixed(1)} MB`;
}

function getExtension(fileName) {
  const parts = fileName.toLowerCase().split(".");
  return parts.length > 1 ? parts.pop() : "";
}