using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

public sealed class UsuarioReservado : IEntidadValidable
{
    public string NombreUsuario { get; set; } = string.Empty;

    public void Validar()
    {
        Reglas.Texto(NombreUsuario, nameof(NombreUsuario), 50, true, false);
    }
}
