using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Infrastructure.Data.Configurations;

/// <summary>Mapea la tabla perfiles y sus restricciones.</summary>
public sealed class PerfilConfiguration : IEntityTypeConfiguration<Perfil>
{
    public void Configure(EntityTypeBuilder<Perfil> builder)
    {
        builder.ToTable("perfiles", tabla =>
        {
            tabla.HasCheckConstraint("ck_perfiles_tipo_enum", "\"tipo\" IN ('persona', 'emprendimiento')");
            tabla.HasCheckConstraint("ck_perfiles_nombre_usuario_longitud", "char_length(\"nombre_usuario\") <= 50");
            tabla.HasCheckConstraint("ck_perfiles_nombre_visible_longitud", "char_length(\"nombre_visible\") <= 255");
            tabla.HasCheckConstraint("ck_perfiles_titular_longitud", "char_length(\"titular\") <= 255");
            tabla.HasCheckConstraint("ck_perfiles_biografia_longitud", "char_length(\"biografia\") <= 2000");
            tabla.HasCheckConstraint("ck_perfiles_sitio_web_longitud", "char_length(\"sitio_web\") <= 2048");
            tabla.HasCheckConstraint("ck_perfiles_estado_enum", "\"estado\" IN ('activo', 'suspendido', 'eliminado')");
        });
        builder.HasBaseType((Type?)null);
        builder.HasKey(e => e.Id).HasName("pk_perfiles");
        builder.Property(e => e.Id).HasColumnName("id").HasColumnType("uuid").IsRequired().HasDefaultValueSql("gen_random_uuid()");
        builder.Property(e => e.Tipo).HasColumnName("tipo").HasColumnType("text").IsRequired().HasMaxLength(14).HasConversion(new ConversorEnumDbml<TipoPerfil>());
        builder.Property(e => e.NombreUsuario).HasColumnName("nombre_usuario").HasColumnType("text").IsRequired().HasMaxLength(50);
        builder.Property(e => e.NombreVisible).HasColumnName("nombre_visible").HasColumnType("text").IsRequired().HasMaxLength(255);
        builder.Property(e => e.Titular).HasColumnName("titular").HasColumnType("text").HasMaxLength(255);
        builder.Property(e => e.Biografia).HasColumnName("biografia").HasColumnType("text").HasMaxLength(2000);
        builder.Property(e => e.FotoArchivoId).HasColumnName("foto_archivo_id").HasColumnType("uuid");
        builder.Property(e => e.PortadaArchivoId).HasColumnName("portada_archivo_id").HasColumnType("uuid");
        builder.Property(e => e.CiudadId).HasColumnName("ciudad_id").HasColumnType("integer");
        builder.Property(e => e.SitioWeb).HasColumnName("sitio_web").HasColumnType("text").HasMaxLength(2048);
        builder.Property(e => e.EsPrivado).HasColumnName("es_privado").HasColumnType("boolean").IsRequired().HasDefaultValue(false).HasSentinel(false);
        builder.Property(e => e.Estado).HasColumnName("estado").HasColumnType("text").IsRequired().HasMaxLength(10).HasConversion(new ConversorEnumDbml<EstadoCuenta>()).HasDefaultValue(EstadoCuenta.Activo).HasSentinel(EstadoCuenta.Activo);
        builder.Property(e => e.TotalSeguidores).HasColumnName("total_seguidores").HasColumnType("integer").IsRequired().HasDefaultValue(0).HasSentinel(0);
        builder.Property(e => e.TotalSeguidos).HasColumnName("total_seguidos").HasColumnType("integer").IsRequired().HasDefaultValue(0).HasSentinel(0);
        builder.Property(e => e.TotalPublicaciones).HasColumnName("total_publicaciones").HasColumnType("integer").IsRequired().HasDefaultValue(0).HasSentinel(0);
        builder.Property(e => e.CreadoEn).HasColumnName("creado_en").HasColumnType("timestamptz").IsRequired().HasDefaultValueSql("now()");
        builder.Property(e => e.ActualizadoEn).HasColumnName("actualizado_en").HasColumnType("timestamptz").IsRequired().HasDefaultValueSql("now()");
        builder.Property(e => e.EliminadoEn).HasColumnName("eliminado_en").HasColumnType("timestamptz");
        builder.HasOne(e => e.Ciudad).WithMany().HasForeignKey(e => e.CiudadId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_perfiles_ciudad_id_ciudades");
        builder.HasIndex(e => e.CiudadId).HasDatabaseName("ix_perfiles_ciudad_id");
        builder.HasOne(e => e.FotoArchivo).WithMany().HasForeignKey(e => e.FotoArchivoId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_perfiles_foto_archivo_id_archivos");
        builder.HasIndex(e => e.FotoArchivoId).HasDatabaseName("ix_perfiles_foto_archivo_id");
        builder.HasOne(e => e.PortadaArchivo).WithMany().HasForeignKey(e => e.PortadaArchivoId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_perfiles_portada_archivo_id_archivos");
        builder.HasIndex(e => e.PortadaArchivoId).HasDatabaseName("ix_perfiles_portada_archivo_id");
        builder.HasIndex(e => new { e.Estado, e.CreadoEn }).HasDatabaseName("ix_perfiles_estado_creado_en");
        builder.HasQueryFilter(e => e.EliminadoEn == null);
    }
}
