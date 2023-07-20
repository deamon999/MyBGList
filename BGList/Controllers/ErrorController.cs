using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BGList.Controllers
{

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
