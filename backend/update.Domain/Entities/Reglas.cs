using update.Domain.Exceptions;
namespace update.Domain.Entities;

/// <summary>Validaciones compartidas, independientes de Entity Framework.</summary>
public static class Reglas
{
    public static void Exigir(bool condicion, string mensaje)
    {
        if (!condicion) throw new ExcepcionDominio(mensaje);
    }
    public static void Texto(string? valor, string campo, int longitud, bool obligatorio, bool admiteVacio = false)
    {
        Exigir(!obligatorio || valor is not null, $"{campo} es obligatorio.");
        if (valor is null) return;
        Exigir(admiteVacio || !obligatorio || !string.IsNullOrWhiteSpace(valor), $"{campo} no puede estar vacío.");
        Exigir(valor.Length <= longitud, $"{campo} admite hasta {longitud} caracteres.");
    }
    public static void FechaUtc(DateTime? valor, string campo)
    {
        Exigir(valor is null || valor.Value.Kind == DateTimeKind.Utc, $"{campo} debe estar en UTC, con sufijo Z.");
    }
}
