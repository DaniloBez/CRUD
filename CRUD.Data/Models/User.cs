using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUD.Data.Models;

public class User
{
    [Key]
    public string NickName { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public string Password { get; set; }
    public string FriendsNickNameSerialized { get; set; }

    [NotMapped]
    public List<string> FriendsNickName { get; set; }
}
