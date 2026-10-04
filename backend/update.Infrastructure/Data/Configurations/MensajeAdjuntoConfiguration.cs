using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Infrastructure.Data.Configurations;

/// <summary>Mapea la tabla mensajes_adjuntos y sus restricciones.</summary>
public sealed class MensajeAdjuntoConfiguration : IEntityTypeConfiguration<MensajeAdjunto>
{
    public void Configure(EntityTypeBuilder<MensajeAdjunto> builder)
    {
        builder.ToTable("mensajes_adjuntos", tabla =>
        {
        });
        builder.HasBaseType((Type?)null);
        builder.HasKey(e => new { e.MensajeId, e.ArchivoId }).HasName("pk_mensajes_adjuntos");
        builder.Property(e => e.MensajeId).HasColumnName("mensaje_id").HasColumnType("uuid").IsRequired().ValueGeneratedNever();
        builder.Property(e => e.ArchivoId).HasColumnName("archivo_id").HasColumnType("uuid").IsRequired().ValueGeneratedNever();
        builder.Property(e => e.Posicion).HasColumnName("posicion").HasColumnType("smallint").IsRequired();
        builder.HasOne(e => e.Mensaje).WithMany().HasForeignKey(e => e.MensajeId).OnDelete(DeleteBehavior.Cascade).HasConstraintName("fk_mensajes_adjuntos_mensaje_id_mensajes");
        builder.HasOne(e => e.Archivo).WithMany().HasForeignKey(e => e.ArchivoId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_mensajes_adjuntos_archivo_id_archivos");
        builder.HasIndex(e => e.ArchivoId).HasDatabaseName("ix_mensajes_adjuntos_archivo_id");
    }
}
