using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Infrastructure.Data.Configurations;

/// <summary>Mapea la tabla motivos_reporte y sus restricciones.</summary>
public sealed class MotivoReporteConfiguration : IEntityTypeConfiguration<MotivoReporte>
{
    public void Configure(EntityTypeBuilder<MotivoReporte> builder)
    {
        builder.ToTable("motivos_reporte", tabla =>
        {
            tabla.HasCheckConstraint("ck_motivos_reporte_codigo_longitud", "char_length(\"codigo\") <= 64");
            tabla.HasCheckConstraint("ck_motivos_reporte_nombre_longitud", "char_length(\"nombre\") <= 255");
        });
        builder.HasBaseType((Type?)null);
        builder.HasKey(e => e.Codigo).HasName("pk_motivos_reporte");
        builder.Property(e => e.Codigo).HasColumnName("codigo").HasColumnType("text").IsRequired().HasMaxLength(64).ValueGeneratedNever();
        builder.Property(e => e.Nombre).HasColumnName("nombre").HasColumnType("text").IsRequired().HasMaxLength(255);
        builder.Property(e => e.Orden).HasColumnName("orden").HasColumnType("smallint").IsRequired().HasDefaultValue((short)0).HasSentinel((short)0);
        builder.Property(e => e.Activo).HasColumnName("activo").HasColumnType("boolean").IsRequired().HasDefaultValue(true).HasSentinel(true);
    }
}
