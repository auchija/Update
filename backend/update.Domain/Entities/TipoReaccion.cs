using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

public sealed class TipoReaccion : IEntidadValidable
{
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Emoji { get; set; } = string.Empty;
    public short Orden { get; set; } = (short)0;
    public bool Activo { get; set; } = true;

    public void Validar()
    {
        Reglas.Texto(Codigo, nameof(Codigo), 64, true, false);
        Reglas.Texto(Nombre, nameof(Nombre), 255, true, false);
        Reglas.Texto(Emoji, nameof(Emoji), 32, true, false);
    }
}
