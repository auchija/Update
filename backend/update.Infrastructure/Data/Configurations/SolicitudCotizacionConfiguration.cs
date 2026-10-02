using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Infrastructure.Data.Configurations;

/// <summary>Mapea la tabla solicitudes_cotizacion y sus restricciones.</summary>
public sealed class SolicitudCotizacionConfiguration : IEntityTypeConfiguration<SolicitudCotizacion>
{
    public void Configure(EntityTypeBuilder<SolicitudCotizacion> builder)
    {
        builder.ToTable("solicitudes_cotizacion", tabla =>
        {
            tabla.HasCheckConstraint("ck_solicitudes_cotizacion_titulo_longitud", "char_length(\"titulo\") <= 255");
            tabla.HasCheckConstraint("ck_solicitudes_cotizacion_mensaje_longitud", "char_length(\"mensaje\") <= 10000");
            tabla.HasCheckConstraint("ck_solicitudes_cotizacion_estado_enum", "\"estado\" IN ('pendiente', 'cotizado', 'aceptado', 'rechazado', 'cancelado', 'vencido', 'completado')");
        });
        builder.HasBaseType((Type?)null);
        builder.HasKey(e => e.Id).HasName("pk_solicitudes_cotizacion");
        builder.Property(e => e.Id).HasColumnName("id").HasColumnType("uuid").IsRequired().HasDefaultValueSql("gen_random_uuid()");
        builder.Property(e => e.Numero).HasColumnName("numero").HasColumnType("bigint").IsRequired().UseIdentityAlwaysColumn();
        builder.HasIndex(e => e.Numero).IsUnique().HasDatabaseName("ux_solicitudes_cotizacion_numero");
        builder.Property(e => e.EmprendimientoId).HasColumnName("emprendimiento_id").HasColumnType("uuid").IsRequired();
        builder.Property(e => e.PerfilSolicitanteId).HasColumnName("perfil_solicitante_id").HasColumnType("uuid").IsRequired();
        builder.Property(e => e.CreadoPorUsuarioId).HasColumnName("creado_por_usuario_id").HasColumnType("uuid").IsRequired();
        builder.Property(e => e.ConversacionId).HasColumnName("conversacion_id").HasColumnType("uuid");
        builder.Property(e => e.Titulo).HasColumnName("titulo").HasColumnType("text").IsRequired().HasMaxLength(255);
        builder.Property(e => e.Mensaje).HasColumnName("mensaje").HasColumnType("text").HasMaxLength(10000);
        builder.Property(e => e.FechaDeseada).HasColumnName("fecha_deseada").HasColumnType("date");
        builder.Property(e => e.CiudadEntregaId).HasColumnName("ciudad_entrega_id").HasColumnType("integer");
        builder.Property(e => e.Estado).HasColumnName("estado").HasColumnType("text").IsRequired().HasMaxLength(10).HasConversion(new ConversorEnumDbml<EstadoSolicitud>()).HasDefaultValue(EstadoSolicitud.Pendiente).HasSentinel(EstadoSolicitud.Pendiente);
        builder.Property(e => e.CotizacionAceptadaId).HasColumnName("cotizacion_aceptada_id").HasColumnType("uuid");
        builder.Property(e => e.CreadoEn).HasColumnName("creado_en").HasColumnType("timestamptz").IsRequired().HasDefaultValueSql("now()");
        builder.Property(e => e.ActualizadoEn).HasColumnName("actualizado_en").HasColumnType("timestamptz").IsRequired().HasDefaultValueSql("now()");
        builder.Property(e => e.CerradoEn).HasColumnName("cerrado_en").HasColumnType("timestamptz");
        builder.HasOne(e => e.Emprendimiento).WithMany().HasForeignKey(e => e.EmprendimientoId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_solicitudes_cotizacion_emprendimiento_id_emprendimientos");
        builder.HasIndex(e => e.EmprendimientoId).HasDatabaseName("ix_solicitudes_cotizacion_emprendimiento_id");
        builder.HasOne(e => e.PerfilSolicitante).WithMany().HasForeignKey(e => e.PerfilSolicitanteId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_solicitudes_cotizacion_perfil_solicitante_id_perfiles");
        builder.HasIndex(e => e.PerfilSolicitanteId).HasDatabaseName("ix_solicitudes_cotizacion_perfil_solicitante_id");
        builder.HasOne(e => e.CreadoPorUsuario).WithMany().HasForeignKey(e => e.CreadoPorUsuarioId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_solicitudes_cotizacion_creado_por_usuario_id_usuarios");
        builder.HasIndex(e => e.CreadoPorUsuarioId).HasDatabaseName("ix_solicitudes_cotizacion_creado_por_usuario_id");
        builder.HasOne(e => e.Conversacion).WithMany().HasForeignKey(e => e.ConversacionId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_solicitudes_cotizacion_conversacion_id_conversaciones");
        builder.HasIndex(e => e.ConversacionId).HasDatabaseName("ix_solicitudes_cotizacion_conversacion_id");
        builder.HasOne(e => e.CiudadEntrega).WithMany().HasForeignKey(e => e.CiudadEntregaId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_solicitudes_cotizacion_ciudad_entrega_id_ciudades");
        builder.HasIndex(e => e.CiudadEntregaId).HasDatabaseName("ix_solicitudes_cotizacion_ciudad_entrega_id");
        builder.HasOne(e => e.CotizacionAceptada).WithMany().HasForeignKey(e => new { e.CotizacionAceptadaId, e.Id }).HasPrincipalKey(e => new { e.Id, e.SolicitudCotizacionId }).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_solicitudes_cotizacion_cotizacion_aceptada_id_id_co_e86cb7ca");
        builder.HasIndex(e => new { e.CotizacionAceptadaId, e.Id }).HasDatabaseName("ix_solicitudes_cotizacion_cotizacion_aceptada_id_id");
        builder.HasIndex(e => new { e.Estado, e.CreadoEn }).HasDatabaseName("ix_solicitudes_cotizacion_estado_creado_en");
    }
}
