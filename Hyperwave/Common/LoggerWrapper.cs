using System;
using System.Text;
using Hyperwave.Shell;

namespace Hyperwave.Common
{
    class LoggerWrapper : Hyperwave.Shell.IShellLogger
    {
        NLog.Logger mLog;
        public LoggerWrapper(string name)
        {
            mLog = NLog.LogManager.GetLogger(name);
        }

        public void Debug(string format, params object[] args)
        {
            Debug(string.Format(format, args));
        }

        public void Debug(string text)
        {
            mLog.Debug(text);
        }

        public void Error(string format, params object[] args)
        {
            Error(string.Format(format, args));
        }

        public void Error(string text)
        {
            mLog.Error(text);
        }

        public void Fatal(string format, params object[] args)
        {
            Fatal(string.Format(format, args));
        }

        public void Fatal(string text)
        {
            mLog.Fatal(text);
        }

        public void Info(string format, params object[] args)
        {
            Info(string.Format(format, args));
        }

        public void Info(string text)
        {
            mLog.Info(text);
        }

        public void Trace(string format, params object[] args)
        {
            Trace(string.Format(format, args));
        }

        public void Trace(string text)
        {
            mLog.Trace(text);
        }

        public void Warning(string format, params object[] args)
        {
            Warning(string.Format(format, args));
        }

        public void Warning(string text)
        {
            mLog.Warn(text);
        }
    }

    class LoggerWrapperFactory : Hyperwave.Shell.IShellLoggerFactory
    {
        public IShellLogger Create(string name)
        {
            return new LoggerWrapper(name);
        }
    }
}
