# Backend de UpDate

API REST en .NET 10 (C#) con PostgreSQL y Entity Framework Core. Los pasos para encenderla junto con la página web están en el [README principal](../README.md#cómo-probarlo). La lista de endpoints está en [ENDPOINTS.md](ENDPOINTS.md).

## Capas

| Proyecto | Qué contiene |
|---|---|
| `update.API` | Controladores, configuración (JWT, CORS, Swagger, almacenamiento), chequeo de salud y manejo central de errores. |
| `update.Application` | Servicios de cada caso de uso (autenticación, mi perfil, feed, emprendimientos, subida de archivos, CRUD genérico) y los DTO que entran y salen. |
| `update.Domain` | Entidades, enums, reglas de validación e interfaces de los repositorios. |
| `update.Infrastructure` | `ApplicationDbContext`, configuración de cada tabla, repositorios, migraciones y el cliente de Supabase Storage. |

## Configuración

Se guarda con `user-secrets` en `update.API` (o con variables de entorno, cambiando `:` por `__`). Ningún valor real va en `appsettings.json`:

| Clave | Para qué |
|---|---|
| `ConnectionStrings:DefaultConnection` | Conexión a PostgreSQL (la base de prueba del equipo está en Supabase). |
| `Jwt:Clave` | Clave para firmar las sesiones; al menos 32 caracteres. |
| `Supabase:Url` | URL del proyecto de Supabase (`https://<proyecto>.supabase.co`). |
| `Supabase:Bucket` | Bucket de Storage donde se guardan los archivos. |
| `Supabase:ServiceRoleKey` | Clave de servicio de Supabase. Es secreta: solo en el servidor, nunca en el frontend ni en el repositorio. |
| `Cors:Origenes` | Opcional. Páginas web que pueden llamar a la API; por defecto `http://localhost:5173`. |

## Migraciones

Desde esta carpeta:

```powershell
dotnet tool restore
$env:ConnectionStrings__DefaultConnection = "Host=...;Database=...;Username=...;Password=..."
# Aplicar las migraciones a la base (crea las tablas, los índices y las categorías base)
dotnet ef database update --project update.Infrastructure --startup-project update.API
# Crear una migración después de cambiar el modelo
dotnet ef migrations add NombreDelCambio --project update.Infrastructure --startup-project update.API --output-dir Migrations
```

`dotnet ef` toma la conexión de la variable de entorno `ConnectionStrings__DefaultConnection` (en `appsettings.json` está vacía a propósito). Los índices y los datos fijos se agregan con migraciones, no con archivos SQL sueltos.

## Datos de demostración

Con la API encendida en `http://localhost:5195` y las migraciones aplicadas, desde la raíz del repositorio:

```powershell
# Crea dos cuentas (demo_backend2_1 y demo_backend2_2), cada una con una publicación y un emprendimiento.
# Se puede correr varias veces sin duplicar nada. Pide una clave de prueba para cada cuenta
# (o tómala de UPDATE_DEMO_CLAVE_1 y UPDATE_DEMO_CLAVE_2).
node backend/demo/sembrar.mjs

# Recorre los endpoints de archivos, historiales y emprendimientos con esas dos cuentas.
# Necesita Supabase Storage configurado. Lo que crea lo borra al final.
node backend/demo/verificar.mjs

# Llena la red social: 10 cuentas con fotos, publicaciones, comentarios, "me gusta",
# seguidores, una cuenta privada y solicitudes pendientes. Las fotos vienen de picsum.photos.
$env:UPDATE_DEMO_CLAVE = "<clave-de-prueba-con-letras-y-numeros>"
node backend/demo/red-social.mjs
```

Para probar también los archivos de producto, ejecuta `backend/demo/producto.sql` en la base (crea un producto de muestra e imprime su id) y define `UPDATE_DEMO_PRODUCTO_ID` con ese id antes de `verificar.mjs`. `sembrar.mjs` y `verificar.mjs` solo aceptan una API local. `red-social.mjs` usa `UPDATE_API` para elegir el servidor y, si no es local, exige `UPDATE_DEMO_CONFIRMAR=si`.

`backend/demo/modelo.svg` es el diagrama de las tablas de archivos, publicaciones y emprendimientos (su fuente es `modelo.dot`).
