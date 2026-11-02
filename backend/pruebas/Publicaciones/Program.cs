using System.Reflection;
using update.Application.DTOs;
using update.Application.Services;
using update.Domain.Entities;
using update.Domain.Enums;
using update.Domain.Exceptions;
using update.Domain.Interfaces;

var usuarioId = Guid.NewGuid();
var perfil = new Perfil { Id = Guid.NewGuid(), UsuarioId = usuarioId, NombreVisible = "Ana", Tipo = TipoPerfil.Persona };
var datos = new List<Publicacion>();
var escrituras = 0;
var repositorio = Simular<IPublicacionRepository>((metodo, argumentos) => metodo switch
{
    "AgregarAsync" => Agregar((Publicacion)argumentos[0]!),
    "ActualizarAsync" => Actualizar(),
    "ObtenerConAutorAsync" => Task.FromResult(datos.FirstOrDefault(p => p.Id == (Guid)argumentos[0]! && p.EliminadoEn == null)),
    "ObtenerFeedAsync" => Task.FromResult<IReadOnlyList<Publicacion>>(datos.Where(p => p.EliminadoEn == null).ToList()),
    _ => throw new NotSupportedException(metodo)
});
var perfiles = Simular<IPerfilRepository>((_, _) => Task.FromResult<IReadOnlyList<Perfil>>([perfil]));
var tipoActivo = true;
var tipos = Simular<ITipoPublicacionRepository>((_, _) => Task.FromResult<TipoPublicacion?>(new TipoPublicacion { Codigo = "general", Activo = tipoActivo }));
var servicioPerfil = new ServicioMiPerfil(perfiles, null!, null!, null!);
var servicio = new ServicioPublicaciones(repositorio, tipos, servicioPerfil);
var ct = CancellationToken.None;

var creada = await servicio.CrearAsync(usuarioId, new() { Contenido = "  Hola  " }, ct);
Exigir(creada.Contenido == "Hola" && creada.Id != Guid.Empty, "Crear devuelve los datos guardados");
Exigir(datos[0].CreadoPorUsuarioId == usuarioId && datos[0].PerfilAutorId == perfil.Id, "Autor obtenido del usuario autenticado");
await DebeFallar<ExcepcionDominio>(() => servicio.CrearAsync(usuarioId, new() { Contenido = "  " }, ct));
await DebeFallar<ExcepcionDominio>(() => servicio.CrearAsync(usuarioId, new() { Contenido = new string('a', 5001) }, ct));
var antes = escrituras;
await DebeFallar<PublicacionAjenaException>(() => servicio.ActualizarAsync(creada.Id, Guid.NewGuid(), new() { Contenido = "Ajena" }, ct));
await DebeFallar<PublicacionAjenaException>(() => servicio.EliminarAsync(creada.Id, Guid.NewGuid(), ct));
Exigir(antes == escrituras && datos[0].Contenido == "Hola", "Las operaciones ajenas no escriben");
var editada = await servicio.ActualizarAsync(creada.Id, usuarioId, new() { Contenido = "Editada" }, ct);
Exigir(editada.Contenido == "Editada" && editada.ActualizadoEn?.Kind == DateTimeKind.Utc, "Edicion con fecha UTC");
var lista = await servicio.ListarAsync(usuarioId, 1, 20, ct);
Exigir(lista.Count == 1 && lista[0].EsPropia, "Consultar publicaciones guardadas");
await servicio.EliminarAsync(creada.Id, usuarioId, ct);
Exigir(datos.Count == 1 && datos[0].EliminadoEn?.Kind == DateTimeKind.Utc, "Borrado logico");
await DebeFallar<PublicacionNoEncontradaException>(() => servicio.EliminarAsync(creada.Id, usuarioId, ct));
tipoActivo = false;
await DebeFallar<ExcepcionDominio>(() => servicio.CrearAsync(usuarioId, new() { Contenido = "Hola" }, ct));
Console.WriteLine("Pruebas de publicaciones correctas.");

Task Agregar(Publicacion p)
{
    p.Id = Guid.NewGuid();
    p.PerfilAutor = perfil;
    datos.Add(p);
    escrituras++;
    return Task.CompletedTask;
}
Task Actualizar() { escrituras++; return Task.CompletedTask; }
static void Exigir(bool condicion, string mensaje)
{
    if (!condicion) throw new Exception(mensaje);
    Console.WriteLine(mensaje);
}
static async Task DebeFallar<T>(Func<Task> accion) where T : Exception
{
    try { await accion(); }
    catch (T) { return; }
    throw new Exception($"Se esperaba {typeof(T).Name}");
}
static T Simular<T>(Func<string, object?[], object?> llamada) where T : class
{
    var instancia = DispatchProxy.Create<T, RepositorioSimulado>();
    ((RepositorioSimulado)(object)instancia).Llamada = llamada;
    return instancia;
}
public class RepositorioSimulado : DispatchProxy
{
    public Func<string, object?[], object?> Llamada { get; set; } = null!;
    protected override object? Invoke(MethodInfo? metodo, object?[]? argumentos) => Llamada(metodo!.Name, argumentos!);
}
