using Microsoft.AspNetCore.Mvc;

namespace Messegify.Application.Controllers;

[ApiController]
[Route("api/ping")]
public class PingController : Controller
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("Messegify API is up and running");
    }
}