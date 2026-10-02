using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

/// <summary>Representa un registro de la tabla intereses_usuario.</summary>
public sealed class InteresUsuario : IEntidadValidable
{
    /// <summary>Columna usuario_id (uuid); obligatoria.</summary>
    public Guid UsuarioId { get; set; }
    /// <summary>Columna categoria_id (integer); obligatoria.</summary>
    public int CategoriaId { get; set; }
    /// <summary>Columna creado_en (timestamptz); obligatoria.</summary>
    public DateTime CreadoEn { get; set; } = DateTime.UtcNow;
    /// <summary>Relación hacia usuarios mediante usuario_id.</summary>
    [JsonIgnore]
    public Usuario Usuario { get; set; } = null!;
    /// <summary>Relación hacia categorias mediante categoria_id.</summary>
    [JsonIgnore]
    public Categoria Categoria { get; set; } = null!;

    /// <summary>Comprueba las reglas de entrada antes de guardar.</summary>
    public void Validar()
    {
        Reglas.Exigir(UsuarioId != Guid.Empty, "UsuarioId es obligatorio.");
        Reglas.Exigir(CategoriaId > 0, "CategoriaId debe ser positivo.");
        Reglas.FechaUtc(CreadoEn, nameof(CreadoEn));
    }
}
