using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Infrastructure.Data.Configurations;

/// <summary>Mapea la tabla resenas y sus restricciones.</summary>
public sealed class ResenaConfiguration : IEntityTypeConfiguration<Resena>
{
    public void Configure(EntityTypeBuilder<Resena> builder)
    {
        builder.ToTable("resenas", tabla =>
        {
            tabla.HasCheckConstraint("ck_resenas_calificacion", "\"calificacion\" BETWEEN 1 AND 5");
            tabla.HasCheckConstraint("ck_resenas_titulo_longitud", "char_length(\"titulo\") <= 255");
            tabla.HasCheckConstraint("ck_resenas_contenido_longitud", "char_length(\"contenido\") <= 10000");
            tabla.HasCheckConstraint("ck_resenas_estado_enum", "\"estado\" IN ('publicado', 'oculto')");
            tabla.HasCheckConstraint("ck_resenas_respuesta_emprendimiento_longitud", "char_length(\"respuesta_emprendimiento\") <= 10000");
        });
        builder.HasBaseType((Type?)null);
        builder.HasKey(e => e.Id).HasName("pk_resenas");
        builder.Property(e => e.Id).HasColumnName("id").HasColumnType("uuid").IsRequired().HasDefaultValueSql("gen_random_uuid()");
        builder.Property(e => e.UsuarioResenadorId).HasColumnName("usuario_resenador_id").HasColumnType("uuid").IsRequired();
        builder.Property(e => e.EmprendimientoId).HasColumnName("emprendimiento_id").HasColumnType("uuid").IsRequired();
        builder.Property(e => e.ProductoId).HasColumnName("producto_id").HasColumnType("uuid");
        builder.Property(e => e.SolicitudCotizacionId).HasColumnName("solicitud_cotizacion_id").HasColumnType("uuid");
        builder.Property(e => e.Calificacion).HasColumnName("calificacion").HasColumnType("smallint").IsRequired();
        builder.Property(e => e.Titulo).HasColumnName("titulo").HasColumnType("text").HasMaxLength(255);
        builder.Property(e => e.Contenido).HasColumnName("contenido").HasColumnType("text").HasMaxLength(10000);
        builder.Property(e => e.Estado).HasColumnName("estado").HasColumnType("text").IsRequired().HasMaxLength(9).HasConversion(new ConversorEnumDbml<EstadoResena>()).HasDefaultValue(EstadoResena.Publicado).HasSentinel(EstadoResena.Publicado);
        builder.Property(e => e.RespuestaEmprendimiento).HasColumnName("respuesta_emprendimiento").HasColumnType("text").HasMaxLength(10000);
        builder.Property(e => e.RespondidoPorUsuarioId).HasColumnName("respondido_por_usuario_id").HasColumnType("uuid");
        builder.Property(e => e.RespondidoEn).HasColumnName("respondido_en").HasColumnType("timestamptz");
        builder.Property(e => e.CreadoEn).HasColumnName("creado_en").HasColumnType("timestamptz").IsRequired().HasDefaultValueSql("now()");
        builder.Property(e => e.ActualizadoEn).HasColumnName("actualizado_en").HasColumnType("timestamptz").IsRequired().HasDefaultValueSql("now()");
        builder.Property(e => e.EliminadoEn).HasColumnName("eliminado_en").HasColumnType("timestamptz");
        builder.HasOne(e => e.UsuarioResenador).WithMany().HasForeignKey(e => e.UsuarioResenadorId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_resenas_usuario_resenador_id_usuarios");
        builder.HasIndex(e => e.UsuarioResenadorId).HasDatabaseName("ix_resenas_usuario_resenador_id");
        builder.HasOne(e => e.Emprendimiento).WithMany().HasForeignKey(e => e.EmprendimientoId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_resenas_emprendimiento_id_emprendimientos");
        builder.HasIndex(e => e.EmprendimientoId).HasDatabaseName("ix_resenas_emprendimiento_id");
        builder.HasOne(e => e.RespondidoPorUsuario).WithMany().HasForeignKey(e => e.RespondidoPorUsuarioId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_resenas_respondido_por_usuario_id_usuarios");
        builder.HasIndex(e => e.RespondidoPorUsuarioId).HasDatabaseName("ix_resenas_respondido_por_usuario_id");
        builder.HasOne(e => e.Producto).WithMany().HasForeignKey(e => new { e.ProductoId, e.EmprendimientoId }).HasPrincipalKey(e => new { e.Id, e.EmprendimientoId }).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_resenas_producto_id_emprendimiento_id_productos");
        builder.HasIndex(e => new { e.ProductoId, e.EmprendimientoId }).HasDatabaseName("ix_resenas_producto_id_emprendimiento_id");
        builder.HasOne(e => e.SolicitudCotizacion).WithMany().HasForeignKey(e => e.SolicitudCotizacionId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_resenas_solicitud_cotizacion_id_solicitudes_cotizacion");
        builder.HasIndex(e => e.SolicitudCotizacionId).HasDatabaseName("ix_resenas_solicitud_cotizacion_id");
        builder.HasIndex(e => new { e.Estado, e.CreadoEn }).HasDatabaseName("ix_resenas_estado_creado_en");
        builder.HasQueryFilter(e => e.EliminadoEn == null);
    }
}
