using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Infrastructure.Data.Configurations;

/// <summary>Mapea la tabla cotizaciones y sus restricciones.</summary>
public sealed class CotizacionConfiguration : IEntityTypeConfiguration<Cotizacion>
{
    public void Configure(EntityTypeBuilder<Cotizacion> builder)
    {
        builder.ToTable("cotizaciones", tabla =>
        {
            tabla.HasCheckConstraint("ck_cotizaciones_notas_longitud", "char_length(\"notas\") <= 4000");
            tabla.HasCheckConstraint("ck_cotizaciones_estado_enum", "\"estado\" IN ('enviado', 'reemplazado', 'aceptado', 'rechazado', 'vencido')");
        });
        builder.HasBaseType((Type?)null);
        builder.HasKey(e => e.Id).HasName("pk_cotizaciones");
        builder.Ignore(e => e.ActualizadoEn);
        builder.Property(e => e.Id).HasColumnName("id").HasColumnType("uuid").IsRequired().HasDefaultValueSql("gen_random_uuid()");
        builder.Property(e => e.SolicitudCotizacionId).HasColumnName("solicitud_cotizacion_id").HasColumnType("uuid").IsRequired();
        builder.Property(e => e.Version).HasColumnName("version").HasColumnType("smallint").IsRequired();
        builder.Property(e => e.CreadoPorUsuarioId).HasColumnName("creado_por_usuario_id").HasColumnType("uuid").IsRequired();
        builder.Property(e => e.MonedaCodigo).HasColumnName("moneda_codigo").HasColumnType("character(3)").IsRequired().HasMaxLength(3).HasDefaultValue("COP");
        builder.Property(e => e.Subtotal).HasColumnName("subtotal").HasColumnType("numeric(14,2)").IsRequired().HasPrecision(14, 2).HasDefaultValue(0m).HasSentinel(0m);
        builder.Property(e => e.Descuento).HasColumnName("descuento").HasColumnType("numeric(14,2)").IsRequired().HasPrecision(14, 2).HasDefaultValue(0m).HasSentinel(0m);
        builder.Property(e => e.Impuesto).HasColumnName("impuesto").HasColumnType("numeric(14,2)").IsRequired().HasPrecision(14, 2).HasDefaultValue(0m).HasSentinel(0m);
        builder.Property(e => e.Total).HasColumnName("total").HasColumnType("numeric(14,2)").HasPrecision(14, 2).HasComputedColumnSql("subtotal - descuento + impuesto", stored: true);
        builder.Property(e => e.ValidoHasta).HasColumnName("valido_hasta").HasColumnType("date");
        builder.Property(e => e.Notas).HasColumnName("notas").HasColumnType("text").HasMaxLength(4000);
        builder.Property(e => e.Estado).HasColumnName("estado").HasColumnType("text").IsRequired().HasMaxLength(11).HasConversion(new ConversorEnumDbml<EstadoCotizacion>()).HasDefaultValue(EstadoCotizacion.Enviado).HasSentinel(EstadoCotizacion.Enviado);
        builder.Property(e => e.CreadoEn).HasColumnName("creado_en").HasColumnType("timestamptz").IsRequired().HasDefaultValueSql("now()");
        builder.HasOne(e => e.SolicitudCotizacion).WithMany().HasForeignKey(e => e.SolicitudCotizacionId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_cotizaciones_solicitud_cotizacion_id_solicitudes_cotizacion");
        builder.HasIndex(e => e.SolicitudCotizacionId).HasDatabaseName("ix_cotizaciones_solicitud_cotizacion_id");
        builder.HasOne(e => e.CreadoPorUsuario).WithMany().HasForeignKey(e => e.CreadoPorUsuarioId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_cotizaciones_creado_por_usuario_id_usuarios");
        builder.HasIndex(e => e.CreadoPorUsuarioId).HasDatabaseName("ix_cotizaciones_creado_por_usuario_id");
        builder.HasOne(e => e.Moneda).WithMany().HasForeignKey(e => e.MonedaCodigo).OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_cotizaciones_moneda_codigo_monedas");
        builder.HasIndex(e => e.MonedaCodigo).HasDatabaseName("ix_cotizaciones_moneda_codigo");
        builder.HasIndex(e => new { e.Estado, e.CreadoEn }).HasDatabaseName("ix_cotizaciones_estado_creado_en");
    }
}
