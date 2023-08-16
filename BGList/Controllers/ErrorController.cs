using Microsoft.AspNetCore.Mvc;

namespace BGList.Controllers
{

    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [ApiController]
    public class ErrorController : ControllerBase
    {

        [ResponseCache(NoStore = true)]
        [HttpGet]
        [Route("/error")]
        public IActionResult error()
        {
            return Problem();
        }
    }
}
