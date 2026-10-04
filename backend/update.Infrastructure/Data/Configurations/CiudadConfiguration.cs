using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Infrastructure.Data.Configurations;

/// <summary>Mapea la tabla ciudades y sus restricciones.</summary>
public sealed class CiudadConfiguration : IEntityTypeConfiguration<Ciudad>
{
    public void Configure(EntityTypeBuilder<Ciudad> builder)
    {
        builder.ToTable("ciudades", tabla =>
        {
            tabla.HasCheckConstraint("ck_ciudades_nombre_longitud", "char_length(\"nombre\") <= 255");
            tabla.HasCheckConstraint("ck_ciudades_codigo_longitud", "char_length(\"codigo\") <= 64");
        });
        builder.HasBaseType((Type?)null);
        builder.HasKey(e => e.Id).HasName("pk_ciudades");
        builder.Property(e => e.Id).HasColumnName("id").HasColumnType("integer").IsRequired().UseIdentityAlwaysColumn();
        builder.Property(e => e.DepartamentoId).HasColumnName("departamento_id").HasColumnType("integer").IsRequired();
        builder.Property(e => e.Nombre).HasColumnName("nombre").HasColumnType("text").IsRequired().HasMaxLength(255);
        builder.Property(e => e.Codigo).HasColumnName("codigo").HasColumnType("text").HasMaxLength(64);
        builder.Property(e => e.Latitud).HasColumnName("latitud").HasColumnType("numeric(9,6)").IsRequired().HasPrecision(9, 6);
        builder.Property(e => e.Longitud).HasColumnName("longitud").HasColumnType("numeric(9,6)").IsRequired().HasPrecision(9, 6);
        builder.HasOne(e => e.Departamento).WithMany().HasForeignKey(e => e.DepartamentoId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_ciudades_departamento_id_departamentos");
        builder.HasIndex(e => e.DepartamentoId).HasDatabaseName("ix_ciudades_departamento_id");
    }
}
