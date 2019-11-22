using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Hyperwave.Config
{
    public class DBOperation : IDisposable
    {
        static int mOpCount = 0;

        static void BeginOperation()
        {
            Interlocked.Increment(ref mOpCount);
        }

        static void EndOperation()
        {
            if (Interlocked.Decrement(ref mOpCount) != 0)
                return;

            if (AllOperatationsFinished != null)
                AllOperatationsFinished(null, new EventArgs());
        }
        public static bool OperationInProgress
        {
            get
            {
                return 0 != Interlocked.CompareExchange(ref mOpCount, mOpCount, mOpCount);
            }
        }

        public static event EventHandler AllOperatationsFinished;

        bool mDisposed = false;

        public DBOperation()
        {
            BeginOperation();
        }

        public void Dispose()
        {
            if (!mDisposed)
            {
                EndOperation();
                mDisposed = true;
            }
        }



    }
}
