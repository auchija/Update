## Integrantes
Miguel Angel Jimenez Londoño
Sebastian Montaño Reyes
Mariana Aristizabal Truque
Alejandro Gonzalez Romo
David Elias Castro
# UpDate

UpDate es una red social para emprendedores. Es nuestro proyecto final de Desarrollo Web en la Universidad Autónoma de Occidente (UAO), en Cali.

## El problema

Cuando alguien empieza un emprendimiento, casi siempre tiene la idea, pero le faltan las personas que lo pueden ayudar a crecer: un mentor que ya pasó por lo mismo, alguien que quiera invertir, un proveedor, un socio o simplemente sus primeros clientes.

Las redes profesionales que existen hoy están pensadas para buscar empleo y mostrar la hoja de vida, no para mostrar lo que uno está construyendo. Por eso el emprendedor termina repartiendo su proyecto entre varias páginas y grupos de chat, y muchos contactos valiosos se pierden.

## Nuestra propuesta

En UpDate lo importante no es la hoja de vida, sino el proyecto. Cada persona crea su perfil, muestra el proyecto en el que trabaja y se conecta con otras personas según el papel que cumple: emprendedor, comprador, mentor, inversionista, proveedor o colaborador. Una misma persona puede tener varios papeles a la vez.

De esas conexiones salen oportunidades: una inversión, una alianza o una venta. Más adelante también habrá eventos y comunidades para encontrarse.

La idea es que cada persona haga este recorrido: **muestra su proyecto, se conecta con otros, encuentra oportunidades y participa en eventos.**

Nos inspiramos en varias plataformas, pero UpDate no copia ninguna. De LinkedIn tomamos los perfiles y las conexiones; de Wellfound e Indie Hackers, que los proyectos sean lo principal; y de Meetup, la idea de comunidades y eventos, que por ahora dejamos para más adelante.

## Qué trae esta primera entrega

Esta entrega es la base del proyecto. Sobre ella vamos a construir el resto de la red social.

- **Página de inicio.** Explica qué es UpDate, cómo funciona en cinco pasos y qué gana cada tipo de persona al unirse.
- **Registro.** Para crear una cuenta, la persona escribe su nombre, elige un nombre de usuario (por ejemplo, `@valentina`), pone su correo y una contraseña, marca uno o varios papeles y acepta los términos. Si algo está mal, el formulario le dice qué debe corregir.
- **Inicio de sesión.** Se puede entrar con el correo o con el nombre de usuario.
- **Servidor conectado a la base de datos.** El servidor tiene una dirección de prueba, `/health`, que sirve para comprobar que está encendido y que puede leer la base de datos.
- **Diseño de la base de datos.** El diagrama entidad-relación y su diseño en formato DBML.

Todas las pantallas se ven bien en celular, tableta y computador. Tienen modo claro y oscuro, y se pueden usar solo con el teclado.

En esta primera entrega el registro y el inicio de sesión funcionaban con cuentas de prueba guardadas en el navegador. Desde la segunda entrega ya pasan por el servidor y la base de datos.

## Qué trae la segunda entrega

- **Registro e inicio de sesión de verdad.** Las cuentas se guardan en la base de datos y la contraseña nunca se guarda tal cual, sino cifrada (como un _hash_). Se puede entrar con el correo o con `@usuario`. Si alguien se equivoca 5 veces seguidas con la contraseña, la cuenta se bloquea 15 minutos.
- **Páginas públicas y privadas.** La página de inicio, el registro y el inicio de sesión son públicas. El feed y el perfil son privados: si alguien escribe su dirección sin haber iniciado sesión, lo mandamos al inicio de sesión. La sesión se mantiene al recargar la página y se cierra con el botón **Cerrar sesión**.
- **Mi perfil.** Cada persona puede ver y editar su nombre, su usuario, un titular, su biografía y su sitio web.
- **Foto de perfil.** La foto se sube al servidor, que revisa que de verdad sea una imagen JPG, PNG o WEBP de máximo 5 MB, la guarda en disco con un nombre único y anota en la base de datos quién la subió, cuánto pesa y su tipo.
- **Publicaciones.** En el feed cada persona puede publicar texto con una imagen opcional, ver lo que publican los demás (de lo más nuevo a lo más viejo) y editar o eliminar solo lo suyo. La imagen sigue el mismo camino que la foto de perfil: se guarda en disco y sus datos en la base de datos, enlazada a la publicación.
- **Mensajes claros cuando algo falla.** Mientras se espera al servidor se muestra que está cargando. Si hay un error, el formulario dice qué campo hay que corregir o qué pasó (sin conexión, cuenta bloqueada, usuario ocupado…).

