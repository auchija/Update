export const POST_MAX_LENGTH = 5000;

export function validatePost({ contenido }) {
  const errors = {};
  const length = contenido.trim().length;

  if (length === 0) {
    errors.contenido = "Escribe algo antes de publicar.";
  } else if (length > POST_MAX_LENGTH) {
    errors.contenido = `Usa máximo ${POST_MAX_LENGTH} caracteres.`;
  }

  return errors;
}
