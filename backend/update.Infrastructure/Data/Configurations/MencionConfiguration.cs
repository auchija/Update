using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Infrastructure.Data.Configurations;

/// <summary>Mapea la tabla menciones y sus restricciones.</summary>
public sealed class MencionConfiguration : IEntityTypeConfiguration<Mencion>
{
    public void Configure(EntityTypeBuilder<Mencion> builder)
    {
        builder.ToTable("menciones", tabla =>
        {
        });
        builder.HasBaseType((Type?)null);
        builder.HasKey(e => new { e.PublicacionId, e.PerfilId }).HasName("pk_menciones");
        builder.Property(e => e.PublicacionId).HasColumnName("publicacion_id").HasColumnType("uuid").IsRequired().ValueGeneratedNever();
        builder.Property(e => e.PerfilId).HasColumnName("perfil_id").HasColumnType("uuid").IsRequired().ValueGeneratedNever();
        builder.HasOne(e => e.Publicacion).WithMany().HasForeignKey(e => e.PublicacionId).OnDelete(DeleteBehavior.Cascade).HasConstraintName("fk_menciones_publicacion_id_publicaciones");
        builder.HasOne(e => e.Perfil).WithMany().HasForeignKey(e => e.PerfilId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_menciones_perfil_id_perfiles");
        builder.HasIndex(e => e.PerfilId).HasDatabaseName("ix_menciones_perfil_id");
    }
}
