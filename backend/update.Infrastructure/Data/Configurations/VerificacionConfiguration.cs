using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Infrastructure.Data.Configurations;

/// <summary>Mapea la tabla verificaciones y sus restricciones.</summary>
public sealed class VerificacionConfiguration : IEntityTypeConfiguration<Verificacion>
{
    public void Configure(EntityTypeBuilder<Verificacion> builder)
    {
        builder.ToTable("verificaciones", tabla =>
        {
            tabla.HasCheckConstraint("ck_verificaciones_estado_enum", "\"estado\" IN ('sin_verificar', 'pendiente', 'aprobado', 'rechazado')");
            tabla.HasCheckConstraint("ck_verificaciones_metodo_longitud", "char_length(\"metodo\") <= 64");
            tabla.HasCheckConstraint("ck_verificaciones_notas_revisor_longitud", "char_length(\"notas_revisor\") <= 4000");
        });
        builder.HasBaseType((Type?)null);
        builder.HasKey(e => e.Id).HasName("pk_verificaciones");
        builder.Ignore(e => e.CreadoEn);
        builder.Ignore(e => e.ActualizadoEn);
        builder.Property(e => e.Id).HasColumnName("id").HasColumnType("uuid").IsRequired().HasDefaultValueSql("gen_random_uuid()");
        builder.Property(e => e.EmprendimientoId).HasColumnName("emprendimiento_id").HasColumnType("uuid").IsRequired();
        builder.Property(e => e.Estado).HasColumnName("estado").HasColumnType("text").IsRequired().HasMaxLength(13).HasConversion(new ConversorEnumDbml<EstadoVerificacion>()).HasDefaultValue(EstadoVerificacion.Pendiente).HasSentinel(EstadoVerificacion.Pendiente);
        builder.Property(e => e.Metodo).HasColumnName("metodo").HasColumnType("text").IsRequired().HasMaxLength(64).HasDefaultValue("documentos");
        builder.Property(e => e.EnviadoPorUsuarioId).HasColumnName("enviado_por_usuario_id").HasColumnType("uuid").IsRequired();
        builder.Property(e => e.RevisadoPorUsuarioId).HasColumnName("revisado_por_usuario_id").HasColumnType("uuid");
        builder.Property(e => e.NotasRevisor).HasColumnName("notas_revisor").HasColumnType("text").HasMaxLength(4000);
        builder.Property(e => e.EnviadoEn).HasColumnName("enviado_en").HasColumnType("timestamptz").IsRequired().HasDefaultValueSql("now()");
        builder.Property(e => e.RevisadoEn).HasColumnName("revisado_en").HasColumnType("timestamptz");
        builder.HasOne(e => e.RevisadoPorUsuario).WithMany().HasForeignKey(e => e.RevisadoPorUsuarioId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_verificaciones_revisado_por_usuario_id_usuarios");
        builder.HasIndex(e => e.RevisadoPorUsuarioId).HasDatabaseName("ix_verificaciones_revisado_por_usuario_id");
        builder.HasOne(e => e.Emprendimiento).WithMany().HasForeignKey(e => e.EmprendimientoId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_verificaciones_emprendimiento_id_emprendimientos");
        builder.HasIndex(e => e.EmprendimientoId).HasDatabaseName("ix_verificaciones_emprendimiento_id");
        builder.HasOne(e => e.EnviadoPorUsuario).WithMany().HasForeignKey(e => e.EnviadoPorUsuarioId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_verificaciones_enviado_por_usuario_id_usuarios");
        builder.HasIndex(e => e.EnviadoPorUsuarioId).HasDatabaseName("ix_verificaciones_enviado_por_usuario_id");
    }
}
