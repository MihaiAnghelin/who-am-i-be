using who_am_i_be.Models;

namespace who_am_i_be.DTOs.Lobby;

public class CreateLobbyDTO
{
    public PlayerDTO AdminPlayer { get; set; } = null!;
    public List<Guid> CategoriesIds { get; set; } = new();
}