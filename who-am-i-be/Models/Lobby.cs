namespace who_am_i_be.Models;

public class Lobby
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreationDate { get; set; } = DateTime.Now;
    public virtual List<Player> Players { get; set; } = new();

    public virtual List<Category> Categories { get; set; } = new();
    public bool HasGameStarted { get; set; }
}