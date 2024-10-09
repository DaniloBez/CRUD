using AutoMapper;
using CRUD.API.DTOs;
using CRUD.API.Services;
using CRUD.Data.Models;
using CRUD.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<UserController> _logger;
    private readonly TokenService _tokenService;

    public UserController(ILogger<UserController> logger, IUserRepository repository, IMapper mapper, TokenService tokenService)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
        _tokenService = tokenService;
    }

    [HttpGet(Name = "GetUsers")]
    public async Task<IActionResult> GetAsync()
    {
        var users = await _repository.GetAllAsync();
        var userResponses = _mapper.Map<UserResponse[]>(users);
        return Ok(userResponses);
    }

    [Authorize]
    [HttpGet("nickName")]
    public async Task<IActionResult> GetByNickName(string nickName)
    {
        var user = await _repository.GetByNickNameAsync(nickName);
        if (user == null) return NotFound();

        var userResponse = _mapper.Map<UserResponse>(user);
        return Ok(userResponse);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateUserRequest userRequest)
    {
        var user = _mapper.Map<User>(userRequest);
        if (!await _repository.ValidateUser(user)) return BadRequest("User with this nickname already exists.");

        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(userRequest.Password);

        user.Password = hashedPassword;

        await _repository.AddAsync(user);

        var userResponse = _mapper.Map<UserResponse>(user);
        return CreatedAtAction(nameof(GetByNickName), new { nickName = user.NickName }, user);
    }

    [Authorize]
    [HttpPut]
    public async Task<IActionResult> Update(UpdateUserRequest userRequest)
    {
        var user = _mapper.Map<User>(userRequest);

        user.NickName = User.Identity.Name;

        var updatedUser = await _repository.UpdateAsync(user);
        if (updatedUser == null) return NotFound();

        return Ok(updatedUser);
    }

    [Authorize]
    [HttpDelete]
    public async Task<IActionResult> Delete()
    {
        var nickName = User.Identity.Name;

        var existingUser = await _repository.GetByNickNameAsync(nickName);
        if (existingUser == null) return NotFound();

        await _repository.DeleteAsync(nickName);
        return Ok();
    }
}