using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using update.Domain.Entities;

namespace update.Infrastructure.Data.Configurations;

public sealed class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("Usuarios");

        builder.HasKey(usuario => usuario.Id);

        builder.Property(usuario => usuario.Id)
            .ValueGeneratedOnAdd();

        builder.Property(usuario => usuario.TipoPerfil)
            .IsRequired()
            .HasConversion<int>()
            .HasDefaultValue(Domain.Enums.TipoPerfil.Persona);

        builder.Property(usuario => usuario.Correo)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(usuario => usuario.HashContrasena)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(usuario => usuario.RolPlataforma)
            .IsRequired()
            .HasConversion<int>()
            .HasDefaultValue(Domain.Enums.RolPlataforma.Usuario);

        builder.Property(usuario => usuario.CorreoVerificadoEn)
            .IsRequired(false);

        builder.Property(usuario => usuario.TerminosAceptadosEn)
            .IsRequired();

        builder.Property(usuario => usuario.UltimoIngresoEn)
            .IsRequired(false);

        builder.Property(usuario => usuario.IntentosFallidos)
            .IsRequired()
            .HasDefaultValue((short)0);

        builder.Property(usuario => usuario.BloqueadoHasta)
            .IsRequired(false);

        builder.Property(usuario => usuario.ContrasenaCambiadaEn)
            .IsRequired(false);

        builder.HasIndex(usuario => usuario.Correo)
            .IsUnique()
            .HasDatabaseName("IX_Usuarios_Correo");

        builder.Property(usuario => usuario.CreadoEn)
            .IsRequired();

        builder.Property(usuario => usuario.ActualizadoEn)
            .IsRequired(false);
    }
}
