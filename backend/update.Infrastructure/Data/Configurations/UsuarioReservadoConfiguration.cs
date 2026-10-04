using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Infrastructure.Data.Configurations;

/// <summary>Mapea la tabla usuarios_reservados y sus restricciones.</summary>
public sealed class UsuarioReservadoConfiguration : IEntityTypeConfiguration<UsuarioReservado>
{
    public void Configure(EntityTypeBuilder<UsuarioReservado> builder)
    {
        builder.ToTable("usuarios_reservados", tabla =>
        {
            tabla.HasCheckConstraint("ck_usuarios_reservados_nombre_usuario_longitud", "char_length(\"nombre_usuario\") <= 50");
        });
        builder.HasBaseType((Type?)null);
        builder.HasKey(e => e.NombreUsuario).HasName("pk_usuarios_reservados");
        builder.Property(e => e.NombreUsuario).HasColumnName("nombre_usuario").HasColumnType("text").IsRequired().HasMaxLength(50).ValueGeneratedNever();
    }
}
