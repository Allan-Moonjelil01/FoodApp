using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using DataAccess;
using Models;               // your RegisterRequest & LoginRequest
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public class AccountController : Controller
{
    private readonly IUnitOfWork _uow;
    private readonly IPasswordHasher<User> _hasher;

    public AccountController(
      IUnitOfWork uow,
      IPasswordHasher<User> hasher)
    {
        _uow = uow;
        _hasher = hasher;
    }

    // GET /Account/Login
    [HttpGet]
    public IActionResult Login() => View(new LoginRequest());

    // POST /Account/Login
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginRequest vm)
    {
        if (!ModelState.IsValid)
            return View(vm);

        var user = await _uow
            .Repository<User>()
            .Query()
            .FirstOrDefaultAsync(u => u.Username == vm.Username);
        if (user == null ||
            _hasher.VerifyHashedPassword(user, user.Password, vm.Password)
              != PasswordVerificationResult.Success)
        {
            ModelState.AddModelError("", "Invalid credentials");
            return View(vm);
        }

        // build claims + sign in cookie
        var claims = new List<Claim> {
          new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
          new Claim(ClaimTypes.Name, user.Username),
          new Claim(ClaimTypes.Role, user.Role==0 ? "Admin":"Customer")
        };
        var cp = new ClaimsPrincipal(
                  new ClaimsIdentity(claims,
                    CookieAuthenticationDefaults.AuthenticationScheme));
        await HttpContext.SignInAsync(cp);

        return RedirectToAction("Index", "Home", new { area = "" });
    }

    // GET /Account/Register
    [HttpGet]
    public IActionResult Register() => View(new RegisterRequest());

    // POST /Account/Register
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterRequest vm)
    {
        if (!ModelState.IsValid)
            return View(vm);

        // check duplicate
        var exists = await _uow
            .Repository<User>()
            .Query()
            .AnyAsync(u => u.Username == vm.Username);
        if (exists)
        {
            ModelState.AddModelError("Username", "Username already taken");
            return View(vm);
        }

        var user = new User
        {
            Username = vm.Username,
            Role = vm.Role
        };
        user.Password = _hasher.HashPassword(user, vm.Password);
        await _uow.Repository<User>().AddAsync(user);
        await _uow.CompleteAsync();

        return RedirectToAction("Login");
    }

    // GET /Account/Logout
    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }

    public IActionResult AccessDenied() => View("AccessDenied");
}
