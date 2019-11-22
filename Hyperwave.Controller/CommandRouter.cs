using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Pipes;
using System.IO;
using Newtonsoft.Json;
using System.Runtime.InteropServices;

namespace Hyperwave.Controller
{
    public class CommandRouter : IDisposable
    {
        const string PIPENAME = @"bcf3934b-3360-4a23-98d6-b8de970072f7";

        NamedPipeClientStream mClient = null;

        Dictionary<string, EventHandler<CommandEventArgs>> mHandlers = new Dictionary<string, EventHandler<CommandEventArgs>>();

        public bool ConnectToExistingClient(int timeout)
        {
            try
            {
                mClient = new NamedPipeClientStream(@"\\.\pipe\" + PIPENAME);
                mClient.Connect(timeout);
                return true;
            }
            catch (TimeoutException)
            {
                mClient.Dispose();
                mClient = null;
                return false;
            }
        }

        public void SendCommand(string cmd,params string[] args)
        {
            SendCommand(new ApplicationCommand()
            {
                Command = cmd,
                Args = args
            });
        }

        public void SendCommand(ApplicationCommand cmd)
        {
            if (mClient == null)
            {
                CommandEventArgs args = new CommandEventArgs()
                {
                    Command = cmd
                };
                DispatchCommand(args);
                HandleResponse(args.Handle);
                return;
            }
            try
            {
                string text = JsonConvert.SerializeObject(cmd);
                byte[] bytes = Encoding.UTF8.GetBytes(text);
                byte[] count = BitConverter.GetBytes(bytes.Length);
                byte[] handle = new byte[Marshal.SizeOf(typeof(long))];
                mClient.Write(count, 0, count.Length);
                mClient.Write(bytes, 0, bytes.Length);
                mClient.Read(handle, 0, handle.Length);

                IntPtr hwnd = (IntPtr)BitConverter.ToInt64(handle,0);
                HandleResponse(hwnd);
            }
            catch(IOException)
            {
            }
            mClient.Dispose();
            mClient = null;
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        private void HandleResponse(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
                return;
            SetForegroundWindow(handle);
        }

        public async Task StartServerLoop()
        {
            if (mClient != null)
                return;

            byte[] count_array = new byte[4];
            byte[] data = null;
            NamedPipeServerStream server = new NamedPipeServerStream(@"\\.\pipe\" + PIPENAME, PipeDirection.InOut, 1, PipeTransmissionMode.Message);
            while (true)
            {
                await server.WaitForConnectionAsync();

                if (4 != await server.ReadAsync(count_array, 0, 4))
                {
                    server.Close();
                    server = null;
                    continue;
                }

                int size = BitConverter.ToInt32(count_array, 0);

                data = new byte[size];

                await server.ReadAsync(data, 0, size);

                string text = Encoding.UTF8.GetString(data);

                CommandEventArgs args = new CommandEventArgs()
                {
                    Command = JsonConvert.DeserializeObject<ApplicationCommand>(text)
                };

                DispatchCommand(args);

                long handle = (long)args.Handle;
                byte[] response = BitConverter.GetBytes(handle);
                await server.WriteAsync(response, 0, response.Length);

                server.Disconnect();
            }
        }

        private void DispatchCommand(CommandEventArgs args)
        {
            EventHandler<CommandEventArgs> handler = null;

            if (mHandlers.TryGetValue(args.Command.Command, out handler))
                handler(this, args);
            else if (CommandRecieved != null)
                CommandRecieved(this, args);
        }

        public void RegisterCommandHandler(string command, EventHandler<CommandEventArgs> handler)
        {
            EventHandler<CommandEventArgs> current;
            if (!mHandlers.TryGetValue(command, out current))
                mHandlers.Add(command, handler);
            else
            {
                current += handler;
                mHandlers[command] = current;
            }
        }

        public void UnregisterCommandHandler(string command,EventHandler<CommandEventArgs> handler)
        {
            EventHandler<CommandEventArgs> current;
            if (!mHandlers.TryGetValue(command, out current))
                return;

            current -= handler;

            if (current == null)
                mHandlers.Remove(command);
            else
                mHandlers[command] = current;
        }

        public void Dispose()
        {
            if(mClient != null)
            {
                mClient.Dispose();
                mClient = null;
            }
        }

        public event EventHandler<CommandEventArgs> CommandRecieved;
    }



    public class CommandEventArgs : EventArgs
    {
        public ApplicationCommand Command { get; set; }
        public IntPtr Handle { get; set; }
        public Task WaitForTask { get; set; }
    }

    public class ApplicationCommand
    {
        public ApplicationCommand()
        {
        }

        public ApplicationCommand(string[] cmdline)
        {
            Command = cmdline[0];
            Args = new string[cmdline.Length - 1];
            if (Args.Length > 1)
                Array.Copy(cmdline, 1, Args, 0, Args.Length);
        }

        public string Command { get; set; }
        public string[] Args { get; set; }
    }
}
