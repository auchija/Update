using update.Domain.Common;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

public sealed class Emprendimiento : BaseEntity, IEntidadValidable
{
    public TipoPerfil TipoPerfil { get; set; } = TipoPerfil.Emprendimiento;
    public int CategoriaId { get; set; }
    public EtapaEmprendimiento Etapa { get; set; } = EtapaEmprendimiento.Idea;
    public DateOnly? FundadoEn { get; set; }
    public string? RazonSocial { get; set; }
    public string? Nit { get; set; }
    public string? CorreoContacto { get; set; }
    public string? TelefonoContacto { get; set; }
    public string? Whatsapp { get; set; }
    public string? Direccion { get; set; }
    public bool EnviosNacionales { get; set; } = false;
    public bool OfreceRemoto { get; set; } = false;
    public EstadoVerificacion EstadoVerificacion { get; set; } = EstadoVerificacion.SinVerificar;
    public DateTime? VerificadoEn { get; set; }
    public decimal? CalificacionPromedio { get; set; }
    public int TotalCalificaciones { get; set; } = 0;
    public Guid CreadoPorUsuarioId { get; set; }
    [JsonIgnore]
    public Categoria Categoria { get; set; } = null!;
    [JsonIgnore]
    public Usuario CreadoPorUsuario { get; set; } = null!;
    [JsonIgnore]
    public Perfil Perfil { get; set; } = null!;

    public void Validar()
    {
        Reglas.Exigir(Id != Guid.Empty, "Id es obligatorio.");
        Reglas.Exigir(Enum.IsDefined(TipoPerfil), "TipoPerfil contiene un valor no permitido.");
        Reglas.Exigir(CategoriaId > 0, "CategoriaId debe ser positivo.");
        Reglas.Exigir(Enum.IsDefined(Etapa), "Etapa contiene un valor no permitido.");
        Reglas.Texto(RazonSocial, nameof(RazonSocial), 255, false, false);
        Reglas.Texto(Nit, nameof(Nit), 255, false, false);
        Reglas.Texto(CorreoContacto, nameof(CorreoContacto), 254, false, false);
        Reglas.Texto(TelefonoContacto, nameof(TelefonoContacto), 30, false, false);
        Reglas.Texto(Whatsapp, nameof(Whatsapp), 30, false, false);
        Reglas.Texto(Direccion, nameof(Direccion), 500, false, false);
        Reglas.Exigir(Enum.IsDefined(EstadoVerificacion), "EstadoVerificacion contiene un valor no permitido.");
        Reglas.FechaUtc(VerificadoEn, nameof(VerificadoEn));
        Reglas.Exigir(TotalCalificaciones >= 0, "TotalCalificaciones no puede ser negativo.");
        Reglas.Exigir(CreadoPorUsuarioId != Guid.Empty, "CreadoPorUsuarioId es obligatorio.");
        Reglas.FechaUtc(CreadoEn, nameof(CreadoEn));
        Reglas.FechaUtc(ActualizadoEn, nameof(ActualizadoEn));
        Reglas.Exigir(TipoPerfil == TipoPerfil.Emprendimiento, "Un emprendimiento requiere un perfil de emprendimiento.");
    }
}
