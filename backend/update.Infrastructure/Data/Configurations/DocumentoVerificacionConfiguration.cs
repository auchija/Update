using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Infrastructure.Data.Configurations;

/// <summary>Mapea la tabla documentos_verificacion y sus restricciones.</summary>
public sealed class DocumentoVerificacionConfiguration : IEntityTypeConfiguration<DocumentoVerificacion>
{
    public void Configure(EntityTypeBuilder<DocumentoVerificacion> builder)
    {
        builder.ToTable("documentos_verificacion", tabla =>
        {
            tabla.HasCheckConstraint("ck_documentos_verificacion_tipo_documento_longitud", "char_length(\"tipo_documento\") <= 64");
        });
        builder.HasBaseType((Type?)null);
        builder.HasKey(e => new { e.VerificacionId, e.ArchivoId }).HasName("pk_documentos_verificacion");
        builder.Property(e => e.VerificacionId).HasColumnName("verificacion_id").HasColumnType("uuid").IsRequired().ValueGeneratedNever();
        builder.Property(e => e.ArchivoId).HasColumnName("archivo_id").HasColumnType("uuid").IsRequired().ValueGeneratedNever();
        builder.Property(e => e.TipoDocumento).HasColumnName("tipo_documento").HasColumnType("text").IsRequired().HasMaxLength(64);
        builder.HasOne(e => e.Verificacion).WithMany().HasForeignKey(e => e.VerificacionId).OnDelete(DeleteBehavior.Cascade).HasConstraintName("fk_documentos_verificacion_verificacion_id_verificaciones");
        builder.HasOne(e => e.Archivo).WithMany().HasForeignKey(e => e.ArchivoId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_documentos_verificacion_archivo_id_archivos");
        builder.HasIndex(e => e.ArchivoId).HasDatabaseName("ix_documentos_verificacion_archivo_id");
    }
}
