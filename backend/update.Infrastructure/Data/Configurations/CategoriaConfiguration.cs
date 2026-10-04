using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Infrastructure.Data.Configurations;

/// <summary>Mapea la tabla categorias y sus restricciones.</summary>
public sealed class CategoriaConfiguration : IEntityTypeConfiguration<Categoria>
{
    public void Configure(EntityTypeBuilder<Categoria> builder)
    {
        builder.ToTable("categorias", tabla =>
        {
            tabla.HasCheckConstraint("ck_categorias_slug_longitud", "char_length(\"slug\") <= 255");
            tabla.HasCheckConstraint("ck_categorias_nombre_longitud", "char_length(\"nombre\") <= 255");
            tabla.HasCheckConstraint("ck_categorias_icono_longitud", "char_length(\"icono\") <= 255");
        });
        builder.HasBaseType((Type?)null);
        builder.HasKey(e => e.Id).HasName("pk_categorias");
        builder.Property(e => e.Id).HasColumnName("id").HasColumnType("integer").IsRequired().UseIdentityAlwaysColumn();
        builder.Property(e => e.PadreId).HasColumnName("padre_id").HasColumnType("integer");
        builder.Property(e => e.Slug).HasColumnName("slug").HasColumnType("text").IsRequired().HasMaxLength(255);
        builder.HasIndex(e => e.Slug).IsUnique().HasDatabaseName("ux_categorias_slug");
        builder.Property(e => e.Nombre).HasColumnName("nombre").HasColumnType("text").IsRequired().HasMaxLength(255);
        builder.Property(e => e.Icono).HasColumnName("icono").HasColumnType("text").HasMaxLength(255);
        builder.Property(e => e.Orden).HasColumnName("orden").HasColumnType("smallint").IsRequired().HasDefaultValue((short)0).HasSentinel((short)0);
        builder.Property(e => e.Activo).HasColumnName("activo").HasColumnType("boolean").IsRequired().HasDefaultValue(true).HasSentinel(true);
        builder.HasOne(e => e.Padre).WithMany().HasForeignKey(e => e.PadreId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_categorias_padre_id_categorias");
        builder.HasIndex(e => e.PadreId).HasDatabaseName("ix_categorias_padre_id");
    }
}
