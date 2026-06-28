using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTOs;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebApplication1.Controllers;
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
    public IActionResult Login([FromBody] LoginDTO dto)
    {
        if (dto.Email != "test@gmail.com" || dto.Password != "password123"){
            return Unauthorized();}
        var claims=new[]
        {
            new Claim("userId","1"),
            new Claim(ClaimTypes.Email,dto.Email),
            new Claim(ClaimTypes.Role,"Admin")
        };    
        var secretKey=_configuration["Jwt:SecretKey"]??throw new Exception("Secret key not found in configuration.");
        var key=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var token=new JwtSecurityToken(
            claims:claims,
            expires:DateTime.UtcNow.AddHours(1),
            signingCredentials:new SigningCredentials(key,SecurityAlgorithms.HmacSha256)
        );
        return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
}
}