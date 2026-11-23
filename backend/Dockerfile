# Imagen de la API para Render. Render no trae .NET 10, por eso se despliega con Docker.
# Se construye desde la carpeta backend:  docker build -t update-api backend

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS compilacion
WORKDIR /src
# Primero solo los proyectos, para que la restauración de paquetes quede en caché entre despliegues.
COPY Directory.Build.props update.slnx ./
COPY update.Domain/update.Domain.csproj update.Domain/
COPY update.Application/update.Application.csproj update.Application/
COPY update.Infrastructure/update.Infrastructure.csproj update.Infrastructure/
COPY update.API/update.API.csproj update.API/
RUN dotnet restore update.API/update.API.csproj
COPY . .
RUN dotnet publish update.API/update.API.csproj -c Release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=compilacion /app .
ENV ASPNETCORE_ENVIRONMENT=Production
USER $APP_UID
# Render indica el puerto en la variable PORT; sin ella (por ejemplo en local) se usa 8080.
CMD ["sh", "-c", "ASPNETCORE_URLS=http://0.0.0.0:${PORT:-8080} exec dotnet update.API.dll"]
