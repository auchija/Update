namespace update.Domain.Interfaces;

public interface IEliminable
{
    DateTime? EliminadoEn { get; set; }
    void Eliminar();
}
