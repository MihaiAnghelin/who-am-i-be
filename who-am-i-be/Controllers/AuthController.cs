using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using who_am_i_be.DTOs;
using who_am_i_be.DTOs.Auth;
using who_am_i_be.Interfaces;
using who_am_i_be.Models;

namespace who_am_i_be.Controllers;

[ApiController]
[Route("/api/auth")]
public class AuthController : Controller
{
    private readonly DataContext _context;
    private readonly ITokenEmitterService _tokenEmitterService;

    public AuthController(DataContext context, ITokenEmitterService tokenEmitterService)
    {
        _context = context;
        this._tokenEmitterService = tokenEmitterService;
    }

    
    [HttpPost("login")]
    public async Task<ServiceResultDTO> Login(LoginDTO input)
    {
        var result = new ServiceResultDTO()
        {
            StatusCode = StatusCodes.Status200OK
        };

        try
        {
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(x => x.Username == input.Username);

            if (existingUser == null)
            {
                result.Data = null!;
                result.Error = "There is no user account associated with the entered email address. Please try again.";
                result.StatusCode = StatusCodes.Status400BadRequest;

                return result;
            }

            if (!BCrypt.Net.BCrypt.Verify(input.Password, existingUser.PasswordHash))
            {
                result.Data = null!;
                result.Error = "The password entered is not correct. Please try again.";
                result.StatusCode = StatusCodes.Status400BadRequest;

                return result;
            }

            result.Data = new LoginResponseDTO
            {
                Token = _tokenEmitterService.GenerateAuthToken(existingUser.Id.ToString()),
                User = existingUser
            };

            return result;
        }
        catch (Exception ex)
        {
            result.StatusCode = StatusCodes.Status500InternalServerError;
            result.Data = null!;
            result.Error = ex.Message;
            return result;
        }
    }

    [HttpPost("register")]
    public async Task<ServiceResultDTO> Register(RegisterDTO input)
    {
        var result = new ServiceResultDTO()
        {
            StatusCode = StatusCodes.Status200OK
        };

        try
        {
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(x => x.Username == input.Username);

            if (existingUser != null)
            {
                result.Data = null!;
                result.Error =
                    "There is already a user account associated with the entered email address. Please try again.";
                result.StatusCode = StatusCodes.Status400BadRequest;

                return result;
            }

            var user = new User
            {
                Username = input.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(input.Password)
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            result.Data = new
            {
                Username = user.Username,
            };

            return result;
        }
        catch (Exception ex)
        {
            result.StatusCode = StatusCodes.Status500InternalServerError;
            result.Data = null!;
            result.Error = ex.Message;
            return result;
        }
    }
}