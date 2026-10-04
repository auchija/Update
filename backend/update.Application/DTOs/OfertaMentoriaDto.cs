using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Application.Validation;
namespace update.Application.DTOs;

/// <summary>Contrato crear para ofertas_mentoria; no expone navegaciones.</summary>
public sealed class CrearOfertaMentoriaDto
{
    [JsonRequired]
    [GuidNoVacio]
    public Guid UsuarioMentorId { get; set; }
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(255)]
    public string Titulo { get; set; } = string.Empty;
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(10000)]
    public string Descripcion { get; set; } = string.Empty;
    public int? CategoriaId { get; set; }
    [EnumDataType(typeof(ModalidadMentoria))]
    public ModalidadMentoria Modalidad { get; set; } = ModalidadMentoria.Virtual;
    public short DuracionMinutos { get; set; } = (short)60;
    public bool EsGratis { get; set; } = true;
    public decimal? Precio { get; set; }
    [Required(AllowEmptyStrings = false)]
    [MaxLength(3)]
    [MinLength(3)]
    public string MonedaCodigo { get; set; } = "COP";
    public bool Activo { get; set; } = true;
}

/// <summary>Contrato actualizar para ofertas_mentoria; no expone navegaciones.</summary>
public sealed class ActualizarOfertaMentoriaDto
{
    [JsonRequired]
    [GuidNoVacio]
    public Guid UsuarioMentorId { get; set; }
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(255)]
    public string Titulo { get; set; } = string.Empty;
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(10000)]
    public string Descripcion { get; set; } = string.Empty;
    public int? CategoriaId { get; set; }
    [EnumDataType(typeof(ModalidadMentoria))]
    public ModalidadMentoria Modalidad { get; set; } = ModalidadMentoria.Virtual;
    public short DuracionMinutos { get; set; } = (short)60;
    public bool EsGratis { get; set; } = true;
    public decimal? Precio { get; set; }
    [Required(AllowEmptyStrings = false)]
    [MaxLength(3)]
    [MinLength(3)]
    public string MonedaCodigo { get; set; } = "COP";
    public bool Activo { get; set; } = true;
}

/// <summary>Contrato respuesta para ofertas_mentoria; no expone navegaciones.</summary>
public sealed class RespuestaOfertaMentoriaDto
{
    public Guid Id { get; set; }
    public Guid UsuarioMentorId { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public int? CategoriaId { get; set; }
    public ModalidadMentoria Modalidad { get; set; } = ModalidadMentoria.Virtual;
    public short DuracionMinutos { get; set; } = (short)60;
    public bool EsGratis { get; set; } = true;
    public decimal? Precio { get; set; }
    public string MonedaCodigo { get; set; } = "COP";
    public bool Activo { get; set; } = true;
    public DateTime CreadoEn { get; set; } = DateTime.UtcNow;
    public DateTime ActualizadoEn { get; set; } = DateTime.UtcNow;
}

