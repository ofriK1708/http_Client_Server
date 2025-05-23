using calc_server.models;
using Microsoft.AspNetCore.Mvc;

namespace calc_server;

[ApiController]
[Route("calculator")]
public class CalcController : ControllerBase
{
    private static readonly Dictionary<string, int> OperationArgumentCount = new()
    {
        ["plus"] = 2,
        ["minus"] = 2,
        ["times"] = 2,
        ["divide"] = 2,
        ["pow"] = 2,
        ["abs"] = 1,
        ["fact"] = 1
    };

    private int PerformOperation(string op, List<int> args)
    {
        var x = args[0];
        var y = args.Count > 1 ? args[1] : 0;

        return op switch
        {
            "plus" => x + y,
            "minus" => x - y,
            "times" => x * y,
            "divide" => y == 0
                ? throw new InvalidOperationException("Error while performing operation Divide: division by 0")
                : x / y,
            "pow" => (int)Math.Pow(x, y),
            "abs" => Math.Abs(x),
            "fact" => x < 0
                ? throw new InvalidOperationException(
                    "Error while performing operation Factorial: not supported for the negative number")
                : Factorial(x),
            _ => throw new InvalidOperationException($"Error: unknown operation: {op}")
        };
    }

    private int Factorial(int n)
    {
        var result = 1;

        for (var i = 2; i <= n; i++) result *= i;

        return result;
    }

    [HttpGet("health")]
    public IActionResult Health()
    {
        return Ok("OK");
    }

    [HttpPost("independent/calculate")]
    public IActionResult IndependentCalculate([FromBody] CalcRequest request)
    {
        var op = request.operation?.ToLowerInvariant();
        var args = request.arguments;

        if (string.IsNullOrWhiteSpace(op) || !OperationArgumentCount.TryGetValue(op, out var expectedArgCount))
            return Conflict
            (
                new CalcResponse
                {
                    errorMessage = $"Error: unknown operation: {request.operation}"
                }
            );

        if (args == null || args.Count != expectedArgCount)
            return Conflict
            (
                new CalcResponse
                {
                    errorMessage = $"Error: Not enough arguments to perform the operation {request.operation}"
                }
            );

        try
        {
            var result = PerformOperation(op, args);
            return Ok(new CalcResponse { result = result });
        }
        catch (Exception ex)
        {
            return Conflict
            (
                new CalcResponse
                {
                    errorMessage = ex.Message
                }
            );
        }
    }
}