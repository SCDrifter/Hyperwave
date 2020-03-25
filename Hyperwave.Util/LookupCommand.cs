using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hyperwave.UserCache;
using CommandTree;

namespace Hyperwave.Util
{
    class LookupCommand
    {
        [NamedValue("",HelpName ="Id", Required = true, HelpText = "entity id to lookup")]
        public long[] EntityIds { get; set; }

        public int Run()
        {
            EntityInfo[] infos = (from x in EntityIds select new EntityInfo { EntityID = x }).ToArray();
            EntityLookup.LookupIDs(infos);
            ConsoleTable table = new ConsoleTable(3, 2);
            table.SetTitle("ID", "Name", "Type");
            foreach(var i in infos)
            {
                table.AddRow(i.EntityID.ToString(),i.Name,i.EntityType.ToString());
            }
            table.Print();
            return 0;
        }
    }
}
