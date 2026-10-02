namespace update.Domain.Interfaces;

/// <summary>Valida los valores antes de persistir una entidad.</summary>
public interface IEntidadValidable
{
    void Validar();
}
