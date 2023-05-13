namespace who_am_i_be.DTOs.Lobby;

public class RerollCharacterDTO
{
    public Guid AdminPlayerId { get; set; }
    public Guid LobbyId { get; set; }
    public Guid PlayerToChangeId { get; set; }
}