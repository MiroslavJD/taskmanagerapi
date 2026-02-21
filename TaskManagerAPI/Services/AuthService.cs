using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Data;
using TaskManagerAPI.Models;

namespace TaskManagerAPI.Services;

public class AuthService
{
    private readonly AppDbContext _db;

    public AuthService(AppDbContext db) => _db = db;

    public async Task<bool> UserExistsAsync(string username)
        => await _db.Users.AnyAsync(u => u.Username == username);

    public async Task<User> RegisterAsync(string username, string password)
    {
        using var hmac = new HMACSHA512();
        var user = new User
        {
            Username = username.Trim(),
            PasswordSalt = hmac.Key,
            PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password))
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        return user;
    }

    public async Task<User?> ValidateUserAsync(string username, string password)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == username);
        if (user is null) return null;

        using var hmac = new HMACSHA512(user.PasswordSalt);
        var computed = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

        if (!computed.SequenceEqual(user.PasswordHash)) return null;
        return user;
    }
}