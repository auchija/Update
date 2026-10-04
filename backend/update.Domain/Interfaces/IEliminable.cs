namespace update.Domain.Interfaces;

/// <summary>Contrato de eliminación lógica para tablas que contienen eliminado_en.</summary>
public interface IEliminable
{
    DateTime? EliminadoEn { get; set; }
    void Eliminar();
}
