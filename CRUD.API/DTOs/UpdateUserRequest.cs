namespace CRUD.API.DTOs
{
    public class UpdateUserRequest
    {
        public required string? Password { get; set; }

        public string? Name { get; set; }
        public int Age { get; set; }
        public List<string>? FriendsNickName { get; set; }
    }
}
