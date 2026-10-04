using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Infrastructure.Data.Configurations;

/// <summary>Mapea la tabla reacciones_comentario y sus restricciones.</summary>
public sealed class ReaccionComentarioConfiguration : IEntityTypeConfiguration<ReaccionComentario>
{
    public void Configure(EntityTypeBuilder<ReaccionComentario> builder)
    {
        builder.ToTable("reacciones_comentario", tabla =>
        {
            tabla.HasCheckConstraint("ck_reacciones_comentario_tipo_reaccion_codigo_longitud", "char_length(\"tipo_reaccion_codigo\") <= 64");
        });
        builder.HasBaseType((Type?)null);
        builder.HasKey(e => new { e.ComentarioId, e.PerfilId }).HasName("pk_reacciones_comentario");
        builder.Property(e => e.ComentarioId).HasColumnName("comentario_id").HasColumnType("uuid").IsRequired().ValueGeneratedNever();
        builder.Property(e => e.PerfilId).HasColumnName("perfil_id").HasColumnType("uuid").IsRequired().ValueGeneratedNever();
        builder.Property(e => e.TipoReaccionCodigo).HasColumnName("tipo_reaccion_codigo").HasColumnType("text").IsRequired().HasMaxLength(64);
        builder.Property(e => e.CreadoEn).HasColumnName("creado_en").HasColumnType("timestamptz").IsRequired().HasDefaultValueSql("now()");
        builder.HasOne(e => e.Comentario).WithMany().HasForeignKey(e => e.ComentarioId).OnDelete(DeleteBehavior.Cascade).HasConstraintName("fk_reacciones_comentario_comentario_id_comentarios");
        builder.HasOne(e => e.Perfil).WithMany().HasForeignKey(e => e.PerfilId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_reacciones_comentario_perfil_id_perfiles");
        builder.HasIndex(e => e.PerfilId).HasDatabaseName("ix_reacciones_comentario_perfil_id");
        builder.HasOne(e => e.TipoReaccion).WithMany().HasForeignKey(e => e.TipoReaccionCodigo).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_reacciones_comentario_tipo_reaccion_codigo_tipos_reaccion");
        builder.HasIndex(e => e.TipoReaccionCodigo).HasDatabaseName("ix_reacciones_comentario_tipo_reaccion_codigo");
    }
}
