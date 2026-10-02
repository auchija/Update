using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Infrastructure.Data.Configurations;

/// <summary>Mapea la tabla tipos_publicacion y sus restricciones.</summary>
public sealed class TipoPublicacionConfiguration : IEntityTypeConfiguration<TipoPublicacion>
{
    public void Configure(EntityTypeBuilder<TipoPublicacion> builder)
    {
        builder.ToTable("tipos_publicacion", tabla =>
        {
            tabla.HasCheckConstraint("ck_tipos_publicacion_codigo_longitud", "char_length(\"codigo\") <= 64");
            tabla.HasCheckConstraint("ck_tipos_publicacion_nombre_longitud", "char_length(\"nombre\") <= 255");
            tabla.HasCheckConstraint("ck_tipos_publicacion_descripcion_longitud", "char_length(\"descripcion\") <= 10000");
            tabla.HasCheckConstraint("ck_tipos_publicacion_icono_longitud", "char_length(\"icono\") <= 255");
        });
        builder.HasBaseType((Type?)null);
        builder.HasKey(e => e.Codigo).HasName("pk_tipos_publicacion");
        builder.Property(e => e.Codigo).HasColumnName("codigo").HasColumnType("text").IsRequired().HasMaxLength(64).ValueGeneratedNever();
        builder.Property(e => e.Nombre).HasColumnName("nombre").HasColumnType("text").IsRequired().HasMaxLength(255);
        builder.Property(e => e.Descripcion).HasColumnName("descripcion").HasColumnType("text").HasMaxLength(10000);
        builder.Property(e => e.Icono).HasColumnName("icono").HasColumnType("text").HasMaxLength(255);
        builder.Property(e => e.PesoDescubrimiento).HasColumnName("peso_descubrimiento").HasColumnType("numeric(4,2)").IsRequired().HasPrecision(4, 2).HasDefaultValue(1.00m).HasSentinel(1.00m);
        builder.Property(e => e.PermiteRespuestaAceptada).HasColumnName("permite_respuesta_aceptada").HasColumnType("boolean").IsRequired().HasDefaultValue(false).HasSentinel(false);
        builder.Property(e => e.Orden).HasColumnName("orden").HasColumnType("smallint").IsRequired().HasDefaultValue((short)0).HasSentinel((short)0);
        builder.Property(e => e.Activo).HasColumnName("activo").HasColumnType("boolean").IsRequired().HasDefaultValue(true).HasSentinel(true);
    }
}
