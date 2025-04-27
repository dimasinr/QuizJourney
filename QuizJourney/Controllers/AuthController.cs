using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizJourney.Data;
using QuizJourney.Models;
using QuizJourney.Helpers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.Data;
using QuizJourney.DTOs;

namespace QuizJourney.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly JwtHelper _jwtHelper;

    public AuthController(ApplicationDbContext context, JwtHelper jwtHelper)
    {
        _context = context;
        _jwtHelper = jwtHelper;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginDto login)
    {
        var user = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Username == login.Username);

        if (user == null || !BCrypt.Net.BCrypt.Verify(login.Password, user.PasswordHash))
        {
            return Unauthorized("Invalid credentials");
        }

        var token = _jwtHelper.GenerateToken(user);
        return Ok(new { token });
    }

}
