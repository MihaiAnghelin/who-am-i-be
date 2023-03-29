using Microsoft.EntityFrameworkCore;

namespace who_am_i_be.Models;

public class DataContext : DbContext
{
    private readonly IConfiguration _configuration;

    protected DataContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public DataContext(DbContextOptions options, IConfiguration configuration) : base(options)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = _configuration.GetConnectionString("DefaultConnection");
        optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
    }
    
    public DbSet<Category> Categories { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Character> Characters { get; set; }
}