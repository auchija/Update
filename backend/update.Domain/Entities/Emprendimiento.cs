using update.Domain.Common;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

/// <summary>Representa un registro de la tabla emprendimientos.</summary>
public sealed class Emprendimiento : BaseEntity, IEntidadValidable
{
    /// <summary>Columna tipo_perfil (tipo_perfil); obligatoria.</summary>
    public TipoPerfil TipoPerfil { get; set; } = TipoPerfil.Emprendimiento;
    /// <summary>Columna categoria_id (integer); obligatoria.</summary>
    public int CategoriaId { get; set; }
    /// <summary>Columna etapa (etapa_emprendimiento); obligatoria.</summary>
    public EtapaEmprendimiento Etapa { get; set; } = EtapaEmprendimiento.Idea;
    /// <summary>Columna fundado_en (date); opcional.</summary>
    public DateOnly? FundadoEn { get; set; }
    /// <summary>Columna razon_social (text); opcional.</summary>
    public string? RazonSocial { get; set; }
    /// <summary>Columna nit (text); opcional.</summary>
    public string? Nit { get; set; }
    /// <summary>Columna correo_contacto (text); opcional.</summary>
    public string? CorreoContacto { get; set; }
    /// <summary>Columna telefono_contacto (text); opcional.</summary>
    public string? TelefonoContacto { get; set; }
    /// <summary>Columna whatsapp (text); opcional.</summary>
    public string? Whatsapp { get; set; }
    /// <summary>Columna direccion (text); opcional.</summary>
    public string? Direccion { get; set; }
    /// <summary>Columna envios_nacionales (boolean); obligatoria.</summary>
    public bool EnviosNacionales { get; set; } = false;
    /// <summary>Columna ofrece_remoto (boolean); obligatoria.</summary>
    public bool OfreceRemoto { get; set; } = false;
    /// <summary>Columna estado_verificacion (estado_verificacion); obligatoria.</summary>
    public EstadoVerificacion EstadoVerificacion { get; set; } = EstadoVerificacion.SinVerificar;
    /// <summary>Columna verificado_en (timestamptz); opcional.</summary>
    public DateTime? VerificadoEn { get; set; }
    /// <summary>Columna calificacion_promedio (numeric(3,2)); opcional.</summary>
    public decimal? CalificacionPromedio { get; set; }
    /// <summary>Columna total_calificaciones (integer); obligatoria.</summary>
    public int TotalCalificaciones { get; set; } = 0;
    /// <summary>Columna creado_por_usuario_id (uuid); obligatoria.</summary>
    public Guid CreadoPorUsuarioId { get; set; }
    /// <summary>Relación hacia categorias mediante categoria_id.</summary>
    [JsonIgnore]
    public Categoria Categoria { get; set; } = null!;
    /// <summary>Relación hacia usuarios mediante creado_por_usuario_id.</summary>
    [JsonIgnore]
    public Usuario CreadoPorUsuario { get; set; } = null!;
    /// <summary>Relación hacia perfiles mediante id, tipo_perfil.</summary>
    [JsonIgnore]
    public Perfil Perfil { get; set; } = null!;

    /// <summary>Comprueba las reglas de entrada antes de guardar.</summary>
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
