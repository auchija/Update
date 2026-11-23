export const SITE_NAME = "UpDate";

export const SITE_DESCRIPTION =
  "La red de emprendedores: muestra tu proyecto, conecta con mentores e inversionistas y encuentra oportunidades para crecer.";

// En local, .env apunta a la API (http://localhost:5195/api). En producción no hace falta:
// Vercel reenvía /api a Render (frontend/vercel.json), así la cookie de sesión es del mismo dominio.
export const API_URL = import.meta.env.VITE_API_URL ?? "/api";
