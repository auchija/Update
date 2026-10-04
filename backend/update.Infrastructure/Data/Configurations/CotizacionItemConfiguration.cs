using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Infrastructure.Data.Configurations;

/// <summary>Mapea la tabla cotizacion_items y sus restricciones.</summary>
public sealed class CotizacionItemConfiguration : IEntityTypeConfiguration<CotizacionItem>
{
    public void Configure(EntityTypeBuilder<CotizacionItem> builder)
    {
        builder.ToTable("cotizacion_items", tabla =>
        {
            tabla.HasCheckConstraint("ck_cotizacion_items_descripcion_longitud", "char_length(\"descripcion\") <= 10000");
        });
        builder.HasBaseType((Type?)null);
        builder.HasKey(e => e.Id).HasName("pk_cotizacion_items");
        builder.Ignore(e => e.CreadoEn);
        builder.Ignore(e => e.ActualizadoEn);
        builder.Property(e => e.Id).HasColumnName("id").HasColumnType("uuid").IsRequired().HasDefaultValueSql("gen_random_uuid()");
        builder.Property(e => e.CotizacionId).HasColumnName("cotizacion_id").HasColumnType("uuid").IsRequired();
        builder.Property(e => e.ProductoId).HasColumnName("producto_id").HasColumnType("uuid");
        builder.Property(e => e.Descripcion).HasColumnName("descripcion").HasColumnType("text").IsRequired().HasMaxLength(10000);
        builder.Property(e => e.Cantidad).HasColumnName("cantidad").HasColumnType("numeric(12,2)").IsRequired().HasPrecision(12, 2);
        builder.Property(e => e.PrecioUnitario).HasColumnName("precio_unitario").HasColumnType("numeric(14,2)").IsRequired().HasPrecision(14, 2);
        builder.Property(e => e.TotalLinea).HasColumnName("total_linea").HasColumnType("numeric(14,2)").HasPrecision(14, 2).HasComputedColumnSql("cantidad * precio_unitario", stored: true);
        builder.HasOne(e => e.Cotizacion).WithMany().HasForeignKey(e => e.CotizacionId).OnDelete(DeleteBehavior.Cascade).HasConstraintName("fk_cotizacion_items_cotizacion_id_cotizaciones");
        builder.HasIndex(e => e.CotizacionId).HasDatabaseName("ix_cotizacion_items_cotizacion_id");
        builder.HasOne(e => e.Producto).WithMany().HasForeignKey(e => e.ProductoId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_cotizacion_items_producto_id_productos");
        builder.HasIndex(e => e.ProductoId).HasDatabaseName("ix_cotizacion_items_producto_id");
    }
}
