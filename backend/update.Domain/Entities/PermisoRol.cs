using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

public sealed class PermisoRol : IEntidadValidable
{
    public RolEmprendimiento Rol { get; set; }
    public string Permiso { get; set; } = string.Empty;

    public void Validar()
    {
        Reglas.Exigir(Enum.IsDefined(Rol), "Rol contiene un valor no permitido.");
        Reglas.Texto(Permiso, nameof(Permiso), 64, true, false);
    }
}
