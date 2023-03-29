namespace who_am_i_be.Models;

public class Category
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public virtual ICollection<Character> Characters { get; set; } = new List<Character>();
}