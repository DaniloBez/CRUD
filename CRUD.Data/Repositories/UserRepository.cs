using CRUD.Data.Models;
using CRUD.Data.Repositories.Interfaces;
using System.Text.Json;

namespace CRUD.Data.Repositories;

public class UserRepository : IUserRepository
{
    private readonly string _filePath = Path.Combine(AppContext.BaseDirectory, "Data", "UsersData.json");

    public async Task AddAsync(User user)
    {
        var users = await GetAllAsync();
        users.Add(user);
        await SaveAllAsync(users);
    }

    public async Task<bool> ValidateUser(User user)
    {
        var users = await GetAllAsync();
        return !users.Any(e => e.NickName == user.NickName);
    }

    public async Task<List<User>> GetAllAsync()
    {
        if (!File.Exists(_filePath))
            return [];

        var json = await File.ReadAllTextAsync(_filePath);

        if (string.IsNullOrWhiteSpace(json))
            return [];

        return JsonSerializer.Deserialize<List<User>>(json) ?? [];
    }

    public async Task<User> GetByNickNameAsync(string nickname)
    {
        var users = await GetAllAsync();
        return users.FirstOrDefault(e => e.NickName == nickname);
    }

    public async Task UpdateAsync(User user)
    {
        var users = await GetAllAsync();
        var existingEntity = users.FirstOrDefault(e => e.NickName == user.NickName);
        if (existingEntity != null)
        {
            existingEntity.NickName = user.NickName;
            existingEntity.Name = user.Name;
            existingEntity.Age = user.Age;
            existingEntity.FriendsNickName = user.FriendsNickName;           
            existingEntity.FriendsNickName = user.FriendsNickName;
            await SaveAllAsync(users);
        }
    }

    public async Task DeleteAsync(string nickName)
    {
        var users = await GetAllAsync();

        var existingEntity = users.FirstOrDefault(e => e.NickName == nickName);
        if (existingEntity != null)
        {
            users.Remove(existingEntity);
        }

        await SaveAllAsync(users);
    }

    private async Task SaveAllAsync(List<User> users)
    {
        var json = JsonSerializer.Serialize(users);
        await File.WriteAllTextAsync(_filePath, json);
        Console.WriteLine(_filePath);
    }
}
