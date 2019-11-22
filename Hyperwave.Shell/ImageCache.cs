using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Hyperwave.Shell
{
    public static class ImageCache
    {
        static string mPath = null;
        static string mPathSaved = null;
        static string mPathDownloaded = null;

        static string GetRandomCode(Random rng,int size)
        {
            string allowedchars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            StringBuilder ret = new StringBuilder(size);
            while(ret.Length < size)
            {
                ret.Append(allowedchars[rng.Next(0, allowedchars.Length)]);
            }

            return ret.ToString();
        }

        static void CreatePath()
        {
            if (mPath != null)
                return;

            Random rng = new Random();
            string path;
            do
            {
                path = Path.Combine(Path.GetTempPath(), string.Format("Hyperwave.ImageCache.{0}", GetRandomCode(rng, 6)));
            }
            while (Directory.Exists(path) || File.Exists(path));

            Directory.CreateDirectory(path);

            mPath = path;

            mPathSaved = Path.Combine(path,"Saved");
            mPathDownloaded = Path.Combine(path, "Downloaded");

            Directory.CreateDirectory(mPathSaved);
            Directory.CreateDirectory(mPathDownloaded);
        }

        delegate Uri UrlTransform(Uri uri);

        public static Uri DownloadImage(Uri url)
        {
            UrlTransform handler = delegate (Uri uri)
           {
               return DownloadImageAsync(url).Result;
           };

            var waiter = handler.BeginInvoke(url, null, null);
            return handler.EndInvoke(waiter);
        }

        public static async Task<Uri> DownloadImageAsync(Uri url)
        {
            if (url.Scheme == "file")
                return url;

            CreatePath();

            string fname;
            Uri ret;

            fname = Path.Combine(mPathDownloaded, Path.GetFileName(url.AbsolutePath));
            ret = new Uri(string.Format("file:///{0}", fname), UriKind.Absolute);

            if (File.Exists(fname))
                return ret;

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Hyperwave", "1.0"));

                HttpResponseMessage msg;

                try
                {
                    msg = await client.GetAsync(url);
                }
                catch (Exception)
                {
                    return url;
                }
                using (var stream = new FileStream(fname, FileMode.Create))
                {
                    StreamContent streamcontent = msg.Content as StreamContent;

                    await streamcontent.CopyToAsync(stream);
                }
            }
            return ret;
        }

        public static Uri SaveImage(BitmapImage image)
        {
            CreatePath();
            string fname;
            Random rng = new Random();
            do
            {
                fname = Path.Combine(mPathSaved, string.Format("SavedBitmap.{0}.png",GetRandomCode(rng,6)));
            }
            while (File.Exists(fname));

            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(image));

            using (var stream = new FileStream(fname, FileMode.Create))
            {
                encoder.Save(stream);
            }

            return new Uri(string.Format("file:///{0}", fname), UriKind.Absolute);
        }

        public static void Clear()
        {
            if (mPath == null)
                return;
            Directory.Delete(mPath, true);
            mPath = null;
            mPathDownloaded = null;
            mPathSaved = null;
        }
    }
}
