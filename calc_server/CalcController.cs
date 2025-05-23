using calc_server.models;
using Microsoft.AspNetCore.Mvc;

namespace calc_server;
[ApiController]
[Route("calculator")]
public class CalcController : ControllerBase
{
    [HttpGet("health")]
    public IActionResult Health()
    {
        return Ok("OK");
    }
}