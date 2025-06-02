using log4net;
using log4net.Repository.Hierarchy; 
namespace calc_server;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("logs")]
public class LogsController : ControllerBase
{
    string[] loggerNames = new string[]
    {
        "request-logger",
        "stack-logger",
        "independent-logger"
    };
    
    [HttpGet("level")]
    public IActionResult GetLogLevel([FromQuery(Name = "logger-name")] string? loggerName)
    {
        if (string.IsNullOrWhiteSpace(loggerName) || !loggerNames.Contains(loggerName))
        {
            return BadRequest("Logger name is invalid.");
        }
        var logger = LogManager.GetLogger(loggerName);
        var logImpl = logger.Logger as Logger;
        var logLevel = logImpl?.EffectiveLevel?.ToString().ToUpper() ?? "Not set";
        return Ok(logLevel);
    }
    [HttpPut("level")]
    public IActionResult SetLogLevel([FromQuery(Name = "logger-name")] string? loggerName, [FromQuery(Name = "logger-level")] string? level)
    {
        if (string.IsNullOrWhiteSpace(loggerName) || !loggerNames.Contains(loggerName))
        {
            return BadRequest("Logger name must be provided.");
        }
        if (string.IsNullOrWhiteSpace(level))
        {
            return BadRequest("Log level must be provided.");
        }
        
        var logger = LogManager.GetLogger(loggerName);
        var logImpl = logger.Logger as Logger;
        
        if (logImpl == null)
        {
            return NotFound($"Logger '{loggerName}' not found.");
        }

        try
        {
            logImpl.Level = logImpl.Hierarchy?.LevelMap[level.ToUpper()];
            return Ok(level.ToUpper());
        }
        catch (KeyNotFoundException)
        {
            return BadRequest($"Invalid log level: {level}");
        }
    }
}