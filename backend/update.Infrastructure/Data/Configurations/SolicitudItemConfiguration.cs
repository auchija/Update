using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Infrastructure.Data.Configurations;

/// <summary>Mapea la tabla solicitud_items y sus restricciones.</summary>
public sealed class SolicitudItemConfiguration : IEntityTypeConfiguration<SolicitudItem>
{
    public void Configure(EntityTypeBuilder<SolicitudItem> builder)
    {
        builder.ToTable("solicitud_items", tabla =>
        {
            tabla.HasCheckConstraint("ck_solicitud_items_descripcion_longitud", "char_length(\"descripcion\") <= 10000");
            tabla.HasCheckConstraint("ck_solicitud_items_notas_longitud", "char_length(\"notas\") <= 4000");
        });
        builder.HasBaseType((Type?)null);
        builder.HasKey(e => e.Id).HasName("pk_solicitud_items");
        builder.Ignore(e => e.CreadoEn);
        builder.Ignore(e => e.ActualizadoEn);
        builder.Property(e => e.Id).HasColumnName("id").HasColumnType("uuid").IsRequired().HasDefaultValueSql("gen_random_uuid()");
        builder.Property(e => e.SolicitudCotizacionId).HasColumnName("solicitud_cotizacion_id").HasColumnType("uuid").IsRequired();
        builder.Property(e => e.ProductoId).HasColumnName("producto_id").HasColumnType("uuid");
        builder.Property(e => e.Descripcion).HasColumnName("descripcion").HasColumnType("text").IsRequired().HasMaxLength(10000);
        builder.Property(e => e.Cantidad).HasColumnName("cantidad").HasColumnType("numeric(12,2)").IsRequired().HasPrecision(12, 2).HasDefaultValue(1m).HasSentinel(1m);
        builder.Property(e => e.Notas).HasColumnName("notas").HasColumnType("text").HasMaxLength(4000);
        builder.HasOne(e => e.SolicitudCotizacion).WithMany().HasForeignKey(e => e.SolicitudCotizacionId).OnDelete(DeleteBehavior.Cascade).HasConstraintName("fk_solicitud_items_solicitud_cotizacion_id_solicitudes_c716681e");
        builder.HasIndex(e => e.SolicitudCotizacionId).HasDatabaseName("ix_solicitud_items_solicitud_cotizacion_id");
        builder.HasOne(e => e.Producto).WithMany().HasForeignKey(e => e.ProductoId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_solicitud_items_producto_id_productos");
        builder.HasIndex(e => e.ProductoId).HasDatabaseName("ix_solicitud_items_producto_id");
    }
}
