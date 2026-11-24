using update.Domain.Common;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

public sealed class Guardado : BaseEntity, IEntidadValidable
{
    public Guid UsuarioId { get; set; }
    public Guid? PublicacionId { get; set; }
    public Guid? ProductoId { get; set; }
    [JsonIgnore]
    public Usuario Usuario { get; set; } = null!;
    [JsonIgnore]
    public Publicacion? Publicacion { get; set; }
    [JsonIgnore]
    public Producto? Producto { get; set; }

    public void Validar()
    {
        Reglas.Exigir(UsuarioId != Guid.Empty, "UsuarioId es obligatorio.");
        Reglas.FechaUtc(CreadoEn, nameof(CreadoEn));
        Reglas.Exigir(PublicacionId.HasValue != ProductoId.HasValue, "Un guardado debe señalar exactamente una publicación o un producto.");
    }
}
