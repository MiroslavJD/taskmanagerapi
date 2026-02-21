using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TaskManagerAPI.Dtos;
using TaskManagerAPI.Dtos.Auth;
using TaskManagerAPI.Services;
using System.Security.Claims;

namespace TaskManagerAPI.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AuthService _auth;
    private readonly IConfiguration _config;

    public AuthController(AuthService auth, IConfiguration config)
    {
        _auth = auth;
        _config = config;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var username = dto.Username.Trim();

        if (await _auth.UserExistsAsync(username))
            return Conflict("Username already exists.");

        var user = await _auth.RegisterAsync(username, dto.Password);
        return Ok(new { user.Id, user.Username });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var user = await _auth.ValidateUserAsync(dto.Username.Trim(), dto.Password);
        if (user is null) return Unauthorized("Invalid credentials.");

        var token = CreateJwt(user.Id, user.Username);
        return Ok(new { token });
    }

    private string CreateJwt(int userId, string username)
    {
        var key = _config["Jwt:Key"]!;
        var issuer = _config["Jwt:Issuer"]!;
        var audience = _config["Jwt:Audience"]!;

        var claims = new[]
        {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Name, username)
        };

        var creds = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
            SecurityAlgorithms.HmacSha256
        );

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}