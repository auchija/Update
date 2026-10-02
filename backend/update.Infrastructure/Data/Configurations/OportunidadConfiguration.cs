using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Infrastructure.Data.Configurations;

/// <summary>Mapea la tabla oportunidades y sus restricciones.</summary>
public sealed class OportunidadConfiguration : IEntityTypeConfiguration<Oportunidad>
{
    public void Configure(EntityTypeBuilder<Oportunidad> builder)
    {
        builder.ToTable("oportunidades", tabla =>
        {
            tabla.HasCheckConstraint("ck_oportunidades_tipo_codigo_longitud", "char_length(\"tipo_codigo\") <= 64");
            tabla.HasCheckConstraint("ck_oportunidades_titulo_longitud", "char_length(\"titulo\") <= 255");
            tabla.HasCheckConstraint("ck_oportunidades_descripcion_longitud", "char_length(\"descripcion\") <= 10000");
            tabla.HasCheckConstraint("ck_oportunidades_estado_enum", "\"estado\" IN ('abierto', 'cerrado', 'cubierto')");
        });
        builder.HasBaseType((Type?)null);
        builder.HasKey(e => e.Id).HasName("pk_oportunidades");
        builder.Property(e => e.Id).HasColumnName("id").HasColumnType("uuid").IsRequired().HasDefaultValueSql("gen_random_uuid()");
        builder.Property(e => e.PerfilAutorId).HasColumnName("perfil_autor_id").HasColumnType("uuid").IsRequired();
        builder.Property(e => e.CreadoPorUsuarioId).HasColumnName("creado_por_usuario_id").HasColumnType("uuid").IsRequired();
        builder.Property(e => e.TipoCodigo).HasColumnName("tipo_codigo").HasColumnType("text").IsRequired().HasMaxLength(64);
        builder.Property(e => e.Titulo).HasColumnName("titulo").HasColumnType("text").IsRequired().HasMaxLength(255);
        builder.Property(e => e.Descripcion).HasColumnName("descripcion").HasColumnType("text").IsRequired().HasMaxLength(10000);
        builder.Property(e => e.CategoriaId).HasColumnName("categoria_id").HasColumnType("integer");
        builder.Property(e => e.CiudadId).HasColumnName("ciudad_id").HasColumnType("integer");
        builder.Property(e => e.EsRemoto).HasColumnName("es_remoto").HasColumnType("boolean").IsRequired().HasDefaultValue(false).HasSentinel(false);
        builder.Property(e => e.Estado).HasColumnName("estado").HasColumnType("text").IsRequired().HasMaxLength(8).HasConversion(new ConversorEnumDbml<EstadoOportunidad>()).HasDefaultValue(EstadoOportunidad.Abierto).HasSentinel(EstadoOportunidad.Abierto);
        builder.Property(e => e.ExpiraEn).HasColumnName("expira_en").HasColumnType("timestamptz");
        builder.Property(e => e.TotalPostulaciones).HasColumnName("total_postulaciones").HasColumnType("integer").IsRequired().HasDefaultValue(0).HasSentinel(0);
        builder.Property(e => e.CreadoEn).HasColumnName("creado_en").HasColumnType("timestamptz").IsRequired().HasDefaultValueSql("now()");
        builder.Property(e => e.ActualizadoEn).HasColumnName("actualizado_en").HasColumnType("timestamptz").IsRequired().HasDefaultValueSql("now()");
        builder.Property(e => e.EliminadoEn).HasColumnName("eliminado_en").HasColumnType("timestamptz");
        builder.HasOne(e => e.PerfilAutor).WithMany().HasForeignKey(e => e.PerfilAutorId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_oportunidades_perfil_autor_id_perfiles");
        builder.HasIndex(e => e.PerfilAutorId).HasDatabaseName("ix_oportunidades_perfil_autor_id");
        builder.HasOne(e => e.CreadoPorUsuario).WithMany().HasForeignKey(e => e.CreadoPorUsuarioId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_oportunidades_creado_por_usuario_id_usuarios");
        builder.HasIndex(e => e.CreadoPorUsuarioId).HasDatabaseName("ix_oportunidades_creado_por_usuario_id");
        builder.HasOne(e => e.Tipo).WithMany().HasForeignKey(e => e.TipoCodigo).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_oportunidades_tipo_codigo_tipos_oportunidad");
        builder.HasIndex(e => e.TipoCodigo).HasDatabaseName("ix_oportunidades_tipo_codigo");
        builder.HasOne(e => e.Categoria).WithMany().HasForeignKey(e => e.CategoriaId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_oportunidades_categoria_id_categorias");
        builder.HasIndex(e => e.CategoriaId).HasDatabaseName("ix_oportunidades_categoria_id");
        builder.HasOne(e => e.Ciudad).WithMany().HasForeignKey(e => e.CiudadId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_oportunidades_ciudad_id_ciudades");
        builder.HasIndex(e => e.CiudadId).HasDatabaseName("ix_oportunidades_ciudad_id");
        builder.HasIndex(e => new { e.Estado, e.CreadoEn }).HasDatabaseName("ix_oportunidades_estado_creado_en");
        builder.HasQueryFilter(e => e.EliminadoEn == null);
    }
}
