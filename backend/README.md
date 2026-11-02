# Backend de UpDate

API REST en .NET 10 (C#) con PostgreSQL y Entity Framework Core. Los pasos para encenderla junto con la página web están en el [README principal](../README.md#cómo-probarlo). La lista de endpoints está en [ENDPOINTS.md](ENDPOINTS.md).

## Capas

| Proyecto | Qué contiene |
|---|---|
| `update.API` | Controladores, configuración (JWT, CORS, Swagger, archivos subidos), chequeo de salud y manejo central de errores. |
| `update.Application` | Servicios de cada caso de uso (autenticación, mi perfil, subida de archivos, CRUD genérico) y los DTO que entran y salen. |
| `update.Domain` | Entidades, enums, reglas de validación e interfaces de los repositorios. |
| `update.Infrastructure` | `ApplicationDbContext`, configuración de cada tabla, repositorios y migraciones. |

## Configuración

Se guarda con `user-secrets` en `update.API` (o con variables de entorno, cambiando `:` por `__`):

| Clave | Para qué |
|---|---|
| `ConnectionStrings:DefaultConnection` | Conexión a PostgreSQL (la base de prueba del equipo está en Supabase). |
| `Jwt:Clave` | Clave para firmar las sesiones; al menos 32 caracteres. |
| `Cors:Origenes` | Opcional. Páginas web que pueden llamar a la API; por defecto `http://localhost:5173`. |

## Migraciones

Desde esta carpeta:

```powershell
dotnet tool restore
# Aplicar las migraciones a la base
dotnet ef database update --project update.Infrastructure --startup-project update.API
# Crear una migración después de cambiar el modelo
dotnet ef migrations add NombreDelCambio --project update.Infrastructure --startup-project update.API --output-dir Migrations
```

`dotnet ef` usa la conexión de `update.API/appsettings.json` y, si no hay ninguna ahí, la variable de entorno `ConnectionStrings__DefaultConnection`. Hoy `appsettings.json` apunta a Supabase, así que las migraciones se aplican ahí.
