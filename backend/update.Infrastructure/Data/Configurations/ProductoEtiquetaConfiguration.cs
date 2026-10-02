using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Infrastructure.Data.Configurations;

/// <summary>Mapea la tabla productos_etiquetas y sus restricciones.</summary>
public sealed class ProductoEtiquetaConfiguration : IEntityTypeConfiguration<ProductoEtiqueta>
{
    public void Configure(EntityTypeBuilder<ProductoEtiqueta> builder)
    {
        builder.ToTable("productos_etiquetas", tabla =>
        {
        });
        builder.HasBaseType((Type?)null);
        builder.HasKey(e => new { e.ProductoId, e.EtiquetaId }).HasName("pk_productos_etiquetas");
        builder.Property(e => e.ProductoId).HasColumnName("producto_id").HasColumnType("uuid").IsRequired().ValueGeneratedNever();
        builder.Property(e => e.EtiquetaId).HasColumnName("etiqueta_id").HasColumnType("integer").IsRequired().ValueGeneratedNever();
        builder.HasOne(e => e.Producto).WithMany().HasForeignKey(e => e.ProductoId).OnDelete(DeleteBehavior.Cascade).HasConstraintName("fk_productos_etiquetas_producto_id_productos");
        builder.HasOne(e => e.Etiqueta).WithMany().HasForeignKey(e => e.EtiquetaId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_productos_etiquetas_etiqueta_id_etiquetas");
        builder.HasIndex(e => e.EtiquetaId).HasDatabaseName("ix_productos_etiquetas_etiqueta_id");
    }
}
