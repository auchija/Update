using System.Security.Cryptography;
using Microsoft.Extensions.Logging;
using update.Domain.Entities;
using update.Domain.Enums;
using update.Domain.Exceptions;
using update.Domain.Interfaces;

namespace update.Application.Services;

public sealed record OpcionesSubida(string Carpeta);

public sealed class ServicioSubidaArchivos(IArchivoRepository archivos, OpcionesSubida opciones, Lazy<IAlmacenObjetos> almacenRemoto,
    IProductoRepository productos, IProductoArchivoRepository adjuntos, FirmaArchivos firmas, ILogger<ServicioSubidaArchivos> logger)
{
    private IAlmacenObjetos Almacen => almacenRemoto.Value;
    public const long TamanoMaximoBytes = 5 * 1024 * 1024;

    private sealed record FormatoImagen(string TipoMime, string Extension, string[] ExtensionesAceptadas);

    private static readonly FormatoImagen Jpeg = new("image/jpeg", "jpg", [".jpg", ".jpeg"]);
    private static readonly FormatoImagen Png = new("image/png", "png", [".png"]);
    private static readonly FormatoImagen Webp = new("image/webp", "webp", [".webp"]);

    private static readonly Dictionary<string, PropositoArchivo> Propositos = new()
    {
        ["foto_perfil"] = PropositoArchivo.FotoPerfil,
        ["publicacion"] = PropositoArchivo.Publicacion,
        ["producto"] = PropositoArchivo.Producto,
        ["emprendimiento"] = PropositoArchivo.Portada
    };

    public async Task<Archivo> SubirImagenAsync(
        Guid usuarioId,
        string? proposito,
        Stream contenido,
        string nombreOriginal,
        long tamanoBytes,
        CancellationToken cancellationToken)
    {
        if (!Propositos.TryGetValue(proposito ?? "foto_perfil", out var propositoArchivo))
            throw new ExcepcionDominio("Proposito no valido.");
        if (tamanoBytes <= 0) throw new ExcepcionDominio("El archivo está vacío.");
        if (tamanoBytes > TamanoMaximoBytes) throw new ExcepcionDominio("El archivo supera los 5 MB.");

        using var memoria = new MemoryStream();
        await contenido.CopyToAsync(memoria, cancellationToken);
        var bytes = memoria.ToArray();

        if (bytes.Length != tamanoBytes || bytes.Length > TamanoMaximoBytes) throw new ExcepcionDominio("Tamano incorrecto.");
        var formato = DetectarFormato(bytes)
            ?? throw new ExcepcionDominio("Sube JPG, PNG, WEBP o PDF.");
        if (formato.Extension == "pdf" && propositoArchivo is PropositoArchivo.FotoPerfil or PropositoArchivo.Portada)
            throw new ExcepcionDominio("La foto y la portada deben ser imagenes.");
        var nombre = Path.GetFileName(nombreOriginal.Replace((char)92, (char)47));
        if (!formato.ExtensionesAceptadas.Contains(Path.GetExtension(nombre).ToLowerInvariant()))
            throw new ExcepcionDominio("La extensión no coincide con el contenido de la imagen.");

        var clave = $"{Guid.NewGuid():N}.{formato.Extension}";
        await Almacen.GuardarAsync(clave, bytes, formato.TipoMime, cancellationToken);

        var archivo = new Archivo
        {
            SubidoPorUsuarioId = usuarioId,
            Tipo = formato.Extension == "pdf" ? TipoArchivo.Documento : TipoArchivo.Imagen,
            Proposito = propositoArchivo,
            ProveedorAlmacenamiento = "supabase",
            Bucket = Almacen.Bucket,
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
            await Almacen.BorrarAsync(clave, CancellationToken.None);
            throw;
        }

        return archivo;
    }

    public async Task<IReadOnlyList<Archivo>> HistorialAsync(Guid usuario, int pagina, int tamano, CancellationToken ct)
        => await archivos.ObtenerHistorialAsync(usuario, pagina, tamano, ct);

    public async Task<(Archivo Archivo, byte[] Datos, bool Restringido)?> AbrirAsync(string clave, long? expira, string? firma, CancellationToken ct)
    {
        if (!ClaveValida(clave)) return null;
        var archivo = (await archivos.BuscarAsync(a => a.ClaveObjeto == clave && a.Estado == EstadoArchivo.Listo
            && a.Visibilidad == VisibilidadArchivo.Publico, 1, 1, ct)).FirstOrDefault();
        if (archivo is null) return null;
        var restringido = archivo.Proposito == PropositoArchivo.Publicacion
            && await archivos.EsDeCuentaPrivadaAsync(archivo.SubidoPorUsuarioId, ct);
        if (restringido && !firmas.EsValida(clave, expira, firma)) return null;
        byte[]? datos;
        if (archivo.ProveedorAlmacenamiento == "local")
        {
            var ruta = Path.Combine(opciones.Carpeta, clave);
            datos = File.Exists(ruta) ? await File.ReadAllBytesAsync(ruta, ct) : null;
        }
        else if (archivo.ProveedorAlmacenamiento == "supabase" && archivo.Bucket == Almacen.Bucket)
            datos = await Almacen.LeerAsync(clave, ct);
        else return null;
        return datos is null ? null : (archivo, datos, restringido);
    }

    public async Task EliminarPropioAsync(Guid usuario, Guid id, CancellationToken ct)
    {
        var a = await archivos.ObtenerPorClaveAsync([id], ct)
            ?? throw new RecursoNoEncontradoException("El archivo no existe o ya fue eliminado.");
        if (a.SubidoPorUsuarioId != usuario) throw new AccesoDenegadoException("Solo puedes eliminar tus propios archivos.");
        if (await archivos.EnUsoAsync(id, ct))
            throw new ConflictoException("Este archivo está en uso. Quítalo primero de la publicación, el perfil o el producto.");
        var esLocal = a.ProveedorAlmacenamiento == "local";
        if (!ClaveValida(a.ClaveObjeto) || (!esLocal && (a.ProveedorAlmacenamiento != "supabase" || a.Bucket != Almacen.Bucket)))
            throw new ConflictoException("Este archivo no se puede eliminar desde aquí.");
        await archivos.EliminarAsync(a, CancellationToken.None);
        try
        {
            if (esLocal) File.Delete(Path.Combine(opciones.Carpeta, a.ClaveObjeto));
            else await Almacen.BorrarAsync(a.ClaveObjeto, CancellationToken.None);
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or HttpRequestException or TaskCanceledException)
        {
            logger.LogWarning(ex, "El archivo {Clave} se quitó de la base pero quedó en el almacenamiento.", a.ClaveObjeto);
        }
    }

    public async Task AdjuntarProductoAsync(Guid usuario, Guid productoId, Guid archivoId, CancellationToken ct)
    {
        var producto = await productos.ObtenerPorClaveAsync([productoId], ct);
        var archivo = await archivos.ObtenerPorClaveAsync([archivoId], ct);
        if (producto is null || archivo is null) throw new RecursoNoEncontradoException("El producto o el archivo no existe.");
        if (producto.CreadoPorUsuarioId != usuario || archivo.SubidoPorUsuarioId != usuario)
            throw new AccesoDenegadoException("Solo puedes usar tus propios productos y archivos.");
        if (archivo.Proposito != PropositoArchivo.Producto || archivo.Estado != EstadoArchivo.Listo)
            throw new ExcepcionDominio("Ese archivo no se puede adjuntar a un producto.");
        if (await adjuntos.ObtenerPorClaveAsync([productoId, archivoId], ct) is null)
            await adjuntos.AgregarAsync(new ProductoArchivo { ProductoId = productoId, ArchivoId = archivoId }, ct);
    }

    public async Task DesvincularProductoAsync(Guid usuario, Guid productoId, Guid archivoId, CancellationToken ct)
    {
        var producto = await productos.ObtenerPorClaveAsync([productoId], ct)
            ?? throw new RecursoNoEncontradoException("El producto no existe.");
        if (producto.CreadoPorUsuarioId != usuario) throw new AccesoDenegadoException("Solo puedes cambiar tus propios productos.");
        var enlace = await adjuntos.ObtenerPorClaveAsync([productoId, archivoId], ct);
        if (enlace is not null) await adjuntos.EliminarAsync(enlace, ct);
    }

    private static bool ClaveValida(string clave) =>
        clave == Path.GetFileName(clave) && Guid.TryParseExact(Path.GetFileNameWithoutExtension(clave), "N", out _)
        && Path.GetExtension(clave) is ".jpg" or ".png" or ".webp" or ".pdf";

    private static FormatoImagen? DetectarFormato(ReadOnlySpan<byte> bytes)
    {
        if (bytes.StartsWith("%PDF-"u8)) return new("application/pdf", "pdf", [".pdf"]);
        if (bytes.StartsWith<byte>([0xFF, 0xD8, 0xFF])) return Jpeg;
        if (bytes.StartsWith<byte>([0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A])) return Png;
        if (bytes.Length >= 12 && bytes.StartsWith("RIFF"u8) && bytes.Slice(8, 4).SequenceEqual("WEBP"u8)) return Webp;
        return null;
    }
}
