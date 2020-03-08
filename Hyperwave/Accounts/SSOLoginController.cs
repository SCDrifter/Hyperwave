using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

using Hyperwave.Auth;
using System.Net;
using System.Threading;
using System.Diagnostics;
using System.Net.Http;
using System.IO;

namespace Hyperwave.Accounts
{
    class SSOLoginController
    {
        CancellationTokenSource mCancelHttpServer;
        Task mHttpServerTask;
        private int mPort;
        
        NLog.Logger mLog = NLog.LogManager.GetCurrentClassLogger();

        EventWaitHandle mCloseBrowserEvent = null;
        

        private string mChallengeCode = null;

        public string AuthCode { get; private set; }
        public string PrivateData { get; set; }
        public string ChallengeCode => mChallengeCode;
        public bool Success { get; private set; }

        public bool IsOpen => mHttpServerTask != null;

        public AccessFlag AccessFlags { get;  set; }
              
        public async Task Run()
        {
            if (mHttpServerTask != null)
                throw new InvalidOperationException("Browser already running");

            mCancelHttpServer = new CancellationTokenSource();
            TaskCompletionSource<int> setport = new TaskCompletionSource<int>();

            mHttpServerTask = RunHttpServer(mCancelHttpServer.Token, setport);
            mPort = await setport.Task;

            mLog.Info($"Running Local server at: http://localhost:{mPort}");

            Success = false;

            string url = SSOAuth.GetUrl(AccessFlags, mPort, PrivateData,out mChallengeCode).ToString();
            
            StartSystemBrowser(url);
            
            var task = mHttpServerTask;
            await task;
            mHttpServerTask = null;
            }

        public void Close()
        {
            Task task = CloseAsync();
        }

        public async Task CloseAsync()
        {
            if (mHttpServerTask == null)
                return;
            mLog.Info("Shuting down server");

            Task task = mHttpServerTask;
            mHttpServerTask = null;

            mCancelHttpServer.Cancel();
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var response = await client.PostAsync($"http://localhost:{mPort}/Shutdown", new StringContent("Shutdown"));
                }
                catch(HttpRequestException)
                {
                }
            }
        }


        private void StartSystemBrowser(string url)
        {
            mLog.Info($"Starting system browser with url: {url}");
            ProcessStartInfo info = new ProcessStartInfo()
            {
                FileName = url,
                UseShellExecute = true
            };

            Process.Start(info);
        }

        private async Task RunHttpServer(CancellationToken cancelled, TaskCompletionSource<int> setport)
        {
            Random rng = new Random();
            int port;
            do
            {
                port = rng.Next(1024, 49151);
            }
            while (!await RunHttpServer(port,cancelled,setport));
        }

        private async Task<bool> RunHttpServer(int port, CancellationToken cancelled,TaskCompletionSource<int> setport)
        {
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add($"http://localhost:{port}/");
            try
            {
                listener.Start();
                setport.SetResult(port);

                byte[] working_response = Encoding.UTF8.GetBytes("<html><head><title>Hyperwave Eve Authentication</title></head><body><h1>Processing...</h1></body></html>");
                byte[] success_response = Encoding.UTF8.GetBytes("<html><head><title>Hyperwave Eve Authentication</title></head><body><center><h1>Login Successfull</h1><h4>You can now close this page.</h4></center></body></html>");
                byte[] response;

                while (!cancelled.IsCancellationRequested && !Success)
                {
                    response = working_response;
                    var context = await listener.GetContextAsync();

                    if (context.Request.HttpMethod == "GET" && CheckUrl(context.Request.Url))
                    {
                        Success = true; 
                        response = success_response;
                        context.Response.StatusCode = 200;
                        context.Response.StatusDescription = "OK";                        
                    }
                    else
                    {
                        context.Response.StatusCode = 200;
                        context.Response.StatusDescription = "OK";
                    }
                    context.Response.ContentType = "text/html";
                    context.Response.ContentEncoding = Encoding.UTF8;
                    context.Response.KeepAlive = false;
                    
                    await context.Response.OutputStream.WriteAsync(response, 0, response.Length);
                    context.Response.Close();
                }
                listener.Stop();
            }
            catch(HttpListenerException)
            {
                listener.Close();
                return false;
            }
            listener.Close();            
            mCloseBrowserEvent?.Set();
            mCloseBrowserEvent?.Dispose();
            mCloseBrowserEvent = null;
            mCancelHttpServer.Dispose();
            mCancelHttpServer = null;
            return true;
        }

     

        private Dictionary<string, string> GetQueryParams(Uri uri)
        {
            return uri.Query
              .Substring(1) // Remove '?'
              .Split('&')
              .Select(q => q.Split('='))
              .ToDictionary(q => q.FirstOrDefault(), q => q.Skip(1).FirstOrDefault());
        }

        private bool CheckUrl(Uri uri)
        {
            if (uri.LocalPath.Equals("/Result", StringComparison.InvariantCultureIgnoreCase))
            {
                Dictionary<string, string> prms = GetQueryParams(uri);
                string code;
                string state;

                prms.TryGetValue("code", out code);
                prms.TryGetValue("state", out state);

                AuthCode = code;
                PrivateData = state;
                Success = true;                
                return true;
            }

            return false;
        }
        
    }
}
