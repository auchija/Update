using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Infrastructure.Data.Configurations;

/// <summary>Mapea la tabla seguimientos y sus restricciones.</summary>
public sealed class SeguimientoConfiguration : IEntityTypeConfiguration<Seguimiento>
{
    public void Configure(EntityTypeBuilder<Seguimiento> builder)
    {
        builder.ToTable("seguimientos", tabla =>
        {
            tabla.HasCheckConstraint("ck_seguimientos_estado_enum", "\"estado\" IN ('pendiente', 'activo')");
        });
        builder.HasBaseType((Type?)null);
        builder.HasKey(e => new { e.UsuarioSeguidorId, e.PerfilSeguidoId }).HasName("pk_seguimientos");
        builder.Property(e => e.UsuarioSeguidorId).HasColumnName("usuario_seguidor_id").HasColumnType("uuid").IsRequired().ValueGeneratedNever();
        builder.Property(e => e.PerfilSeguidoId).HasColumnName("perfil_seguido_id").HasColumnType("uuid").IsRequired().ValueGeneratedNever();
        builder.Property(e => e.Estado).HasColumnName("estado").HasColumnType("text").IsRequired().HasMaxLength(9).HasConversion(new ConversorEnumDbml<EstadoSeguimiento>()).HasDefaultValue(EstadoSeguimiento.Activo).HasSentinel(EstadoSeguimiento.Activo);
        builder.Property(e => e.CreadoEn).HasColumnName("creado_en").HasColumnType("timestamptz").IsRequired().HasDefaultValueSql("now()");
        builder.Property(e => e.AceptadoEn).HasColumnName("aceptado_en").HasColumnType("timestamptz");
        builder.HasOne(e => e.UsuarioSeguidor).WithMany().HasForeignKey(e => e.UsuarioSeguidorId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_seguimientos_usuario_seguidor_id_usuarios");
        builder.HasOne(e => e.PerfilSeguido).WithMany().HasForeignKey(e => e.PerfilSeguidoId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_seguimientos_perfil_seguido_id_perfiles");
        builder.HasIndex(e => e.PerfilSeguidoId).HasDatabaseName("ix_seguimientos_perfil_seguido_id");
        builder.HasIndex(e => new { e.Estado, e.CreadoEn }).HasDatabaseName("ix_seguimientos_estado_creado_en");
    }
}
