using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Infrastructure.Data.Configurations;

/// <summary>Mapea la tabla historial_solicitudes y sus restricciones.</summary>
public sealed class HistorialSolicitudConfiguration : IEntityTypeConfiguration<HistorialSolicitud>
{
    public void Configure(EntityTypeBuilder<HistorialSolicitud> builder)
    {
        builder.ToTable("historial_solicitudes", tabla =>
        {
            tabla.HasCheckConstraint("ck_historial_solicitudes_estado_anterior_enum", "\"estado_anterior\" IN ('pendiente', 'cotizado', 'aceptado', 'rechazado', 'cancelado', 'vencido', 'completado')");
            tabla.HasCheckConstraint("ck_historial_solicitudes_estado_nuevo_enum", "\"estado_nuevo\" IN ('pendiente', 'cotizado', 'aceptado', 'rechazado', 'cancelado', 'vencido', 'completado')");
            tabla.HasCheckConstraint("ck_historial_solicitudes_nota_longitud", "char_length(\"nota\") <= 4000");
        });
        builder.HasBaseType((Type?)null);
        builder.HasKey(e => e.Id).HasName("pk_historial_solicitudes");
        builder.Property(e => e.Id).HasColumnName("id").HasColumnType("bigint").IsRequired().UseIdentityAlwaysColumn();
        builder.Property(e => e.SolicitudCotizacionId).HasColumnName("solicitud_cotizacion_id").HasColumnType("uuid").IsRequired();
        builder.Property(e => e.EstadoAnterior).HasColumnName("estado_anterior").HasColumnType("text").HasMaxLength(10).HasConversion(new ConversorEnumDbml<EstadoSolicitud>());
        builder.Property(e => e.EstadoNuevo).HasColumnName("estado_nuevo").HasColumnType("text").IsRequired().HasMaxLength(10).HasConversion(new ConversorEnumDbml<EstadoSolicitud>());
        builder.Property(e => e.UsuarioActorId).HasColumnName("usuario_actor_id").HasColumnType("uuid");
        builder.Property(e => e.Nota).HasColumnName("nota").HasColumnType("text").HasMaxLength(4000);
        builder.Property(e => e.CreadoEn).HasColumnName("creado_en").HasColumnType("timestamptz").IsRequired().HasDefaultValueSql("now()");
        builder.HasOne(e => e.SolicitudCotizacion).WithMany().HasForeignKey(e => e.SolicitudCotizacionId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_historial_solicitudes_solicitud_cotizacion_id_solic_3cc7a2f1");
        builder.HasIndex(e => e.SolicitudCotizacionId).HasDatabaseName("ix_historial_solicitudes_solicitud_cotizacion_id");
        builder.HasOne(e => e.UsuarioActor).WithMany().HasForeignKey(e => e.UsuarioActorId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_historial_solicitudes_usuario_actor_id_usuarios");
        builder.HasIndex(e => e.UsuarioActorId).HasDatabaseName("ix_historial_solicitudes_usuario_actor_id");
    }
}
