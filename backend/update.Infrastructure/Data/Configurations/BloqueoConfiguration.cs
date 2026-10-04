using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Infrastructure.Data.Configurations;

/// <summary>Mapea la tabla bloqueos y sus restricciones.</summary>
public sealed class BloqueoConfiguration : IEntityTypeConfiguration<Bloqueo>
{
    public void Configure(EntityTypeBuilder<Bloqueo> builder)
    {
        builder.ToTable("bloqueos", tabla =>
        {
        });
        builder.HasBaseType((Type?)null);
        builder.HasKey(e => new { e.UsuarioBloqueadorId, e.PerfilBloqueadoId }).HasName("pk_bloqueos");
        builder.Property(e => e.UsuarioBloqueadorId).HasColumnName("usuario_bloqueador_id").HasColumnType("uuid").IsRequired().ValueGeneratedNever();
        builder.Property(e => e.PerfilBloqueadoId).HasColumnName("perfil_bloqueado_id").HasColumnType("uuid").IsRequired().ValueGeneratedNever();
        builder.Property(e => e.CreadoEn).HasColumnName("creado_en").HasColumnType("timestamptz").IsRequired().HasDefaultValueSql("now()");
        builder.HasOne(e => e.UsuarioBloqueador).WithMany().HasForeignKey(e => e.UsuarioBloqueadorId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_bloqueos_usuario_bloqueador_id_usuarios");
        builder.HasOne(e => e.PerfilBloqueado).WithMany().HasForeignKey(e => e.PerfilBloqueadoId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_bloqueos_perfil_bloqueado_id_perfiles");
        builder.HasIndex(e => e.PerfilBloqueadoId).HasDatabaseName("ix_bloqueos_perfil_bloqueado_id");
    }
}
