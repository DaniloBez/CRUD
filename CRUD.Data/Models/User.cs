namespace CRUD.Data.Models;

public class User
{
    public string NickName { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public List<string> FriendsNickName { get; set; }
    public string Password { get; set; }
}
