using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Infrastructure.Data.Configurations;

/// <summary>Mapea la tabla reportes y sus restricciones.</summary>
public sealed class ReporteConfiguration : IEntityTypeConfiguration<Reporte>
{
    public void Configure(EntityTypeBuilder<Reporte> builder)
    {
        builder.ToTable("reportes", tabla =>
        {
            tabla.HasCheckConstraint("ck_reportes_objetivo_tipo_enum", "\"objetivo_tipo\" IN ('perfil', 'publicacion', 'comentario', 'producto', 'mensaje', 'resena', 'oportunidad')");
            tabla.HasCheckConstraint("ck_reportes_motivo_codigo_longitud", "char_length(\"motivo_codigo\") <= 64");
            tabla.HasCheckConstraint("ck_reportes_detalles_longitud", "char_length(\"detalles\") <= 4000");
            tabla.HasCheckConstraint("ck_reportes_estado_enum", "\"estado\" IN ('abierto', 'en_revision', 'resuelto', 'descartado')");
            tabla.HasCheckConstraint("ck_reportes_nota_resolucion_longitud", "char_length(\"nota_resolucion\") <= 4000");
        });
        builder.HasBaseType((Type?)null);
        builder.HasKey(e => e.Id).HasName("pk_reportes");
        builder.Ignore(e => e.ActualizadoEn);
        builder.Property(e => e.Id).HasColumnName("id").HasColumnType("uuid").IsRequired().HasDefaultValueSql("gen_random_uuid()");
        builder.Property(e => e.UsuarioReportanteId).HasColumnName("usuario_reportante_id").HasColumnType("uuid").IsRequired();
        builder.Property(e => e.ObjetivoTipo).HasColumnName("objetivo_tipo").HasColumnType("text").IsRequired().HasMaxLength(11).HasConversion(new ConversorEnumDbml<ObjetivoReporte>());
        builder.Property(e => e.ObjetivoId).HasColumnName("objetivo_id").HasColumnType("uuid").IsRequired();
        builder.Property(e => e.MotivoCodigo).HasColumnName("motivo_codigo").HasColumnType("text").IsRequired().HasMaxLength(64);
        builder.Property(e => e.Detalles).HasColumnName("detalles").HasColumnType("text").HasMaxLength(4000);
        builder.Property(e => e.Estado).HasColumnName("estado").HasColumnType("text").IsRequired().HasMaxLength(11).HasConversion(new ConversorEnumDbml<EstadoReporte>()).HasDefaultValue(EstadoReporte.Abierto).HasSentinel(EstadoReporte.Abierto);
        builder.Property(e => e.ResueltoPorUsuarioId).HasColumnName("resuelto_por_usuario_id").HasColumnType("uuid");
        builder.Property(e => e.NotaResolucion).HasColumnName("nota_resolucion").HasColumnType("text").HasMaxLength(4000);
        builder.Property(e => e.CreadoEn).HasColumnName("creado_en").HasColumnType("timestamptz").IsRequired().HasDefaultValueSql("now()");
        builder.Property(e => e.ResueltoEn).HasColumnName("resuelto_en").HasColumnType("timestamptz");
        builder.HasOne(e => e.Motivo).WithMany().HasForeignKey(e => e.MotivoCodigo).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_reportes_motivo_codigo_motivos_reporte");
        builder.HasIndex(e => e.MotivoCodigo).HasDatabaseName("ix_reportes_motivo_codigo");
        builder.HasOne(e => e.ResueltoPorUsuario).WithMany().HasForeignKey(e => e.ResueltoPorUsuarioId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_reportes_resuelto_por_usuario_id_usuarios");
        builder.HasIndex(e => e.ResueltoPorUsuarioId).HasDatabaseName("ix_reportes_resuelto_por_usuario_id");
        builder.HasOne(e => e.UsuarioReportante).WithMany().HasForeignKey(e => e.UsuarioReportanteId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_reportes_usuario_reportante_id_usuarios");
        builder.HasIndex(e => e.UsuarioReportanteId).HasDatabaseName("ix_reportes_usuario_reportante_id");
        builder.HasIndex(e => new { e.Estado, e.CreadoEn }).HasDatabaseName("ix_reportes_estado_creado_en");
    }
}
