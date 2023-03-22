using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using who_am_i_be.DTOs;
using who_am_i_be.Models;

namespace who_am_i_be.Controllers;

[ApiController]
[Route("[controller]")]
public class AdminController : Controller
{
    private readonly DataContext _context;

    public AdminController(DataContext context)
    {
        _context = context;
    }

    [HttpGet("categories")]
    public async Task<ServiceResultDTO> GetCategories()
    {
        var categories = await _context.Categories.ToListAsync();

        var result = new ServiceResultDTO()
        {
            Data = categories,
            StatusCode = StatusCodes.Status200OK
        };
        return result;
    }

    [HttpPost("categories")]
    public async Task<ServiceResultDTO> AddCategory(Category category)
    {
        await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();

        var result = new ServiceResultDTO()
        {
            Data = category,
            StatusCode = StatusCodes.Status200OK
        };

        return result;
    }

    [HttpDelete("categories/{id:guid}")]
    public async Task<ServiceResultDTO> DeleteCategory(Guid id)
    {
        var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);
        if (category == null)
            return new ServiceResultDTO()
            {
                Data = null!,
                Error = "Category not found",
                StatusCode = StatusCodes.Status404NotFound
            };

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();

        var result = new ServiceResultDTO()
        {
            Data = category,
            StatusCode = StatusCodes.Status200OK
        };

        return result;
    }
}