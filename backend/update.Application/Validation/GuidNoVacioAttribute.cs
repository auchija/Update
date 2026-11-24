using System.ComponentModel.DataAnnotations;
namespace update.Application.Validation;

[AttributeUsage(AttributeTargets.Property)]
public sealed class GuidNoVacioAttribute : ValidationAttribute
{
    public GuidNoVacioAttribute() : base("{0} no puede ser un UUID vacío.") { }
    public override bool IsValid(object? value) => value is null || value is Guid id && id != Guid.Empty;
}
