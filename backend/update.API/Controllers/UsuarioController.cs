using Microsoft.AspNetCore.Mvc;
using update.Domain.Interfaces;
using update.Domain.Entities;

namespace update.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUsuarioRepository _usuarioRepository;

    public UsersController(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Usuario>>> ObtenerTodos()
    {
        var usuarios = await _usuarioRepository.GetAllAsync();
        return Ok(usuarios);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Usuario>> ObtenerPorId(int id)
    {
        var usuario = await _usuarioRepository.GetByIdAsync(id);
        if (usuario == null)
            return NotFound();

        return Ok(usuario);
    }

    [HttpGet("correo/{correo}")]
    public async Task<ActionResult<Usuario>> ObtenerPorCorreo(string correo)
    {
        var usuario = await _usuarioRepository.ObtenerPorCorreoAsync(correo);
        if (usuario == null)
            return NotFound(new { mensaje = "Usuario no encontrado" });

        return Ok(usuario);
}
