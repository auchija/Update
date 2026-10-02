using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Infrastructure.Data.Configurations;

/// <summary>Mapea la tabla enlaces_emprendimiento y sus restricciones.</summary>
public sealed class EnlaceEmprendimientoConfiguration : IEntityTypeConfiguration<EnlaceEmprendimiento>
{
    public void Configure(EntityTypeBuilder<EnlaceEmprendimiento> builder)
    {
        builder.ToTable("enlaces_emprendimiento", tabla =>
        {
            tabla.HasCheckConstraint("ck_enlaces_emprendimiento_plataforma_longitud", "char_length(\"plataforma\") <= 64");
            tabla.HasCheckConstraint("ck_enlaces_emprendimiento_url_longitud", "char_length(\"url\") <= 2048");
        });
        builder.HasBaseType((Type?)null);
        builder.HasKey(e => e.Id).HasName("pk_enlaces_emprendimiento");
        builder.Ignore(e => e.CreadoEn);
        builder.Ignore(e => e.ActualizadoEn);
        builder.Property(e => e.Id).HasColumnName("id").HasColumnType("uuid").IsRequired().HasDefaultValueSql("gen_random_uuid()");
        builder.Property(e => e.EmprendimientoId).HasColumnName("emprendimiento_id").HasColumnType("uuid").IsRequired();
        builder.Property(e => e.Plataforma).HasColumnName("plataforma").HasColumnType("text").IsRequired().HasMaxLength(64);
        builder.Property(e => e.Url).HasColumnName("url").HasColumnType("text").IsRequired().HasMaxLength(2048);
        builder.Property(e => e.Orden).HasColumnName("orden").HasColumnType("smallint").IsRequired().HasDefaultValue((short)0).HasSentinel((short)0);
        builder.HasOne(e => e.Emprendimiento).WithMany().HasForeignKey(e => e.EmprendimientoId).OnDelete(DeleteBehavior.Cascade).HasConstraintName("fk_enlaces_emprendimiento_emprendimiento_id_emprendimientos");
        builder.HasIndex(e => e.EmprendimientoId).HasDatabaseName("ix_enlaces_emprendimiento_emprendimiento_id");
    }
}
