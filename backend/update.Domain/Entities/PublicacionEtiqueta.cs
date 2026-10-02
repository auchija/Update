using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

/// <summary>Representa un registro de la tabla publicaciones_etiquetas.</summary>
public sealed class PublicacionEtiqueta : IEntidadValidable
{
    /// <summary>Columna publicacion_id (uuid); obligatoria.</summary>
    public Guid PublicacionId { get; set; }
    /// <summary>Columna etiqueta_id (integer); obligatoria.</summary>
    public int EtiquetaId { get; set; }
    /// <summary>Relación hacia publicaciones mediante publicacion_id.</summary>
    [JsonIgnore]
    public Publicacion Publicacion { get; set; } = null!;
    /// <summary>Relación hacia etiquetas mediante etiqueta_id.</summary>
    [JsonIgnore]
    public Etiqueta Etiqueta { get; set; } = null!;

    /// <summary>Comprueba las reglas de entrada antes de guardar.</summary>
    public void Validar()
    {
        Reglas.Exigir(PublicacionId != Guid.Empty, "PublicacionId es obligatorio.");
        Reglas.Exigir(EtiquetaId > 0, "EtiquetaId debe ser positivo.");
    }
}
