using who_am_i_be.Models;

namespace who_am_i_be.DTOs.Auth;

public class LoginResponseDTO
{
    public string Token { get; set; } = string.Empty;
    public User User { get; set; } = null!;
}