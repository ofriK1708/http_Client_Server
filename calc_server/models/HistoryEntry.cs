// ReSharper disable InconsistentNaming
namespace calc_server.models;

public class HistoryEntry
{
    public string? flavor { get; init; }  // "STACK" or "INDEPENDENT"
    public string? operation { get; init; }
    public List<int>? arguments { get; init; }
    public int result { get; init; }
}