using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

public sealed class InteresUsuario : IEntidadValidable
{
    public Guid UsuarioId { get; set; }
    public int CategoriaId { get; set; }
    public DateTime CreadoEn { get; set; } = DateTime.UtcNow;
    [JsonIgnore]
    public Usuario Usuario { get; set; } = null!;
    [JsonIgnore]
    public Categoria Categoria { get; set; } = null!;

    public void Validar()
    {
        Reglas.Exigir(UsuarioId != Guid.Empty, "UsuarioId es obligatorio.");
        Reglas.Exigir(CategoriaId > 0, "CategoriaId debe ser positivo.");
        Reglas.FechaUtc(CreadoEn, nameof(CreadoEn));
    }
}
