# UpDate

UpDate es una red social profesional pensada para emprendedores. Es nuestro proyecto final de Desarrollo Web en la Universidad Autónoma de Occidente (UAO), en Cali.

## El problema

Quien está empezando un emprendimiento suele tener una idea o un proyecto en marcha, pero le cuesta encontrar a las personas que lo pueden hacer crecer: un mentor que ya pasó por lo mismo, un inversionista, un proveedor, alguien con quien asociarse o simplemente sus primeros clientes. Las redes profesionales que existen hoy giran alrededor de la hoja de vida y la búsqueda de empleo, no alrededor de lo que la gente está construyendo. Por eso, el emprendedor termina repartiendo su proyecto entre varias plataformas y grupos de chat, y los contactos valiosos se pierden.

## Nuestra propuesta

En UpDate el centro no es el currículum sino el proyecto. Cada persona crea su perfil, muestra el proyecto en el que trabaja y se conecta con otros según el papel que cumplen: emprendedor, comprador, mentor, inversionista, proveedor o colaborador. Una misma persona puede tener varios de estos papeles a la vez. De esas conexiones salen oportunidades (una inversión, una alianza, una venta) y, más adelante, eventos y comunidades donde encontrarse.

El recorrido que queremos que viva cada usuario es sencillo: un emprendedor muestra su proyecto, hace conexiones, encuentra oportunidades y participa en eventos.

Tomamos ideas de varias plataformas, pero UpDate no es una copia de ninguna. De LinkedIn tomamos los perfiles y las conexiones; de Wellfound e Indie Hackers, el protagonismo de los proyectos; y de Meetup, la idea de comunidades y eventos, que por ahora dejamos como propuesta a futuro.

## Alcance de esta primera entrega

En este primer avance construimos la base del proyecto. Del lado del frontend ya están la página de inicio (landing), que explica qué es UpDate, cómo funciona y para quién es, y las pantallas de registro e inicio de sesión con validación de formularios. Al registrarse, la persona escribe su nombre, elige un nombre de usuario único (su @handle), su correo y una contraseña, marca uno o varios de los papeles mencionados y acepta los términos. Para iniciar sesión puede usar el correo o el @handle.

Todas las vistas se adaptan a celular, tableta y computador. Tienen modo claro y oscuro y se pueden usar por completo con el teclado.

Como el backend todavía se está construyendo, el registro y el inicio de sesión funcionan por ahora con usuarios de prueba que simulan las respuestas del servidor. Cuando la API esté lista, bastará con cambiar una variable de entorno para conectarlos de verdad.

El feed, los perfiles completos, el chat y los eventos quedan para las siguientes entregas.

## Arquitectura

El proyecto está dividido en dos partes que viven en este mismo repositorio.

El **frontend** es una aplicación de React hecha con Vite y se publica en Vercel. Usa React Router para moverse entre páginas y CSS propio (con CSS Modules) para los estilos, sin librerías de diseño. Nunca se comunica directamente con la base de datos: todo lo pide al backend a través de un único archivo de servicios.

El **backend** es una API que se publica en Railway y está a cargo del otro equipo. Es el único que habla con la base de datos, una PostgreSQL alojada en Supabase. Las imágenes, los videos y los documentos que suban los usuarios se guardarán en Cloudflare R2, siempre pasando por el backend.

Para la sesión, el backend entrega dos llaves. La primera es un token de acceso que dura quince minutos y que el frontend guarda solo en memoria, nunca en el navegador. La segunda es un token de renovación que dura treinta días y viaja en una cookie protegida a la que el código del navegador no puede acceder. Cuando el token de acceso vence, el frontend pide uno nuevo sin que el usuario tenga que volver a iniciar sesión.

El diseño de la base de datos (diagrama entidad-relación) se entrega en un documento aparte.

## Cómo está organizado el repositorio

En la carpeta `frontend` está la aplicación web y en la carpeta `backend` está la API. Cada una tiene su propio README con los detalles técnicos.

## Cómo probar el frontend en tu computador

Necesitas tener instalado Node.js 22 o una versión más reciente. Desde una terminal, entra a la carpeta del frontend, instala las dependencias, crea tu archivo de configuración a partir del ejemplo y arranca el servidor de desarrollo:


La aplicación queda disponible en http://localhost:5173. Para entrar sin crear una cuenta puedes usar el correo `demo@correo.com` (o el usuario `@valentina`) con la contraseña `Demo1234`.
