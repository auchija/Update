using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Infrastructure.Data.Configurations;

/// <summary>Mapea la tabla tipos_reaccion y sus restricciones.</summary>
public sealed class TipoReaccionConfiguration : IEntityTypeConfiguration<TipoReaccion>
{
    public void Configure(EntityTypeBuilder<TipoReaccion> builder)
    {
        builder.ToTable("tipos_reaccion", tabla =>
        {
            tabla.HasCheckConstraint("ck_tipos_reaccion_codigo_longitud", "char_length(\"codigo\") <= 64");
            tabla.HasCheckConstraint("ck_tipos_reaccion_nombre_longitud", "char_length(\"nombre\") <= 255");
            tabla.HasCheckConstraint("ck_tipos_reaccion_emoji_longitud", "char_length(\"emoji\") <= 32");
        });
        builder.HasBaseType((Type?)null);
        builder.HasKey(e => e.Codigo).HasName("pk_tipos_reaccion");
        builder.Property(e => e.Codigo).HasColumnName("codigo").HasColumnType("text").IsRequired().HasMaxLength(64).ValueGeneratedNever();
        builder.Property(e => e.Nombre).HasColumnName("nombre").HasColumnType("text").IsRequired().HasMaxLength(255);
        builder.Property(e => e.Emoji).HasColumnName("emoji").HasColumnType("text").IsRequired().HasMaxLength(32);
        builder.Property(e => e.Orden).HasColumnName("orden").HasColumnType("smallint").IsRequired().HasDefaultValue((short)0).HasSentinel((short)0);
        builder.Property(e => e.Activo).HasColumnName("activo").HasColumnType("boolean").IsRequired().HasDefaultValue(true).HasSentinel(true);
    }
}
