using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

public sealed class Departamento : IEntidadValidable
{
    public int Id { get; set; }
    public string PaisCodigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string? Codigo { get; set; }
    [JsonIgnore]
    public Pais Pais { get; set; } = null!;

    public void Validar()
    {
        Reglas.Texto(PaisCodigo, nameof(PaisCodigo), 2, true, false);
        Reglas.Exigir(PaisCodigo is null || PaisCodigo.Length == 2, "PaisCodigo debe tener 2 caracteres.");
        Reglas.Texto(Nombre, nameof(Nombre), 255, true, false);
        Reglas.Texto(Codigo, nameof(Codigo), 64, false, false);
    }
}
