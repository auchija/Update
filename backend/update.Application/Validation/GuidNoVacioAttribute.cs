using System.ComponentModel.DataAnnotations;
namespace update.Application.Validation;

/// <summary>Impide UUID vacío en relaciones y claves asignadas por el cliente.</summary>
[AttributeUsage(AttributeTargets.Property)]
public sealed class GuidNoVacioAttribute : ValidationAttribute
{
    public GuidNoVacioAttribute() : base("{0} no puede ser un UUID vacío.") { }
    public override bool IsValid(object? value) => value is null || value is Guid id && id != Guid.Empty;
}
