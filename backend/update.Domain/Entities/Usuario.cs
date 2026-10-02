using update.Domain.Common;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

/// <summary>Representa un registro de la tabla usuarios.</summary>
public sealed class Usuario : BaseEntity, IEntidadValidable
{
    /// <summary>Columna tipo_perfil (tipo_perfil); obligatoria.</summary>
    public TipoPerfil TipoPerfil { get; set; } = TipoPerfil.Persona;
    /// <summary>Columna correo (text); obligatoria.</summary>
    public string Correo { get; set; } = string.Empty;
    /// <summary>Columna hash_contrasena (text); opcional.</summary>
    public string? HashContrasena { get; set; }
    /// <summary>Columna rol_plataforma (rol_plataforma); obligatoria.</summary>
    public RolPlataforma RolPlataforma { get; set; } = RolPlataforma.Usuario;
    /// <summary>Columna correo_verificado_en (timestamptz); opcional.</summary>
    public DateTime? CorreoVerificadoEn { get; set; }
    /// <summary>Columna terminos_aceptados_en (timestamptz); obligatoria.</summary>
    public DateTime TerminosAceptadosEn { get; set; }
    /// <summary>Columna ultimo_ingreso_en (timestamptz); opcional.</summary>
    public DateTime? UltimoIngresoEn { get; set; }
    /// <summary>Columna intentos_fallidos (smallint); obligatoria.</summary>
    public short IntentosFallidos { get; set; } = (short)0;
    /// <summary>Columna bloqueado_hasta (timestamptz); opcional.</summary>
    public DateTime? BloqueadoHasta { get; set; }
    /// <summary>Columna contrasena_cambiada_en (timestamptz); opcional.</summary>
    public DateTime? ContrasenaCambiadaEn { get; set; }
    /// <summary>Relación hacia perfiles mediante id, tipo_perfil.</summary>
    [JsonIgnore]
    public Perfil Perfil { get; set; } = null!;

    /// <summary>Comprueba las reglas de entrada antes de guardar.</summary>
    public void Validar()
    {
        Reglas.Exigir(Id != Guid.Empty, "Id es obligatorio.");
        Reglas.Exigir(Enum.IsDefined(TipoPerfil), "TipoPerfil contiene un valor no permitido.");
        Reglas.Texto(Correo, nameof(Correo), 254, true, false);
        Reglas.Texto(HashContrasena, nameof(HashContrasena), 512, false, false);
        Reglas.Exigir(Enum.IsDefined(RolPlataforma), "RolPlataforma contiene un valor no permitido.");
        Reglas.FechaUtc(CorreoVerificadoEn, nameof(CorreoVerificadoEn));
        Reglas.FechaUtc(TerminosAceptadosEn, nameof(TerminosAceptadosEn));
        Reglas.FechaUtc(UltimoIngresoEn, nameof(UltimoIngresoEn));
        Reglas.FechaUtc(BloqueadoHasta, nameof(BloqueadoHasta));
        Reglas.FechaUtc(ContrasenaCambiadaEn, nameof(ContrasenaCambiadaEn));
        Reglas.FechaUtc(CreadoEn, nameof(CreadoEn));
        Reglas.FechaUtc(ActualizadoEn, nameof(ActualizadoEn));
        Reglas.Exigir(TipoPerfil == TipoPerfil.Persona, "Un usuario debe tener un perfil de persona.");
    }
    /// <summary>
    /// Verifica si la cuenta está bloqueada actualmente
    /// </summary>
    public bool EstaBloqueada() 
        => BloqueadoHasta.HasValue && BloqueadoHasta > DateTime.UtcNow;

    /// <summary>
    /// Verifica si el correo ha sido verificado
    /// </summary>
    public bool CorreoEsVerificado() 
        => CorreoVerificadoEn.HasValue;

    /// <summary>
    /// Incrementa los intentos fallidos
    /// </summary>
    public void IncrementarIntentosFallidos()
    {
        Reglas.Exigir(IntentosFallidos < short.MaxValue, "El contador de intentos alcanzó su límite.");
        IntentosFallidos++;
        ActualizadoEn = DateTime.UtcNow;
    }

    /// <summary>
    /// Bloquea la cuenta por un período determinado (ej: 15 minutos)
    /// </summary>
    public void BloquearTempo(TimeSpan duracion)
    {
        Reglas.Exigir(duracion > TimeSpan.Zero, "La duración del bloqueo debe ser positiva.");
        BloqueadoHasta = DateTime.UtcNow.Add(duracion);
        ActualizadoEn = DateTime.UtcNow;
    }

    /// <summary>
    /// Reinicia los intentos fallidos cuando hay login exitoso
    /// </summary>
    public void ReiniciarIntentosFallidos()
    {
        IntentosFallidos = 0;
        BloqueadoHasta = null;
        UltimoIngresoEn = DateTime.UtcNow;
        ActualizadoEn = DateTime.UtcNow;
    }

    /// <summary>
    /// Marca la contraseña como cambiada
    /// </summary>
    public void MarcarContrasenaCambiada()
    {
        ContrasenaCambiadaEn = DateTime.UtcNow;
        ActualizadoEn = DateTime.UtcNow;
    }

    /// <summary>
    /// Verifica el correo como validado
    /// </summary>
    public void VerificarCorreo()
    {
        CorreoVerificadoEn = DateTime.UtcNow;
        ActualizadoEn = DateTime.UtcNow;
    }
}
