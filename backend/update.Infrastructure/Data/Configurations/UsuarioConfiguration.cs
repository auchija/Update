using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Infrastructure.Data.Configurations;

/// <summary>Mapea la tabla usuarios y sus restricciones.</summary>
public sealed class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("usuarios", tabla =>
        {
            tabla.HasCheckConstraint("ck_usuarios_tipo_perfil_enum", "\"tipo_perfil\" IN ('persona', 'emprendimiento')");
            tabla.HasCheckConstraint("ck_usuarios_correo_longitud", "char_length(\"correo\") <= 254");
            tabla.HasCheckConstraint("ck_usuarios_hash_contrasena_longitud", "char_length(\"hash_contrasena\") <= 512");
            tabla.HasCheckConstraint("ck_usuarios_rol_plataforma_enum", "\"rol_plataforma\" IN ('usuario', 'moderador', 'administrador')");
            tabla.HasCheckConstraint("ck_usuarios_tipo_perfil", "\"tipo_perfil\" = 'persona'");
        });
        builder.HasBaseType((Type?)null);
        builder.HasKey(e => e.Id).HasName("pk_usuarios");
        builder.Property(e => e.Id).HasColumnName("id").HasColumnType("uuid").IsRequired().ValueGeneratedNever();
        builder.Property(e => e.TipoPerfil).HasColumnName("tipo_perfil").HasColumnType("text").IsRequired().HasMaxLength(14).HasConversion(new ConversorEnumDbml<TipoPerfil>()).HasDefaultValue(TipoPerfil.Persona).HasSentinel(TipoPerfil.Persona);
        builder.Property(e => e.Correo).HasColumnName("correo").HasColumnType("text").IsRequired().HasMaxLength(254);
        builder.Property(e => e.HashContrasena).HasColumnName("hash_contrasena").HasColumnType("text").HasMaxLength(512);
        builder.Property(e => e.RolPlataforma).HasColumnName("rol_plataforma").HasColumnType("text").IsRequired().HasMaxLength(13).HasConversion(new ConversorEnumDbml<RolPlataforma>()).HasDefaultValue(RolPlataforma.Usuario).HasSentinel(RolPlataforma.Usuario);
        builder.Property(e => e.CorreoVerificadoEn).HasColumnName("correo_verificado_en").HasColumnType("timestamptz");
        builder.Property(e => e.TerminosAceptadosEn).HasColumnName("terminos_aceptados_en").HasColumnType("timestamptz").IsRequired();
        builder.Property(e => e.UltimoIngresoEn).HasColumnName("ultimo_ingreso_en").HasColumnType("timestamptz");
        builder.Property(e => e.IntentosFallidos).HasColumnName("intentos_fallidos").HasColumnType("smallint").IsRequired().HasDefaultValue((short)0).HasSentinel((short)0);
        builder.Property(e => e.BloqueadoHasta).HasColumnName("bloqueado_hasta").HasColumnType("timestamptz");
        builder.Property(e => e.ContrasenaCambiadaEn).HasColumnName("contrasena_cambiada_en").HasColumnType("timestamptz");
        builder.Property(e => e.CreadoEn).HasColumnName("creado_en").HasColumnType("timestamptz").IsRequired().HasDefaultValueSql("now()");
        builder.Property(e => e.ActualizadoEn).HasColumnName("actualizado_en").HasColumnType("timestamptz").IsRequired().HasDefaultValueSql("now()");
        builder.HasOne(e => e.Perfil).WithOne().HasForeignKey<Usuario>(e => new { e.Id, e.TipoPerfil }).HasPrincipalKey<Perfil>(e => new { e.Id, e.Tipo }).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_usuarios_id_tipo_perfil_perfiles");
        builder.HasIndex(e => new { e.Id, e.TipoPerfil }).HasDatabaseName("ix_usuarios_id_tipo_perfil");
    }
}
