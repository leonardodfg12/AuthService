using AuthService.Application.DTOs;
using AuthService.Application.Handlers;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.API.Controllers;

[ApiController]
[Route("auth")]
public class AuthController(AuthHandler handler) : ControllerBase
{
    /// <summary>
    /// Realiza o registro de um novo usuário.
    /// </summary>
    [HttpPost("registrar")]
    public async Task<IActionResult> Registrar([FromBody] RegisterUserCommand command)
    {
        try
        {
            await handler.RegisterAsync(command);
            return Ok(new { message = "Usuário registrado com sucesso" });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Realiza login e retorna o token JWT.
    /// </summary>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        try
        {
            var token = await handler.LoginAsync(request);
            return Ok(new { token });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { error = ex.Message });
        }
    }
    
    /// <summary>
    /// Lista todos usuarios cadastrados.
    /// </summary>
    [HttpGet("usuarios")]
    public async Task<IActionResult> ListarUsuarios()
    {
        try
        {
            var usuarios = await handler.ListarUsuariosAsync();
            return Ok(usuarios);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Erro ao listar usuários: " + ex.Message });
        }
    }
}