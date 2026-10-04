using update.Domain.Common;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

/// <summary>Representa un registro de la tabla guardados.</summary>
public sealed class Guardado : BaseEntity, IEntidadValidable
{
    /// <summary>Columna usuario_id (uuid); obligatoria.</summary>
    public Guid UsuarioId { get; set; }
    /// <summary>Columna publicacion_id (uuid); opcional.</summary>
    public Guid? PublicacionId { get; set; }
    /// <summary>Columna producto_id (uuid); opcional.</summary>
    public Guid? ProductoId { get; set; }
    /// <summary>Relación hacia usuarios mediante usuario_id.</summary>
    [JsonIgnore]
    public Usuario Usuario { get; set; } = null!;
    /// <summary>Relación hacia publicaciones mediante publicacion_id.</summary>
    [JsonIgnore]
    public Publicacion? Publicacion { get; set; }
    /// <summary>Relación hacia productos mediante producto_id.</summary>
    [JsonIgnore]
    public Producto? Producto { get; set; }

    /// <summary>Comprueba las reglas de entrada antes de guardar.</summary>
    public void Validar()
    {
        Reglas.Exigir(UsuarioId != Guid.Empty, "UsuarioId es obligatorio.");
        Reglas.FechaUtc(CreadoEn, nameof(CreadoEn));
        Reglas.Exigir(PublicacionId.HasValue != ProductoId.HasValue, "Un guardado debe señalar exactamente una publicación o un producto.");
    }
}
