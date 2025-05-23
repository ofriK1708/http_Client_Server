// ReSharper disable InconsistentNaming
namespace calc_server.models;

public class CalcRequest
{
    public List<int>? arguments { get; set; }
    public string? operation { get; set; }
}