El chat y los eventos quedan para las siguientes entregas.

## Dónde está cada punto que se pidió

- **Problema, alcance y arquitectura:** en este mismo documento, en las secciones [El problema](#el-problema), [Qué trae esta primera entrega](#qué-trae-esta-primera-entrega) y [Arquitectura](#arquitectura).
- **Diagrama entidad-relación:** en el archivo [`bases-de-datos/diagrama-er.svg`](bases-de-datos/diagrama-er.svg). Se explica en [La base de datos](#la-base-de-datos).
- **Proyecto base con la prueba `/health` conectada a la base de datos:** la página web está en la carpeta [`frontend`](frontend/) y el servidor en la carpeta [`backend`](backend/). Los pasos para probarlo están en [Cómo probarlo](#cómo-probarlo).
- **Página de inicio, registro e inicio de sesión que se adaptan a cualquier pantalla:** en la carpeta [`frontend/src/pages`](frontend/src/pages/).
- **Repositorio Git y README:** este repositorio y este documento.

Segunda entrega:

- **Login y registro conectados a la base de datos:** [`AuthController.cs`](backend/update.API/Controllers/AuthController.cs) en el servidor y [`authService.js`](frontend/src/services/authService.js) en la página web.
- **Rutas públicas y privadas:** [`PrivateRoute.jsx`](frontend/src/routes/PrivateRoute.jsx), [`PublicOnlyRoute.jsx`](frontend/src/routes/PublicOnlyRoute.jsx) y [`App.jsx`](frontend/src/App.jsx).
- **CRUD de la API usado desde la página web:** las publicaciones (`GET`, `POST`, `PUT` y `DELETE` en `/api/feed`) en [`FeedController.cs`](backend/update.API/Controllers/FeedController.cs) y [`FeedPage.jsx`](frontend/src/pages/app/FeedPage.jsx), y el perfil (`GET` y `PUT /api/perfil/yo`) desde [`perfilService.js`](frontend/src/services/perfilService.js). El manejo de carga y errores está en [`api.js`](frontend/src/services/api.js). Todos los endpoints están documentados en [`backend/ENDPOINTS.md`](backend/ENDPOINTS.md).
- **Subida de archivos (`multipart/form-data`):** `POST /api/archivos/subir` en [`SubidaArchivosController.cs`](backend/update.API/Controllers/SubidaArchivosController.cs).

## Arquitectura

La arquitectura es la forma en que está organizado el sistema: qué partes tiene, qué hace cada una y cómo se comunican entre sí.

### Las tres partes

UpDate está dividido en tres partes independientes. Cada una tiene una sola responsabilidad:

```
   ┌──────────────────────────┐
   │        Página web        │   Lo que la persona ve y usa en su navegador
   │   (frontend, en React)   │
   └────────────┬─────────────┘
                │  le pide información al servidor
                │  y el servidor le responde
   ┌────────────▼─────────────┐
   │         Servidor         │   Recibe los pedidos, revisa que tengan
   │  (backend, en .NET / C#) │   sentido y decide qué se puede hacer
   └────────────┬─────────────┘
                │  lee y guarda información
   ┌────────────▼─────────────┐
   │      Base de datos       │   Guarda toda la información de la
   │       (PostgreSQL)       │   red social de forma ordenada
   └──────────────────────────┘
```

1. **La página web (frontend)** es lo que la persona ve: la página de inicio, el registro y el inicio de sesión. Está hecha con React, una herramienta de JavaScript para construir páginas interactivas. Se encarga de mostrar la información y de revisar los formularios antes de enviarlos, por ejemplo, que el correo esté bien escrito.
2. **El servidor (backend)** es el intermediario. Está hecho con .NET, en el lenguaje C#. Recibe lo que pide la página web, revisa que el pedido sea válido y es la única parte que puede leer o guardar información en la base de datos.
3. **La base de datos** guarda toda la información: personas, emprendimientos, publicaciones, mensajes y demás. Usamos PostgreSQL, una base de datos relacional, es decir, que guarda la información en tablas relacionadas entre sí.

### Por qué está separada así

- **Seguridad:** la página web nunca toca la base de datos. Todo pasa por el servidor, que revisa cada pedido, así nadie puede leer o cambiar información sin permiso.
- **Orden:** cada parte se puede cambiar sin romper las otras. Por ejemplo, podríamos rediseñar la página web sin tocar el servidor.
- **Trabajo en equipo:** una persona puede trabajar en la página web mientras otra trabaja en el servidor.

### Cómo viaja un pedido

Así funciona, por ejemplo, la dirección de prueba `/health`:

1. Alguien abre `http://localhost:5195/health` en el navegador.
2. El servidor recibe el pedido y le hace una pregunta sencilla a la base de datos para saber si responde.
3. Si la base de datos responde, el servidor contesta que todo está bien. Si no, contesta con un error.

El registro sigue el mismo camino: la página web revisa el formulario y se lo envía al servidor, el servidor vuelve a revisar los datos y los guarda en la base de datos, y luego le responde a la página web si la cuenta se creó o qué campo hay que corregir.

### Cómo está organizada cada parte por dentro

**Página web** (carpeta [`frontend`](frontend/)):

- **Pantallas** (`src/pages`): la página de inicio, el registro y el inicio de sesión. La página de inicio está dividida en secciones, una por archivo.
- **Piezas reutilizables** (`src/components`): botones, campos de formulario, tarjetas, la barra superior y el pie de página. Se hacen una vez y se usan en todas las pantallas.
- **Comunicación con el servidor** (`src/services`): es la única parte de la página web que le habla al servidor. Si algo cambia en el servidor, solo hay que ajustar aquí.
- **Reglas y ayudas** (`src/hooks` y `src/utils`): la lógica de los formularios y las reglas de validación, por ejemplo, que la contraseña tenga al menos 8 caracteres.
- **Estilos**: cada pieza tiene su propio archivo de estilos, y los colores y tamaños generales están en un solo lugar (`src/styles`).

**Servidor** (carpeta [`backend`](backend/)):

El servidor está dividido en capas. Cada capa solo conoce a la de abajo:

- **API** (`update.API`): recibe los pedidos de la página web. Aquí están los controladores (`Controllers`), el chequeo de salud `/health` y el manejo de errores (`Errors`), que decide qué responder cuando algo falla.
- **Aplicación** (`update.Application`): las reglas de cada caso de uso, por ejemplo cómo se registra una persona o cómo se sube una foto (`Services`), y la forma de los datos que entran y salen (`DTOs`).
- **Dominio** (`update.Domain`): las entidades del negocio (usuario, perfil, publicación…) con sus reglas básicas.
- **Infraestructura** (`update.Infrastructure`): cómo se guarda todo en PostgreSQL, con Entity Framework y las migraciones.

La lista completa de direcciones del servidor está en [`backend/ENDPOINTS.md`](backend/ENDPOINTS.md).

**Base de datos** (carpeta [`bases-de-datos`](bases-de-datos/)): el diagrama y su diseño. Se explica en la siguiente sección.

### Dónde funciona cada parte

Por ahora todo funciona en un solo computador y nada está publicado en internet. Cada parte usa su propia dirección:

| Parte         | Dirección                  |
| ------------- | -------------------------- |
| Página web    | `http://localhost:5173`    |
| Servidor      | `http://localhost:5195`    |
| Base de datos | PostgreSQL (local, puerto `5432`, o en Supabase) |

Las fotos que suben las personas se guardan en la carpeta `backend/update.API/uploads`, que no se sube al repositorio.

### Lo que viene

La arquitectura ya está lista para crecer. En las próximas entregas llegarán los comentarios y reacciones en las publicaciones, el chat y los eventos. Los videos y documentos seguirán el mismo camino que las fotos: pasarán siempre por el servidor, que guarda el archivo y anota sus datos en la base de datos.

## La base de datos

Todo en UpDate gira alrededor del **perfil**. Un perfil puede ser de una persona o de un emprendimiento, y lo demás se organiza a su alrededor:

- **Personas y emprendimientos:** quién es cada uno, qué papeles tiene y quiénes forman el equipo de cada emprendimiento.
- **Conexiones:** a quién sigue cada persona y a quién bloqueó.
- **Publicaciones:** lo que la gente comparte, con comentarios, reacciones y etiquetas.
- **Productos y servicios** que ofrece cada emprendimiento, con sus reseñas.
- **Oportunidades y mentorías:** convocatorias para buscar socios o inversión, y mentores que ofrecen acompañamiento.
- **Cotizaciones:** un comprador pide un precio y el emprendimiento le responde.
- **Mensajes** entre perfiles, de dos personas o en grupo.
- **Notificaciones y reportes:** avisos de novedades y denuncias de contenido inapropiado.

En total son 56 tablas. Los archivos están en la carpeta [`bases-de-datos`](bases-de-datos/):

- [`diagrama-er.svg`](bases-de-datos/diagrama-er.svg): el diagrama entidad-relación, con todas las tablas y cómo se relacionan.
- [`update.dbml`](bases-de-datos/update.dbml): el mismo diseño en el formato de la página [dbdiagram.io](https://dbdiagram.io), donde se puede ver y editar el diagrama.

Las tablas se crean con las migraciones del servidor (carpeta `backend/update.Infrastructure/Migrations`), como se explica en [Cómo probarlo](#cómo-probarlo).

## Cómo probarlo

Hay que tener instalados estos tres programas:

- [Node.js](https://nodejs.org) (versión 22 o más nueva), para la página web.
- [.NET](https://dotnet.microsoft.com/download) (versión 10), para el servidor.
- [PostgreSQL](https://www.postgresql.org/download/) (versión 13 o más nueva), para la base de datos. Trae pgAdmin, un programa para manejar la base de datos con ventanas.

### 1. Abrir la página web

En una terminal, dentro de la carpeta del proyecto, escribir:

```bash
cd frontend
npm install
cp .env.example .env
npm run dev
```

Después, abrir http://localhost:5173 en el navegador. La página necesita que el servidor esté encendido (pasos 2 y 3).

### 2. Darle al servidor sus datos privados

En otra terminal, dentro de la carpeta del proyecto, guardar la dirección de la base de datos y una clave para firmar las sesiones. Hay que cambiar `tu-contraseña` por la contraseña de PostgreSQL (o usar la cadena de conexión de Supabase):

```bash
cd backend
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5432;Database=update;Username=postgres;Password=tu-contraseña" --project update.API
dotnet user-secrets set "Jwt:Clave" "una-clave-larga-de-al-menos-32-caracteres-123456" --project update.API
```

Estos datos quedan guardados solo en ese computador; no se suben al repositorio.

### 3. Crear las tablas y encender el servidor

Desde la carpeta `backend`:

```bash
dotnet tool restore
dotnet ef database update --project update.Infrastructure --startup-project update.API
dotnet run --project update.API --launch-profile http
```

El segundo comando crea las tablas con las migraciones del proyecto. Si la base de Supabase del equipo ya las tiene, no hace nada y se puede saltar.

### 4. Comprobar que todo funciona

Abrir http://localhost:5195/health en el navegador.

- Si dice que todo está bien, el servidor está encendido y se conectó a la base de datos.
- Si aparece un error, el servidor no pudo conectarse: puede que la base de datos esté apagada o que la contraseña o el puerto estén mal.

En http://localhost:5195/swagger se pueden ver y probar todos los endpoints. La base de Supabase del equipo es de prueba, así que se puede usar para registrarse, iniciar sesión y subir fotos sin miedo.
