using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Infrastructure.Data.Configurations;

/// <summary>Mapea la tabla departamentos y sus restricciones.</summary>
public sealed class DepartamentoConfiguration : IEntityTypeConfiguration<Departamento>
{
    public void Configure(EntityTypeBuilder<Departamento> builder)
    {
        builder.ToTable("departamentos", tabla =>
        {
            tabla.HasCheckConstraint("ck_departamentos_nombre_longitud", "char_length(\"nombre\") <= 255");
            tabla.HasCheckConstraint("ck_departamentos_codigo_longitud", "char_length(\"codigo\") <= 64");
        });
        builder.HasBaseType((Type?)null);
        builder.HasKey(e => e.Id).HasName("pk_departamentos");
        builder.Property(e => e.Id).HasColumnName("id").HasColumnType("integer").IsRequired().UseIdentityAlwaysColumn();
        builder.Property(e => e.PaisCodigo).HasColumnName("pais_codigo").HasColumnType("character(2)").IsRequired().HasMaxLength(2);
        builder.Property(e => e.Nombre).HasColumnName("nombre").HasColumnType("text").IsRequired().HasMaxLength(255);
        builder.Property(e => e.Codigo).HasColumnName("codigo").HasColumnType("text").HasMaxLength(64);
        builder.HasOne(e => e.Pais).WithMany().HasForeignKey(e => e.PaisCodigo).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_departamentos_pais_codigo_paises");
        builder.HasIndex(e => e.PaisCodigo).HasDatabaseName("ix_departamentos_pais_codigo");
    }
}
