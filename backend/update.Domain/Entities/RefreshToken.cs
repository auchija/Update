using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;

namespace update.Domain.Entities;
public sealed class RefreshToken
{
    public Guid Id { get; set; }
    public Guid UsuarioId { get; set; }
    public string Hash { get; set; } = string.Empty;
    public DateTime ExpiraEn { get; set; }
    public DateTime? RevocadoEn { get; set; }

    public bool EstaActivo(DateTime ahora) =>
        RevocadoEn is null && ExpiraEn > ahora;

    /// <summary>Tras renovar, el token anterior solo sirve durante la gracia: cubre recargas dobles y pestañas simultáneas.</summary>
    public void Rotar(DateTime ahora, TimeSpan gracia)
    {
        var limite = ahora.Add(gracia);
        if (limite < ExpiraEn) ExpiraEn = limite;
    }
}
