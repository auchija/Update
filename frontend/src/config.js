// Configuración general de la app. Único lugar donde vive el nombre del producto.

export const SITE_NAME = "UpDate";

export const SITE_DESCRIPTION =
  "La red de emprendedores: muestra tu proyecto, conecta con mentores e inversionistas y encuentra oportunidades para crecer.";

// Variables de entorno (archivo .env). En Vite deben empezar por VITE_ para llegar al navegador.
export const API_URL = import.meta.env.VITE_API_URL ?? "";

// true: los servicios usan los usuarios de prueba de src/data/users.js en lugar del backend.
export const USE_MOCKS = import.meta.env.VITE_USE_MOCKS !== "false";
