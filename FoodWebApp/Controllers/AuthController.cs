using Microsoft.AspNetCore.Mvc;
using DataAccess;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Models;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUnitOfWork _uow;
    private readonly IPasswordHasher<User> _hasher;
    private readonly IConfiguration _config;

    public AuthController(
        IUnitOfWork uow,
        IPasswordHasher<User> hasher,
        IConfiguration config)
    {
        _uow = uow;
        _hasher = hasher;
        _config = config;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest req)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // check duplicate
        var exists = (await _uow.Repository<User>().Query()
                         .AnyAsync(u => u.Username == req.Username));
        if (exists)
            return Conflict("Username already taken");

        var user = new User
        {
            Username = req.Username,
            Role = req.Role
        };
        // hash into Password field
        user.Password = _hasher.HashPassword(user, req.Password);

        await _uow.Repository<User>().AddAsync(user);
        await _uow.CompleteAsync();

        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest req)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await _uow.Repository<User>().Query()
                      .FirstOrDefaultAsync(u => u.Username == req.Username);
        if (user == null)
            return Unauthorized("Invalid creds");

        var res = _hasher.VerifyHashedPassword(user, user.Password, req.Password);
        if (res != PasswordVerificationResult.Success)
            return Unauthorized("Invalid creds");

        var token = GenerateJwtToken(user);
        return Ok(token);
    }

    private AuthResponse GenerateJwtToken(User user)
    {
        var jwt = _config.GetSection("Jwt");
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwt["Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim> {
            new Claim(JwtRegisteredClaimNames.Sub, user.Username),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Role == 0 ? "Admin" : "Customer"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var expires = DateTime.UtcNow.AddMinutes(
            double.Parse(jwt["ExpireMinutes"]));

        var token = new JwtSecurityToken(
            issuer: jwt["Issuer"],
            audience: jwt["Audience"],
            claims: claims,
            expires: expires,
            signingCredentials: creds);

        return new AuthResponse
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expires = expires
        };
    }
}
