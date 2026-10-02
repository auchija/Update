using update.Domain.Common;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

/// <summary>Representa un registro de la tabla productos.</summary>
public sealed class Producto : BaseEntity, IEntidadValidable, IEliminable
{
    /// <summary>Columna emprendimiento_id (uuid); obligatoria.</summary>
    public Guid EmprendimientoId { get; set; }
    /// <summary>Columna creado_por_usuario_id (uuid); obligatoria.</summary>
    public Guid CreadoPorUsuarioId { get; set; }
    /// <summary>Columna tipo (tipo_producto); obligatoria.</summary>
    public TipoProducto Tipo { get; set; }
    /// <summary>Columna titulo (text); obligatoria.</summary>
    public string Titulo { get; set; } = string.Empty;
    /// <summary>Columna slug (text); obligatoria.</summary>
    public string Slug { get; set; } = string.Empty;
    /// <summary>Columna descripcion (text); opcional.</summary>
    public string? Descripcion { get; set; }
    /// <summary>Columna categoria_id (integer); obligatoria.</summary>
    public int CategoriaId { get; set; }
    /// <summary>Columna tipo_precio (tipo_precio); obligatoria.</summary>
    public TipoPrecio TipoPrecio { get; set; } = TipoPrecio.Fijo;
    /// <summary>Columna precio (numeric(14,2)); opcional.</summary>
    public decimal? Precio { get; set; }
    /// <summary>Columna moneda_codigo (char(3)); obligatoria.</summary>
    public string MonedaCodigo { get; set; } = "COP";
    /// <summary>Columna estado_inventario (estado_inventario); obligatoria.</summary>
    public EstadoInventario EstadoInventario { get; set; } = EstadoInventario.Disponible;
    /// <summary>Columna cantidad_inventario (integer); opcional.</summary>
    public int? CantidadInventario { get; set; }
    /// <summary>Columna ciudad_id (integer); opcional.</summary>
    public int? CiudadId { get; set; }
    /// <summary>Columna estado (estado_producto); obligatoria.</summary>
    public EstadoProducto Estado { get; set; } = EstadoProducto.Borrador;
    /// <summary>Columna calificacion_promedio (numeric(3,2)); opcional.</summary>
    public decimal? CalificacionPromedio { get; set; }
    /// <summary>Columna total_calificaciones (integer); obligatoria.</summary>
    public int TotalCalificaciones { get; set; } = 0;
    /// <summary>Columna total_guardados (integer); obligatoria.</summary>
    public int TotalGuardados { get; set; } = 0;
    /// <summary>Columna publicado_en (timestamptz); opcional.</summary>
    public DateTime? PublicadoEn { get; set; }
    /// <summary>Columna eliminado_en (timestamptz); opcional.</summary>
    public DateTime? EliminadoEn { get; set; }
    /// <summary>Relación hacia emprendimientos mediante emprendimiento_id.</summary>
    [JsonIgnore]
    public Emprendimiento Emprendimiento { get; set; } = null!;
    /// <summary>Relación hacia usuarios mediante creado_por_usuario_id.</summary>
    [JsonIgnore]
    public Usuario CreadoPorUsuario { get; set; } = null!;
    /// <summary>Relación hacia categorias mediante categoria_id.</summary>
    [JsonIgnore]
    public Categoria Categoria { get; set; } = null!;
    /// <summary>Relación hacia monedas mediante moneda_codigo.</summary>
    [JsonIgnore]
    public Moneda Moneda { get; set; } = null!;
    /// <summary>Relación hacia ciudades mediante ciudad_id.</summary>
    [JsonIgnore]
    public Ciudad? Ciudad { get; set; }

    /// <summary>Comprueba las reglas de entrada antes de guardar.</summary>
    public void Validar()
    {
        Reglas.Exigir(EmprendimientoId != Guid.Empty, "EmprendimientoId es obligatorio.");
        Reglas.Exigir(CreadoPorUsuarioId != Guid.Empty, "CreadoPorUsuarioId es obligatorio.");
        Reglas.Exigir(Enum.IsDefined(Tipo), "Tipo contiene un valor no permitido.");
        Reglas.Texto(Titulo, nameof(Titulo), 255, true, false);
        Reglas.Texto(Slug, nameof(Slug), 255, true, false);
        Reglas.Texto(Descripcion, nameof(Descripcion), 10000, false, false);
        Reglas.Exigir(CategoriaId > 0, "CategoriaId debe ser positivo.");
        Reglas.Exigir(Enum.IsDefined(TipoPrecio), "TipoPrecio contiene un valor no permitido.");
        Reglas.Exigir(Precio is null || Precio >= 0, "Precio no puede ser negativo.");
        Reglas.Texto(MonedaCodigo, nameof(MonedaCodigo), 3, true, false);
        Reglas.Exigir(MonedaCodigo is null || MonedaCodigo.Length == 3, "MonedaCodigo debe tener 3 caracteres.");
        Reglas.Exigir(Enum.IsDefined(EstadoInventario), "EstadoInventario contiene un valor no permitido.");
        Reglas.Exigir(CantidadInventario is null || CantidadInventario >= 0, "CantidadInventario no puede ser negativo.");
        Reglas.Exigir(Enum.IsDefined(Estado), "Estado contiene un valor no permitido.");
        Reglas.Exigir(TotalCalificaciones >= 0, "TotalCalificaciones no puede ser negativo.");
        Reglas.Exigir(TotalGuardados >= 0, "TotalGuardados no puede ser negativo.");
        Reglas.FechaUtc(PublicadoEn, nameof(PublicadoEn));
        Reglas.FechaUtc(CreadoEn, nameof(CreadoEn));
        Reglas.FechaUtc(ActualizadoEn, nameof(ActualizadoEn));
        Reglas.FechaUtc(EliminadoEn, nameof(EliminadoEn));
        Reglas.Exigir(TipoPrecio is not (TipoPrecio.Fijo or TipoPrecio.Desde) || Precio.HasValue, "El tipo de precio seleccionado requiere un precio.");
    }

    /// <summary>Marca el registro como eliminado sin borrarlo físicamente.</summary>
    public void Eliminar()
    {
        EliminadoEn ??= DateTime.UtcNow;
        ActualizadoEn = DateTime.UtcNow;
    }

/// <summary>Publica un producto después de validar sus datos.</summary>
public void Publicar()
{
    Validar();
    Reglas.Exigir(Estado is EstadoProducto.Borrador or EstadoProducto.Pausado, "El producto no puede publicarse desde su estado actual.");
    Estado = EstadoProducto.Activo;
    PublicadoEn ??= DateTime.UtcNow;
    ActualizadoEn = DateTime.UtcNow;
}
}
