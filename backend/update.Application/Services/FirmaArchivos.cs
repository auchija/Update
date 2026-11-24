using System.Security.Cryptography;
using System.Text;

namespace update.Application.Services;

/// <summary>Firma enlaces temporales para las etiquetas img, que no pueden enviar el token de sesión.</summary>
public sealed class FirmaArchivos(byte[] secreto)
{
    private const long VentanaSegundos = 3600;
    private readonly byte[] clave = HKDF.DeriveKey(HashAlgorithmName.SHA256, secreto, 32, info: "update:archivos"u8.ToArray());

    /// <summary>La caducidad se redondea por hora (1 a 2 horas) para que el navegador reutilice la misma URL.</summary>
    public string Firmar(string claveObjeto)
    {
        var expira = (DateTimeOffset.UtcNow.ToUnixTimeSeconds() / VentanaSegundos + 2) * VentanaSegundos;
        return $"exp={expira}&firma={Calcular(claveObjeto, expira)}";
    }

    public bool EsValida(string claveObjeto, long? expira, string? firma)
    {
        if (expira is not long valor || firma is null || valor <= DateTimeOffset.UtcNow.ToUnixTimeSeconds()) return false;
        return CryptographicOperations.FixedTimeEquals(
            Encoding.UTF8.GetBytes(firma), Encoding.UTF8.GetBytes(Calcular(claveObjeto, valor)));
    }

    private string Calcular(string claveObjeto, long expira)
        => Convert.ToHexStringLower(HMACSHA256.HashData(clave, Encoding.UTF8.GetBytes($"{claveObjeto}:{expira}")));
}
