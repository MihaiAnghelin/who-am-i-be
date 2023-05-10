namespace who_am_i_be.Models;

public sealed class Character
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Guid? CategoryId { get; set; }
    public Category? Category { get; set; } = null!;
}