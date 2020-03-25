using CommandTree;
using Hyperwave.Common;
using Hyperwave.UserCache;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hyperwave.Util
{
    
    public class CacheClearCommand
    {
        [FlagValue('u', "characters", HelpText = "Removes all characters from cache")]
        public bool ClearCharacters { get; set; }

        [FlagValue('c', "corporation", HelpText = "Removes all corporations from cache")]
        public bool ClearCorporations { get; set; }

        [FlagValue('a', "alliances", HelpText = "Removes all alliances from cache")]
        public bool ClearAlliances { get; set; }
        
        [FlagValue('m', "mailinglist", HelpText = "Removes all characters from cache")]
        public bool ClearMailingLists { get; set; }

        [FlagValue('*', "all", HelpText = "Removes everthing from cache")]
        public bool ClearAll
        {
            get
            {
                return ClearCharacters && ClearCorporations && ClearAlliances && ClearMailingLists;
            }
            set
            {
                if (value)
                {
                    ClearCharacters = value;
                    ClearAlliances = value;
                    ClearCorporations = value;
                    ClearMailingLists = value;
                }
            }
        }

        public int Run()
        {
            int total = 0;

            if (ClearCharacters)
            {
                int count = EntityLookup.PurgeRecords(EntityType.Character);
                Console.WriteLine($"{count} character record(s) purged.");
                total += count;
            }
            if (ClearCorporations)
            {
                int count = EntityLookup.PurgeRecords(EntityType.Corporation);
                Console.WriteLine($"{count} corporation record(s) purged.");
                total += count;
            }
            if (ClearAlliances)
            {
                int count = EntityLookup.PurgeRecords(EntityType.Alliance);
                Console.WriteLine($"{count} alliance record(s) purged.");
                total += count;
            }

            if (ClearMailingLists)
            {
                int count = EntityLookup.PurgeRecords(EntityType.Mailinglist);
                Console.WriteLine($"{count} mailinglist record(s) purged.");
                total += count;
            }

            Console.WriteLine($"{total} record(s) purged in total.");
            return 0;
        }
    }
}
