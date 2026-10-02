using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Infrastructure.Data.Configurations;

/// <summary>Mapea la tabla conversaciones y sus restricciones.</summary>
public sealed class ConversacionConfiguration : IEntityTypeConfiguration<Conversacion>
{
    public void Configure(EntityTypeBuilder<Conversacion> builder)
    {
        builder.ToTable("conversaciones", tabla =>
        {
            tabla.HasCheckConstraint("ck_conversaciones_tipo_enum", "\"tipo\" IN ('directa', 'grupo')");
            tabla.HasCheckConstraint("ck_conversaciones_titulo_longitud", "char_length(\"titulo\") <= 255");
            tabla.HasCheckConstraint("ck_conversaciones_clave_directa_longitud", "char_length(\"clave_directa\") <= 1024");
        });
        builder.HasBaseType((Type?)null);
        builder.HasKey(e => e.Id).HasName("pk_conversaciones");
        builder.Property(e => e.Id).HasColumnName("id").HasColumnType("uuid").IsRequired().HasDefaultValueSql("gen_random_uuid()");
        builder.Property(e => e.Tipo).HasColumnName("tipo").HasColumnType("text").IsRequired().HasMaxLength(7).HasConversion(new ConversorEnumDbml<TipoConversacion>()).HasDefaultValue(TipoConversacion.Directa).HasSentinel(TipoConversacion.Directa);
        builder.Property(e => e.Titulo).HasColumnName("titulo").HasColumnType("text").HasMaxLength(255);
        builder.Property(e => e.ClaveDirecta).HasColumnName("clave_directa").HasColumnType("text").HasMaxLength(1024);
        builder.HasIndex(e => e.ClaveDirecta).IsUnique().HasDatabaseName("ux_conversaciones_clave_directa");
        builder.Property(e => e.CreadoPorUsuarioId).HasColumnName("creado_por_usuario_id").HasColumnType("uuid").IsRequired();
        builder.Property(e => e.UltimoMensajeId).HasColumnName("ultimo_mensaje_id").HasColumnType("uuid");
        builder.Property(e => e.UltimoMensajeEn).HasColumnName("ultimo_mensaje_en").HasColumnType("timestamptz");
        builder.Property(e => e.CreadoEn).HasColumnName("creado_en").HasColumnType("timestamptz").IsRequired().HasDefaultValueSql("now()");
        builder.Property(e => e.ActualizadoEn).HasColumnName("actualizado_en").HasColumnType("timestamptz").IsRequired().HasDefaultValueSql("now()");
        builder.HasOne(e => e.CreadoPorUsuario).WithMany().HasForeignKey(e => e.CreadoPorUsuarioId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_conversaciones_creado_por_usuario_id_usuarios");
        builder.HasIndex(e => e.CreadoPorUsuarioId).HasDatabaseName("ix_conversaciones_creado_por_usuario_id");
        builder.HasOne(e => e.UltimoMensaje).WithMany().HasForeignKey(e => e.UltimoMensajeId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_conversaciones_ultimo_mensaje_id_mensajes");
        builder.HasIndex(e => e.UltimoMensajeId).HasDatabaseName("ix_conversaciones_ultimo_mensaje_id");
    }
}
