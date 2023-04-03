namespace who_am_i_be.Models;

public class Player
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Avatar { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public bool IsAdmin { get; set; }
    public Guid CharacterId { get; set; }
    public Character Character { get; set; } = null!;

    public Guid LobbyId { get; set; }
    public Lobby Lobby { get; set; } = null!;
}