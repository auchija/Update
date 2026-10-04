using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Infrastructure.Data.Configurations;

/// <summary>Mapea la tabla etiquetas y sus restricciones.</summary>
public sealed class EtiquetaConfiguration : IEntityTypeConfiguration<Etiqueta>
{
    public void Configure(EntityTypeBuilder<Etiqueta> builder)
    {
        builder.ToTable("etiquetas", tabla =>
        {
            tabla.HasCheckConstraint("ck_etiquetas_nombre_longitud", "char_length(\"nombre\") <= 255");
        });
        builder.HasBaseType((Type?)null);
        builder.HasKey(e => e.Id).HasName("pk_etiquetas");
        builder.Property(e => e.Id).HasColumnName("id").HasColumnType("integer").IsRequired().UseIdentityAlwaysColumn();
        builder.Property(e => e.Nombre).HasColumnName("nombre").HasColumnType("text").IsRequired().HasMaxLength(255);
        builder.HasIndex(e => e.Nombre).IsUnique().HasDatabaseName("ux_etiquetas_nombre");
        builder.Property(e => e.TotalUsos).HasColumnName("total_usos").HasColumnType("integer").IsRequired().HasDefaultValue(0).HasSentinel(0);
        builder.Property(e => e.CreadoEn).HasColumnName("creado_en").HasColumnType("timestamptz").IsRequired().HasDefaultValueSql("now()");
    }
}
