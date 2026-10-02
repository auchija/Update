using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Infrastructure.Data.Configurations;

/// <summary>Mapea la tabla permisos_rol y sus restricciones.</summary>
public sealed class PermisoRolConfiguration : IEntityTypeConfiguration<PermisoRol>
{
    public void Configure(EntityTypeBuilder<PermisoRol> builder)
    {
        builder.ToTable("permisos_rol", tabla =>
        {
            tabla.HasCheckConstraint("ck_permisos_rol_rol_enum", "\"rol\" IN ('propietario', 'administrador', 'editor', 'soporte', 'miembro')");
            tabla.HasCheckConstraint("ck_permisos_rol_permiso_longitud", "char_length(\"permiso\") <= 64");
        });
        builder.HasBaseType((Type?)null);
        builder.HasKey(e => new { e.Rol, e.Permiso }).HasName("pk_permisos_rol");
        builder.Property(e => e.Rol).HasColumnName("rol").HasColumnType("text").IsRequired().HasMaxLength(13).HasConversion(new ConversorEnumDbml<RolEmprendimiento>()).ValueGeneratedNever();
        builder.Property(e => e.Permiso).HasColumnName("permiso").HasColumnType("text").IsRequired().HasMaxLength(64).ValueGeneratedNever();
    }
}
