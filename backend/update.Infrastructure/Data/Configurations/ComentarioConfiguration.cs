using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Infrastructure.Data.Configurations;

/// <summary>Mapea la tabla comentarios y sus restricciones.</summary>
public sealed class ComentarioConfiguration : IEntityTypeConfiguration<Comentario>
{
    public void Configure(EntityTypeBuilder<Comentario> builder)
    {
        builder.ToTable("comentarios", tabla =>
        {
            tabla.HasCheckConstraint("ck_comentarios_contenido_longitud", "char_length(\"contenido\") <= 10000");
        });
        builder.HasBaseType((Type?)null);
        builder.HasKey(e => e.Id).HasName("pk_comentarios");
        builder.Property(e => e.Id).HasColumnName("id").HasColumnType("uuid").IsRequired().HasDefaultValueSql("gen_random_uuid()");
        builder.Property(e => e.PublicacionId).HasColumnName("publicacion_id").HasColumnType("uuid").IsRequired();
        builder.Property(e => e.ComentarioPadreId).HasColumnName("comentario_padre_id").HasColumnType("uuid");
        builder.Property(e => e.PerfilAutorId).HasColumnName("perfil_autor_id").HasColumnType("uuid").IsRequired();
        builder.Property(e => e.CreadoPorUsuarioId).HasColumnName("creado_por_usuario_id").HasColumnType("uuid").IsRequired();
        builder.Property(e => e.RespondeAPerfilId).HasColumnName("responde_a_perfil_id").HasColumnType("uuid");
        builder.Property(e => e.Contenido).HasColumnName("contenido").HasColumnType("text").IsRequired().HasMaxLength(10000);
        builder.Property(e => e.TotalReacciones).HasColumnName("total_reacciones").HasColumnType("integer").IsRequired().HasDefaultValue(0).HasSentinel(0);
        builder.Property(e => e.TotalRespuestas).HasColumnName("total_respuestas").HasColumnType("integer").IsRequired().HasDefaultValue(0).HasSentinel(0);
        builder.Property(e => e.EditadoEn).HasColumnName("editado_en").HasColumnType("timestamptz");
        builder.Property(e => e.CreadoEn).HasColumnName("creado_en").HasColumnType("timestamptz").IsRequired().HasDefaultValueSql("now()");
        builder.Property(e => e.ActualizadoEn).HasColumnName("actualizado_en").HasColumnType("timestamptz").IsRequired().HasDefaultValueSql("now()");
        builder.Property(e => e.EliminadoEn).HasColumnName("eliminado_en").HasColumnType("timestamptz");
        builder.HasOne(e => e.Publicacion).WithMany().HasForeignKey(e => e.PublicacionId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_comentarios_publicacion_id_publicaciones");
        builder.HasIndex(e => e.PublicacionId).HasDatabaseName("ix_comentarios_publicacion_id");
        builder.HasOne(e => e.PerfilAutor).WithMany().HasForeignKey(e => e.PerfilAutorId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_comentarios_perfil_autor_id_perfiles");
        builder.HasIndex(e => e.PerfilAutorId).HasDatabaseName("ix_comentarios_perfil_autor_id");
        builder.HasOne(e => e.CreadoPorUsuario).WithMany().HasForeignKey(e => e.CreadoPorUsuarioId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_comentarios_creado_por_usuario_id_usuarios");
        builder.HasIndex(e => e.CreadoPorUsuarioId).HasDatabaseName("ix_comentarios_creado_por_usuario_id");
        builder.HasOne(e => e.RespondeAPerfil).WithMany().HasForeignKey(e => e.RespondeAPerfilId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_comentarios_responde_a_perfil_id_perfiles");
        builder.HasIndex(e => e.RespondeAPerfilId).HasDatabaseName("ix_comentarios_responde_a_perfil_id");
        builder.HasOne(e => e.ComentarioPadre).WithMany().HasForeignKey(e => new { e.ComentarioPadreId, e.PublicacionId }).HasPrincipalKey(e => new { e.Id, e.PublicacionId }).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_comentarios_comentario_padre_id_publicacion_id_comentarios");
        builder.HasIndex(e => new { e.ComentarioPadreId, e.PublicacionId }).HasDatabaseName("ix_comentarios_comentario_padre_id_publicacion_id");
        builder.HasQueryFilter(e => e.EliminadoEn == null);
    }
}
