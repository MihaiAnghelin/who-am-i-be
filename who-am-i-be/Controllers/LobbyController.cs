using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using who_am_i_be.DTOs;
using who_am_i_be.DTOs.Lobby;
using who_am_i_be.Models;

namespace who_am_i_be.Controllers;

[ApiController, Route("api/lobby")]
public class LobbyController : Controller
{
    private readonly DataContext _context;

    public LobbyController(DataContext context)
    {
        _context = context;
    }

    [HttpGet("{lobbyId:guid}")]
    public async Task<ServiceResultDTO> GetLobby(Guid lobbyId)
    {
        var lobby = await _context.Lobbies
            .Include(x => x.Players)
            .ThenInclude(x => x.Character)
            .Select(lob => new
            {
                lob.Id,
                lob.HasGameStarted,
                lob.CreationDate,
                players = lob.Players.Select(player => new
                {
                    player.Id,
                    player.Name,
                    player.IsAdmin,
                    character = player.Character == null
                        ? null
                        : new
                        {
                            player.Character.Id,
                            player.Character.Name,
                            CategoryName = player.Character.Category!.Name,
                        }
                })
            })
            .FirstOrDefaultAsync(x => x.Id == lobbyId);

        if (lobby is null)
            return new ServiceResultDTO
            {
                Error = "Lobby not found",
                StatusCode = StatusCodes.Status404NotFound
            };

        return new ServiceResultDTO
        {
            Data = lobby,
            StatusCode = StatusCodes.Status200OK
        };
    }

    [HttpPost("join")]
    public async Task<ServiceResultDTO> JoinLobby(JoinLobbyDTO lobbyDTO)
    {
        var lobby = await _context.Lobbies
            .Include(x => x.Players)
            .FirstOrDefaultAsync(x => x.Id == lobbyDTO.LobbyId);

        if (lobby is null)
            return new ServiceResultDTO
            {
                Error = "Lobby not found",
                StatusCode = StatusCodes.Status404NotFound
            };

        var player = new Player
        {
            Id = default,
            Name = lobbyDTO.Player.Name,
            Avatar = lobbyDTO.Player.Avatar,
            Color = lobbyDTO.Player.Color,
            IsAdmin = false,
            LobbyId = lobby.Id,
        };

        lobby.Players.Add(player);

        await _context.SaveChangesAsync();

        return new ServiceResultDTO
        {
            Data = new
            {
                LobbyId = lobby.Id,
                PlayerId = player.Id,
            },
            StatusCode = StatusCodes.Status200OK
        };
    }

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

    [HttpPost("create")]
    public async Task<ServiceResultDTO> CreateLobby(CreateLobbyDTO lobbyDTO)
    {
        var categories = await _context.Categories
            .Where(x => lobbyDTO.CategoriesIds.Contains(x.Id))
            .ToListAsync();

        var lobby = new Lobby()
        {
            Categories = categories,
        };

        var player = new Player
        {
            Name = lobbyDTO.AdminPlayer.Name,
            Avatar = lobbyDTO.AdminPlayer.Avatar,
            Color = lobbyDTO.AdminPlayer.Color,
            IsAdmin = true,
            LobbyId = lobby.Id,
        };

        await _context.Players.AddAsync(player);

        lobby.Players.Add(player);

        await _context.Lobbies.AddAsync(lobby);
        await _context.SaveChangesAsync();

        return new ServiceResultDTO
        {
            Data = new
            {
                LobbyId = lobby.Id,
                PlayerId = player.Id,
            },
            StatusCode = StatusCodes.Status200OK
        };
    }

    [HttpPost("start")]
    public async Task<ServiceResultDTO> StartLobby(StartLobbyDTO lobbyDTO)
    {
        var lobby = await _context.Lobbies
            .FirstOrDefaultAsync(x => x.Id == lobbyDTO.LobbyId);

        if (lobby is null)
            return new ServiceResultDTO
            {
                Error = "Lobby not found",
                StatusCode = StatusCodes.Status404NotFound
            };

        lobby.HasGameStarted = true;
        lobby.CreationDate = DateTime.Now;

        _context.Lobbies.Update(lobby);
        await _context.SaveChangesAsync();

        return new ServiceResultDTO
        {
            Data = "Game Started",
            StatusCode = StatusCodes.Status200OK
        };
    }

