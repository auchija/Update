using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Infrastructure.Data.Configurations;

/// <summary>Mapea la tabla intereses_usuario y sus restricciones.</summary>
public sealed class InteresUsuarioConfiguration : IEntityTypeConfiguration<InteresUsuario>
{
    public void Configure(EntityTypeBuilder<InteresUsuario> builder)
    {
        builder.ToTable("intereses_usuario", tabla =>
        {
        });
        builder.HasBaseType((Type?)null);
        builder.HasKey(e => new { e.UsuarioId, e.CategoriaId }).HasName("pk_intereses_usuario");
        builder.Property(e => e.UsuarioId).HasColumnName("usuario_id").HasColumnType("uuid").IsRequired().ValueGeneratedNever();
        builder.Property(e => e.CategoriaId).HasColumnName("categoria_id").HasColumnType("integer").IsRequired().ValueGeneratedNever();
        builder.Property(e => e.CreadoEn).HasColumnName("creado_en").HasColumnType("timestamptz").IsRequired().HasDefaultValueSql("now()");
        builder.HasOne(e => e.Usuario).WithMany().HasForeignKey(e => e.UsuarioId).OnDelete(DeleteBehavior.Cascade).HasConstraintName("fk_intereses_usuario_usuario_id_usuarios");
        builder.HasOne(e => e.Categoria).WithMany().HasForeignKey(e => e.CategoriaId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_intereses_usuario_categoria_id_categorias");
        builder.HasIndex(e => e.CategoriaId).HasDatabaseName("ix_intereses_usuario_categoria_id");
    }
}
