using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Infrastructure.Data.Configurations;

/// <summary>Mapea la tabla miembros_emprendimiento y sus restricciones.</summary>
public sealed class MiembroEmprendimientoConfiguration : IEntityTypeConfiguration<MiembroEmprendimiento>
{
    public void Configure(EntityTypeBuilder<MiembroEmprendimiento> builder)
    {
        builder.ToTable("miembros_emprendimiento", tabla =>
        {
            tabla.HasCheckConstraint("ck_miembros_emprendimiento_rol_enum", "\"rol\" IN ('propietario', 'administrador', 'editor', 'soporte', 'miembro')");
            tabla.HasCheckConstraint("ck_miembros_emprendimiento_titulo_longitud", "char_length(\"titulo\") <= 255");
            tabla.HasCheckConstraint("ck_miembros_emprendimiento_estado_enum", "\"estado\" IN ('invitado', 'activo', 'retirado', 'expulsado')");
        });
        builder.HasBaseType((Type?)null);
        builder.HasKey(e => new { e.EmprendimientoId, e.UsuarioId }).HasName("pk_miembros_emprendimiento");
        builder.Property(e => e.EmprendimientoId).HasColumnName("emprendimiento_id").HasColumnType("uuid").IsRequired().ValueGeneratedNever();
        builder.Property(e => e.UsuarioId).HasColumnName("usuario_id").HasColumnType("uuid").IsRequired().ValueGeneratedNever();
        builder.Property(e => e.Rol).HasColumnName("rol").HasColumnType("text").IsRequired().HasMaxLength(13).HasConversion(new ConversorEnumDbml<RolEmprendimiento>()).HasDefaultValue(RolEmprendimiento.Miembro).HasSentinel(RolEmprendimiento.Miembro);
        builder.Property(e => e.Titulo).HasColumnName("titulo").HasColumnType("text").HasMaxLength(255);
        builder.Property(e => e.Estado).HasColumnName("estado").HasColumnType("text").IsRequired().HasMaxLength(9).HasConversion(new ConversorEnumDbml<EstadoMiembro>()).HasDefaultValue(EstadoMiembro.Invitado).HasSentinel(EstadoMiembro.Invitado);
        builder.Property(e => e.InvitadoPorUsuarioId).HasColumnName("invitado_por_usuario_id").HasColumnType("uuid");
        builder.Property(e => e.CreadoEn).HasColumnName("creado_en").HasColumnType("timestamptz").IsRequired().HasDefaultValueSql("now()");
        builder.Property(e => e.UnidoEn).HasColumnName("unido_en").HasColumnType("timestamptz");
        builder.Property(e => e.ActualizadoEn).HasColumnName("actualizado_en").HasColumnType("timestamptz").IsRequired().HasDefaultValueSql("now()");
        builder.HasOne(e => e.Emprendimiento).WithMany().HasForeignKey(e => e.EmprendimientoId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_miembros_emprendimiento_emprendimiento_id_emprendimientos");
        builder.HasOne(e => e.Usuario).WithMany().HasForeignKey(e => e.UsuarioId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_miembros_emprendimiento_usuario_id_usuarios");
        builder.HasIndex(e => e.UsuarioId).HasDatabaseName("ix_miembros_emprendimiento_usuario_id");
        builder.HasOne(e => e.InvitadoPorUsuario).WithMany().HasForeignKey(e => e.InvitadoPorUsuarioId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_miembros_emprendimiento_invitado_por_usuario_id_usuarios");
        builder.HasIndex(e => e.InvitadoPorUsuarioId).HasDatabaseName("ix_miembros_emprendimiento_invitado_por_usuario_id");
        builder.HasIndex(e => new { e.Estado, e.CreadoEn }).HasDatabaseName("ix_miembros_emprendimiento_estado_creado_en");
    }
}
