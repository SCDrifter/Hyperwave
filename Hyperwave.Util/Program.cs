using CommandTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hyperwave.Util
{
    class Program
    {
        static int Main(string[] args)
        {
            try
            {
                return CommandlineParser.Parse<Program>(args);
            }
            catch (CommandLineException e)
            {
                CommandlineParser.PrintHelp(e.CommandContext, e.Message);
                return -1;
            }
        }

        [Command("cache", HelpText = "Manage the name cache")]
        public CacheCommands CacheCommands { get; } = new CacheCommands();

    }

    class CacheCommands
    {
        [Command("clear", HelpText = "Removes names from the cache")]
        public CacheClearCommand CacheClearCommand { get; } = new CacheClearCommand();

        [Command("lookup", HelpText = "Resolves a list of id's into names")]
        public LookupCommand LookupCommand { get; } = new LookupCommand();
    }
}