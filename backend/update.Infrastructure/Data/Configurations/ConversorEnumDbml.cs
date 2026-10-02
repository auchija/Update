using System.Text.Json;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
namespace update.Infrastructure.Data.Configurations;

/// <summary>Convierte PascalCase de C# a las etiquetas snake_case exactas del DBML.</summary>
public sealed class ConversorEnumDbml<TEnum>() : ValueConverter<TEnum, string>(
    valor => ATexto(valor), texto => DesdeTexto(texto)) where TEnum : struct, Enum
{
    public static string ATexto(TEnum valor) => JsonNamingPolicy.SnakeCaseLower.ConvertName(valor.ToString());
    public static TEnum DesdeTexto(string texto)
    {
        foreach (var valor in Enum.GetValues<TEnum>())
            if (string.Equals(ATexto(valor), texto, StringComparison.Ordinal)) return valor;
        throw new InvalidOperationException($"Valor '{texto}' no válido para {typeof(TEnum).Name}.");
    }
}
