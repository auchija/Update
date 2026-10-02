using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Infrastructure.Data.Configurations;

/// <summary>Mapea la tabla solicitudes_mentoria y sus restricciones.</summary>
public sealed class SolicitudMentoriaConfiguration : IEntityTypeConfiguration<SolicitudMentoria>
{
    public void Configure(EntityTypeBuilder<SolicitudMentoria> builder)
    {
        builder.ToTable("solicitudes_mentoria", tabla =>
        {
            tabla.HasCheckConstraint("ck_solicitudes_mentoria_mensaje_longitud", "char_length(\"mensaje\") <= 10000");
            tabla.HasCheckConstraint("ck_solicitudes_mentoria_estado_enum", "\"estado\" IN ('solicitado', 'aceptado', 'rechazado', 'agendado', 'completado', 'cancelado')");
            tabla.HasCheckConstraint("ck_solicitudes_mentoria_url_reunion_longitud", "char_length(\"url_reunion\") <= 2048");
            tabla.HasCheckConstraint("ck_solicitudes_mentoria_calificacion_mentoreado", "\"calificacion_mentoreado\" BETWEEN 1 AND 5");
            tabla.HasCheckConstraint("ck_solicitudes_mentoria_comentario_mentoreado_longitud", "char_length(\"comentario_mentoreado\") <= 4000");
        });
        builder.HasBaseType((Type?)null);
        builder.HasKey(e => e.Id).HasName("pk_solicitudes_mentoria");
        builder.Property(e => e.Id).HasColumnName("id").HasColumnType("uuid").IsRequired().HasDefaultValueSql("gen_random_uuid()");
        builder.Property(e => e.OfertaId).HasColumnName("oferta_id").HasColumnType("uuid").IsRequired();
        builder.Property(e => e.PerfilMentoreadoId).HasColumnName("perfil_mentoreado_id").HasColumnType("uuid").IsRequired();
        builder.Property(e => e.CreadoPorUsuarioId).HasColumnName("creado_por_usuario_id").HasColumnType("uuid").IsRequired();
        builder.Property(e => e.Mensaje).HasColumnName("mensaje").HasColumnType("text").IsRequired().HasMaxLength(10000);
        builder.Property(e => e.Estado).HasColumnName("estado").HasColumnType("text").IsRequired().HasMaxLength(10).HasConversion(new ConversorEnumDbml<EstadoMentoria>()).HasDefaultValue(EstadoMentoria.Solicitado).HasSentinel(EstadoMentoria.Solicitado);
        builder.Property(e => e.AgendadoEn).HasColumnName("agendado_en").HasColumnType("timestamptz");
        builder.Property(e => e.UrlReunion).HasColumnName("url_reunion").HasColumnType("text").HasMaxLength(2048);
        builder.Property(e => e.CalificacionMentoreado).HasColumnName("calificacion_mentoreado").HasColumnType("smallint");
        builder.Property(e => e.ComentarioMentoreado).HasColumnName("comentario_mentoreado").HasColumnType("text").HasMaxLength(4000);
        builder.Property(e => e.CreadoEn).HasColumnName("creado_en").HasColumnType("timestamptz").IsRequired().HasDefaultValueSql("now()");
        builder.Property(e => e.ActualizadoEn).HasColumnName("actualizado_en").HasColumnType("timestamptz").IsRequired().HasDefaultValueSql("now()");
        builder.HasOne(e => e.Oferta).WithMany().HasForeignKey(e => e.OfertaId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_solicitudes_mentoria_oferta_id_ofertas_mentoria");
        builder.HasIndex(e => e.OfertaId).HasDatabaseName("ix_solicitudes_mentoria_oferta_id");
        builder.HasOne(e => e.PerfilMentoreado).WithMany().HasForeignKey(e => e.PerfilMentoreadoId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_solicitudes_mentoria_perfil_mentoreado_id_perfiles");
        builder.HasIndex(e => e.PerfilMentoreadoId).HasDatabaseName("ix_solicitudes_mentoria_perfil_mentoreado_id");
        builder.HasOne(e => e.CreadoPorUsuario).WithMany().HasForeignKey(e => e.CreadoPorUsuarioId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_solicitudes_mentoria_creado_por_usuario_id_usuarios");
        builder.HasIndex(e => e.CreadoPorUsuarioId).HasDatabaseName("ix_solicitudes_mentoria_creado_por_usuario_id");
        builder.HasIndex(e => new { e.Estado, e.CreadoEn }).HasDatabaseName("ix_solicitudes_mentoria_estado_creado_en");
    }
}
