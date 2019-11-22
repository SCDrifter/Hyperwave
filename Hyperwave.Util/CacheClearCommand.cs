using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hyperwave.Util
{
    [Verb("clear_cache",HelpText ="Clears local name cache")]
    class CacheClearCommand
    {
        [Option('u',"characters",Default =false,HelpText = "Removes all characters from cache")]
        public bool ClearCharacters { get; set; }

        [Option('c', "corporation", Default = false, HelpText = "Removes all corporations from cache")]
        public bool ClearCorporations { get; set; }

        [Option('a', "alliances", Default = false, HelpText = "Removes all alliances from cache")]
        public bool ClearAlliances { get; set; }
        
        [Option('m', "mailinglist", Default = false, HelpText = "Removes all characters from cache")]
        public bool ClearMailingLists { get; set; }

        [Option('*', "all", Default = false, HelpText = "Removes everthing from cache")]
        public bool ClearAll
        {
            get
            {
                return ClearCharacters && ClearCorporations && ClearAlliances && ClearMailingLists;
            }
            set
            {
                ClearCharacters = value;
                ClearAlliances = value;
                ClearCorporations = value;
                ClearMailingLists = value;
            }
        }

        public int Run()
        {
            return -1;
        }
    }
}
