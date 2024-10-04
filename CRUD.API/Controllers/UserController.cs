using AutoMapper;
using CRUD.API.DTOs;
using CRUD.Data.Models;
using CRUD.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<UserController> _logger;

    public UserController(ILogger<UserController> logger, IUserRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet(Name = "GetUsers")]
    public async Task<IActionResult> GetAsync()
    {
        var users = await _repository.GetAllAsync();
        var userResponses = _mapper.Map<UserResponse[]>(users);
        return Ok(userResponses);
    }

    [Authorize]
    [HttpGet("{nickName}")]
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

        var newUser = new User
        {
            NickName = userRequest.NickName,
            Name = userRequest.Name,
            Age = userRequest.Age,
            Password = hashedPassword,
            FriendsNickName = userRequest.FriendsNickName
        };

        await _repository.AddAsync(newUser);

        var userResponse = _mapper.Map<UserResponse>(newUser);
        return CreatedAtAction(nameof(GetByNickName), new { nickName = newUser.NickName }, newUser);
    }

    [Authorize]
    [HttpPut]
    public async Task<IActionResult> Update(CreateUserRequest userRequest)
    {
        var nickName = User.Identity.Name;

        var existingUser = await _repository.GetByNickNameAsync(nickName);

        if (!await _repository.ValidateUser(userRequest.NickName))
            return BadRequest("User with this nickname already exists.");

        var user = _mapper.Map(userRequest, existingUser);
        await _repository.UpdateAsync(user, nickName);
        return Ok(user);
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