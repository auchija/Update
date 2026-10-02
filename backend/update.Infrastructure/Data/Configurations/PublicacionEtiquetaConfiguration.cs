using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Infrastructure.Data.Configurations;

/// <summary>Mapea la tabla publicaciones_etiquetas y sus restricciones.</summary>
public sealed class PublicacionEtiquetaConfiguration : IEntityTypeConfiguration<PublicacionEtiqueta>
{
    public void Configure(EntityTypeBuilder<PublicacionEtiqueta> builder)
    {
        builder.ToTable("publicaciones_etiquetas", tabla =>
        {
        });
        builder.HasBaseType((Type?)null);
        builder.HasKey(e => new { e.PublicacionId, e.EtiquetaId }).HasName("pk_publicaciones_etiquetas");
        builder.Property(e => e.PublicacionId).HasColumnName("publicacion_id").HasColumnType("uuid").IsRequired().ValueGeneratedNever();
        builder.Property(e => e.EtiquetaId).HasColumnName("etiqueta_id").HasColumnType("integer").IsRequired().ValueGeneratedNever();
        builder.HasOne(e => e.Publicacion).WithMany().HasForeignKey(e => e.PublicacionId).OnDelete(DeleteBehavior.Cascade).HasConstraintName("fk_publicaciones_etiquetas_publicacion_id_publicaciones");
        builder.HasOne(e => e.Etiqueta).WithMany().HasForeignKey(e => e.EtiquetaId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_publicaciones_etiquetas_etiqueta_id_etiquetas");
        builder.HasIndex(e => e.EtiquetaId).HasDatabaseName("ix_publicaciones_etiquetas_etiqueta_id");
    }
}
