using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Infrastructure.Data.Configurations;

/// <summary>Mapea la tabla tokens_recuperacion y sus restricciones.</summary>
public sealed class TokenRecuperacionConfiguration : IEntityTypeConfiguration<TokenRecuperacion>
{
    public void Configure(EntityTypeBuilder<TokenRecuperacion> builder)
    {
        builder.ToTable("tokens_recuperacion", tabla =>
        {
            tabla.HasCheckConstraint("ck_tokens_recuperacion_hash_token_longitud", "char_length(\"hash_token\") <= 256");
        });
        builder.HasBaseType((Type?)null);
        builder.HasKey(e => e.Id).HasName("pk_tokens_recuperacion");
        builder.Ignore(e => e.ActualizadoEn);
        builder.Property(e => e.Id).HasColumnName("id").HasColumnType("uuid").IsRequired().HasDefaultValueSql("gen_random_uuid()");
        builder.Property(e => e.UsuarioId).HasColumnName("usuario_id").HasColumnType("uuid").IsRequired();
        builder.Property(e => e.HashToken).HasColumnName("hash_token").HasColumnType("text").IsRequired().HasMaxLength(256);
        builder.HasIndex(e => e.HashToken).IsUnique().HasDatabaseName("ux_tokens_recuperacion_hash_token");
        builder.Property(e => e.IpSolicitud).HasColumnName("ip_solicitud").HasColumnType("inet");
        builder.Property(e => e.CreadoEn).HasColumnName("creado_en").HasColumnType("timestamptz").IsRequired().HasDefaultValueSql("now()");
        builder.Property(e => e.ExpiraEn).HasColumnName("expira_en").HasColumnType("timestamptz").IsRequired();
        builder.Property(e => e.UsadoEn).HasColumnName("usado_en").HasColumnType("timestamptz");
        builder.HasOne(e => e.Usuario).WithMany().HasForeignKey(e => e.UsuarioId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_tokens_recuperacion_usuario_id_usuarios");
        builder.HasIndex(e => e.UsuarioId).HasDatabaseName("ix_tokens_recuperacion_usuario_id");
    }
}
