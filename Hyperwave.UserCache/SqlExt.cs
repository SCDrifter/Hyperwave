using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hyperwave.UserCache
{
    static class SqlExt
    {
        /// <summary>
        /// This will add an array of parameters to a SqlCommand. This is used for an IN statement.
        /// Use the returned value for the IN part of your SQL call. (i.e. SELECT * FROM table WHERE field IN ({paramNameRoot}))
        /// </summary>
        /// <param name="cmd">The SqlCommand object to add parameters to.</param>
        /// <param name="values">The array of strings that need to be added as parameters.</param>
        /// <param name="name_root">What the parameter should be named followed by a unique value for each value. This value surrounded by {} in the CommandText will be replaced.</param>
        /// <param name="start">The beginning number to append to the end of paramNameRoot for each value.</param>
        /// <param name="separator">The string that separates the parameter names in the sql command.</param>
        public static SQLiteParameter[] AddArrayParameters<T>(this SQLiteCommand cmd, IEnumerable<T> values, string name_root, int start = 1, string separator = ", ")
        {
            /* An array cannot be simply added as a parameter to a SqlCommand so we need to loop through things and add it manually. 
             * Each item in the array will end up being it's own SqlParameter so the return value for this must be used as part of the
             * IN statement in the CommandText.
             */
            var parameters = new List<SQLiteParameter>();
            var parameterNames = new List<string>();
            var paramNbr = start;
            foreach (var value in values)
            {
                var paramName = string.Format("@{0}{1}", name_root, paramNbr++);
                parameterNames.Add(paramName);
                parameters.Add(cmd.Parameters.AddWithValue(paramName, value));
            }

            cmd.CommandText = cmd.CommandText.Replace("{" + name_root + "}", string.Join(separator, parameterNames));

            return parameters.ToArray();
        }
    }
}
