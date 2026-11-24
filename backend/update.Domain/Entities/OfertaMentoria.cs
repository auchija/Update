using update.Domain.Common;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

public sealed class OfertaMentoria : BaseEntity, IEntidadValidable
{
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
    [JsonIgnore]
    public Usuario UsuarioMentor { get; set; } = null!;
    [JsonIgnore]
    public Categoria? Categoria { get; set; }
    [JsonIgnore]
    public Moneda Moneda { get; set; } = null!;

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
