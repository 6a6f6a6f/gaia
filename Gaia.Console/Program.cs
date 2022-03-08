using System.IO;
using System.Net.NetworkInformation;
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
    Console.WriteLine("-t/--threads <1...{0}>     Number of threads.", Environment.ProcessorCount);
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
    
    var index = Array.IndexOf(args, "-c") + 1;
    if (index == 0) index = Array.IndexOf(args, "--configuration") + 1;
    
    if (index > args.Length - 1 || !File.Exists(args[index]))
    {
        Print.Error("You configuration do not exist!");
        Environment.Exit(1);
    }

    return args[index];
}

var arguments = new Arguments
{
    Verbose = ParseVerbose(),
    ConfigurationPath = ParseConfigurationPath()
};

Print.Informational(arguments.ConfigurationPath);
