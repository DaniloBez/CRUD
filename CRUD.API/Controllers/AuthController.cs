using CRUD.API.DTOs;
using CRUD.API.Services;
using CRUD.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRUD.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly TokenService _tokenService;
    private readonly IUserRepository _repository;

    public AuthController(TokenService tokenService, IUserRepository repository)
    {
        _tokenService = tokenService;
        _repository = repository;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await _repository.GetByNickNameAsync(request.Username);

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
        {
            return Unauthorized("Invalid username or password");
        }

        var token = _tokenService.GenerateToken(user.NickName);
        return Ok(new { Token = token });
    }
}