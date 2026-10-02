using update.Domain.Common;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

/// <summary>Representa un registro de la tabla oportunidades.</summary>
public sealed class Oportunidad : BaseEntity, IEntidadValidable, IEliminable
{
    /// <summary>Columna perfil_autor_id (uuid); obligatoria.</summary>
    public Guid PerfilAutorId { get; set; }
    /// <summary>Columna creado_por_usuario_id (uuid); obligatoria.</summary>
    public Guid CreadoPorUsuarioId { get; set; }
    /// <summary>Columna tipo_codigo (text); obligatoria.</summary>
    public string TipoCodigo { get; set; } = string.Empty;
    /// <summary>Columna titulo (text); obligatoria.</summary>
    public string Titulo { get; set; } = string.Empty;
    /// <summary>Columna descripcion (text); obligatoria.</summary>
    public string Descripcion { get; set; } = string.Empty;
    /// <summary>Columna categoria_id (integer); opcional.</summary>
    public int? CategoriaId { get; set; }
    /// <summary>Columna ciudad_id (integer); opcional.</summary>
    public int? CiudadId { get; set; }
    /// <summary>Columna es_remoto (boolean); obligatoria.</summary>
    public bool EsRemoto { get; set; } = false;
    /// <summary>Columna estado (estado_oportunidad); obligatoria.</summary>
    public EstadoOportunidad Estado { get; set; } = EstadoOportunidad.Abierto;
    /// <summary>Columna expira_en (timestamptz); opcional.</summary>
    public DateTime? ExpiraEn { get; set; }
    /// <summary>Columna total_postulaciones (integer); obligatoria.</summary>
    public int TotalPostulaciones { get; set; } = 0;
    /// <summary>Columna eliminado_en (timestamptz); opcional.</summary>
    public DateTime? EliminadoEn { get; set; }
    /// <summary>Relación hacia perfiles mediante perfil_autor_id.</summary>
    [JsonIgnore]
    public Perfil PerfilAutor { get; set; } = null!;
    /// <summary>Relación hacia usuarios mediante creado_por_usuario_id.</summary>
    [JsonIgnore]
    public Usuario CreadoPorUsuario { get; set; } = null!;
    /// <summary>Relación hacia tipos_oportunidad mediante tipo_codigo.</summary>
    [JsonIgnore]
    public TipoOportunidad Tipo { get; set; } = null!;
    /// <summary>Relación hacia categorias mediante categoria_id.</summary>
    [JsonIgnore]
    public Categoria? Categoria { get; set; }
    /// <summary>Relación hacia ciudades mediante ciudad_id.</summary>
    [JsonIgnore]
    public Ciudad? Ciudad { get; set; }

    /// <summary>Comprueba las reglas de entrada antes de guardar.</summary>
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

    /// <summary>Marca el registro como eliminado sin borrarlo físicamente.</summary>
    public void Eliminar()
    {
        EliminadoEn ??= DateTime.UtcNow;
        ActualizadoEn = DateTime.UtcNow;
    }
}
