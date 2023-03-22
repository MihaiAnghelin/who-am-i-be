using who_am_i_be.Extensions;
using who_am_i_be.Models;

var builder = WebApplication.CreateBuilder(args);
IConfiguration configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureSwagger();
builder.Services.ConfigureCors();
builder.Services.ConfigureServices();
builder.Services.ConfigureJwtAuth(configuration);
builder.Services.AddDbContext<DataContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.MapSwagger();
app.UseHttpsRedirection();

app.MapControllers();

app.UseCors();
app.AddJwtAuth();
app.UseAuthorization();

app.MapControllers();


app.Run();