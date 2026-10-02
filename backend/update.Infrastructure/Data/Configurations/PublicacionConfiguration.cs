using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Infrastructure.Data.Configurations;

/// <summary>Mapea la tabla publicaciones y sus restricciones.</summary>
public sealed class PublicacionConfiguration : IEntityTypeConfiguration<Publicacion>
{
    public void Configure(EntityTypeBuilder<Publicacion> builder)
    {
        builder.ToTable("publicaciones", tabla =>
        {
            tabla.HasCheckConstraint("ck_publicaciones_tipo_publicacion_codigo_longitud", "char_length(\"tipo_publicacion_codigo\") <= 64");
            tabla.HasCheckConstraint("ck_publicaciones_contenido_longitud", "char_length(\"contenido\") <= 10000");
            tabla.HasCheckConstraint("ck_publicaciones_visibilidad_enum", "\"visibilidad\" IN ('publico', 'seguidores')");
        });
        builder.HasBaseType((Type?)null);
        builder.HasKey(e => e.Id).HasName("pk_publicaciones");
        builder.Property(e => e.Id).HasColumnName("id").HasColumnType("uuid").IsRequired().HasDefaultValueSql("gen_random_uuid()");
        builder.Property(e => e.PerfilAutorId).HasColumnName("perfil_autor_id").HasColumnType("uuid").IsRequired();
        builder.Property(e => e.CreadoPorUsuarioId).HasColumnName("creado_por_usuario_id").HasColumnType("uuid").IsRequired();
        builder.Property(e => e.TipoPublicacionCodigo).HasColumnName("tipo_publicacion_codigo").HasColumnType("text").IsRequired().HasMaxLength(64).HasDefaultValue("general");
        builder.Property(e => e.Contenido).HasColumnName("contenido").HasColumnType("text").IsRequired().HasMaxLength(10000).HasDefaultValue("");
        builder.Property(e => e.Visibilidad).HasColumnName("visibilidad").HasColumnType("text").IsRequired().HasMaxLength(10).HasConversion(new ConversorEnumDbml<VisibilidadPublicacion>()).HasDefaultValue(VisibilidadPublicacion.Publico).HasSentinel(VisibilidadPublicacion.Publico);
        builder.Property(e => e.ProductoId).HasColumnName("producto_id").HasColumnType("uuid");
        builder.Property(e => e.OportunidadId).HasColumnName("oportunidad_id").HasColumnType("uuid");
        builder.Property(e => e.PublicacionCompartidaId).HasColumnName("publicacion_compartida_id").HasColumnType("uuid");
        builder.Property(e => e.ComentarioAceptadoId).HasColumnName("comentario_aceptado_id").HasColumnType("uuid");
        builder.Property(e => e.ComentariosHabilitados).HasColumnName("comentarios_habilitados").HasColumnType("boolean").IsRequired().HasDefaultValue(true).HasSentinel(true);
        builder.Property(e => e.Fijado).HasColumnName("fijado").HasColumnType("boolean").IsRequired().HasDefaultValue(false).HasSentinel(false);
        builder.Property(e => e.TotalReacciones).HasColumnName("total_reacciones").HasColumnType("integer").IsRequired().HasDefaultValue(0).HasSentinel(0);
        builder.Property(e => e.TotalComentarios).HasColumnName("total_comentarios").HasColumnType("integer").IsRequired().HasDefaultValue(0).HasSentinel(0);
        builder.Property(e => e.TotalCompartidos).HasColumnName("total_compartidos").HasColumnType("integer").IsRequired().HasDefaultValue(0).HasSentinel(0);
        builder.Property(e => e.TotalGuardados).HasColumnName("total_guardados").HasColumnType("integer").IsRequired().HasDefaultValue(0).HasSentinel(0);
        builder.Property(e => e.PuntajeDescubrimiento).HasColumnName("puntaje_descubrimiento").HasColumnType("float8").IsRequired().HasDefaultValue(0d).HasSentinel(0d);
        builder.Property(e => e.PuntajeActualizadoEn).HasColumnName("puntaje_actualizado_en").HasColumnType("timestamptz");
        builder.Property(e => e.EditadoEn).HasColumnName("editado_en").HasColumnType("timestamptz");
        builder.Property(e => e.CreadoEn).HasColumnName("creado_en").HasColumnType("timestamptz").IsRequired().HasDefaultValueSql("now()");
        builder.Property(e => e.ActualizadoEn).HasColumnName("actualizado_en").HasColumnType("timestamptz").IsRequired().HasDefaultValueSql("now()");
        builder.Property(e => e.EliminadoEn).HasColumnName("eliminado_en").HasColumnType("timestamptz");
        builder.HasOne(e => e.PerfilAutor).WithMany().HasForeignKey(e => e.PerfilAutorId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_publicaciones_perfil_autor_id_perfiles");
        builder.HasIndex(e => e.PerfilAutorId).HasDatabaseName("ix_publicaciones_perfil_autor_id");
        builder.HasOne(e => e.CreadoPorUsuario).WithMany().HasForeignKey(e => e.CreadoPorUsuarioId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_publicaciones_creado_por_usuario_id_usuarios");
        builder.HasIndex(e => e.CreadoPorUsuarioId).HasDatabaseName("ix_publicaciones_creado_por_usuario_id");
        builder.HasOne(e => e.TipoPublicacion).WithMany().HasForeignKey(e => e.TipoPublicacionCodigo).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_publicaciones_tipo_publicacion_codigo_tipos_publicacion");
        builder.HasIndex(e => e.TipoPublicacionCodigo).HasDatabaseName("ix_publicaciones_tipo_publicacion_codigo");
        builder.HasOne(e => e.Producto).WithMany().HasForeignKey(e => e.ProductoId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_publicaciones_producto_id_productos");
        builder.HasIndex(e => e.ProductoId).HasDatabaseName("ix_publicaciones_producto_id");
        builder.HasOne(e => e.Oportunidad).WithMany().HasForeignKey(e => e.OportunidadId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_publicaciones_oportunidad_id_oportunidades");
        builder.HasIndex(e => e.OportunidadId).HasDatabaseName("ix_publicaciones_oportunidad_id");
        builder.HasOne(e => e.PublicacionCompartida).WithMany().HasForeignKey(e => e.PublicacionCompartidaId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_publicaciones_publicacion_compartida_id_publicaciones");
        builder.HasIndex(e => e.PublicacionCompartidaId).HasDatabaseName("ix_publicaciones_publicacion_compartida_id");
        builder.HasOne(e => e.ComentarioAceptado).WithMany().HasForeignKey(e => new { e.ComentarioAceptadoId, e.Id }).HasPrincipalKey(e => new { e.Id, e.PublicacionId }).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_publicaciones_comentario_aceptado_id_id_comentarios");
        builder.HasIndex(e => new { e.ComentarioAceptadoId, e.Id }).HasDatabaseName("ix_publicaciones_comentario_aceptado_id_id");
        builder.HasQueryFilter(e => e.EliminadoEn == null);
    }
}
