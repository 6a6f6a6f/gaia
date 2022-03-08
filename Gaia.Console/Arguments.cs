namespace Gaia.Console;

public record Arguments
{
    public bool Verbose { get; init; } = false;
    public string ConfigurationPath { get; init; } = string.Empty;
    public List<int> Ports { get; init; } = new();
    public List<string> Targets { get; init; } = new();
    public ScanType ScanType { get; init; } = new();
    public int Threads { get; init; } = 1;
}