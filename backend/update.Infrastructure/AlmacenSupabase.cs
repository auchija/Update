using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using update.Application.Services;

namespace update.Infrastructure;

public sealed class AlmacenSupabase(HttpClient http, string bucket) : IAlmacenObjetos
{
    public string Bucket { get; } = bucket;
    private string Ruta(string clave) => $"object/{Uri.EscapeDataString(Bucket)}/{Uri.EscapeDataString(clave)}";

    public async Task GuardarAsync(string clave, byte[] datos, string mime, CancellationToken ct)
    {
        using var cuerpo = new ByteArrayContent(datos);
        cuerpo.Headers.ContentType = new MediaTypeHeaderValue(mime);
        using var respuesta = await http.PostAsync(Ruta(clave), cuerpo, ct);
        respuesta.EnsureSuccessStatusCode();
    }

    public async Task<byte[]?> LeerAsync(string clave, CancellationToken ct)
    {
        using var respuesta = await http.GetAsync($"object/authenticated/{Uri.EscapeDataString(Bucket)}/{Uri.EscapeDataString(clave)}", ct);
        if (respuesta.StatusCode == HttpStatusCode.NotFound) return null;
        respuesta.EnsureSuccessStatusCode();
        return await respuesta.Content.ReadAsByteArrayAsync(ct);
    }

    public async Task BorrarAsync(string clave, CancellationToken ct)
    {
        using var peticion = new HttpRequestMessage(HttpMethod.Delete, $"object/{Uri.EscapeDataString(Bucket)}")
        {
            Content = JsonContent.Create(new { prefixes = new[] { clave } })
        };
        using var respuesta = await http.SendAsync(peticion, ct);
        if (respuesta.StatusCode != HttpStatusCode.NotFound) respuesta.EnsureSuccessStatusCode();
    }
}
