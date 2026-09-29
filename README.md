# UpDate

Red social profesional para emprendedores. Combina perfiles y conexiones con el foco en los **proyectos** que la gente construye: emprendedor → proyecto → conexiones → oportunidades → eventos.

Proyecto final de Desarrollo Web — Universidad Autónoma de Occidente (UAO), Cali.

## Avance 1

- **Landing** pública: qué es UpDate, cómo funciona y para quién es.
- **Registro** e **inicio de sesión** con validación de formularios (por ahora con usuarios de prueba, sin backend).
- Modo claro, oscuro y del sistema.
- Diseño responsive (móvil, tablet y escritorio) y accesible (WCAG 2.1 AA).

## Estructura del repositorio

```
/
├── frontend/   Aplicación web (React + Vite) — se publica en Vercel
└── backend/    API (equipo de backend) — se publica en Railway
```

## Cómo ejecutar el frontend

Necesitas Node.js 22 o superior.

```bash
cd frontend
npm install
cp .env.example .env   # solo la primera vez
npm run dev            # abre http://localhost:5173
```

Cuenta de prueba: `demo@correo.com` (o el usuario `@valentina`) con la contraseña `Demo1234`.

Más detalles (scripts y estructura de carpetas) en [`frontend/README.md`](frontend/README.md).

## Tecnologías del frontend

- React 19 + Vite
- React Router (páginas `/`, `/login` y `/registro`)
- CSS propio con CSS Modules (sin frameworks de estilos)
- lucide-react (iconos)

> El nombre del producto se define en un solo lugar: `frontend/src/config.js` (`SITE_NAME`).
