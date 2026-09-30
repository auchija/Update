using update.Domain.Common;
using update.Domain.Enums;

namespace update.Domain.Entities;

public class Usuario : BaseEntity
{
    /// <summary>
    /// Tipo de perfil (Persona, Empresa, Gobierno)
    /// </summary>
    public TipoPerfil TipoPerfil { get; set; } = TipoPerfil.Persona;

    /// <summary>
    /// Correo electrónico único del usuario
    /// </summary>
    public string Correo { get; set; } = string.Empty;

    /// <summary>
    /// Hash de la contraseña (nunca almacenar en texto plano)
    /// </summary>
    public string? HashContrasena { get; set; }

    /// <summary>
    /// Rol en la plataforma
    /// </summary>
    public RolPlataforma RolPlataforma { get; set; } = RolPlataforma.Usuario;

    /// <summary>
    /// Fecha cuando el correo fue verificado
    /// </summary>
    public DateTime? CorreoVerificadoEn { get; set; }

    /// <summary>
    /// Fecha cuando aceptó los términos y condiciones
    /// </summary>
    public DateTime TerminosAceptadosEn { get; set; }

    /// <summary>
    /// Último ingreso a la plataforma
    /// </summary>
    public DateTime? UltimoIngresoEn { get; set; }

    /// <summary>
    /// Contador de intentos fallidos de login
    /// </summary>
    public short IntentosFallidos { get; set; } = 0;

    /// <summary>
    /// Fecha hasta cuando la cuenta está bloqueada (para prevenir fuerza bruta)
    /// </summary>
    public DateTime? BloqueadoHasta { get; set; }

    /// <summary>
    /// Fecha del último cambio de contraseña
    /// </summary>
    public DateTime? ContrasenaCambiadaEn { get; set; }

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
        IntentosFallidos++;
        ActualizadoEn = DateTime.UtcNow;
    }

    /// <summary>
    /// Bloquea la cuenta por un período determinado (ej: 15 minutos)
    /// </summary>
    public void BloquearTempo(TimeSpan duracion)
    {
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