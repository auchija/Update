using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Infrastructure.Data.Configurations;

/// <summary>Mapea la tabla notificaciones y sus restricciones.</summary>
public sealed class NotificacionConfiguration : IEntityTypeConfiguration<Notificacion>
{
    public void Configure(EntityTypeBuilder<Notificacion> builder)
    {
        builder.ToTable("notificaciones", tabla =>
        {
            tabla.HasCheckConstraint("ck_notificaciones_tipo_enum", "\"tipo\" IN ('seguimiento', 'solicitud_seguimiento', 'seguimiento_aceptado', 'reaccion', 'comentario', 'respuesta', 'mencion', 'compartido', 'mensaje', 'cotizacion_actualizada', 'postulacion_actualizada', 'mentoria_actualizada', 'resena', 'invitacion_emprendimiento', 'respuesta_aceptada', 'sistema')");
            tabla.HasCheckConstraint("ck_notificaciones_entidad_tipo_longitud", "char_length(\"entidad_tipo\") <= 64");
            tabla.HasCheckConstraint("ck_notificaciones_clave_grupo_longitud", "char_length(\"clave_grupo\") <= 1024");
        });
        builder.HasBaseType((Type?)null);
        builder.HasKey(e => e.Id).HasName("pk_notificaciones");
        builder.Ignore(e => e.ActualizadoEn);
        builder.Property(e => e.Id).HasColumnName("id").HasColumnType("uuid").IsRequired().HasDefaultValueSql("gen_random_uuid()");
        builder.Property(e => e.UsuarioDestinatarioId).HasColumnName("usuario_destinatario_id").HasColumnType("uuid").IsRequired();
        builder.Property(e => e.Tipo).HasColumnName("tipo").HasColumnType("text").IsRequired().HasMaxLength(25).HasConversion(new ConversorEnumDbml<TipoNotificacion>());
        builder.Property(e => e.PerfilActorId).HasColumnName("perfil_actor_id").HasColumnType("uuid");
        builder.Property(e => e.EntidadTipo).HasColumnName("entidad_tipo").HasColumnType("text").HasMaxLength(64);
        builder.Property(e => e.EntidadId).HasColumnName("entidad_id").HasColumnType("uuid");
        builder.Property(e => e.ClaveGrupo).HasColumnName("clave_grupo").HasColumnType("text").HasMaxLength(1024);
        builder.Property(e => e.Datos).HasColumnName("datos").HasColumnType("jsonb").IsRequired() .HasDefaultValueSql("'{}'::jsonb");
        builder.Property(e => e.LeidoEn).HasColumnName("leido_en").HasColumnType("timestamptz");
        builder.Property(e => e.CreadoEn).HasColumnName("creado_en").HasColumnType("timestamptz").IsRequired().HasDefaultValueSql("now()");
        builder.HasOne(e => e.UsuarioDestinatario).WithMany().HasForeignKey(e => e.UsuarioDestinatarioId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_notificaciones_usuario_destinatario_id_usuarios");
        builder.HasIndex(e => e.UsuarioDestinatarioId).HasDatabaseName("ix_notificaciones_usuario_destinatario_id");
        builder.HasOne(e => e.PerfilActor).WithMany().HasForeignKey(e => e.PerfilActorId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_notificaciones_perfil_actor_id_perfiles");
        builder.HasIndex(e => e.PerfilActorId).HasDatabaseName("ix_notificaciones_perfil_actor_id");
    }
}
