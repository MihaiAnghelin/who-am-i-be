using who_am_i_be.Models;

namespace who_am_i_be.DTOs.Lobby;

public class RemovePlayerDTO
{
    public Player AdminPlayer { get; set; }
    public Guid PlayerIdToRemove { get; set; }
}