using who_am_i_be.Models;

namespace who_am_i_be.DTOs.Lobby;

public class CharactersLobbyDTO
{
    public PlayerDTO AdminPlayer { get; set; }
    public Guid LobbyId { get; set; }
}