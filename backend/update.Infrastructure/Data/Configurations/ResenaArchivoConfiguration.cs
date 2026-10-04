using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Infrastructure.Data.Configurations;

/// <summary>Mapea la tabla resenas_archivos y sus restricciones.</summary>
public sealed class ResenaArchivoConfiguration : IEntityTypeConfiguration<ResenaArchivo>
{
    public void Configure(EntityTypeBuilder<ResenaArchivo> builder)
    {
        builder.ToTable("resenas_archivos", tabla =>
        {
        });
        builder.HasBaseType((Type?)null);
        builder.HasKey(e => new { e.ResenaId, e.ArchivoId }).HasName("pk_resenas_archivos");
        builder.Property(e => e.ResenaId).HasColumnName("resena_id").HasColumnType("uuid").IsRequired().ValueGeneratedNever();
        builder.Property(e => e.ArchivoId).HasColumnName("archivo_id").HasColumnType("uuid").IsRequired().ValueGeneratedNever();
        builder.Property(e => e.Posicion).HasColumnName("posicion").HasColumnType("smallint").IsRequired();
        builder.HasOne(e => e.Resena).WithMany().HasForeignKey(e => e.ResenaId).OnDelete(DeleteBehavior.Cascade).HasConstraintName("fk_resenas_archivos_resena_id_resenas");
        builder.HasOne(e => e.Archivo).WithMany().HasForeignKey(e => e.ArchivoId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_resenas_archivos_archivo_id_archivos");
        builder.HasIndex(e => e.ArchivoId).HasDatabaseName("ix_resenas_archivos_archivo_id");
    }
}
