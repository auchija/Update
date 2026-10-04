using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Infrastructure.Data.Configurations;

/// <summary>Mapea la tabla paises y sus restricciones.</summary>
public sealed class PaisConfiguration : IEntityTypeConfiguration<Pais>
{
    public void Configure(EntityTypeBuilder<Pais> builder)
    {
        builder.ToTable("paises", tabla =>
        {
            tabla.HasCheckConstraint("ck_paises_nombre_longitud", "char_length(\"nombre\") <= 255");
            tabla.HasCheckConstraint("ck_paises_prefijo_telefono_longitud", "char_length(\"prefijo_telefono\") <= 30");
        });
        builder.HasBaseType((Type?)null);
        builder.HasKey(e => e.Codigo).HasName("pk_paises");
        builder.Property(e => e.Codigo).HasColumnName("codigo").HasColumnType("character(2)").IsRequired().HasMaxLength(2).ValueGeneratedNever();
        builder.Property(e => e.Nombre).HasColumnName("nombre").HasColumnType("text").IsRequired().HasMaxLength(255);
        builder.Property(e => e.MonedaDefectoCodigo).HasColumnName("moneda_defecto_codigo").HasColumnType("character(3)").IsRequired().HasMaxLength(3);
        builder.Property(e => e.PrefijoTelefono).HasColumnName("prefijo_telefono").HasColumnType("text").IsRequired().HasMaxLength(30);
        builder.HasOne(e => e.MonedaDefecto).WithMany().HasForeignKey(e => e.MonedaDefectoCodigo).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_paises_moneda_defecto_codigo_monedas");
        builder.HasIndex(e => e.MonedaDefectoCodigo).HasDatabaseName("ix_paises_moneda_defecto_codigo");
    }
}
