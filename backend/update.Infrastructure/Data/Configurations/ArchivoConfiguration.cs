using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Infrastructure.Data.Configurations;

/// <summary>Mapea la tabla archivos y sus restricciones.</summary>
public sealed class ArchivoConfiguration : IEntityTypeConfiguration<Archivo>
{
    public void Configure(EntityTypeBuilder<Archivo> builder)
    {
        builder.ToTable("archivos", tabla =>
        {
            tabla.HasCheckConstraint("ck_archivos_tipo_enum", "\"tipo\" IN ('imagen', 'video', 'documento')");
            tabla.HasCheckConstraint("ck_archivos_proposito_enum", "\"proposito\" IN ('foto_perfil', 'portada', 'publicacion', 'producto', 'mensaje', 'resena', 'verificacion', 'otro')");
            tabla.HasCheckConstraint("ck_archivos_proveedor_almacenamiento_longitud", "char_length(\"proveedor_almacenamiento\") <= 255");
            tabla.HasCheckConstraint("ck_archivos_bucket_longitud", "char_length(\"bucket\") <= 255");
            tabla.HasCheckConstraint("ck_archivos_clave_objeto_longitud", "char_length(\"clave_objeto\") <= 1024");
            tabla.HasCheckConstraint("ck_archivos_nombre_original_longitud", "char_length(\"nombre_original\") <= 255");
            tabla.HasCheckConstraint("ck_archivos_tipo_mime_longitud", "char_length(\"tipo_mime\") <= 127");
            tabla.HasCheckConstraint("ck_archivos_extension_longitud", "char_length(\"extension\") <= 16");
            tabla.HasCheckConstraint("ck_archivos_visibilidad_enum", "\"visibilidad\" IN ('publico', 'privado')");
            tabla.HasCheckConstraint("ck_archivos_estado_enum", "\"estado\" IN ('pendiente', 'listo', 'fallido', 'eliminado')");
        });
        builder.HasBaseType((Type?)null);
        builder.HasKey(e => e.Id).HasName("pk_archivos");
        builder.Property(e => e.Id).HasColumnName("id").HasColumnType("uuid").IsRequired().HasDefaultValueSql("gen_random_uuid()");
        builder.Property(e => e.SubidoPorUsuarioId).HasColumnName("subido_por_usuario_id").HasColumnType("uuid").IsRequired();
        builder.Property(e => e.Tipo).HasColumnName("tipo").HasColumnType("text").IsRequired().HasMaxLength(9).HasConversion(new ConversorEnumDbml<TipoArchivo>());
        builder.Property(e => e.Proposito).HasColumnName("proposito").HasColumnType("text").IsRequired().HasMaxLength(12).HasConversion(new ConversorEnumDbml<PropositoArchivo>());
        builder.Property(e => e.ProveedorAlmacenamiento).HasColumnName("proveedor_almacenamiento").HasColumnType("text").IsRequired().HasMaxLength(255).HasDefaultValue("r2");
        builder.Property(e => e.Bucket).HasColumnName("bucket").HasColumnType("text").IsRequired().HasMaxLength(255);
        builder.Property(e => e.ClaveObjeto).HasColumnName("clave_objeto").HasColumnType("text").IsRequired().HasMaxLength(1024);
        builder.HasIndex(e => e.ClaveObjeto).IsUnique().HasDatabaseName("ux_archivos_clave_objeto");
        builder.Property(e => e.NombreOriginal).HasColumnName("nombre_original").HasColumnType("text").HasMaxLength(255);
        builder.Property(e => e.TipoMime).HasColumnName("tipo_mime").HasColumnType("text").IsRequired().HasMaxLength(127);
        builder.Property(e => e.Extension).HasColumnName("extension").HasColumnType("text").IsRequired().HasMaxLength(16);
        builder.Property(e => e.TamanoBytes).HasColumnName("tamano_bytes").HasColumnType("bigint").IsRequired();
        builder.Property(e => e.ChecksumSha256).HasColumnName("checksum_sha256").HasColumnType("character(64)").HasMaxLength(64);
        builder.Property(e => e.Ancho).HasColumnName("ancho").HasColumnType("integer");
        builder.Property(e => e.Alto).HasColumnName("alto").HasColumnType("integer");
        builder.Property(e => e.DuracionSegundos).HasColumnName("duracion_segundos").HasColumnType("numeric(8,2)").HasPrecision(8, 2);
        builder.Property(e => e.Visibilidad).HasColumnName("visibilidad").HasColumnType("text").IsRequired().HasMaxLength(7).HasConversion(new ConversorEnumDbml<VisibilidadArchivo>()).HasDefaultValue(VisibilidadArchivo.Publico).HasSentinel(VisibilidadArchivo.Publico);
        builder.Property(e => e.Estado).HasColumnName("estado").HasColumnType("text").IsRequired().HasMaxLength(9).HasConversion(new ConversorEnumDbml<EstadoArchivo>()).HasDefaultValue(EstadoArchivo.Pendiente).HasSentinel(EstadoArchivo.Pendiente);
        builder.Property(e => e.CreadoEn).HasColumnName("creado_en").HasColumnType("timestamptz").IsRequired().HasDefaultValueSql("now()");
        builder.Property(e => e.ActualizadoEn).HasColumnName("actualizado_en").HasColumnType("timestamptz").IsRequired().HasDefaultValueSql("now()");
        builder.Property(e => e.EliminadoEn).HasColumnName("eliminado_en").HasColumnType("timestamptz");
        builder.HasOne(e => e.SubidoPorUsuario).WithMany().HasForeignKey(e => e.SubidoPorUsuarioId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_archivos_subido_por_usuario_id_usuarios");
        builder.HasIndex(e => e.SubidoPorUsuarioId).HasDatabaseName("ix_archivos_subido_por_usuario_id");
        builder.HasIndex(e => new { e.Estado, e.CreadoEn }).HasDatabaseName("ix_archivos_estado_creado_en");
        builder.HasQueryFilter(e => e.EliminadoEn == null);
    }
}
