using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Infrastructure.Data.Configurations;

/// <summary>Mapea la tabla participantes y sus restricciones.</summary>
public sealed class ParticipanteConfiguration : IEntityTypeConfiguration<Participante>
{
    public void Configure(EntityTypeBuilder<Participante> builder)
    {
        builder.ToTable("participantes", tabla =>
        {
            tabla.HasCheckConstraint("ck_participantes_rol_enum", "\"rol\" IN ('miembro', 'administrador')");
        });
        builder.HasBaseType((Type?)null);
        builder.HasKey(e => new { e.ConversacionId, e.PerfilId }).HasName("pk_participantes");
        builder.Property(e => e.ConversacionId).HasColumnName("conversacion_id").HasColumnType("uuid").IsRequired().ValueGeneratedNever();
        builder.Property(e => e.PerfilId).HasColumnName("perfil_id").HasColumnType("uuid").IsRequired().ValueGeneratedNever();
        builder.Property(e => e.Rol).HasColumnName("rol").HasColumnType("text").IsRequired().HasMaxLength(13).HasConversion(new ConversorEnumDbml<RolParticipante>()).HasDefaultValue(RolParticipante.Miembro).HasSentinel(RolParticipante.Miembro);
        builder.Property(e => e.UsuarioAsignadoId).HasColumnName("usuario_asignado_id").HasColumnType("uuid");
        builder.Property(e => e.UnidoEn).HasColumnName("unido_en").HasColumnType("timestamptz").IsRequired().HasDefaultValueSql("now()");
        builder.Property(e => e.SalioEn).HasColumnName("salio_en").HasColumnType("timestamptz");
        builder.Property(e => e.UltimoMensajeLeidoId).HasColumnName("ultimo_mensaje_leido_id").HasColumnType("uuid");
        builder.Property(e => e.UltimaLecturaEn).HasColumnName("ultima_lectura_en").HasColumnType("timestamptz");
        builder.Property(e => e.SilenciadoHasta).HasColumnName("silenciado_hasta").HasColumnType("timestamptz");
        builder.Property(e => e.ArchivadoEn).HasColumnName("archivado_en").HasColumnType("timestamptz");
        builder.HasOne(e => e.Conversacion).WithMany().HasForeignKey(e => e.ConversacionId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_participantes_conversacion_id_conversaciones");
        builder.HasOne(e => e.Perfil).WithMany().HasForeignKey(e => e.PerfilId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_participantes_perfil_id_perfiles");
        builder.HasIndex(e => e.PerfilId).HasDatabaseName("ix_participantes_perfil_id");
        builder.HasOne(e => e.UsuarioAsignado).WithMany().HasForeignKey(e => e.UsuarioAsignadoId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_participantes_usuario_asignado_id_usuarios");
        builder.HasIndex(e => e.UsuarioAsignadoId).HasDatabaseName("ix_participantes_usuario_asignado_id");
        builder.HasOne(e => e.UltimoMensajeLeido).WithMany().HasForeignKey(e => e.UltimoMensajeLeidoId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_participantes_ultimo_mensaje_leido_id_mensajes");
        builder.HasIndex(e => e.UltimoMensajeLeidoId).HasDatabaseName("ix_participantes_ultimo_mensaje_leido_id");
    }
}
