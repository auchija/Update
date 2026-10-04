using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Infrastructure.Data.Configurations;

/// <summary>Mapea la tabla guardados y sus restricciones.</summary>
public sealed class GuardadoConfiguration : IEntityTypeConfiguration<Guardado>
{
    public void Configure(EntityTypeBuilder<Guardado> builder)
    {
        builder.ToTable("guardados", tabla =>
        {
            tabla.HasCheckConstraint("ck_guardados_un_objetivo", "(\"publicacion_id\" IS NOT NULL) <> (\"producto_id\" IS NOT NULL)");
        });
        builder.HasBaseType((Type?)null);
        builder.HasKey(e => e.Id).HasName("pk_guardados");
        builder.Ignore(e => e.ActualizadoEn);
        builder.Property(e => e.Id).HasColumnName("id").HasColumnType("uuid").IsRequired().HasDefaultValueSql("gen_random_uuid()");
        builder.Property(e => e.UsuarioId).HasColumnName("usuario_id").HasColumnType("uuid").IsRequired();
        builder.Property(e => e.PublicacionId).HasColumnName("publicacion_id").HasColumnType("uuid");
        builder.Property(e => e.ProductoId).HasColumnName("producto_id").HasColumnType("uuid");
        builder.Property(e => e.CreadoEn).HasColumnName("creado_en").HasColumnType("timestamptz").IsRequired().HasDefaultValueSql("now()");
        builder.HasOne(e => e.Usuario).WithMany().HasForeignKey(e => e.UsuarioId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_guardados_usuario_id_usuarios");
        builder.HasIndex(e => e.UsuarioId).HasDatabaseName("ix_guardados_usuario_id");
        builder.HasOne(e => e.Publicacion).WithMany().HasForeignKey(e => e.PublicacionId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_guardados_publicacion_id_publicaciones");
        builder.HasIndex(e => e.PublicacionId).HasDatabaseName("ix_guardados_publicacion_id");
        builder.HasOne(e => e.Producto).WithMany().HasForeignKey(e => e.ProductoId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_guardados_producto_id_productos");
        builder.HasIndex(e => e.ProductoId).HasDatabaseName("ix_guardados_producto_id");
    }
}
