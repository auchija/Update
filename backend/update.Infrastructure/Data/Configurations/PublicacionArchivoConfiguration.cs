using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Infrastructure.Data.Configurations;

/// <summary>Mapea la tabla publicaciones_archivos y sus restricciones.</summary>
public sealed class PublicacionArchivoConfiguration : IEntityTypeConfiguration<PublicacionArchivo>
{
    public void Configure(EntityTypeBuilder<PublicacionArchivo> builder)
    {
        builder.ToTable("publicaciones_archivos", tabla =>
        {
            tabla.HasCheckConstraint("ck_publicaciones_archivos_texto_alternativo_longitud", "char_length(\"texto_alternativo\") <= 255");
        });
        builder.HasBaseType((Type?)null);
        builder.HasKey(e => new { e.PublicacionId, e.ArchivoId }).HasName("pk_publicaciones_archivos");
        builder.Property(e => e.PublicacionId).HasColumnName("publicacion_id").HasColumnType("uuid").IsRequired().ValueGeneratedNever();
        builder.Property(e => e.ArchivoId).HasColumnName("archivo_id").HasColumnType("uuid").IsRequired().ValueGeneratedNever();
        builder.Property(e => e.Posicion).HasColumnName("posicion").HasColumnType("smallint").IsRequired();
        builder.Property(e => e.TextoAlternativo).HasColumnName("texto_alternativo").HasColumnType("text").HasMaxLength(255);
        builder.HasOne(e => e.Archivo).WithMany().HasForeignKey(e => e.ArchivoId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_publicaciones_archivos_archivo_id_archivos");
        builder.HasIndex(e => e.ArchivoId).HasDatabaseName("ix_publicaciones_archivos_archivo_id");
        builder.HasOne(e => e.Publicacion).WithMany().HasForeignKey(e => e.PublicacionId).OnDelete(DeleteBehavior.Cascade).HasConstraintName("fk_publicaciones_archivos_publicacion_id_publicaciones");
    }
}
