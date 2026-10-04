using update.Domain.Common;
using Microsoft.EntityFrameworkCore;
using update.Domain.Entities;
using update.Domain.Interfaces;
namespace update.Infrastructure.Data;

/// <summary>Contexto PostgreSQL; aplica configuraciones y auditoría UTC.</summary>
public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> opciones) : DbContext(opciones)
{
    public DbSet<Archivo> Archivos => Set<Archivo>();
    public DbSet<Bloqueo> Bloqueos => Set<Bloqueo>();
    public DbSet<Categoria> Categorias => Set<Categoria>();
    public DbSet<Ciudad> Ciudades => Set<Ciudad>();
    public DbSet<Comentario> Comentarios => Set<Comentario>();
    public DbSet<Conversacion> Conversaciones => Set<Conversacion>();
    public DbSet<CotizacionItem> CotizacionItems => Set<CotizacionItem>();
    public DbSet<Cotizacion> Cotizaciones => Set<Cotizacion>();
    public DbSet<Departamento> Departamentos => Set<Departamento>();
    public DbSet<DocumentoVerificacion> DocumentosVerificacion => Set<DocumentoVerificacion>();
    public DbSet<Emprendimiento> Emprendimientos => Set<Emprendimiento>();
    public DbSet<EnlaceEmprendimiento> EnlacesEmprendimiento => Set<EnlaceEmprendimiento>();
    public DbSet<Etiqueta> Etiquetas => Set<Etiqueta>();
    public DbSet<Guardado> Guardados => Set<Guardado>();
    public DbSet<HistorialSolicitud> HistorialSolicitudes => Set<HistorialSolicitud>();
    public DbSet<InteresUsuario> InteresesUsuario => Set<InteresUsuario>();
    public DbSet<Mencion> Menciones => Set<Mencion>();
    public DbSet<Mensaje> Mensajes => Set<Mensaje>();
    public DbSet<MensajeAdjunto> MensajesAdjuntos => Set<MensajeAdjunto>();
    public DbSet<MiembroEmprendimiento> MiembrosEmprendimiento => Set<MiembroEmprendimiento>();
    public DbSet<Moneda> Monedas => Set<Moneda>();
    public DbSet<MotivoReporte> MotivosReporte => Set<MotivoReporte>();
    public DbSet<Notificacion> Notificaciones => Set<Notificacion>();
    public DbSet<OfertaMentoria> OfertasMentoria => Set<OfertaMentoria>();
    public DbSet<Oportunidad> Oportunidades => Set<Oportunidad>();
    public DbSet<Pais> Paises => Set<Pais>();
    public DbSet<Participante> Participantes => Set<Participante>();
    public DbSet<Perfil> Perfiles => Set<Perfil>();
    public DbSet<PermisoRol> PermisosRol => Set<PermisoRol>();
    public DbSet<Postulacion> Postulaciones => Set<Postulacion>();
    public DbSet<Producto> Productos => Set<Producto>();
    public DbSet<ProductoArchivo> ProductosArchivos => Set<ProductoArchivo>();
    public DbSet<ProductoEtiqueta> ProductosEtiquetas => Set<ProductoEtiqueta>();
    public DbSet<Publicacion> Publicaciones => Set<Publicacion>();
    public DbSet<PublicacionArchivo> PublicacionesArchivos => Set<PublicacionArchivo>();
    public DbSet<PublicacionEtiqueta> PublicacionesEtiquetas => Set<PublicacionEtiqueta>();
    public DbSet<ReaccionComentario> ReaccionesComentario => Set<ReaccionComentario>();
    public DbSet<ReaccionPublicacion> ReaccionesPublicacion => Set<ReaccionPublicacion>();
    public DbSet<Reporte> Reportes => Set<Reporte>();
    public DbSet<Resena> Resenas => Set<Resena>();
    public DbSet<ResenaArchivo> ResenasArchivos => Set<ResenaArchivo>();
    public DbSet<Seguimiento> Seguimientos => Set<Seguimiento>();
    public DbSet<Sesion> Sesiones => Set<Sesion>();
    public DbSet<SolicitudItem> SolicitudItems => Set<SolicitudItem>();
    public DbSet<SolicitudCotizacion> SolicitudesCotizacion => Set<SolicitudCotizacion>();
    public DbSet<SolicitudMentoria> SolicitudesMentoria => Set<SolicitudMentoria>();
    public DbSet<TipoOportunidad> TiposOportunidad => Set<TipoOportunidad>();
    public DbSet<TipoPersona> TiposPersona => Set<TipoPersona>();
    public DbSet<TipoPublicacion> TiposPublicacion => Set<TipoPublicacion>();
    public DbSet<TipoReaccion> TiposReaccion => Set<TipoReaccion>();
    public DbSet<TokenRecuperacion> TokensRecuperacion => Set<TokenRecuperacion>();
    public DbSet<UsuarioTipoPersona> UsuarioTiposPersona => Set<UsuarioTipoPersona>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<UsuarioReservado> UsuariosReservados => Set<UsuarioReservado>();
    public DbSet<VarianteArchivo> VariantesArchivo => Set<VarianteArchivo>();
    public DbSet<Verificacion> Verificaciones => Set<Verificacion>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        PrepararCambios();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }
    public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        PrepararCambios();
        return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    private void PrepararCambios()
    {
        ChangeTracker.DetectChanges();
        var ahora = DateTime.UtcNow;
        // PostgreSQL conserva microsegundos; normalizar evita diferencias entre POST y GET.
        ahora = new DateTime(ahora.Ticks - ahora.Ticks % 10, DateTimeKind.Utc);
        foreach (var entrada in ChangeTracker.Entries().ToList())
        {
            if (entrada.State == EntityState.Deleted && entrada.Entity is IEliminable eliminable)
            {
                entrada.State = EntityState.Modified;
                eliminable.Eliminar();
            }
            if (entrada.State is not (EntityState.Added or EntityState.Modified)) continue;
            var creado = entrada.Metadata.FindProperty(nameof(BaseEntity.CreadoEn));
            var actualizado = entrada.Metadata.FindProperty(nameof(BaseEntity.ActualizadoEn));
            if (creado is not null)
            {
                if (entrada.State == EntityState.Added) entrada.Property(creado.Name).CurrentValue = ahora;
                else entrada.Property(creado.Name).IsModified = false;
            }
            if (actualizado is not null) entrada.Property(actualizado.Name).CurrentValue = ahora;
            if (entrada.Entity is IEntidadValidable validable) validable.Validar();
        }
    }
}
