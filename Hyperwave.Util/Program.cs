using CommandLine;
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
            return Parser.Default.ParseArguments<TaskCommand, CacheClearCommand>(args)
                .MapResult(
                (TaskCommand task) => task.Run(),
                (CacheClearCommand task) => task.Run(),
                errs => 1
                );
              
        }
    }
}
