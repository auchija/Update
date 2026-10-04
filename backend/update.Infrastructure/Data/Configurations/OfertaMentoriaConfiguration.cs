using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Infrastructure.Data.Configurations;

/// <summary>Mapea la tabla ofertas_mentoria y sus restricciones.</summary>
public sealed class OfertaMentoriaConfiguration : IEntityTypeConfiguration<OfertaMentoria>
{
    public void Configure(EntityTypeBuilder<OfertaMentoria> builder)
    {
        builder.ToTable("ofertas_mentoria", tabla =>
        {
            tabla.HasCheckConstraint("ck_ofertas_mentoria_titulo_longitud", "char_length(\"titulo\") <= 255");
            tabla.HasCheckConstraint("ck_ofertas_mentoria_descripcion_longitud", "char_length(\"descripcion\") <= 10000");
            tabla.HasCheckConstraint("ck_ofertas_mentoria_modalidad_enum", "\"modalidad\" IN ('virtual', 'presencial', 'ambas')");
        });
        builder.HasBaseType((Type?)null);
        builder.HasKey(e => e.Id).HasName("pk_ofertas_mentoria");
        builder.Property(e => e.Id).HasColumnName("id").HasColumnType("uuid").IsRequired().HasDefaultValueSql("gen_random_uuid()");
        builder.Property(e => e.UsuarioMentorId).HasColumnName("usuario_mentor_id").HasColumnType("uuid").IsRequired();
        builder.Property(e => e.Titulo).HasColumnName("titulo").HasColumnType("text").IsRequired().HasMaxLength(255);
        builder.Property(e => e.Descripcion).HasColumnName("descripcion").HasColumnType("text").IsRequired().HasMaxLength(10000);
        builder.Property(e => e.CategoriaId).HasColumnName("categoria_id").HasColumnType("integer");
        builder.Property(e => e.Modalidad).HasColumnName("modalidad").HasColumnType("text").IsRequired().HasMaxLength(10).HasConversion(new ConversorEnumDbml<ModalidadMentoria>()).HasDefaultValue(ModalidadMentoria.Virtual).HasSentinel(ModalidadMentoria.Virtual);
        builder.Property(e => e.DuracionMinutos).HasColumnName("duracion_minutos").HasColumnType("smallint").IsRequired().HasDefaultValue((short)60).HasSentinel((short)60);
        builder.Property(e => e.EsGratis).HasColumnName("es_gratis").HasColumnType("boolean").IsRequired().HasDefaultValue(true).HasSentinel(true);
        builder.Property(e => e.Precio).HasColumnName("precio").HasColumnType("numeric(14,2)").HasPrecision(14, 2);
        builder.Property(e => e.MonedaCodigo).HasColumnName("moneda_codigo").HasColumnType("character(3)").IsRequired().HasMaxLength(3).HasDefaultValue("COP");
        builder.Property(e => e.Activo).HasColumnName("activo").HasColumnType("boolean").IsRequired().HasDefaultValue(true).HasSentinel(true);
        builder.Property(e => e.CreadoEn).HasColumnName("creado_en").HasColumnType("timestamptz").IsRequired().HasDefaultValueSql("now()");
        builder.Property(e => e.ActualizadoEn).HasColumnName("actualizado_en").HasColumnType("timestamptz").IsRequired().HasDefaultValueSql("now()");
        builder.HasOne(e => e.UsuarioMentor).WithMany().HasForeignKey(e => e.UsuarioMentorId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_ofertas_mentoria_usuario_mentor_id_usuarios");
        builder.HasIndex(e => e.UsuarioMentorId).HasDatabaseName("ix_ofertas_mentoria_usuario_mentor_id");
        builder.HasOne(e => e.Categoria).WithMany().HasForeignKey(e => e.CategoriaId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_ofertas_mentoria_categoria_id_categorias");
        builder.HasIndex(e => e.CategoriaId).HasDatabaseName("ix_ofertas_mentoria_categoria_id");
        builder.HasOne(e => e.Moneda).WithMany().HasForeignKey(e => e.MonedaCodigo).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_ofertas_mentoria_moneda_codigo_monedas");
        builder.HasIndex(e => e.MonedaCodigo).HasDatabaseName("ix_ofertas_mentoria_moneda_codigo");
    }
}
