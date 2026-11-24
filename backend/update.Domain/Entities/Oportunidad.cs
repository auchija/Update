using update.Domain.Common;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

public sealed class Oportunidad : BaseEntity, IEntidadValidable, IEliminable
{
    public Guid PerfilAutorId { get; set; }
    public Guid CreadoPorUsuarioId { get; set; }
    public string TipoCodigo { get; set; } = string.Empty;
    public string Titulo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public int? CategoriaId { get; set; }
    public int? CiudadId { get; set; }
    public bool EsRemoto { get; set; } = false;
    public EstadoOportunidad Estado { get; set; } = EstadoOportunidad.Abierto;
    public DateTime? ExpiraEn { get; set; }
    public int TotalPostulaciones { get; set; } = 0;
    public DateTime? EliminadoEn { get; set; }
    [JsonIgnore]
    public Perfil PerfilAutor { get; set; } = null!;
    [JsonIgnore]
    public Usuario CreadoPorUsuario { get; set; } = null!;
    [JsonIgnore]
    public TipoOportunidad Tipo { get; set; } = null!;
    [JsonIgnore]
    public Categoria? Categoria { get; set; }
    [JsonIgnore]
    public Ciudad? Ciudad { get; set; }

    public void Validar()
    {
        Reglas.Exigir(PerfilAutorId != Guid.Empty, "PerfilAutorId es obligatorio.");
        Reglas.Exigir(CreadoPorUsuarioId != Guid.Empty, "CreadoPorUsuarioId es obligatorio.");
        Reglas.Texto(TipoCodigo, nameof(TipoCodigo), 64, true, false);
        Reglas.Texto(Titulo, nameof(Titulo), 255, true, false);
        Reglas.Texto(Descripcion, nameof(Descripcion), 10000, true, false);
        Reglas.Exigir(Enum.IsDefined(Estado), "Estado contiene un valor no permitido.");
        Reglas.FechaUtc(ExpiraEn, nameof(ExpiraEn));
        Reglas.Exigir(TotalPostulaciones >= 0, "TotalPostulaciones no puede ser negativo.");
        Reglas.FechaUtc(CreadoEn, nameof(CreadoEn));
        Reglas.FechaUtc(ActualizadoEn, nameof(ActualizadoEn));
        Reglas.FechaUtc(EliminadoEn, nameof(EliminadoEn));
    }

    public void Eliminar()
    {
        EliminadoEn ??= DateTime.UtcNow;
        ActualizadoEn = DateTime.UtcNow;
    }
}
