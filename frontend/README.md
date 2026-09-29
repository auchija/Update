# Frontend — UpDate

Aplicación web hecha con **React + Vite**. Estilos con CSS propio: variables y utilidades en `src/styles/global.css` y un CSS Module por componente.

## Cómo ejecutarlo

Necesitas Node.js 22 o superior.

```bash
cd frontend
npm install
cp .env.example .env   # solo la primera vez
npm run dev            # abre http://localhost:5173
```

## Scripts

| Comando           | Qué hace                                      |
| ----------------- | --------------------------------------------- |
| `npm run dev`     | Servidor de desarrollo con recarga automática |
| `npm run build`   | Genera la versión para publicar en `dist/`    |
| `npm run preview` | Sirve la versión de `dist/` para revisarla    |
| `npm run lint`    | Revisa el código con Oxlint                   |
| `npm run format`  | Da formato al código con Prettier             |

## Estructura

```
src/
├── main.jsx          punto de entrada: monta la app en index.html
├── App.jsx           rutas: qué página se muestra en cada URL
├── config.js         nombre del producto y variables de entorno
├── styles/           global.css: colores, fuentes, reset y utilidades
├── pages/            Landing, Login y Registro (cada una con su .module.css)
├── components/       piezas reutilizables: Button, Input, Card, Header...
├── services/         api.js (peticiones), session.js (sesión en memoria), authService.js
└── data/             usuarios de prueba y lista de personas (la usan registro y landing)
```

## Datos de prueba

Con `VITE_USE_MOCKS=true` (valor por defecto) no hace falta backend. Cuenta de prueba:

- Correo: `demo@correo.com` o usuario: `@valentina`
- Contraseña: `Demo1234`
