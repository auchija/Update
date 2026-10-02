using System.ComponentModel.DataAnnotations;
namespace update.Application.Validation;

/// <summary>Exige fechas con zona UTC explícita antes de llegar a Npgsql.</summary>
[AttributeUsage(AttributeTargets.Property)]
public sealed class FechaUtcAttribute : ValidationAttribute
{
    public FechaUtcAttribute() : base("{0} debe ser una fecha UTC con sufijo Z.") { }
    public override bool IsValid(object? value) => value is null || value is DateTime fecha && fecha.Kind == DateTimeKind.Utc;
}
