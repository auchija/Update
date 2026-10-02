using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Infrastructure.Data.Configurations;

/// <summary>Mapea la tabla productos y sus restricciones.</summary>
public sealed class ProductoConfiguration : IEntityTypeConfiguration<Producto>
{
    public void Configure(EntityTypeBuilder<Producto> builder)
    {
        builder.ToTable("productos", tabla =>
        {
            tabla.HasCheckConstraint("ck_productos_tipo_enum", "\"tipo\" IN ('producto', 'servicio')");
            tabla.HasCheckConstraint("ck_productos_titulo_longitud", "char_length(\"titulo\") <= 255");
            tabla.HasCheckConstraint("ck_productos_slug_longitud", "char_length(\"slug\") <= 255");
            tabla.HasCheckConstraint("ck_productos_descripcion_longitud", "char_length(\"descripcion\") <= 10000");
            tabla.HasCheckConstraint("ck_productos_tipo_precio_enum", "\"tipo_precio\" IN ('fijo', 'desde', 'a_convenir', 'gratis')");
            tabla.HasCheckConstraint("ck_productos_estado_inventario_enum", "\"estado_inventario\" IN ('disponible', 'agotado', 'bajo_pedido', 'no_aplica')");
            tabla.HasCheckConstraint("ck_productos_estado_enum", "\"estado\" IN ('borrador', 'activo', 'pausado', 'archivado')");
        });
        builder.HasBaseType((Type?)null);
        builder.HasKey(e => e.Id).HasName("pk_productos");
        builder.Property(e => e.Id).HasColumnName("id").HasColumnType("uuid").IsRequired().HasDefaultValueSql("gen_random_uuid()");
        builder.Property(e => e.EmprendimientoId).HasColumnName("emprendimiento_id").HasColumnType("uuid").IsRequired();
        builder.Property(e => e.CreadoPorUsuarioId).HasColumnName("creado_por_usuario_id").HasColumnType("uuid").IsRequired();
        builder.Property(e => e.Tipo).HasColumnName("tipo").HasColumnType("text").IsRequired().HasMaxLength(8).HasConversion(new ConversorEnumDbml<TipoProducto>());
        builder.Property(e => e.Titulo).HasColumnName("titulo").HasColumnType("text").IsRequired().HasMaxLength(255);
        builder.Property(e => e.Slug).HasColumnName("slug").HasColumnType("text").IsRequired().HasMaxLength(255);
        builder.Property(e => e.Descripcion).HasColumnName("descripcion").HasColumnType("text").HasMaxLength(10000);
        builder.Property(e => e.CategoriaId).HasColumnName("categoria_id").HasColumnType("integer").IsRequired();
        builder.Property(e => e.TipoPrecio).HasColumnName("tipo_precio").HasColumnType("text").IsRequired().HasMaxLength(10).HasConversion(new ConversorEnumDbml<TipoPrecio>()).HasDefaultValue(TipoPrecio.Fijo).HasSentinel(TipoPrecio.Fijo);
        builder.Property(e => e.Precio).HasColumnName("precio").HasColumnType("numeric(14,2)").HasPrecision(14, 2);
        builder.Property(e => e.MonedaCodigo).HasColumnName("moneda_codigo").HasColumnType("character(3)").IsRequired().HasMaxLength(3).HasDefaultValue("COP");
        builder.Property(e => e.EstadoInventario).HasColumnName("estado_inventario").HasColumnType("text").IsRequired().HasMaxLength(11).HasConversion(new ConversorEnumDbml<EstadoInventario>()).HasDefaultValue(EstadoInventario.Disponible).HasSentinel(EstadoInventario.Disponible);
        builder.Property(e => e.CantidadInventario).HasColumnName("cantidad_inventario").HasColumnType("integer");
        builder.Property(e => e.CiudadId).HasColumnName("ciudad_id").HasColumnType("integer");
        builder.Property(e => e.Estado).HasColumnName("estado").HasColumnType("text").IsRequired().HasMaxLength(9).HasConversion(new ConversorEnumDbml<EstadoProducto>()).HasDefaultValue(EstadoProducto.Borrador).HasSentinel(EstadoProducto.Borrador);
        builder.Property(e => e.CalificacionPromedio).HasColumnName("calificacion_promedio").HasColumnType("numeric(3,2)").HasPrecision(3, 2);
        builder.Property(e => e.TotalCalificaciones).HasColumnName("total_calificaciones").HasColumnType("integer").IsRequired().HasDefaultValue(0).HasSentinel(0);
        builder.Property(e => e.TotalGuardados).HasColumnName("total_guardados").HasColumnType("integer").IsRequired().HasDefaultValue(0).HasSentinel(0);
        builder.Property(e => e.PublicadoEn).HasColumnName("publicado_en").HasColumnType("timestamptz");
        builder.Property(e => e.CreadoEn).HasColumnName("creado_en").HasColumnType("timestamptz").IsRequired().HasDefaultValueSql("now()");
        builder.Property(e => e.ActualizadoEn).HasColumnName("actualizado_en").HasColumnType("timestamptz").IsRequired().HasDefaultValueSql("now()");
        builder.Property(e => e.EliminadoEn).HasColumnName("eliminado_en").HasColumnType("timestamptz");
        builder.HasOne(e => e.Emprendimiento).WithMany().HasForeignKey(e => e.EmprendimientoId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_productos_emprendimiento_id_emprendimientos");
        builder.HasIndex(e => e.EmprendimientoId).HasDatabaseName("ix_productos_emprendimiento_id");
        builder.HasOne(e => e.CreadoPorUsuario).WithMany().HasForeignKey(e => e.CreadoPorUsuarioId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_productos_creado_por_usuario_id_usuarios");
        builder.HasIndex(e => e.CreadoPorUsuarioId).HasDatabaseName("ix_productos_creado_por_usuario_id");
        builder.HasOne(e => e.Categoria).WithMany().HasForeignKey(e => e.CategoriaId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_productos_categoria_id_categorias");
        builder.HasIndex(e => e.CategoriaId).HasDatabaseName("ix_productos_categoria_id");
        builder.HasOne(e => e.Moneda).WithMany().HasForeignKey(e => e.MonedaCodigo).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_productos_moneda_codigo_monedas");
        builder.HasIndex(e => e.MonedaCodigo).HasDatabaseName("ix_productos_moneda_codigo");
        builder.HasOne(e => e.Ciudad).WithMany().HasForeignKey(e => e.CiudadId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_productos_ciudad_id_ciudades");
        builder.HasIndex(e => e.CiudadId).HasDatabaseName("ix_productos_ciudad_id");
        builder.HasIndex(e => new { e.Estado, e.CreadoEn }).HasDatabaseName("ix_productos_estado_creado_en");
        builder.HasQueryFilter(e => e.EliminadoEn == null);
    }
}
