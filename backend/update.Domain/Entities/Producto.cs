using update.Domain.Common;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

public sealed class Producto : BaseEntity, IEntidadValidable, IEliminable
{
    public Guid EmprendimientoId { get; set; }
    public Guid CreadoPorUsuarioId { get; set; }
    public TipoProducto Tipo { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public int CategoriaId { get; set; }
    public TipoPrecio TipoPrecio { get; set; } = TipoPrecio.Fijo;
    public decimal? Precio { get; set; }
    public string MonedaCodigo { get; set; } = "COP";
    public EstadoInventario EstadoInventario { get; set; } = EstadoInventario.Disponible;
    public int? CantidadInventario { get; set; }
    public int? CiudadId { get; set; }
    public EstadoProducto Estado { get; set; } = EstadoProducto.Borrador;
    public decimal? CalificacionPromedio { get; set; }
    public int TotalCalificaciones { get; set; } = 0;
    public int TotalGuardados { get; set; } = 0;
    public DateTime? PublicadoEn { get; set; }
    public DateTime? EliminadoEn { get; set; }
    [JsonIgnore]
    public Emprendimiento Emprendimiento { get; set; } = null!;
    [JsonIgnore]
    public Usuario CreadoPorUsuario { get; set; } = null!;
    [JsonIgnore]
    public Categoria Categoria { get; set; } = null!;
    [JsonIgnore]
    public Moneda Moneda { get; set; } = null!;
    [JsonIgnore]
    public Ciudad? Ciudad { get; set; }

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

    public void Eliminar()
    {
        EliminadoEn ??= DateTime.UtcNow;
        ActualizadoEn = DateTime.UtcNow;
    }

public void Publicar()
{
    Validar();
    Reglas.Exigir(Estado is EstadoProducto.Borrador or EstadoProducto.Pausado, "El producto no puede publicarse desde su estado actual.");
    Estado = EstadoProducto.Activo;
    PublicadoEn ??= DateTime.UtcNow;
    ActualizadoEn = DateTime.UtcNow;
}
}
