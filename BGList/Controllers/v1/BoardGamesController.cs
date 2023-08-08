using System.Linq.Dynamic.Core;
using System.Text.Json;

using BGList.Constants;
using BGList.DTO;
using BGList.DTO.v1;
using BGList.Model;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace BGList.Controllers.v1
{

    [Route("v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class BoardGamesController : ControllerBase
    {
        private readonly ILogger<BoardGamesController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _memoryCache;
        public BoardGamesController(ILogger<BoardGamesController> logger, ApplicationDbContext context, IMemoryCache cache)
        {
            _logger = logger;
            _context = context;
            _memoryCache = cache;
        }

        [HttpGet(Name = "GetBoardGames")]
        [ResponseCache(CacheProfileName = BGListConstants.CACHE_DEFAULT_60_STORE)]
        public async Task<RestDTO<BoardGame[]>> Get([FromQuery] RequestDTO input)
        {
            _logger.LogInformation(CustomLogEvents.BoardGamesController_Get, "Get method started.");

            var query = _context.BoardGames.AsQueryable();
            if (!string.IsNullOrEmpty(input.FilterQuery))
                query = query.Where(b => b.Name.Contains(input.FilterQuery));
            var recordCount = await query.CountAsync();
            BoardGame[]? result = null;
            var cacheKey = $"{input.GetType()}-{JsonSerializer.Serialize(input)}";
            if (!_memoryCache.TryGetValue<BoardGame[]>(cacheKey, out result))
            {
                query = query
                .OrderBy($"{input.SortColumn} {input.SortOrder}")
                .Skip(input.PageIndex * input.PageSize)
                .Take(input.PageSize);
                result = await query.ToArrayAsync();
                _memoryCache.Set(cacheKey, result, new TimeSpan(0, 0, 30));
            }

            return new RestDTO<BoardGame[]>()
            {
                Data = result,
                PageIndex = input.PageIndex,
                PageSize = input.PageSize,
                RecordCount = recordCount,
                Links = new List<LinkDTO> {
                new LinkDTO(
                Url.Action(null,
                "BoardGames",
                new { input.PageIndex, input.PageSize },
                Request.Scheme)!,
                "self",
                "GET"),
                }
            };
        }

        [HttpPost(Name = "CreateBoardGame")]
        [ResponseCache(NoStore = true)]
        public async Task<RestDTO<BoardGame?>> Post(BoardGameDTO model)
        {
            var count = await _context.BoardGames.CountAsync();
            var boardgame = await _context.BoardGames
            .FirstOrDefaultAsync();
            if (boardgame != null)
            {
                if (!string.IsNullOrEmpty(model.Name))
                    boardgame.Name = model.Name;
                if (model.Year.HasValue && model.Year.Value > 0)
                    boardgame.Year = model.Year.Value;
                boardgame.LastModifiedDate = DateTime.Now;
                boardgame.Id = ++count;
                await _context.BoardGames.AddAsync(boardgame);

            }
            await _context.SaveChangesAsync();
            return new RestDTO<BoardGame?>()
            {
                Data = boardgame,
                Links = new List<LinkDTO>
                {
                    new LinkDTO(
                        Url.Action(
                        null,
                        "BoardGames",
                        model,
                        Request.Scheme)!,
                    "self",
                    "POST"),
                }
            };
        }


        [HttpPut(Name = "UpdateBoardGame")]
        [ResponseCache(NoStore = true)]
        public async Task<RestDTO<BoardGame?>> Put(BoardGameDTO model)
        {
            var boardgame = await _context.BoardGames
            .Where(b => b.Id == model.Id)
            .FirstOrDefaultAsync();
            if (boardgame != null)
            {
                if (!string.IsNullOrEmpty(model.Name))
                    boardgame.Name = model.Name;
                if (model.Year.HasValue && model.Year.Value > 0)
                    boardgame.Year = model.Year.Value;
                boardgame.LastModifiedDate = DateTime.Now;
                _context.BoardGames.Update(boardgame);

            }
            await _context.SaveChangesAsync();
            return new RestDTO<BoardGame?>()
            {
                Data = boardgame,
                Links = new List<LinkDTO>
                {
                    new LinkDTO(
                        Url.Action(
                        null,
                        "BoardGames",
                        model,
                        Request.Scheme)!,
                    "self",
                    "POST"),
                }
            };
        }


        [HttpDelete(Name = "DeleteBoardGame")]
        [ResponseCache(NoStore = true)]
        public async Task<RestDTO<BoardGame?>> Delete(int id)
        {
            var boardgame = await _context.BoardGames
            .Where(b => b.Id == id)
            .FirstOrDefaultAsync();
            if (boardgame != null)
            {
                _context.BoardGames.Remove(boardgame);
                await _context.SaveChangesAsync();
            };
            return new RestDTO<BoardGame?>()
            {
                Data = boardgame,

                Links = new List<LinkDTO>
                {
                    new LinkDTO(
                        Url.Action(
                        null,
                        "BoardGames",
                        id,
                        Request.Scheme)!,
                    "self",
                    "DELETE"),
                }
            };
        }
    }
}
