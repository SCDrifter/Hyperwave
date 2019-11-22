using CommandLine;
using Microsoft.Win32.TaskScheduler;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Hyperwave.Util
{
    [Verb("task",Hidden = true)]
    class TaskCommand
    {
        [Value(0,MetaName ="enable/disable")]
        public TaskState State { get; set; }

        [Option("path",Required =true)]
        public string FilePath { get; set; }

        [Option("interval")]
        public decimal Interval { get; set; }

        bool CheckAdmin()
        {
            WindowsPrincipal pricipal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
            bool isadmin = pricipal.IsInRole(WindowsBuiltInRole.Administrator);

            if (!isadmin)
            {
                Console.WriteLine("Need administrator Access");
                return false;
            }
            return true;
        }

        public int Run()
        {
            if (!CheckAdmin())
                return 1;

            if(Interval < 60)
            {
                Parser.Default.Settings.HelpWriter.WriteLine("Invalid interval");
                return 2;
            }

            using (var service = new TaskService())
            {
                string taskid = "Hyperwave Mail Checker";
                var current_task = service.FindTask(taskid);
                if (current_task != null)
                {
                    current_task.Folder.DeleteTask(current_task.Name);
                }
                if (State == TaskState.disable)
                    return 0;

                var folder = service.RootFolder;

                var task = service.NewTask();
                task.RegistrationInfo.Description = "Hyperwave Background Mail checking";
                task.RegistrationInfo.Author = "Michael J";

                task.Actions.Add(FilePath, "CheckMail", Path.GetDirectoryName(FilePath));


                task.Settings.MultipleInstances = TaskInstancesPolicy.IgnoreNew;


                LogonTrigger login_trigger = task.Triggers.Add(new LogonTrigger());
                login_trigger.Delay = TimeSpan.FromMinutes(2);
                login_trigger.Repetition = new RepetitionPattern(TimeSpan.FromSeconds((double)Interval), TimeSpan.Zero, false);

                RegistrationTrigger reg_trigger = task.Triggers.Add(new RegistrationTrigger());
                reg_trigger.Repetition = new RepetitionPattern(TimeSpan.FromSeconds((double)Interval), TimeSpan.Zero, false);


                folder.RegisterTaskDefinition(taskid, task);

                return 0;
            }
        }

        public  enum TaskState
        {
            enable,
            disable
        }
    }
}
