using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hyperwave.Controller
{
    static class Extensions
    {
        delegate void WaitForConnectionAsyncCallback(NamedPipeServerStream self, TaskCompletionSource<object> tcs);

        private static void WaitCallback(NamedPipeServerStream self, TaskCompletionSource<object> tcs)
        {
            self.WaitForConnection();
            tcs.SetResult(null);
        }
        public static async Task WaitForConnectionAsync(this NamedPipeServerStream self)
        {
            TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();
            WaitForConnectionAsyncCallback cb = WaitCallback;
            cb.BeginInvoke(self, tcs, delegate (IAsyncResult result) { cb.EndInvoke(result); },null);

            await tcs.Task;
        }

        public static T GetValueOrDefault<K, T>(this Dictionary<K,T> self,K key,T defaultvalue = default(T))
        {
            T ret;
            if (!self.TryGetValue(key, out ret))
                ret = defaultvalue;

            return ret;
        }
    }
}
