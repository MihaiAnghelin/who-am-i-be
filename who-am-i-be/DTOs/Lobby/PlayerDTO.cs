namespace who_am_i_be.DTOs.Lobby;

public class PlayerDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Avatar { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public bool IsAdmin { get; set; } = false;
}