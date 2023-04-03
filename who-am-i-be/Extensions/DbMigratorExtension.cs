using Microsoft.EntityFrameworkCore;
using who_am_i_be.Models;

namespace who_am_i_be.Extensions;

public static class DbMigratorExtension
{
    public static void ApplyMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<DataContext>();
        context.Database.Migrate();
    }
}