using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

/// <summary>Representa un registro de la tabla permisos_rol.</summary>
public sealed class PermisoRol : IEntidadValidable
{
    /// <summary>Columna rol (rol_emprendimiento); obligatoria.</summary>
    public RolEmprendimiento Rol { get; set; }
    /// <summary>Columna permiso (text); obligatoria.</summary>
    public string Permiso { get; set; } = string.Empty;

    /// <summary>Comprueba las reglas de entrada antes de guardar.</summary>
    public void Validar()
    {
        Reglas.Exigir(Enum.IsDefined(Rol), "Rol contiene un valor no permitido.");
        Reglas.Texto(Permiso, nameof(Permiso), 64, true, false);
    }
}
