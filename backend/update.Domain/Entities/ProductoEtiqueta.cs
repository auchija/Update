using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

public sealed class ProductoEtiqueta : IEntidadValidable
{
    public Guid ProductoId { get; set; }
    public int EtiquetaId { get; set; }
    [JsonIgnore]
    public Producto Producto { get; set; } = null!;
    [JsonIgnore]
    public Etiqueta Etiqueta { get; set; } = null!;

    public void Validar()
    {
        Reglas.Exigir(ProductoId != Guid.Empty, "ProductoId es obligatorio.");
        Reglas.Exigir(EtiquetaId > 0, "EtiquetaId debe ser positivo.");
    }
}
