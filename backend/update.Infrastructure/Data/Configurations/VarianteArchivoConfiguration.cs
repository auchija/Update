using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Infrastructure.Data.Configurations;

/// <summary>Mapea la tabla variantes_archivo y sus restricciones.</summary>
public sealed class VarianteArchivoConfiguration : IEntityTypeConfiguration<VarianteArchivo>
{
    public void Configure(EntityTypeBuilder<VarianteArchivo> builder)
    {
        builder.ToTable("variantes_archivo", tabla =>
        {
            tabla.HasCheckConstraint("ck_variantes_archivo_variante_longitud", "char_length(\"variante\") <= 64");
            tabla.HasCheckConstraint("ck_variantes_archivo_clave_objeto_longitud", "char_length(\"clave_objeto\") <= 1024");
            tabla.HasCheckConstraint("ck_variantes_archivo_tipo_mime_longitud", "char_length(\"tipo_mime\") <= 127");
        });
        builder.HasBaseType((Type?)null);
        builder.HasKey(e => new { e.ArchivoId, e.Variante }).HasName("pk_variantes_archivo");
        builder.Property(e => e.ArchivoId).HasColumnName("archivo_id").HasColumnType("uuid").IsRequired().ValueGeneratedNever();
        builder.Property(e => e.Variante).HasColumnName("variante").HasColumnType("text").IsRequired().HasMaxLength(64).ValueGeneratedNever();
        builder.Property(e => e.ClaveObjeto).HasColumnName("clave_objeto").HasColumnType("text").IsRequired().HasMaxLength(1024);
        builder.HasIndex(e => e.ClaveObjeto).IsUnique().HasDatabaseName("ux_variantes_archivo_clave_objeto");
        builder.Property(e => e.TipoMime).HasColumnName("tipo_mime").HasColumnType("text").IsRequired().HasMaxLength(127);
        builder.Property(e => e.Ancho).HasColumnName("ancho").HasColumnType("integer");
        builder.Property(e => e.Alto).HasColumnName("alto").HasColumnType("integer");
        builder.Property(e => e.TamanoBytes).HasColumnName("tamano_bytes").HasColumnType("bigint").IsRequired();
        builder.HasOne(e => e.Archivo).WithMany().HasForeignKey(e => e.ArchivoId).OnDelete(DeleteBehavior.Cascade).HasConstraintName("fk_variantes_archivo_archivo_id_archivos");
    }
}
