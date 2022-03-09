using System.Net;

using Gaia.Console;
using Gaia.Shared;

void ShowHelp()
{
    Print.Informational("Gaia is a simple way to manage multiple nmap instance at once.\n");
    Console.WriteLine("-h/--help                 Show this page.");
    Console.WriteLine("-v/--verbose              Show verbose messages.");
    Console.WriteLine("-c/--configuration <path> Gaia's configuration file (must be a valid .JSON).");
    Console.WriteLine("-p/--ports <port(s)>      Ports to bind into nmap.");
    Console.WriteLine("-t/--targets <target(s)>  Targets to bind into nmap.");
    Console.WriteLine("-s/--scan <tcp,udp,icmp>  Type of nmap's scan.");
    Console.WriteLine("-th/--threads <1...>      Number of threads.");
}

if (!args.Any())
{
    Print.Error("There is not available arguments!");
    Print.Informational("You should try --help.");
    
    Environment.Exit(1);
}
if (args.Any(a => a is "--help" or "-h"))
{
    ShowHelp();
    Environment.Exit(0);
}

bool ParseVerbose() => args.Any(a => a is "-v" or "--verbose");

string ParseConfigurationPath()
{
    if (!args.Any(a => a is "-c" or "--configuration")) return string.Empty;

    var index = args.Any(a => a == "-c")
        ? Array.IndexOf(args, "-c") + 1
        : Array.IndexOf(args, "--configuration") + 1;

    return index <= args.Length && File.Exists(args[index]) 
        ? args[index] 
        : string.Empty;
}

List<int> ParsePorts()
{
    var ports = new List<int>();

    if (!args.Any(a => a is "-p" or "--ports")) return ports;
    var index = args.Any(a => a == "-p")
        ? Array.IndexOf(args, "-p") + 1
        : Array.IndexOf(args, "--ports") + 1;

    if (index >= args.Length) return ports;

    return args[index] // 80,443,8080,8443,21a (string)
        .Split(",") // 80 443 8080 8443 21a (array of string)
        .Where(p => int.TryParse(p, out _)) // 80 443 8080 8443 (array of strings but ints)
        .Select(int.Parse) // 80 443 8080 8443 (array of int)
        .Where(p => p is >= 1 and <= 65564)
        .ToList(); // return the created list of integers (List<int>)
}

List<string> ParseTargets()
{
    var targets = new List<string>();

    if (!args.Any(a => a is "-t" or "--targets")) return targets;
    var index = args.Any(a => a == "-t")
        ? Array.IndexOf(args, "-t") + 1
        : Array.IndexOf(args, "--targets") + 1;

    if (index >= args.Length) return targets;

    return args[index]
        .Split(",")
        .Where(t => IPAddress.TryParse(t, out _))
        .ToList();
}

ScanType ParseScanType()
{
    if (!args.Any(a => a is "-s" or "--scan")) return ScanType.Unknown;
    var index = args.Any(a => a == "-s")
        ? Array.IndexOf(args, "-s") + 1
        : Array.IndexOf(args, "--scan") + 1;

    return Enum.GetNames<ScanType>()
        .All(s =>
            !string.Equals(s, args[index], StringComparison.InvariantCultureIgnoreCase)
        )
        ? ScanType.Unknown
        : Enum.Parse<ScanType>(args[index], true);
}

int ParseThreads()
{
    if (!args.Any(a => a is "-th" or "--threads")) return 1;

    var index = args.Any(a => a == "-th")
        ? Array.IndexOf(args, "-th") + 1
        : Array.IndexOf(args, "--threads") + 1;

    return int.TryParse(args[index], out var threads)
        ? threads
        : 1;
}

var arguments = new Arguments
{
    Verbose = ParseVerbose(),
    ConfigurationPath = ParseConfigurationPath(),
    Ports = ParsePorts(),
    Targets = ParseTargets(),
    ScanType = ParseScanType(),
    Threads = ParseThreads()
};

Console.WriteLine(arguments.ConfigurationPath);
Console.WriteLine(arguments.Ports.Count);
Console.WriteLine(arguments.Targets.Count);
Console.WriteLine(arguments.ScanType);
Console.WriteLine(arguments.Threads);
