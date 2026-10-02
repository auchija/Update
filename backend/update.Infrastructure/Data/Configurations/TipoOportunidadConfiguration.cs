using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Infrastructure.Data.Configurations;

/// <summary>Mapea la tabla tipos_oportunidad y sus restricciones.</summary>
public sealed class TipoOportunidadConfiguration : IEntityTypeConfiguration<TipoOportunidad>
{
    public void Configure(EntityTypeBuilder<TipoOportunidad> builder)
    {
        builder.ToTable("tipos_oportunidad", tabla =>
        {
            tabla.HasCheckConstraint("ck_tipos_oportunidad_codigo_longitud", "char_length(\"codigo\") <= 64");
            tabla.HasCheckConstraint("ck_tipos_oportunidad_nombre_longitud", "char_length(\"nombre\") <= 255");
            tabla.HasCheckConstraint("ck_tipos_oportunidad_descripcion_longitud", "char_length(\"descripcion\") <= 10000");
        });
        builder.HasBaseType((Type?)null);
        builder.HasKey(e => e.Codigo).HasName("pk_tipos_oportunidad");
        builder.Property(e => e.Codigo).HasColumnName("codigo").HasColumnType("text").IsRequired().HasMaxLength(64).ValueGeneratedNever();
        builder.Property(e => e.Nombre).HasColumnName("nombre").HasColumnType("text").IsRequired().HasMaxLength(255);
        builder.Property(e => e.Descripcion).HasColumnName("descripcion").HasColumnType("text").HasMaxLength(10000);
        builder.Property(e => e.Orden).HasColumnName("orden").HasColumnType("smallint").IsRequired().HasDefaultValue((short)0).HasSentinel((short)0);
        builder.Property(e => e.Activo).HasColumnName("activo").HasColumnType("boolean").IsRequired().HasDefaultValue(true).HasSentinel(true);
    }
}
