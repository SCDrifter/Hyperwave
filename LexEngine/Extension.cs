using System;
using System.Collections.Generic;
using System.Text;

namespace DarkzideGames.Lexer
{
    public static class Extension
    {
        public static void Replace<TKey, TValue>(this Dictionary<TKey, TValue> self, TKey key, TValue value)
        {
            if (self.ContainsKey(key))
                self[key] = value;
            else
                self.Add(key, value);
        }
        public static void Replace<TKey, TValue>(this SortedDictionary<TKey, TValue> self, TKey key, TValue value)
        {
            if (self.ContainsKey(key))
                self[key] = value;
            else
                self.Add(key, value);
        }
        public static bool TryAdd<TKey, TValue>(this Dictionary<TKey, TValue> self, TKey key, TValue value)
        {
            if (!self.ContainsKey(key))
            {
                self.Add(key, value);
                return true;
            }
            return false;
        }
        public static bool TryAdd<TKey, TValue>(this SortedDictionary<TKey, TValue> self, TKey key, TValue value)
        {
            if (!self.ContainsKey(key))
            {
                self.Add(key, value);
                return true;
            }
            return false;
        }
        public static bool TryAdd<TValue>(this IList<TValue> self, TValue value)
        {
            if (!self.Contains(value))
            {
                self.Add(value);
                return true;
            }
            return false;
        }

        public static void AppendFormatLine(this StringBuilder self, string format, params object[] args)
        {
            string text = string.Format(format, args);
            self.AppendLine(text);
        }

        public static TValue GetDefault<TKey,TValue>(this Dictionary<TKey,TValue> self,TKey key,TValue defvalue = default(TValue))
        {
            TValue ret;
            if (!self.TryGetValue(key, out ret))
                ret = defvalue;
            return ret;
        }
        public static int Count(this string self, char chr)
        {
            int ret = 0;
            for (int i = self.IndexOf(chr); i != -1; i = self.IndexOf(chr, i + 1))
            {
                ret++;
            }
            return ret;
        }
        public static string ReplaceAll(this string self, string[] text, string[] replace)
        {
            StringBuilder ret = new StringBuilder(self);
            for (int i = 0; i < text.Length; i++)
            {
                int j = i % replace.Length;
                ret.Replace(text[i], replace[j]);
            }
            return ret.ToString();
        }
    }
}
