namespace CRUD.API.DTOs;

public class UserResponse
{
    public required string NickName { get; set; }

    public string? Name { get; set; }
    public int Age { get; set; }
    public List<string>? FriendsNickName { get; set; }
}
