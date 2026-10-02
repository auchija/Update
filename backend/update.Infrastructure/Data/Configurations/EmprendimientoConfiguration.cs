using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Infrastructure.Data.Configurations;

/// <summary>Mapea la tabla emprendimientos y sus restricciones.</summary>
public sealed class EmprendimientoConfiguration : IEntityTypeConfiguration<Emprendimiento>
{
    public void Configure(EntityTypeBuilder<Emprendimiento> builder)
    {
        builder.ToTable("emprendimientos", tabla =>
        {
            tabla.HasCheckConstraint("ck_emprendimientos_tipo_perfil_enum", "\"tipo_perfil\" IN ('persona', 'emprendimiento')");
            tabla.HasCheckConstraint("ck_emprendimientos_etapa_enum", "\"etapa\" IN ('idea', 'validacion', 'lanzamiento', 'crecimiento', 'consolidado')");
            tabla.HasCheckConstraint("ck_emprendimientos_razon_social_longitud", "char_length(\"razon_social\") <= 255");
            tabla.HasCheckConstraint("ck_emprendimientos_nit_longitud", "char_length(\"nit\") <= 255");
            tabla.HasCheckConstraint("ck_emprendimientos_correo_contacto_longitud", "char_length(\"correo_contacto\") <= 254");
            tabla.HasCheckConstraint("ck_emprendimientos_telefono_contacto_longitud", "char_length(\"telefono_contacto\") <= 30");
            tabla.HasCheckConstraint("ck_emprendimientos_whatsapp_longitud", "char_length(\"whatsapp\") <= 30");
            tabla.HasCheckConstraint("ck_emprendimientos_direccion_longitud", "char_length(\"direccion\") <= 500");
            tabla.HasCheckConstraint("ck_emprendimientos_estado_verificacion_enum", "\"estado_verificacion\" IN ('sin_verificar', 'pendiente', 'aprobado', 'rechazado')");
            tabla.HasCheckConstraint("ck_emprendimientos_tipo_perfil", "\"tipo_perfil\" = 'emprendimiento'");
        });
        builder.HasBaseType((Type?)null);
        builder.HasKey(e => e.Id).HasName("pk_emprendimientos");
        builder.Property(e => e.Id).HasColumnName("id").HasColumnType("uuid").IsRequired().ValueGeneratedNever();
        builder.Property(e => e.TipoPerfil).HasColumnName("tipo_perfil").HasColumnType("text").IsRequired().HasMaxLength(14).HasConversion(new ConversorEnumDbml<TipoPerfil>()).HasDefaultValue(TipoPerfil.Emprendimiento).HasSentinel(TipoPerfil.Emprendimiento);
        builder.Property(e => e.CategoriaId).HasColumnName("categoria_id").HasColumnType("integer").IsRequired();
        builder.Property(e => e.Etapa).HasColumnName("etapa").HasColumnType("text").IsRequired().HasMaxLength(11).HasConversion(new ConversorEnumDbml<EtapaEmprendimiento>()).HasDefaultValue(EtapaEmprendimiento.Idea).HasSentinel(EtapaEmprendimiento.Idea);
        builder.Property(e => e.FundadoEn).HasColumnName("fundado_en").HasColumnType("date");
        builder.Property(e => e.RazonSocial).HasColumnName("razon_social").HasColumnType("text").HasMaxLength(255);
        builder.Property(e => e.Nit).HasColumnName("nit").HasColumnType("text").HasMaxLength(255);
        builder.Property(e => e.CorreoContacto).HasColumnName("correo_contacto").HasColumnType("text").HasMaxLength(254);
        builder.Property(e => e.TelefonoContacto).HasColumnName("telefono_contacto").HasColumnType("text").HasMaxLength(30);
        builder.Property(e => e.Whatsapp).HasColumnName("whatsapp").HasColumnType("text").HasMaxLength(30);
        builder.Property(e => e.Direccion).HasColumnName("direccion").HasColumnType("text").HasMaxLength(500);
        builder.Property(e => e.EnviosNacionales).HasColumnName("envios_nacionales").HasColumnType("boolean").IsRequired().HasDefaultValue(false).HasSentinel(false);
        builder.Property(e => e.OfreceRemoto).HasColumnName("ofrece_remoto").HasColumnType("boolean").IsRequired().HasDefaultValue(false).HasSentinel(false);
        builder.Property(e => e.EstadoVerificacion).HasColumnName("estado_verificacion").HasColumnType("text").IsRequired().HasMaxLength(13).HasConversion(new ConversorEnumDbml<EstadoVerificacion>()).HasDefaultValue(EstadoVerificacion.SinVerificar).HasSentinel(EstadoVerificacion.SinVerificar);
        builder.Property(e => e.VerificadoEn).HasColumnName("verificado_en").HasColumnType("timestamptz");
        builder.Property(e => e.CalificacionPromedio).HasColumnName("calificacion_promedio").HasColumnType("numeric(3,2)").HasPrecision(3, 2);
        builder.Property(e => e.TotalCalificaciones).HasColumnName("total_calificaciones").HasColumnType("integer").IsRequired().HasDefaultValue(0).HasSentinel(0);
        builder.Property(e => e.CreadoPorUsuarioId).HasColumnName("creado_por_usuario_id").HasColumnType("uuid").IsRequired();
        builder.Property(e => e.CreadoEn).HasColumnName("creado_en").HasColumnType("timestamptz").IsRequired().HasDefaultValueSql("now()");
        builder.Property(e => e.ActualizadoEn).HasColumnName("actualizado_en").HasColumnType("timestamptz").IsRequired().HasDefaultValueSql("now()");
        builder.HasOne(e => e.Categoria).WithMany().HasForeignKey(e => e.CategoriaId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_emprendimientos_categoria_id_categorias");
        builder.HasIndex(e => e.CategoriaId).HasDatabaseName("ix_emprendimientos_categoria_id");
        builder.HasOne(e => e.CreadoPorUsuario).WithMany().HasForeignKey(e => e.CreadoPorUsuarioId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_emprendimientos_creado_por_usuario_id_usuarios");
        builder.HasIndex(e => e.CreadoPorUsuarioId).HasDatabaseName("ix_emprendimientos_creado_por_usuario_id");
        builder.HasOne(e => e.Perfil).WithOne().HasForeignKey<Emprendimiento>(e => new { e.Id, e.TipoPerfil }).HasPrincipalKey<Perfil>(e => new { e.Id, e.Tipo }).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_emprendimientos_id_tipo_perfil_perfiles");
        builder.HasIndex(e => new { e.Id, e.TipoPerfil }).HasDatabaseName("ix_emprendimientos_id_tipo_perfil");
    }
}
