using BGList.DTO.v2;
using BGList.Model;

using Microsoft.AspNetCore.Mvc;

namespace BGList.Controllers.v2
{

    [Route("v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("2.0")]
    public class BoardGamesController : ControllerBase
    {
        private readonly ILogger<BoardGamesController> _logger;
        public BoardGamesController(ILogger<BoardGamesController> logger)
        {
            _logger = logger;
        }

        [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 60)]
        [HttpGet(Name = "GetBoardGames")]
        public RestDTO<BoardGame[]> Get()
        {
            return new RestDTO<BoardGame[]>()
            {
                Items = new[] {
                        new BoardGame()
                        {
                            Id = 1,
                            Name = "Axis & Allies",
                            Year = 1981
                        },
                        new BoardGame()
                        {
                            Id = 2,
                            Name = "Citadels",
                            Year = 2000
                        },
                        new BoardGame()
                        {
                            Id = 3,
                            Name = "Terraforming Mars",
                            Year = 2016
                        }
                    },
                Links = new List<DTO.v2.LinkDTO>
                    {
                        new LinkDTO(
                    Url.Action(null, "BoardGames", null, Request.Scheme)!,
                    "self",
                    "GET"),
                    }
            };
        }
    }
}
