namespace update.Application.Services;

public interface IAlmacenObjetos
{
    string Bucket { get; }
    Task GuardarAsync(string clave, byte[] datos, string mime, CancellationToken ct);
    Task<byte[]?> LeerAsync(string clave, CancellationToken ct);
    Task BorrarAsync(string clave, CancellationToken ct);
}
