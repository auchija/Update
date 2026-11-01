using update.Domain.Common;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;

namespace update.Domain.Entities;

/// <summary>
/// Representa una cuenta de usuario de la plataforma.
/// Un usuario puede tener uno o varios perfiles.
/// </summary>
public sealed class Usuario : BaseEntity, IEntidadValidable
{
    /// <summary>Correo electrónico de acceso a la cuenta.</summary>
    public string Correo { get; set; } = string.Empty;

    /// <summary>Hash de la contraseña; opcional si se utiliza autenticación externa.</summary>
    public string? HashContrasena { get; set; }

    /// <summary>Rol del usuario dentro de la plataforma.</summary>
    public RolPlataforma RolPlataforma { get; set; } = RolPlataforma.Usuario;

    /// <summary>Fecha en que se verificó el correo electrónico.</summary>
    public DateTime? CorreoVerificadoEn { get; set; }

    /// <summary>Fecha en que el usuario aceptó los términos y condiciones.</summary>
    public DateTime TerminosAceptadosEn { get; set; }

    /// <summary>Fecha del último inicio de sesión.</summary>
    public DateTime? UltimoIngresoEn { get; set; }

    /// <summary>Número de intentos fallidos de inicio de sesión.</summary>
    public short IntentosFallidos { get; set; } = 0;

    /// <summary>Fecha hasta la que la cuenta permanece bloqueada.</summary>
    public DateTime? BloqueadoHasta { get; set; }

    /// <summary>Fecha del último cambio de contraseña.</summary>
    public DateTime? ContrasenaCambiadaEn { get; set; }

    /// <summary>
    /// Perfiles asociados a la cuenta.
    /// Un usuario puede tener un perfil personal y varios perfiles de emprendimiento.
    /// </summary>
    [JsonIgnore]
    public ICollection<Perfil> Perfiles { get; set; } = new List<Perfil>();

    /// <summary>Comprueba las reglas de entrada antes de guardar.</summary>
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

    /// <summary>Verifica si la cuenta está bloqueada actualmente.</summary>
    public bool EstaBloqueada()
        => BloqueadoHasta.HasValue && BloqueadoHasta > DateTime.UtcNow;

    /// <summary>Verifica si el correo ha sido verificado.</summary>
    public bool CorreoEsVerificado()
        => CorreoVerificadoEn.HasValue;

    /// <summary>Incrementa los intentos fallidos de inicio de sesión.</summary>
    public void IncrementarIntentosFallidos()
    {
        Reglas.Exigir(
            IntentosFallidos < short.MaxValue,
            "El contador de intentos alcanzó su límite."
        );

        IntentosFallidos++;
        ActualizadoEn = DateTime.UtcNow;
    }

    /// <summary>Bloquea la cuenta por un período determinado.</summary>
    public void BloquearTempo(TimeSpan duracion)
    {
        Reglas.Exigir(
            duracion > TimeSpan.Zero,
            "La duración del bloqueo debe ser positiva."
        );

        BloqueadoHasta = DateTime.UtcNow.Add(duracion);
        ActualizadoEn = DateTime.UtcNow;
    }

    /// <summary>Reinicia los intentos fallidos después de un inicio de sesión exitoso.</summary>
    public void ReiniciarIntentosFallidos()
    {
        IntentosFallidos = 0;
        BloqueadoHasta = null;
        UltimoIngresoEn = DateTime.UtcNow;
        ActualizadoEn = DateTime.UtcNow;
    }

    /// <summary>Marca la contraseña como cambiada.</summary>
    public void MarcarContrasenaCambiada()
    {
        ContrasenaCambiadaEn = DateTime.UtcNow;
        ActualizadoEn = DateTime.UtcNow;
    }

    /// <summary>Marca el correo electrónico como verificado.</summary>
    public void VerificarCorreo()
    {
        CorreoVerificadoEn = DateTime.UtcNow;
        ActualizadoEn = DateTime.UtcNow;
    }
}