using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Infrastructure.Data.Configurations;

/// <summary>Mapea la tabla monedas y sus restricciones.</summary>
public sealed class MonedaConfiguration : IEntityTypeConfiguration<Moneda>
{
    public void Configure(EntityTypeBuilder<Moneda> builder)
    {
        builder.ToTable("monedas", tabla =>
        {
            tabla.HasCheckConstraint("ck_monedas_nombre_longitud", "char_length(\"nombre\") <= 255");
            tabla.HasCheckConstraint("ck_monedas_simbolo_longitud", "char_length(\"simbolo\") <= 255");
        });
        builder.HasBaseType((Type?)null);
        builder.HasKey(e => e.Codigo).HasName("pk_monedas");
        builder.Property(e => e.Codigo).HasColumnName("codigo").HasColumnType("character(3)").IsRequired().HasMaxLength(3).ValueGeneratedNever();
        builder.Property(e => e.Nombre).HasColumnName("nombre").HasColumnType("text").IsRequired().HasMaxLength(255);
        builder.Property(e => e.Simbolo).HasColumnName("simbolo").HasColumnType("text").IsRequired().HasMaxLength(255);
        builder.Property(e => e.Decimales).HasColumnName("decimales").HasColumnType("smallint").IsRequired().HasDefaultValue((short)0).HasSentinel((short)0);
        builder.Property(e => e.Activo).HasColumnName("activo").HasColumnType("boolean").IsRequired().HasDefaultValue(true).HasSentinel(true);
    }
}
