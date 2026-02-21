using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Data;
using TaskManagerAPI.Dtos;

using TaskManagerAPI.Models;

namespace TaskManagerAPI.Services;

public class UserService : IUserService
{
    private readonly AppDbContext _db;

    public UserService(AppDbContext db) => _db = db;

    public async Task<List<UserDto>> GetAllAsync()
    {
        return await _db.Users
            .OrderByDescending(u => u.Id)
            .Select(u => new UserDto
            {
                Id = u.Id,
                Username = u.Username
            })
            .ToListAsync();
    }

    public async Task<UserDto?> GetByIdAsync(int id)
    {
        return await _db.Users
            .Where(u => u.Id == id)
            .Select(u => new UserDto
            {
                Id = u.Id,
                Username = u.Username
            })
            .FirstOrDefaultAsync();
    }

    public async Task<UserDto> CreateAsync(CreateUserDto dto)
    {
        // NOTE: тук НЕ правим парола. AuthService/Controller се грижи за паролите.
        // Users endpoint е "admin-like" демо.
        var user = new User
        {
            Username = dto.Username.Trim()
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        return new UserDto { Id = user.Id, Username = user.Username };
    }

    public async Task<bool> UpdateAsync(int id, UpdateUserDto dto)
    {
        var user = await _db.Users.FindAsync(id);
        if (user is null) return false;

        user.Username = dto.Username.Trim();
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var user = await _db.Users.FindAsync(id);
        if (user is null) return false;

        _db.Users.Remove(user);
        await _db.SaveChangesAsync();
        return true;
    }
}