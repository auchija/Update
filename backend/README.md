# Backend de Update: feat/desarrollo-backend

Se implementan todas las tablas del proyecto en sus cuatro capas. La guía con rutas completas y todo el código está en IMPLEMENTACION_9_PASOS.md. Inventario de PK y endpoints: modelo/Inventario_tablas_y_endpoints.md. Decisiones de límites, enums, identidad, eliminación y migración: modelo/Decisiones_del_modelo.md.

## Arrancar en PowerShell

Requisitos: SDK .NET 10, Node.js 22 o posterior y PostgreSQL. Ejecuta desde la carpeta backend. Usa una base vacía o una base administrada por la migración CreateUsuariosTable del proyecto.

```powershell
Set-Location "D:\UNITY\Update\backend"
dotnet tool restore
dotnet restore update.slnx
dotnet build update.slnx -m:1
$env:ConnectionStrings__DefaultConnection = Read-Host "Cadena PostgreSQL de tu base"
$env:Jwt__Clave = node -e "console.log(require('crypto').randomBytes(48).toString('base64url'))"
dotnet ef database update --project update.Infrastructure/update.Infrastructure.csproj --startup-project update.API/update.API.csproj --context ApplicationDbContext
# Copia el token de desarrollo mostrado; dura 30 minutos.
node verificacion/emitir-token-desarrollo.mjs
dotnet run --project update.API/update.API.csproj --launch-profile http
```

Direcciones: http://localhost:5195/health ; http://localhost:5195/health/live ; http://localhost:5195/health/ready ; http://localhost:5195/swagger . Se mantiene el puerto del perfil http original. Swagger está disponible en desarrollo; pega el token de prueba en Authorize para probar los CRUD. Las variables anteriores duran esta terminal. Para arrancar desde Visual Studio, guarda Jwt:Clave y ConnectionStrings:DefaultConnection en user-secrets del proyecto update.API; dotnet ef necesita la variable ConnectionStrings__DefaultConnection.

El frontend no se modificó. Para conectarlo al puerto actual de la API configura VITE_API_URL=http://localhost:5195/api al iniciarlo. Sus formularios de autenticación siguen usando sus mocks originales; estos CRUD no crean un servicio de login.

## Migraciones

CreateUsuariosTable se conserva y ya se incluye ImplementarModeloUpDate. Para aplicar ambas basta database update; no generes otra inicial idéntica. Para futuras modificaciones del modelo:

```powershell
dotnet ef migrations add NombreDelCambio --project update.Infrastructure/update.Infrastructure.csproj --startup-project update.API/update.API.csproj --context ApplicationDbContext --output-dir Migrations
dotnet ef migrations script --idempotent --project update.Infrastructure/update.Infrastructure.csproj --startup-project update.API/update.API.csproj --context ApplicationDbContext --output migraciones/ModeloActualizado.sql
dotnet ef database update --project update.Infrastructure/update.Infrastructure.csproj --startup-project update.API/update.API.csproj --context ApplicationDbContext
```

migraciones/ImplementarModeloUpDate.sql contiene el SQL idempotente completo; InicialUsuarios.sql, TransicionUsuarios.sql y RevertirModelo.sql permiten revisar/probar cada etapa. RevertirModelo elimina las tablas nuevas y restaura el esquema anterior de Usuarios; no es un comando de arranque.

La migración crea perfiles para usuarios anteriores usando sus UUID y conserva correo, hash, rol y fechas. Rechaza datos incompatibles antes de modificar la tabla; se explica en modelo/Decisiones_del_modelo.md. Una base creada mediante bases-de-datos/update.sql con enums nativos requiere otro plan de transición, porque esta implementación usa enums como texto.

## Verificación

```powershell
dotnet run --project verificacion/update.Verificacion.csproj -- .
npm --prefix verificacion/postgres ci
npm --prefix verificacion/postgres run test:migracion
npm --prefix verificacion/postgres test
```

La verificación C# compara entidades, columnas, nulabilidad, PK, FK y filtros con el DBML. Los scripts Node usan PostgreSQL PGlite aislado en memoria, con puertos de prueba 55432 y 5089; no apuntan a tu base. Se inicia la DLL Debug compilada y se generan tokens efímeros. DOTNET_UPDATE permite indicar otra ruta del ejecutable dotnet.

Resultados incluidos: compilación con cero avisos/errores, modelo 56/451/123, CRUD representativo, validación, defaults con enum cero/bool false, hashes omitidos, claves simples/compuestas, salud, Swagger, CORS y borrado lógico. La migración se probó desde cero y sobre dos usuarios anteriores, dos ejecuciones idempotentes, reversión/reaplicación y rechazo de un perfil incompatible. No hay cambios pendientes entre modelo y snapshot.
