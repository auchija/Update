using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

public sealed class Mencion : IEntidadValidable
{
    public Guid PublicacionId { get; set; }
    public Guid PerfilId { get; set; }
    [JsonIgnore]
    public Publicacion Publicacion { get; set; } = null!;
    [JsonIgnore]
    public Perfil Perfil { get; set; } = null!;

    public void Validar()
    {
        Reglas.Exigir(PublicacionId != Guid.Empty, "PublicacionId es obligatorio.");
        Reglas.Exigir(PerfilId != Guid.Empty, "PerfilId es obligatorio.");
    }
}
