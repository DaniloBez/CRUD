using CRUD.Data.Models;

namespace CRUD.Data.Repositories.Interfaces;

public interface IUserRepository
{
    Task<List<User>> GetAllAsync();
    Task<User> GetByNickNameAsync(string nickName);
    Task AddAsync(User user);
    Task<User> UpdateAsync(User user);
    Task DeleteAsync(string nickName);
    Task<bool> ValidateUser(User user);
    Task<bool> ValidateUser(string nickName);
}
