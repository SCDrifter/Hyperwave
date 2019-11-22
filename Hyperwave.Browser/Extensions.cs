using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hyperwave.Browser
{
    static class Extensions
    {
        public static void InvokeIfRequired(this Control self, Action action)
        {
            if (self.InvokeRequired)
                self.BeginInvoke(action);
            else
                action();
        }

        public static bool Match(this Regex self, string input,out Match match)
        {
            match = self.Match(input);
            return match.Success;
        }

        public static bool TryAdd<K,T>(this Dictionary<K, T> self,K key,T value)
        {
            if (self.ContainsKey(key))
                return false;
            self.Add(key, value);
            return true;
        }

        public static void Replace<K, T>(this Dictionary<K, T> self, K key, T value)
        {
            if (self.ContainsKey(key))
                self[key] = value;
            else
                self.Add(key, value);
        }
    }
}
