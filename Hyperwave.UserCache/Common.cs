using System;
using System.Data.SQLite;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace Hyperwave.UserCache
{
    static class Common
    {
        static internal SQLiteConnection DB = null;


        static internal async Task InitializeDBAsync()
        {
            if (DB == null)
            {
                PrepareDB();

                await DB.OpenAsync();
            }
        }


        private static void PrepareDB()
        {
            string fname = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            fname = Path.Combine(fname, @"Zukalitech\Hyperwave");

            if (!Directory.Exists(fname))
                Directory.CreateDirectory(fname);

            fname = Path.Combine(fname, "UserCache.sqlite");

            if (!File.Exists(fname))
                ExtractDB(fname);

            DB = new SQLiteConnection(string.Format("DataSource={0}", fname));
        }

        static internal void InitializeDB()
        {
            if (DB == null)
            {
                PrepareDB();
                DB.Open();
            }
        }

        private static void ExtractDB(string fname)
        {
            var asm = Assembly.GetExecutingAssembly();

            using (var istream = asm.GetManifestResourceStream("Hyperwave.UserCache.UserCache.sqlite"))
            {
                using (var ostream = new FileStream(fname, FileMode.CreateNew, FileAccess.Write))
                {
                    istream.CopyTo(ostream);
                }
            }
        }
    }
}
