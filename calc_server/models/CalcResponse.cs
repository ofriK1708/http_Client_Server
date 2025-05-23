// ReSharper disable InconsistentNaming
namespace calc_server.models;

public class CalcResponse
{
    public int? result { get; set; }
    public string? errorMessage { get; set; }
}