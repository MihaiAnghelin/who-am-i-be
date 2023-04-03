namespace who_am_i_be.DTOs.Lobby;

public class JoinLobbyDTO
{
    public Guid LobbyId { get; set; }
    public PlayerDTO Player { get; set; } = null!;
}