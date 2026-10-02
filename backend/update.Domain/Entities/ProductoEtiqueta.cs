using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

/// <summary>Representa un registro de la tabla productos_etiquetas.</summary>
public sealed class ProductoEtiqueta : IEntidadValidable
{
    /// <summary>Columna producto_id (uuid); obligatoria.</summary>
    public Guid ProductoId { get; set; }
    /// <summary>Columna etiqueta_id (integer); obligatoria.</summary>
    public int EtiquetaId { get; set; }
    /// <summary>Relación hacia productos mediante producto_id.</summary>
    [JsonIgnore]
    public Producto Producto { get; set; } = null!;
    /// <summary>Relación hacia etiquetas mediante etiqueta_id.</summary>
    [JsonIgnore]
    public Etiqueta Etiqueta { get; set; } = null!;

    /// <summary>Comprueba las reglas de entrada antes de guardar.</summary>
    public void Validar()
    {
        Reglas.Exigir(ProductoId != Guid.Empty, "ProductoId es obligatorio.");
        Reglas.Exigir(EtiquetaId > 0, "EtiquetaId debe ser positivo.");
    }
}
