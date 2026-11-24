using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

public sealed class Ciudad : IEntidadValidable
{
    public int Id { get; set; }
    public int DepartamentoId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Codigo { get; set; }
    public decimal Latitud { get; set; }
    public decimal Longitud { get; set; }
    [JsonIgnore]
    public Departamento Departamento { get; set; } = null!;

    public void Validar()
    {
        Reglas.Exigir(DepartamentoId > 0, "DepartamentoId debe ser positivo.");
        Reglas.Texto(Nombre, nameof(Nombre), 255, true, false);
        Reglas.Texto(Codigo, nameof(Codigo), 64, false, false);
        Reglas.Exigir(Latitud is >= -90m and <= 90m && Longitud is >= -180m and <= 180m, "Las coordenadas están fuera de rango.");
    }
}
