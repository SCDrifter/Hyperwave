using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace CommandTree
{
    static class Extensions
    {
        public static bool Match(this Regex self, string input, out Match match)
        {
            match = self.Match(input);
            return match.Success;
        }
        public static void Replace<TKey,TValue>(this Dictionary<TKey,TValue> self,TKey key,TValue value)
        {
            if (self.ContainsKey(key))
                self[key] = value;
            else
                self.Add(key, value);
        }
    }
}
