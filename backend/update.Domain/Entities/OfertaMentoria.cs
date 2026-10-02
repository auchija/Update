using update.Domain.Common;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

/// <summary>Representa un registro de la tabla ofertas_mentoria.</summary>
public sealed class OfertaMentoria : BaseEntity, IEntidadValidable
{
    /// <summary>Columna usuario_mentor_id (uuid); obligatoria.</summary>
    public Guid UsuarioMentorId { get; set; }
    /// <summary>Columna titulo (text); obligatoria.</summary>
    public string Titulo { get; set; } = string.Empty;
    /// <summary>Columna descripcion (text); obligatoria.</summary>
    public string Descripcion { get; set; } = string.Empty;
    /// <summary>Columna categoria_id (integer); opcional.</summary>
    public int? CategoriaId { get; set; }
    /// <summary>Columna modalidad (modalidad_mentoria); obligatoria.</summary>
    public ModalidadMentoria Modalidad { get; set; } = ModalidadMentoria.Virtual;
    /// <summary>Columna duracion_minutos (smallint); obligatoria.</summary>
    public short DuracionMinutos { get; set; } = (short)60;
    /// <summary>Columna es_gratis (boolean); obligatoria.</summary>
    public bool EsGratis { get; set; } = true;
    /// <summary>Columna precio (numeric(14,2)); opcional.</summary>
    public decimal? Precio { get; set; }
    /// <summary>Columna moneda_codigo (char(3)); obligatoria.</summary>
    public string MonedaCodigo { get; set; } = "COP";
    /// <summary>Columna activo (boolean); obligatoria.</summary>
    public bool Activo { get; set; } = true;
    /// <summary>Relación hacia usuarios mediante usuario_mentor_id.</summary>
    [JsonIgnore]
    public Usuario UsuarioMentor { get; set; } = null!;
    /// <summary>Relación hacia categorias mediante categoria_id.</summary>
    [JsonIgnore]
    public Categoria? Categoria { get; set; }
    /// <summary>Relación hacia monedas mediante moneda_codigo.</summary>
    [JsonIgnore]
    public Moneda Moneda { get; set; } = null!;

    /// <summary>Comprueba las reglas de entrada antes de guardar.</summary>
    public void Validar()
    {
        Reglas.Exigir(UsuarioMentorId != Guid.Empty, "UsuarioMentorId es obligatorio.");
        Reglas.Texto(Titulo, nameof(Titulo), 255, true, false);
        Reglas.Texto(Descripcion, nameof(Descripcion), 10000, true, false);
        Reglas.Exigir(Enum.IsDefined(Modalidad), "Modalidad contiene un valor no permitido.");
        Reglas.Exigir(Precio is null || Precio >= 0, "Precio no puede ser negativo.");
        Reglas.Texto(MonedaCodigo, nameof(MonedaCodigo), 3, true, false);
        Reglas.Exigir(MonedaCodigo is null || MonedaCodigo.Length == 3, "MonedaCodigo debe tener 3 caracteres.");
        Reglas.FechaUtc(CreadoEn, nameof(CreadoEn));
        Reglas.FechaUtc(ActualizadoEn, nameof(ActualizadoEn));
    }
}
