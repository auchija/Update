using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using update.Application.Services;

namespace update.API.Controllers;

[ApiController, Authorize, Route("api/mis-emprendimientos")]
public sealed class MisEmprendimientosController(MisEmprendimientos servicio) : ControllerBase
{
    private bool Usuario(out Guid id) => Guid.TryParse(User.FindFirst("sub")?.Value, out id);

    [HttpGet("categorias")]
    public async Task<IActionResult> Categorias(int pagina = 1, int tamanoPagina = 20, CancellationToken ct = default)
        => Ok(await servicio.ListarCategorias(pagina, tamanoPagina, ct));

    [HttpGet]
    public async Task<IActionResult> Listar(int pagina = 1, int tamanoPagina = 20, CancellationToken ct = default)
        => Usuario(out var u) ? Ok(await servicio.Listar(u, pagina, tamanoPagina, ct)) : Unauthorized();

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Obtener(Guid id, CancellationToken ct)
    {
        if (!Usuario(out var u)) return Unauthorized();
        var e = await servicio.Obtener(u, id, ct);
        return e is null ? NotFound() : Ok(e);
    }

    [HttpPost]
    public async Task<IActionResult> Crear(DatosEmprendimiento datos, CancellationToken ct)
    {
        if (!Usuario(out var u)) return Unauthorized();
        var e = await servicio.Guardar(u, null, datos, ct);
        return CreatedAtAction(nameof(Obtener), new { id = e!.Id }, e);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Actualizar(Guid id, DatosEmprendimiento datos, CancellationToken ct)
    {
        if (!Usuario(out var u)) return Unauthorized();
        var e = await servicio.Guardar(u, id, datos, ct);
        return e is null ? NotFound() : Ok(e);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Eliminar(Guid id, CancellationToken ct)
    {
        if (!Usuario(out var u)) return Unauthorized();
        return await servicio.Eliminar(u, id, ct) ? NoContent() : NotFound();
    }
}
