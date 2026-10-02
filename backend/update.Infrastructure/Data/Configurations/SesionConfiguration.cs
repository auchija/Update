using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Infrastructure.Data.Configurations;

/// <summary>Mapea la tabla sesiones y sus restricciones.</summary>
public sealed class SesionConfiguration : IEntityTypeConfiguration<Sesion>
{
    public void Configure(EntityTypeBuilder<Sesion> builder)
    {
        builder.ToTable("sesiones", tabla =>
        {
            tabla.HasCheckConstraint("ck_sesiones_hash_token_refresco_longitud", "char_length(\"hash_token_refresco\") <= 256");
            tabla.HasCheckConstraint("ck_sesiones_agente_usuario_longitud", "char_length(\"agente_usuario\") <= 255");
            tabla.HasCheckConstraint("ck_sesiones_motivo_revocacion_longitud", "char_length(\"motivo_revocacion\") <= 255");
        });
        builder.HasBaseType((Type?)null);
        builder.HasKey(e => e.Id).HasName("pk_sesiones");
        builder.Ignore(e => e.ActualizadoEn);
        builder.Property(e => e.Id).HasColumnName("id").HasColumnType("uuid").IsRequired().HasDefaultValueSql("gen_random_uuid()");
        builder.Property(e => e.UsuarioId).HasColumnName("usuario_id").HasColumnType("uuid").IsRequired();
        builder.Property(e => e.HashTokenRefresco).HasColumnName("hash_token_refresco").HasColumnType("text").IsRequired().HasMaxLength(256);
        builder.HasIndex(e => e.HashTokenRefresco).IsUnique().HasDatabaseName("ux_sesiones_hash_token_refresco");
        builder.Property(e => e.FamiliaId).HasColumnName("familia_id").HasColumnType("uuid").IsRequired();
        builder.Property(e => e.ReemplazadoPorId).HasColumnName("reemplazado_por_id").HasColumnType("uuid");
        builder.Property(e => e.AgenteUsuario).HasColumnName("agente_usuario").HasColumnType("text").HasMaxLength(255);
        builder.Property(e => e.DireccionIp).HasColumnName("direccion_ip").HasColumnType("inet");
        builder.Property(e => e.CreadoEn).HasColumnName("creado_en").HasColumnType("timestamptz").IsRequired().HasDefaultValueSql("now()");
        builder.Property(e => e.UltimoUsoEn).HasColumnName("ultimo_uso_en").HasColumnType("timestamptz").IsRequired().HasDefaultValueSql("now()");
        builder.Property(e => e.ExpiraEn).HasColumnName("expira_en").HasColumnType("timestamptz").IsRequired();
        builder.Property(e => e.RevocadoEn).HasColumnName("revocado_en").HasColumnType("timestamptz");
        builder.Property(e => e.MotivoRevocacion).HasColumnName("motivo_revocacion").HasColumnType("text").HasMaxLength(255);
        builder.HasOne(e => e.Usuario).WithMany().HasForeignKey(e => e.UsuarioId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_sesiones_usuario_id_usuarios");
        builder.HasIndex(e => e.UsuarioId).HasDatabaseName("ix_sesiones_usuario_id");
        builder.HasOne(e => e.ReemplazadoPor).WithMany().HasForeignKey(e => e.ReemplazadoPorId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_sesiones_reemplazado_por_id_sesiones");
        builder.HasIndex(e => e.ReemplazadoPorId).HasDatabaseName("ix_sesiones_reemplazado_por_id");
    }
}
