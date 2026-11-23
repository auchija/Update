using System.ComponentModel.DataAnnotations;
using update.Domain.Entities;
using update.Domain.Enums;
using update.Domain.Exceptions;
using update.Domain.Interfaces;

namespace update.Application.Services;

public sealed record DatosEmprendimiento(
    [Required, StringLength(120)] string Nombre,
    [Range(1, int.MaxValue)] int CategoriaId,
    EtapaEmprendimiento Etapa,
    Guid? PortadaArchivoId);

/// <summary>PortadaUrl sale como ruta relativa ("/uploads/..."); el controlador la completa con el host de la API.</summary>
public sealed record VistaEmprendimiento(Guid Id, string Nombre, int CategoriaId, EtapaEmprendimiento Etapa, Guid? PortadaArchivoId, string? PortadaUrl);

public sealed record CategoriaResumen(int Id, string Nombre, string Slug);

public sealed class MisEmprendimientos(IEmprendimientoRepository negocios, IPerfilRepository perfiles,
    ICategoriaRepository categorias, IArchivoRepository archivos, IUnitOfWork unidad)
{
    public async Task<IReadOnlyList<CategoriaResumen>> ListarCategorias(int pagina, int tamano, CancellationToken ct)
        => (await categorias.BuscarAsync(c => c.Activo, pagina, tamano, ct))
            .Select(c => new CategoriaResumen(c.Id, c.Nombre, c.Slug)).ToList();

    private static VistaEmprendimiento Vista(Emprendimiento e, Perfil p, string? clave) =>
        new(e.Id, p.NombreVisible, e.CategoriaId, e.Etapa, p.PortadaArchivoId, clave is null ? null : $"/uploads/{clave}");

    public async Task<VistaEmprendimiento?> Obtener(Guid usuario, Guid id, CancellationToken ct)
    {
        var e = await negocios.ObtenerPorClaveAsync([id], ct);
        if (e is null || e.CreadoPorUsuarioId != usuario) return null;
        var p = await perfiles.ObtenerPorClaveAsync([id], ct);
        if (p is null) return null;
        var a = p.PortadaArchivoId is Guid foto ? await archivos.ObtenerPorClaveAsync([foto], ct) : null;
        return Vista(e, p, a?.ClaveObjeto);
    }

    public async Task<IReadOnlyList<VistaEmprendimiento>> Listar(Guid usuario, int pagina, int tamano, CancellationToken ct)
    {
        var lista = await negocios.BuscarAsync(e => e.CreadoPorUsuarioId == usuario && e.Perfil.EliminadoEn == null, pagina, tamano, ct);
        var ids = lista.Select(e => e.Id).ToList();
        if (ids.Count == 0) return [];
        var autores = (await perfiles.BuscarAsync(p => ids.Contains(p.Id), 1, ids.Count, ct)).ToDictionary(p => p.Id);
        var fotos = autores.Values.Where(p => p.PortadaArchivoId.HasValue).Select(p => p.PortadaArchivoId!.Value).ToList();
        var claves = fotos.Count == 0 ? new Dictionary<Guid,string>() :
            (await archivos.BuscarAsync(a => fotos.Contains(a.Id), 1, fotos.Count, ct)).ToDictionary(a => a.Id, a => a.ClaveObjeto);
        return lista.Where(e => autores.ContainsKey(e.Id)).Select(e => Vista(e, autores[e.Id],
            claves.GetValueOrDefault(autores[e.Id].PortadaArchivoId ?? Guid.Empty))).ToList();
    }

    public async Task<VistaEmprendimiento?> Guardar(Guid usuario, Guid? id, DatosEmprendimiento datos, CancellationToken ct)
    {
        var nuevo = !id.HasValue;
        var clave = id ?? Guid.NewGuid();
        var e = nuevo ? new Emprendimiento { Id = clave, CreadoPorUsuarioId = usuario } : await negocios.ObtenerPorClaveAsync([clave], ct);
        if (e is null || e.CreadoPorUsuarioId != usuario) return null;
        var p = nuevo ? new Perfil { Id = clave, UsuarioId = usuario, Tipo = TipoPerfil.Emprendimiento,
            NombreUsuario = $"e_{clave:N}"[..24] } : await perfiles.ObtenerPorClaveAsync([clave], ct);
        if (p is null) return null;
        if (await categorias.ObtenerPorClaveAsync([datos.CategoriaId], ct) is not { Activo: true })
            throw new ExcepcionDominio("Categoria no disponible.");
        Archivo? portada = null;
        if (datos.PortadaArchivoId is Guid foto)
        {
            portada = await archivos.ObtenerPorClaveAsync([foto], ct);
            if (portada is not { Estado: EstadoArchivo.Listo, Proposito: PropositoArchivo.Portada } || portada.SubidoPorUsuarioId != usuario)
                throw new ExcepcionDominio("La portada no existe o no es tuya.");
        }
        p.NombreVisible = datos.Nombre.Trim();
        p.PortadaArchivoId = portada?.Id;
        e.CategoriaId = datos.CategoriaId;
        e.Etapa = datos.Etapa;
        await unidad.EjecutarEnTransaccionAsync(async token =>
        {
            if (nuevo) { await perfiles.AgregarAsync(p, token); await negocios.AgregarAsync(e, token); }
            else { await perfiles.ActualizarAsync(p, token); await negocios.ActualizarAsync(e, token); }
            return true;
        }, ct);
        return Vista(e, p, portada?.ClaveObjeto);
    }

    public async Task<bool> Eliminar(Guid usuario, Guid id, CancellationToken ct)
    {
        var e = await negocios.ObtenerPorClaveAsync([id], ct);
        if (e is null || e.CreadoPorUsuarioId != usuario) return false;
        var p = await perfiles.ObtenerPorClaveAsync([id], ct);
        if (p is null) return false;
        await unidad.EjecutarEnTransaccionAsync(async t =>
        {
            await negocios.EliminarAsync(e, t);
            await perfiles.EliminarAsync(p, t);
            return true;
        }, ct);
        return true;
    }
}
