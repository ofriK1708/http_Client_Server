// ReSharper disable InconsistentNaming
namespace calc_server.models;

public class HistoryEntry
{
    public string? flavor { get; set; }  // "STACK" or "INDEPENDENT"
    public string? operation { get; set; }
    public List<int>? arguments { get; set; }
    public int result { get; set; }
}