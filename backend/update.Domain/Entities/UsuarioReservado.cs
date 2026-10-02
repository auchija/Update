using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

/// <summary>Representa un registro de la tabla usuarios_reservados.</summary>
public sealed class UsuarioReservado : IEntidadValidable
{
    /// <summary>Columna nombre_usuario (text); obligatoria.</summary>
    public string NombreUsuario { get; set; } = string.Empty;

    /// <summary>Comprueba las reglas de entrada antes de guardar.</summary>
    public void Validar()
    {
        Reglas.Texto(NombreUsuario, nameof(NombreUsuario), 50, true, false);
    }
}
