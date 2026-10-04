using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Infrastructure.Data.Configurations;

/// <summary>Mapea la tabla usuario_tipos_persona y sus restricciones.</summary>
public sealed class UsuarioTipoPersonaConfiguration : IEntityTypeConfiguration<UsuarioTipoPersona>
{
    public void Configure(EntityTypeBuilder<UsuarioTipoPersona> builder)
    {
        builder.ToTable("usuario_tipos_persona", tabla =>
        {
            tabla.HasCheckConstraint("ck_usuario_tipos_persona_tipo_persona_codigo_longitud", "char_length(\"tipo_persona_codigo\") <= 64");
        });
        builder.HasBaseType((Type?)null);
        builder.HasKey(e => new { e.UsuarioId, e.TipoPersonaCodigo }).HasName("pk_usuario_tipos_persona");
        builder.Property(e => e.UsuarioId).HasColumnName("usuario_id").HasColumnType("uuid").IsRequired().ValueGeneratedNever();
        builder.Property(e => e.TipoPersonaCodigo).HasColumnName("tipo_persona_codigo").HasColumnType("text").IsRequired().HasMaxLength(64).ValueGeneratedNever();
        builder.Property(e => e.CreadoEn).HasColumnName("creado_en").HasColumnType("timestamptz").IsRequired().HasDefaultValueSql("now()");
        builder.HasOne(e => e.Usuario).WithMany().HasForeignKey(e => e.UsuarioId).OnDelete(DeleteBehavior.Cascade).HasConstraintName("fk_usuario_tipos_persona_usuario_id_usuarios");
        builder.HasOne(e => e.TipoPersona).WithMany().HasForeignKey(e => e.TipoPersonaCodigo).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_usuario_tipos_persona_tipo_persona_codigo_tipos_persona");
        builder.HasIndex(e => e.TipoPersonaCodigo).HasDatabaseName("ix_usuario_tipos_persona_tipo_persona_codigo");
    }
}
