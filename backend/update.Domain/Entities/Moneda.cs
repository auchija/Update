using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

public sealed class Moneda : IEntidadValidable
{
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Simbolo { get; set; } = string.Empty;
    public short Decimales { get; set; } = (short)0;
    public bool Activo { get; set; } = true;

    public void Validar()
    {
        Reglas.Texto(Codigo, nameof(Codigo), 3, true, false);
        Reglas.Exigir(Codigo is null || Codigo.Length == 3, "Codigo debe tener 3 caracteres.");
        Reglas.Texto(Nombre, nameof(Nombre), 255, true, false);
        Reglas.Texto(Simbolo, nameof(Simbolo), 255, true, false);
    }
}
