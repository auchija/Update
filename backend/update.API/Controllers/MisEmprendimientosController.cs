using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using update.Application.Services;

namespace update.API.Controllers;

/// <summary>Emprendimientos del usuario con sesión: cada uno es un perfil de tipo emprendimiento con categoría, etapa y portada.</summary>
[ApiController, Authorize, Route("api/mis-emprendimientos")]
public sealed class MisEmprendimientosController(MisEmprendimientos servicio) : ControllerBase
{
    private bool Usuario(out Guid id) => Guid.TryParse(User.FindFirst("sub")?.Value, out id);

    /// <summary>Categorías activas para elegir al crear un emprendimiento.</summary>
    [HttpGet("categorias")]
    [ProducesResponseType<IReadOnlyList<CategoriaResumen>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IReadOnlyList<CategoriaResumen>>> Categorias(int pagina = 1, int tamanoPagina = 20, CancellationToken ct = default)
        => Ok(await servicio.ListarCategorias(pagina, tamanoPagina, ct));

    [HttpGet]
    [ProducesResponseType<IReadOnlyList<VistaEmprendimiento>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IReadOnlyList<VistaEmprendimiento>>> Listar(int pagina = 1, int tamanoPagina = 20, CancellationToken ct = default)
    {
        if (!Usuario(out var u)) return Unauthorized();
        var lista = await servicio.Listar(u, pagina, tamanoPagina, ct);
        return Ok(lista.Select(ConUrlCompleta).ToList());
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType<VistaEmprendimiento>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<VistaEmprendimiento>> Obtener(Guid id, CancellationToken ct)
    {
        if (!Usuario(out var u)) return Unauthorized();
        var e = await servicio.Obtener(u, id, ct);
        return e is null ? NotFound() : Ok(ConUrlCompleta(e));
    }

    [HttpPost]
    [ProducesResponseType<VistaEmprendimiento>(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<VistaEmprendimiento>> Crear(DatosEmprendimiento datos, CancellationToken ct)
    {
        if (!Usuario(out var u)) return Unauthorized();
        var e = await servicio.Guardar(u, null, datos, ct);
        return CreatedAtAction(nameof(Obtener), new { id = e!.Id }, ConUrlCompleta(e));
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType<VistaEmprendimiento>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<VistaEmprendimiento>> Actualizar(Guid id, DatosEmprendimiento datos, CancellationToken ct)
    {
        if (!Usuario(out var u)) return Unauthorized();
        var e = await servicio.Guardar(u, id, datos, ct);
        return e is null ? NotFound() : Ok(ConUrlCompleta(e));
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Eliminar(Guid id, CancellationToken ct)
    {
        if (!Usuario(out var u)) return Unauthorized();
        return await servicio.Eliminar(u, id, ct) ? NoContent() : NotFound();
    }

    private VistaEmprendimiento ConUrlCompleta(VistaEmprendimiento e) => e.PortadaUrl is null
        ? e
        : e with { PortadaUrl = $"{Request.Scheme}://{Request.Host}{e.PortadaUrl}" };
}
