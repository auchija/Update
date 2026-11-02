using System.Security.Cryptography;
using update.Domain.Entities;
using update.Domain.Enums;
using update.Domain.Exceptions;
using update.Domain.Interfaces;

namespace update.Application.Services;

public sealed record OpcionesSubida(string Carpeta);

public sealed class ServicioSubidaArchivos(IArchivoRepository archivos, OpcionesSubida opciones)
{
    public const long TamanoMaximoBytes = 5 * 1024 * 1024;

    private sealed record FormatoImagen(string TipoMime, string Extension, string[] ExtensionesAceptadas);

    private static readonly FormatoImagen Jpeg = new("image/jpeg", "jpg", [".jpg", ".jpeg"]);
    private static readonly FormatoImagen Png = new("image/png", "png", [".png"]);
    private static readonly FormatoImagen Webp = new("image/webp", "webp", [".webp"]);

    public async Task<Archivo> SubirFotoPerfilAsync(
        Guid usuarioId,
        Stream contenido,
        string nombreOriginal,
        long tamanoBytes,
        CancellationToken cancellationToken)
    {
        if (tamanoBytes <= 0) throw new ExcepcionDominio("El archivo está vacío.");
        if (tamanoBytes > TamanoMaximoBytes) throw new ExcepcionDominio("La foto no puede pesar más de 5 MB.");

        using var memoria = new MemoryStream();
        await contenido.CopyToAsync(memoria, cancellationToken);
        var bytes = memoria.ToArray();

        var formato = DetectarFormato(bytes)
            ?? throw new ExcepcionDominio("Sube una imagen JPG, PNG o WEBP.");
        var nombre = Path.GetFileName(nombreOriginal);
        if (!formato.ExtensionesAceptadas.Contains(Path.GetExtension(nombre).ToLowerInvariant()))
            throw new ExcepcionDominio("La extensión no coincide con el contenido de la imagen.");

        var clave = $"{Guid.NewGuid():N}.{formato.Extension}";
        var ruta = Path.Combine(opciones.Carpeta, clave);
        await File.WriteAllBytesAsync(ruta, bytes, cancellationToken);

        var archivo = new Archivo
        {
            SubidoPorUsuarioId = usuarioId,
            Tipo = TipoArchivo.Imagen,
            Proposito = PropositoArchivo.FotoPerfil,
            ProveedorAlmacenamiento = "local",
            Bucket = "uploads",
            ClaveObjeto = clave,
            NombreOriginal = nombre.Length <= 255 ? nombre : nombre[^255..],
            TipoMime = formato.TipoMime,
            Extension = formato.Extension,
            TamanoBytes = bytes.Length,
            ChecksumSha256 = Convert.ToHexStringLower(SHA256.HashData(bytes)),
            Visibilidad = VisibilidadArchivo.Publico
        };
        archivo.MarcarListo();

        try
        {
            await archivos.AgregarAsync(archivo, cancellationToken);
        }
        catch
        {
            File.Delete(ruta);
            throw;
        }

        return archivo;
    }

    private static FormatoImagen? DetectarFormato(ReadOnlySpan<byte> bytes)
    {
        if (bytes.StartsWith<byte>([0xFF, 0xD8, 0xFF])) return Jpeg;
        if (bytes.StartsWith<byte>([0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A])) return Png;
        if (bytes.Length >= 12 && bytes.StartsWith("RIFF"u8) && bytes.Slice(8, 4).SequenceEqual("WEBP"u8)) return Webp;
        return null;
    }
}
