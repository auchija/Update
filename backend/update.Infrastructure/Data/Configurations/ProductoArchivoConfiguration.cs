using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Infrastructure.Data.Configurations;

/// <summary>Mapea la tabla productos_archivos y sus restricciones.</summary>
public sealed class ProductoArchivoConfiguration : IEntityTypeConfiguration<ProductoArchivo>
{
    public void Configure(EntityTypeBuilder<ProductoArchivo> builder)
    {
        builder.ToTable("productos_archivos", tabla =>
        {
            tabla.HasCheckConstraint("ck_productos_archivos_texto_alternativo_longitud", "char_length(\"texto_alternativo\") <= 255");
        });
        builder.HasBaseType((Type?)null);
        builder.HasKey(e => new { e.ProductoId, e.ArchivoId }).HasName("pk_productos_archivos");
        builder.Property(e => e.ProductoId).HasColumnName("producto_id").HasColumnType("uuid").IsRequired().ValueGeneratedNever();
        builder.Property(e => e.ArchivoId).HasColumnName("archivo_id").HasColumnType("uuid").IsRequired().ValueGeneratedNever();
        builder.Property(e => e.Posicion).HasColumnName("posicion").HasColumnType("smallint").IsRequired();
        builder.Property(e => e.TextoAlternativo).HasColumnName("texto_alternativo").HasColumnType("text").HasMaxLength(255);
        builder.HasOne(e => e.Producto).WithMany().HasForeignKey(e => e.ProductoId).OnDelete(DeleteBehavior.Cascade).HasConstraintName("fk_productos_archivos_producto_id_productos");
        builder.HasOne(e => e.Archivo).WithMany().HasForeignKey(e => e.ArchivoId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_productos_archivos_archivo_id_archivos");
        builder.HasIndex(e => e.ArchivoId).HasDatabaseName("ix_productos_archivos_archivo_id");
    }
}
