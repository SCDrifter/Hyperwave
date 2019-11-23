using Eve.Api.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hyperwave.Config
{
    public static class ExceptionHandler
    {
        public static event EventHandler<ExceptionEventArgs<ApiException>> ApiExceptionOccurred;
        public static event EventHandler<ExceptionEventArgs<Exception>> OtherExceptionOccurred;

        public static void HandleApiException(object source,ApiException e)
        {
            LastException = e.Message;
            if (ApiExceptionOccurred != null)
                ApiExceptionOccurred(source, new ExceptionEventArgs<ApiException>() { Exception = e });
        }

        public static void HandleOtherException(object source, Exception e)
        {
            LastException = e.Message;
            if (OtherExceptionOccurred != null)
                OtherExceptionOccurred(source, new ExceptionEventArgs<Exception>() { Exception = e });
        }

        public static string LastException
        {
            get;
            set;
        }
    }
    public class ExceptionEventArgs<T> : EventArgs
        where T : Exception
    {
        public T Exception { get; internal set; }
    }
}
