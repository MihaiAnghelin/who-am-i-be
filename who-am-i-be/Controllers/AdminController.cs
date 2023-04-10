using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using who_am_i_be.DTOs;
using who_am_i_be.Models;

namespace who_am_i_be.Controllers;

[ApiController]
[Authorize]
[Route("api/admin")]
public class AdminController : Controller
{
    private readonly DataContext _context;

    public AdminController(DataContext context)
    {
        _context = context;
    }

    #region Categories Endpoints

    [HttpGet("categories")]
    public async Task<ServiceResultDTO> GetCategories()
    {
        var categories = await _context.Categories
            .Select(category => new
            {
                Id = category.Id,
                Name = category.Name,
            })
            .ToListAsync();

        var result = new ServiceResultDTO()
        {
            Data = categories,
            StatusCode = StatusCodes.Status200OK
        };
        return result;
    }

    [HttpPost("categories")]
    public async Task<ServiceResultDTO> AddCategory(CategoryDTO categoryInput)
    {
        var category = new Category
        {
            Name = categoryInput.Name
        };

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

        //remove all characters that are associated with this category
        var characters = await _context.Characters.Where(x => x.CategoryId == id).ToListAsync();
        _context.Characters.RemoveRange(characters);

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();

        var result = new ServiceResultDTO()
        {
            Data = category,
            StatusCode = StatusCodes.Status200OK
        };

        return result;
    }

    #endregion Categories Endpoints

    #region Characters Endpoints

    //get all characters
    [HttpGet("characters")]
    public async Task<ServiceResultDTO> GetCharacters()
    {
        var categories = await _context.Categories
            .Include(c => c.Characters)
            .OrderBy(x => x.Name)
            .Select(c => new
            {
                c.Id,
                c.Name,
                Characters = c.Characters
                    .OrderBy(x => x.Name)
                    .Select(x => new
                    {
                        x.Id,
                        x.Name,
                        x.CategoryId
                    })
            })
            .ToListAsync();

        var result = new ServiceResultDTO()
        {
            Data = categories,
            StatusCode = StatusCodes.Status200OK
        };

        return result;
    }

    //get all characters by category
    [HttpGet("characters/{categoryId:guid}")]
    public async Task<ServiceResultDTO> GetCharactersByCategory(Guid categoryId)
    {
        var characters = await _context.Characters
            .Include(x => x.Category)
            .Where(x => x.CategoryId == categoryId)
            .ToListAsync();

        var result = new ServiceResultDTO()
        {
            Data = characters,
            StatusCode = StatusCodes.Status200OK
        };
        return result;
    }

    //add a character
    [HttpPost("characters")]
    public async Task<ServiceResultDTO> AddCharacter(CharacterDTO inputCharacter)
    {
        var createdCharacter = new Character
        {
            Name = inputCharacter.Name,
            CategoryId = inputCharacter.CategoryId,
        };

        await _context.Characters.AddAsync(createdCharacter);
        await _context.SaveChangesAsync();

        var result = new ServiceResultDTO()
        {
            Data = createdCharacter,
            StatusCode = StatusCodes.Status200OK
        };

        return result;
    }

    //delete a character
    [HttpDelete("characters/{id:guid}")]
    public async Task<ServiceResultDTO> DeleteCharacter(Guid id)
    {
        var character = await _context.Characters.FirstOrDefaultAsync(x => x.Id == id);
        if (character == null)
            return new ServiceResultDTO()
            {
                Data = null!,
                Error = "Character not found",
                StatusCode = StatusCodes.Status404NotFound
            };

        _context.Characters.Remove(character);
        await _context.SaveChangesAsync();

        var result = new ServiceResultDTO()
        {
            Data = character,
            StatusCode = StatusCodes.Status200OK
        };

        return result;
    }

    #endregion Characters Endpoints
}