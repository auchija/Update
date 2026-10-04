using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Infrastructure.Data.Configurations;

/// <summary>Mapea la tabla postulaciones y sus restricciones.</summary>
public sealed class PostulacionConfiguration : IEntityTypeConfiguration<Postulacion>
{
    public void Configure(EntityTypeBuilder<Postulacion> builder)
    {
        builder.ToTable("postulaciones", tabla =>
        {
            tabla.HasCheckConstraint("ck_postulaciones_mensaje_longitud", "char_length(\"mensaje\") <= 10000");
            tabla.HasCheckConstraint("ck_postulaciones_estado_enum", "\"estado\" IN ('pendiente', 'aceptado', 'rechazado', 'retirado')");
        });
        builder.HasBaseType((Type?)null);
        builder.HasKey(e => e.Id).HasName("pk_postulaciones");
        builder.Property(e => e.Id).HasColumnName("id").HasColumnType("uuid").IsRequired().HasDefaultValueSql("gen_random_uuid()");
        builder.Property(e => e.OportunidadId).HasColumnName("oportunidad_id").HasColumnType("uuid").IsRequired();
        builder.Property(e => e.PerfilPostulanteId).HasColumnName("perfil_postulante_id").HasColumnType("uuid").IsRequired();
        builder.Property(e => e.CreadoPorUsuarioId).HasColumnName("creado_por_usuario_id").HasColumnType("uuid").IsRequired();
        builder.Property(e => e.Mensaje).HasColumnName("mensaje").HasColumnType("text").IsRequired().HasMaxLength(10000);
        builder.Property(e => e.Estado).HasColumnName("estado").HasColumnType("text").IsRequired().HasMaxLength(9).HasConversion(new ConversorEnumDbml<EstadoPostulacion>()).HasDefaultValue(EstadoPostulacion.Pendiente).HasSentinel(EstadoPostulacion.Pendiente);
        builder.Property(e => e.CreadoEn).HasColumnName("creado_en").HasColumnType("timestamptz").IsRequired().HasDefaultValueSql("now()");
        builder.Property(e => e.ActualizadoEn).HasColumnName("actualizado_en").HasColumnType("timestamptz").IsRequired().HasDefaultValueSql("now()");
        builder.HasOne(e => e.Oportunidad).WithMany().HasForeignKey(e => e.OportunidadId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_postulaciones_oportunidad_id_oportunidades");
        builder.HasIndex(e => e.OportunidadId).HasDatabaseName("ix_postulaciones_oportunidad_id");
        builder.HasOne(e => e.PerfilPostulante).WithMany().HasForeignKey(e => e.PerfilPostulanteId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_postulaciones_perfil_postulante_id_perfiles");
        builder.HasIndex(e => e.PerfilPostulanteId).HasDatabaseName("ix_postulaciones_perfil_postulante_id");
        builder.HasOne(e => e.CreadoPorUsuario).WithMany().HasForeignKey(e => e.CreadoPorUsuarioId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_postulaciones_creado_por_usuario_id_usuarios");
        builder.HasIndex(e => e.CreadoPorUsuarioId).HasDatabaseName("ix_postulaciones_creado_por_usuario_id");
        builder.HasIndex(e => new { e.Estado, e.CreadoEn }).HasDatabaseName("ix_postulaciones_estado_creado_en");
    }
}
