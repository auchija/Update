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
- **Servidor conectado a la base de datos.** El servidor tiene una dirección de prueba, `/api/health`, que sirve para comprobar que está encendido y que puede leer la base de datos.
- **Diseño de la base de datos.** El diagrama entidad-relación y el archivo que crea todas las tablas.

Todas las pantallas se ven bien en celular, tableta y computador. Tienen modo claro y oscuro, y se pueden usar solo con el teclado.

Por ahora el registro y el inicio de sesión funcionan con cuentas de prueba que se guardan en el navegador, porque el servidor todavía no tiene esa parte. Cuando la tenga, solo habrá que cambiar una opción para conectarlos.

Las publicaciones, los perfiles completos, el chat y los eventos quedan para las siguientes entregas.

## Dónde está cada punto que se pidió

- **Problema, alcance y arquitectura:** en este mismo documento, en las secciones [El problema](#el-problema), [Qué trae esta primera entrega](#qué-trae-esta-primera-entrega) y [Arquitectura](#arquitectura).
- **Diagrama entidad-relación:** en el archivo [`bases-de-datos/diagrama-er.svg`](bases-de-datos/diagrama-er.svg). Se explica en [La base de datos](#la-base-de-datos).
- **Proyecto base con la prueba `/health` conectada a la base de datos:** la página web está en la carpeta [`frontend`](frontend/) y el servidor en la carpeta [`backend`](backend/). Los pasos para probarlo están en [Cómo probarlo](#cómo-probarlo).
- **Página de inicio, registro e inicio de sesión que se adaptan a cualquier pantalla:** en la carpeta [`frontend/src/pages`](frontend/src/pages/).
- **Repositorio Git y README:** este repositorio y este documento.

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

Así funciona, por ejemplo, la dirección de prueba `/api/health`:

1. Alguien abre `http://localhost:4000/api/health` en el navegador.
2. El servidor recibe el pedido y le hace una pregunta sencilla a la base de datos: qué versión es y cuántas tablas tiene.
3. Si la base de datos responde, el servidor contesta `"status": "ok"` con esos datos. Si no responde en 5 segundos, contesta `"status": "error"`.

Cuando el servidor tenga el registro, el camino será el mismo: la página web revisa el formulario y se lo envía al servidor, el servidor vuelve a revisar los datos y los guarda en la base de datos, y luego le responde a la página web si la cuenta se creó o qué hay que corregir.

### Cómo está organizada cada parte por dentro

**Página web** (carpeta [`frontend`](frontend/)):

- **Pantallas** (`src/pages`): la página de inicio, el registro y el inicio de sesión. La página de inicio está dividida en secciones, una por archivo.
- **Piezas reutilizables** (`src/components`): botones, campos de formulario, tarjetas, la barra superior y el pie de página. Se hacen una vez y se usan en todas las pantallas.
- **Comunicación con el servidor** (`src/services`): es la única parte de la página web que le habla al servidor. Si algo cambia en el servidor, solo hay que ajustar aquí.
- **Reglas y ayudas** (`src/hooks` y `src/utils`): la lógica de los formularios y las reglas de validación, por ejemplo, que la contraseña tenga al menos 8 caracteres.
- **Estilos**: cada pieza tiene su propio archivo de estilos, y los colores y tamaños generales están en un solo lugar (`src/styles`).

**Servidor** (carpeta [`backend`](backend/)):

- **Conexión con la base de datos** (`UpDate.Api/Database`): cómo se conecta el servidor a PostgreSQL.
- **Chequeo de salud** (`UpDate.Api/Health`): la dirección `/api/health`.
- **Manejo de errores** (`UpDate.Api/Errors`): qué responder cuando algo falla o cuando se pide una dirección que no existe.
- **Configuración** (`UpDate.Api/Configuration`): qué páginas web tienen permiso para hablar con el servidor.
- **Pruebas automáticas** (`UpDate.Api.Tests`): revisan solas que el servidor responda bien cuando la base de datos funciona y cuando no.

**Base de datos** (carpeta [`bases-de-datos`](bases-de-datos/)): el diagrama y los archivos que la crean. Se explica en la siguiente sección.

### Dónde funciona cada parte

Por ahora todo funciona en un solo computador y nada está publicado en internet. Cada parte usa su propia dirección:

| Parte         | Dirección                  |
| ------------- | -------------------------- |
| Página web    | `http://localhost:5173`    |
| Servidor      | `http://localhost:4000`    |
| Base de datos | `localhost`, puerto `5432` |

### Lo que viene

La arquitectura ya está lista para crecer. En las próximas entregas el servidor tendrá el registro, el inicio de sesión, las publicaciones y el chat. Las fotos, videos y documentos que suban las personas se guardarán en un almacenamiento de archivos aparte, y también pasarán siempre por el servidor.

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
- [`update.sql`](bases-de-datos/update.sql): el archivo que crea la base de datos completa.
- [`update.dbml`](bases-de-datos/update.dbml): el mismo diseño en el formato de la página [dbdiagram.io](https://dbdiagram.io), donde se puede ver y editar el diagrama.

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

Después, abrir http://localhost:5173 en el navegador.

Para entrar sin crear una cuenta se puede usar el correo `demo@correo.com` (o el usuario `@valentina`) con la contraseña `Demo1234`.

### 2. Crear la base de datos

En pgAdmin:

1. Crear una base de datos nueva llamada `update`.
2. Sobre esa base, abrir la herramienta de consultas (_Query Tool_), abrir el archivo [`bases-de-datos/update.sql`](bases-de-datos/update.sql) y ejecutarlo. Así se crean las 56 tablas.

### 3. Encender el servidor

En otra terminal, dentro de la carpeta del proyecto, darle al servidor los datos para entrar a la base de datos. Hay que cambiar `tu-contraseña` por la contraseña que se eligió al instalar PostgreSQL:

```bash
cd backend
dotnet user-secrets set "ConnectionStrings:Database" "Host=localhost;Port=5432;Database=update;Username=postgres;Password=tu-contraseña" --project UpDate.Api
```

Si PostgreSQL quedó instalado en otro puerto (por ejemplo, `5433`), también se cambia el `5432`. La contraseña queda guardada solo en ese computador; no se sube al repositorio.

Después, encender el servidor:

```bash
dotnet run --project UpDate.Api
```

### 4. Comprobar que todo funciona

Abrir http://localhost:4000/api/health en el navegador.

- Si aparece `"status": "ok"` y `"tables": 56`, todo está bien: el servidor está encendido, se conectó a la base de datos y encontró las 56 tablas.
- Si aparece `"status": "error"`, el servidor no pudo conectarse: puede que la base de datos esté apagada o que la contraseña o el puerto estén mal.

El servidor también tiene pruebas automáticas que revisan esto solas. Se ejecutan desde la carpeta `backend` con:

```bash
dotnet test
```
