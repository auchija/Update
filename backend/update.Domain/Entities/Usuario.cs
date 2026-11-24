using update.Domain.Common;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;

namespace update.Domain.Entities;

public sealed class Usuario : BaseEntity, IEntidadValidable
{
    public string Correo { get; set; } = string.Empty;

    public string? HashContrasena { get; set; }

    public RolPlataforma RolPlataforma { get; set; } = RolPlataforma.Usuario;

    public DateTime? CorreoVerificadoEn { get; set; }

    public DateTime TerminosAceptadosEn { get; set; }

    public DateTime? UltimoIngresoEn { get; set; }

    public short IntentosFallidos { get; set; } = 0;

    public DateTime? BloqueadoHasta { get; set; }

    public DateTime? ContrasenaCambiadaEn { get; set; }

    [JsonIgnore]
    public ICollection<Perfil> Perfiles { get; set; } = new List<Perfil>();

    public const int MaxIntentosFallidos = 5;
    public static readonly TimeSpan DuracionBloqueo = TimeSpan.FromMinutes(15);

    public bool EstaBloqueada(DateTime ahora) =>
        BloqueadoHasta is not null && BloqueadoHasta > ahora;

    public void RegistrarIntentoFallido(DateTime ahora)
    {
        if (BloqueadoHasta is not null && BloqueadoHasta <= ahora)
        {
            BloqueadoHasta = null;
            IntentosFallidos = 0;
        }

        IntentosFallidos++;

        if (IntentosFallidos >= MaxIntentosFallidos)
            BloqueadoHasta = ahora + DuracionBloqueo;
    }

    public void Validar()
    {
        Reglas.Exigir(Id != Guid.Empty, "Id es obligatorio.");

        Reglas.Texto(Correo, nameof(Correo), 254, true, false);
        Reglas.Texto(HashContrasena, nameof(HashContrasena), 512, false, false);

        Reglas.Exigir(
            Enum.IsDefined(RolPlataforma),
            "RolPlataforma contiene un valor no permitido."
        );

        Reglas.FechaUtc(CorreoVerificadoEn, nameof(CorreoVerificadoEn));
        Reglas.FechaUtc(TerminosAceptadosEn, nameof(TerminosAceptadosEn));
        Reglas.FechaUtc(UltimoIngresoEn, nameof(UltimoIngresoEn));
        Reglas.FechaUtc(BloqueadoHasta, nameof(BloqueadoHasta));
        Reglas.FechaUtc(ContrasenaCambiadaEn, nameof(ContrasenaCambiadaEn));
        Reglas.FechaUtc(CreadoEn, nameof(CreadoEn));
        Reglas.FechaUtc(ActualizadoEn, nameof(ActualizadoEn));
    }

    public bool EstaBloqueada()
        => BloqueadoHasta.HasValue && BloqueadoHasta > DateTime.UtcNow;

    public bool CorreoEsVerificado()
        => CorreoVerificadoEn.HasValue;

    public void IncrementarIntentosFallidos()
    {
        Reglas.Exigir(
            IntentosFallidos < short.MaxValue,
            "El contador de intentos alcanzó su límite."
        );

        IntentosFallidos++;
        ActualizadoEn = DateTime.UtcNow;
    }

    public void BloquearTempo(TimeSpan duracion)
    {
        Reglas.Exigir(
            duracion > TimeSpan.Zero,
            "La duración del bloqueo debe ser positiva."
        );

        BloqueadoHasta = DateTime.UtcNow.Add(duracion);
        ActualizadoEn = DateTime.UtcNow;
    }

    public void ReiniciarIntentosFallidos()
    {
        IntentosFallidos = 0;
        BloqueadoHasta = null;
        UltimoIngresoEn = DateTime.UtcNow;
        ActualizadoEn = DateTime.UtcNow;
    }

    public void MarcarContrasenaCambiada()
    {
        ContrasenaCambiadaEn = DateTime.UtcNow;
        ActualizadoEn = DateTime.UtcNow;
    }

    public void VerificarCorreo()
    {
        CorreoVerificadoEn = DateTime.UtcNow;
        ActualizadoEn = DateTime.UtcNow;
    }
}