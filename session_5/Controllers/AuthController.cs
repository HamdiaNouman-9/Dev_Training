namespace Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DTOs;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    public AuthController(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginDto request)
    {
        // Validate the username and password (for simplicity, we use hardcoded values)
        if (request.Username == "admin" && request.Password == "admin123")
        {
            var secretKey= _configuration["Jwt:SecretKey"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Generate JWT token
            var token =new JwtSecurityToken(
claims: new[]
{
    new Claim("username",request.Username)
},expires:DateTime.UtcNow.AddHours(1),signingCredentials:creds);            
            return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
        }
        else
        {
            return Unauthorized("Invalid username or password.");
        }
    }
}