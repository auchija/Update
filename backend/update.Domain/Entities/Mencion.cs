using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

/// <summary>Representa un registro de la tabla menciones.</summary>
public sealed class Mencion : IEntidadValidable
{
    /// <summary>Columna publicacion_id (uuid); obligatoria.</summary>
    public Guid PublicacionId { get; set; }
    /// <summary>Columna perfil_id (uuid); obligatoria.</summary>
    public Guid PerfilId { get; set; }
    /// <summary>Relación hacia publicaciones mediante publicacion_id.</summary>
    [JsonIgnore]
    public Publicacion Publicacion { get; set; } = null!;
    /// <summary>Relación hacia perfiles mediante perfil_id.</summary>
    [JsonIgnore]
    public Perfil Perfil { get; set; } = null!;

    /// <summary>Comprueba las reglas de entrada antes de guardar.</summary>
    public void Validar()
    {
        Reglas.Exigir(PublicacionId != Guid.Empty, "PublicacionId es obligatorio.");
        Reglas.Exigir(PerfilId != Guid.Empty, "PerfilId es obligatorio.");
    }
}
