using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Infrastructure.Data.Configurations;

/// <summary>Mapea la tabla mensajes y sus restricciones.</summary>
public sealed class MensajeConfiguration : IEntityTypeConfiguration<Mensaje>
{
    public void Configure(EntityTypeBuilder<Mensaje> builder)
    {
        builder.ToTable("mensajes", tabla =>
        {
            tabla.HasCheckConstraint("ck_mensajes_tipo_enum", "\"tipo\" IN ('texto', 'archivo', 'producto', 'cotizacion', 'sistema')");
            tabla.HasCheckConstraint("ck_mensajes_contenido_longitud", "char_length(\"contenido\") <= 10000");
        });
        builder.HasBaseType((Type?)null);
        builder.HasKey(e => e.Id).HasName("pk_mensajes");
        builder.Ignore(e => e.ActualizadoEn);
        builder.Property(e => e.Id).HasColumnName("id").HasColumnType("uuid").IsRequired().HasDefaultValueSql("gen_random_uuid()");
        builder.Property(e => e.ConversacionId).HasColumnName("conversacion_id").HasColumnType("uuid").IsRequired();
        builder.Property(e => e.PerfilRemitenteId).HasColumnName("perfil_remitente_id").HasColumnType("uuid").IsRequired();
        builder.Property(e => e.UsuarioRemitenteId).HasColumnName("usuario_remitente_id").HasColumnType("uuid");
        builder.Property(e => e.Tipo).HasColumnName("tipo").HasColumnType("text").IsRequired().HasMaxLength(10).HasConversion(new ConversorEnumDbml<TipoMensaje>()).HasDefaultValue(TipoMensaje.Texto).HasSentinel(TipoMensaje.Texto);
        builder.Property(e => e.Contenido).HasColumnName("contenido").HasColumnType("text").HasMaxLength(10000);
        builder.Property(e => e.ProductoId).HasColumnName("producto_id").HasColumnType("uuid");
        builder.Property(e => e.SolicitudCotizacionId).HasColumnName("solicitud_cotizacion_id").HasColumnType("uuid");
        builder.Property(e => e.RespondeAMensajeId).HasColumnName("responde_a_mensaje_id").HasColumnType("uuid");
        builder.Property(e => e.CreadoEn).HasColumnName("creado_en").HasColumnType("timestamptz").IsRequired().HasDefaultValueSql("now()");
        builder.Property(e => e.EditadoEn).HasColumnName("editado_en").HasColumnType("timestamptz");
        builder.Property(e => e.EliminadoEn).HasColumnName("eliminado_en").HasColumnType("timestamptz");
        builder.HasOne(e => e.UsuarioRemitente).WithMany().HasForeignKey(e => e.UsuarioRemitenteId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_mensajes_usuario_remitente_id_usuarios");
        builder.HasIndex(e => e.UsuarioRemitenteId).HasDatabaseName("ix_mensajes_usuario_remitente_id");
        builder.HasOne(e => e.Producto).WithMany().HasForeignKey(e => e.ProductoId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_mensajes_producto_id_productos");
        builder.HasIndex(e => e.ProductoId).HasDatabaseName("ix_mensajes_producto_id");
        builder.HasOne(e => e.RespondeAMensaje).WithMany().HasForeignKey(e => e.RespondeAMensajeId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_mensajes_responde_a_mensaje_id_mensajes");
        builder.HasIndex(e => e.RespondeAMensajeId).HasDatabaseName("ix_mensajes_responde_a_mensaje_id");
        builder.HasOne(e => e.ParticipanteRemitente).WithMany().HasForeignKey(e => new { e.ConversacionId, e.PerfilRemitenteId }).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_mensajes_conversacion_id_perfil_remitente_id_participantes");
        builder.HasIndex(e => new { e.ConversacionId, e.PerfilRemitenteId }).HasDatabaseName("ix_mensajes_conversacion_id_perfil_remitente_id");
        builder.HasOne(e => e.SolicitudCotizacion).WithMany().HasForeignKey(e => e.SolicitudCotizacionId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_mensajes_solicitud_cotizacion_id_solicitudes_cotizacion");
        builder.HasIndex(e => e.SolicitudCotizacionId).HasDatabaseName("ix_mensajes_solicitud_cotizacion_id");
        builder.HasQueryFilter(e => e.EliminadoEn == null);
    }
}
