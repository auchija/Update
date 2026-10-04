using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Infrastructure.Data.Configurations;

/// <summary>Mapea la tabla reacciones_publicacion y sus restricciones.</summary>
public sealed class ReaccionPublicacionConfiguration : IEntityTypeConfiguration<ReaccionPublicacion>
{
    public void Configure(EntityTypeBuilder<ReaccionPublicacion> builder)
    {
        builder.ToTable("reacciones_publicacion", tabla =>
        {
            tabla.HasCheckConstraint("ck_reacciones_publicacion_tipo_reaccion_codigo_longitud", "char_length(\"tipo_reaccion_codigo\") <= 64");
        });
        builder.HasBaseType((Type?)null);
        builder.HasKey(e => new { e.PublicacionId, e.PerfilId }).HasName("pk_reacciones_publicacion");
        builder.Property(e => e.PublicacionId).HasColumnName("publicacion_id").HasColumnType("uuid").IsRequired().ValueGeneratedNever();
        builder.Property(e => e.PerfilId).HasColumnName("perfil_id").HasColumnType("uuid").IsRequired().ValueGeneratedNever();
        builder.Property(e => e.TipoReaccionCodigo).HasColumnName("tipo_reaccion_codigo").HasColumnType("text").IsRequired().HasMaxLength(64);
        builder.Property(e => e.CreadoEn).HasColumnName("creado_en").HasColumnType("timestamptz").IsRequired().HasDefaultValueSql("now()");
        builder.Property(e => e.ActualizadoEn).HasColumnName("actualizado_en").HasColumnType("timestamptz").IsRequired().HasDefaultValueSql("now()");
        builder.HasOne(e => e.Publicacion).WithMany().HasForeignKey(e => e.PublicacionId).OnDelete(DeleteBehavior.Cascade).HasConstraintName("fk_reacciones_publicacion_publicacion_id_publicaciones");
        builder.HasOne(e => e.Perfil).WithMany().HasForeignKey(e => e.PerfilId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_reacciones_publicacion_perfil_id_perfiles");
        builder.HasIndex(e => e.PerfilId).HasDatabaseName("ix_reacciones_publicacion_perfil_id");
        builder.HasOne(e => e.TipoReaccion).WithMany().HasForeignKey(e => e.TipoReaccionCodigo).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_reacciones_publicacion_tipo_reaccion_codigo_tipos_reaccion");
        builder.HasIndex(e => e.TipoReaccionCodigo).HasDatabaseName("ix_reacciones_publicacion_tipo_reaccion_codigo");
    }
}
