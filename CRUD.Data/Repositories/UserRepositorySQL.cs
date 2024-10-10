using CRUD.Data.Data;
using CRUD.Data.Models;
using CRUD.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace CRUD.Data.Repositories
{
    public class UserRepositorySQL : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepositorySQL(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(User user)
        {
            SerializeFriends(user);
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        private void SerializeFriends(User user)
        {
            user.FriendsNickNameSerialized = JsonConvert.SerializeObject(user.FriendsNickName);
        }

        private void DeserializeFriends(User user) 
        {
            user.FriendsNickName = JsonConvert.DeserializeObject<List<string>>(user.FriendsNickNameSerialized);
        }

        public async Task DeleteAsync(string nickName)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.NickName == nickName);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<User>> GetAllAsync()
        {
            var users = await _context.Users.ToListAsync();
            foreach (var user in users)
                DeserializeFriends(user);
            return users;
        }

        public async Task<User> GetByNickNameAsync(string nickName)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.NickName == nickName);
            if (user != null)
                DeserializeFriends(user);
            return user;
        }

        public async Task<User> UpdateAsync(User user)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.NickName == user.NickName);
            if (existingUser != null)
            {
                existingUser.Name = user.Name;
                existingUser.Age = user.Age;
                existingUser.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                existingUser.FriendsNickName = user.FriendsNickName;
                SerializeFriends(existingUser);
                await _context.SaveChangesAsync();
                return existingUser;
            }
            return null;
        }

        public async Task<bool> ValidateUser(User user)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.NickName == user.NickName);
            return existingUser == null;
        }

        public async Task<bool> ValidateUser(string nickName)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.NickName == nickName);
            return existingUser == null;
        }
    }
}
