using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using update.Domain.Entities;
using update.Domain.Enums;
using update.Domain.Exceptions;
using update.Infrastructure.Data;
using update.Infrastructure.Data.Configurations;

var raiz = args.Length > 0 ? Path.GetFullPath(args[0]) : Directory.GetCurrentDirectory();
using var documento = JsonDocument.Parse(File.ReadAllText(Path.Combine(raiz, "modelo", "modelo_analizado.json")));
using var contexto = new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>()
    .UseNpgsql("Host=localhost;Database=update_verificacion;Username=postgres").Options);
var modelo = contexto.GetService<IDesignTimeModel>().Model;
var tablas = documento.RootElement.GetProperty("tables");
Exigir(modelo.GetEntityTypes().Count() == tablas.GetArrayLength(), "Cantidad incorrecta de entidades.");
var totalColumnas = 0;
foreach (var tabla in tablas.EnumerateArray())
{
    var nombre = tabla.GetProperty("name").GetString()!;
    var entidad = modelo.GetEntityTypes().Single(e => e.GetTableName() == nombre);
    var objeto = StoreObjectIdentifier.Table(nombre, null);
    var columnas = tabla.GetProperty("cols").EnumerateArray().ToArray();
    Exigir(entidad.GetProperties().Count() == columnas.Length, $"Columnas extra o faltantes en {nombre}.");
    foreach (var columna in columnas)
    {
        var sql = columna.GetProperty("name").GetString()!;
        var propiedad = entidad.GetProperties().Single(p => p.GetColumnName(objeto) == sql);
        Exigir(propiedad.IsNullable == columna.GetProperty("nullable").GetBoolean(), $"Nulabilidad incorrecta: {nombre}.{sql}.");
        if (columna.GetProperty("type").GetString() == "smallint") Exigir(Nullable.GetUnderlyingType(propiedad.ClrType) == typeof(short) || propiedad.ClrType == typeof(short), $"SMALLINT incorrecto: {nombre}.{sql}.");
        if (columna.GetProperty("type").GetString() == "bigint") Exigir(Nullable.GetUnderlyingType(propiedad.ClrType) == typeof(long) || propiedad.ClrType == typeof(long), $"BIGINT incorrecto: {nombre}.{sql}.");
        totalColumnas++;
    }
    var clavesEsperadas = tabla.GetProperty("keys").EnumerateArray().Select(c => c.GetString()).ToArray();
    var clavesReales = entidad.FindPrimaryKey()!.Properties.Select(p => p.GetColumnName(objeto)).ToArray();
    Exigir(clavesEsperadas.SequenceEqual(clavesReales), $"Clave incorrecta en {nombre}.");
    var filtro = entidad.GetDeclaredQueryFilters().Any();
    Exigir(filtro == columnas.Any(c => c.GetProperty("name").GetString() == "eliminado_en"), $"Filtro incorrecto en {nombre}.");
}
var referencias = documento.RootElement.GetProperty("references");
var fks = modelo.GetEntityTypes().SelectMany(e => e.GetForeignKeys()).ToArray();
Exigir(fks.Length == referencias.GetArrayLength(), "Cantidad incorrecta de relaciones.");
foreach (var referencia in referencias.EnumerateArray())
{
    var dep = referencia.GetProperty("dep").GetString()!;
    var principal = referencia.GetProperty("principal").GetString()!;
    var esperadas = referencia.GetProperty("fk").EnumerateArray().Select(x => x.GetString()).ToArray();
    var fk = fks.Single(f => f.DeclaringEntityType.GetTableName() == dep && f.PrincipalEntityType.GetTableName() == principal
        && f.Properties.Select(p => p.GetColumnName()).SequenceEqual(esperadas));
    Exigir(fk.PrincipalKey.Properties.Select(p => p.GetColumnName()).SequenceEqual(
        referencia.GetProperty("pk").EnumerateArray().Select(x => x.GetString())), $"Destino incorrecto en {dep}.");
}
Exigir(ConversorEnumDbml<EstadoVerificacion>.ATexto(EstadoVerificacion.SinVerificar) == "sin_verificar", "Conversión enum incorrecta.");
Exigir(ConversorEnumDbml<TipoPrecio>.DesdeTexto("a_convenir") == TipoPrecio.AConvenir, "Lectura enum incorrecta.");
var archivo = new Archivo { SubidoPorUsuarioId = Guid.NewGuid(), Bucket = "archivos", ClaveObjeto = "prueba/imagen.png", TipoMime = "image/png", Extension = "png", TamanoBytes = 1 };
archivo.Validar();
archivo.MarcarListo();
Exigir(archivo.Estado == EstadoArchivo.Listo, "Cambio de estado fallido.");
archivo.Eliminar();
Exigir(archivo.EliminadoEn?.Kind == DateTimeKind.Utc && archivo.Estado == EstadoArchivo.Eliminado, "Borrado lógico fallido.");
var guardado = new Guardado { UsuarioId = Guid.NewGuid(), PublicacionId = Guid.NewGuid(), ProductoId = Guid.NewGuid() };
var rechazado = false;
try { guardado.Validar(); } catch (ExcepcionDominio) { rechazado = true; }
Exigir(rechazado, "Un guardado con dos objetivos debe rechazarse.");
var sqlCrear = contexto.GetService<IRelationalDatabaseCreator>().GenerateCreateScript();
Directory.CreateDirectory(Path.Combine(raiz, "verificacion", "resultados"));
File.WriteAllText(Path.Combine(raiz, "verificacion", "resultados", "crear_esquema.sql"), sqlCrear);
Console.WriteLine(JsonSerializer.Serialize(new { estado = "correcto", tablas = tablas.GetArrayLength(), columnas = totalColumnas, relaciones = fks.Length, pruebasDominio = 4 }));

static void Exigir(bool condicion, string mensaje)
{
    if (!condicion) throw new InvalidOperationException(mensaje);
}
