using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using update.Domain.Entities;

namespace update.Infrastructure.Data.Configurations;

/// <summary>Mapea la tabla refresh_tokens y sus restricciones.</summary>
public sealed class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("refresh_tokens", tabla =>
        {
        });

        builder.HasBaseType((Type?)null);

        builder.HasKey(e => e.Id)
            .HasName("pk_refresh_tokens");

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .HasColumnType("uuid")
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(e => e.UsuarioId)
            .HasColumnName("usuario_id")
            .HasColumnType("uuid")
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(e => e.Hash)
            .HasColumnName("hash")
            .HasColumnType("text")
            .IsRequired();

        builder.Property(e => e.ExpiraEn)
            .HasColumnName("expira_en")
            .HasColumnType("timestamp with time zone")
            .IsRequired();

        builder.Property(e => e.RevocadoEn)
            .HasColumnName("revocado_en")
            .HasColumnType("timestamp with time zone");

        builder.HasIndex(e => e.UsuarioId)
            .HasDatabaseName("ix_refresh_tokens_usuario_id");
    }
}