    [HttpPost("getCharacters")]
    public async Task<ServiceResultDTO> GetCharacters([FromBody] CharactersLobbyDTO lobbyDTO)
    {
        var adminPlayer = await _context.Players
            .FirstOrDefaultAsync(player => player.Id == lobbyDTO.AdminPlayer.Id);

        if (adminPlayer is null)
            return new ServiceResultDTO
            {
                Error = "Player not found",
                StatusCode = StatusCodes.Status404NotFound
            };

        if (!adminPlayer.IsAdmin)
            return new ServiceResultDTO
            {
                Error = "Player is not admin",
                StatusCode = StatusCodes.Status404NotFound
            };

        var lobby = await _context.Lobbies
            .Include(lobby => lobby.Categories)
            .ThenInclude(category => category.Characters)
            .Include(lobby => lobby.Players)
            .FirstOrDefaultAsync(lobby => lobby.Id == lobbyDTO.LobbyId);

        if (lobby is null)
            return new ServiceResultDTO
            {
                Error = "Lobby not found",
                StatusCode = StatusCodes.Status404NotFound
            };

        if (lobby.Players.All(p => p.Id != adminPlayer.Id))
            return new ServiceResultDTO
            {
                Error = "Player is not in the lobby",
                StatusCode = StatusCodes.Status404NotFound
            };

        // assign a random character that is not already taken and is in the lobby categories to each player
        var characters = lobby.Players
            .Select(player =>
            {
                var random = new Random();
                var character = lobby.Categories
                    .SelectMany(category => category.Characters)
                    .Where(character => lobby.Players.All(p => p.CharacterId != character.Id))
                    .OrderBy(_ => random.Next())
                    .First();

                player.Character = character;

                return new
                {
                    PlayerId = player.Id,
                    CharacterId = character.Id,
                    CharacterName = character.Name,
                };
            })
            .ToList();

        _context.Players.UpdateRange(lobby.Players);
        await _context.SaveChangesAsync();

        return new ServiceResultDTO
        {
            Data = characters,
            StatusCode = StatusCodes.Status200OK
        };
    }

    [HttpPost("rerollCharacter")]
    public async Task<ServiceResultDTO> RerollCharacter([FromBody] RerollCharacterDTO rerollDTO)
    {
        var adminPlayer = await _context.Players
            .FirstOrDefaultAsync(player => player.Id == rerollDTO.AdminPlayerId);
        
        if (adminPlayer is null)
            return new ServiceResultDTO
            {
                Error = "Admin not found",
                StatusCode = StatusCodes.Status404NotFound
            };

        if (!adminPlayer.IsAdmin)
            return new ServiceResultDTO
            {
                Error = "Player is not admin",
                StatusCode = StatusCodes.Status404NotFound
            };
        
        
        if (!adminPlayer.IsAdmin)
            return new ServiceResultDTO
            {
                Error = "Player is not admin",
                StatusCode = StatusCodes.Status404NotFound
            };

        var lob = await _context.Lobbies
            .Include(lobby => lobby.Players)
            .FirstOrDefaultAsync(lobby => lobby.Id == rerollDTO.LobbyId);

        if (lob is null)
            return new ServiceResultDTO
            {
                Error = "Lobby not found",
                StatusCode = StatusCodes.Status404NotFound
            };

        if (lob.Players.All(p => p.Id != adminPlayer.Id))
            return new ServiceResultDTO
            {
                Error = "Player is not in the lobby",
                StatusCode = StatusCodes.Status404NotFound
            };
        
        if(lob.Players.All(p => p.Id != rerollDTO.PlayerToChangeId))
            return new ServiceResultDTO
            {
                Error = "Player to change is not in the lobby",
                StatusCode = StatusCodes.Status404NotFound
            };
        
        var player = await _context.Players
            .Include(player => player.Lobby)
            .ThenInclude(lobby => lobby!.Categories)
            .ThenInclude(category => category.Characters)
            .FirstOrDefaultAsync(player => player.Id == rerollDTO.PlayerToChangeId);

        if (player is null)
            return new ServiceResultDTO
            {
                Error = "Player not found",
                StatusCode = StatusCodes.Status404NotFound
            };

        var random = new Random();
        var character = player.Lobby!.Categories
            .SelectMany(category => category.Characters)
            .Where(character => player.Lobby.Players.All(p => p.CharacterId != character.Id))
            .OrderBy(_ => random.Next())
            .First();

        player.Character = character;

        _context.Players.Update(player);
        await _context.SaveChangesAsync();

        return new ServiceResultDTO
        {
            Data = new
            {
                PlayerId = player.Id,
                CharacterId = character.Id,
                CharacterName = character.Name,
            },
            StatusCode = StatusCodes.Status200OK
        };
    }

    [HttpPost("removePlayer")]
    public async Task<ServiceResultDTO> RemovePlayer(RemovePlayerDTO lobbyDTO)
    {
        if (!lobbyDTO.AdminPlayer.IsAdmin)
            return new ServiceResultDTO
            {
                Error = "Player is not admin",
                StatusCode = StatusCodes.Status404NotFound
            };

        var player = await _context.Players
            .Include(player => player.Lobby)
            .FirstOrDefaultAsync(player => player.Id == lobbyDTO.PlayerIdToRemove);

        if (player is null)
            return new ServiceResultDTO
            {
                Error = "Player not found",
                StatusCode = StatusCodes.Status404NotFound
            };

        if (player.IsAdmin)
            return new ServiceResultDTO
            {
                Error = "Admin cannot leave the lobby",
                StatusCode = StatusCodes.Status404NotFound
            };

        _context.Players.Remove(player);
        await _context.SaveChangesAsync();

        return new ServiceResultDTO
        {
            Data = "Player removed",
            StatusCode = StatusCodes.Status200OK
        };
    }
}