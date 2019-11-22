using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Hyperwave.Config;
using NeoSmart.Utils;
using System.Security.Cryptography;

namespace Hyperwave.Auth
{
    public static class SSOAuth
    {
        static string GenerateCode(int size)
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] data = new byte[size];
                rng.GetBytes(data);
                return UrlBase64.Encode(data);
            }
        }

        static string HashCode(string challenge_code)
        {
            using(var hash = SHA256.Create())
            {
                byte[] data = Encoding.ASCII.GetBytes(challenge_code);
                return UrlBase64.Encode(hash.ComputeHash(data));
            }
        }
        public static Uri GetUrl(AccessFlag flags,int local_port,string private_data,out string challenge_code)
        {

            challenge_code = GenerateCode(32);

            string scopes = GetScopes(flags);

            StringBuilder url = new StringBuilder("/api/hyperwave/login?");
            url.Append($"&port={local_port}");
            url.AppendFormat("&scope={0}", Uri.EscapeUriString(scopes));
            url.AppendFormat("&code_challenge={0}", HashCode(challenge_code));
            url.AppendFormat("&state={0}", Uri.EscapeUriString(private_data));

            return new Uri(SSOConfiguration.LoginPortalUrl, url.ToString());
        }

        private static string GetScopes(AccessFlag flags)
        {
            List<string> access = new List<string>(8)
            {
                "esi-mail.read_mail.v1"
            };

            if (flags.HasFlag(AccessFlag.MailWrite))
            {
                access.Add("esi-mail.organize_mail.v1");
            }

            if (flags.HasFlag(AccessFlag.MailSend))
            {
                access.Add("esi-mail.send_mail.v1");
            }

            if (flags.HasFlag(AccessFlag.ContactsRead))
            {
                access.Add("characterContactsRead");
            }

            if (flags.HasFlag(AccessFlag.ContactsWrite))
            {
                access.Add("characterContactsWrite");
            }

            if (flags.HasFlag(AccessFlag.CalendarRead))
            {
                access.Add("characterCalendarRead");
            }

            if (flags.HasFlag(AccessFlag.CalendarWrite))
            {
                access.Add("esi-calendar.respond_calendar_events.v1");
            }

            if (flags.HasFlag(AccessFlag.WalletRead))
            {
                access.Add("esi-wallet.read_character_wallet.v1");
            }

            string scopes = string.Join(" ", access);
            return scopes;
        }
                
        public static async Task<TokenInfo> GetTokenInfoAsync(string authcode,string challenge_code, CancellationToken token)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Hyperwave", "1.0"));


                HttpContent content = new FormUrlEncodedContent(new Dictionary<string, string>()
                {
                    {"grant_type","authorization_code" },
                    {"code",authcode },
                    {"client_id", SSOConfiguration.ClientId},
                    { "code_verifier",challenge_code }
                });

                DateTime tm = DateTime.Now;

                HttpResponseMessage msg;

                try
                {
                    msg = await client.PostAsync("https://login.eveonline.com/v2/oauth/token", content, token);
                }
                catch (Exception)
                {
                    return null;
                }

                if (!msg.IsSuccessStatusCode)
                    return null;

                StreamContent stream = msg.Content as StreamContent;

                string text = await stream.ReadAsStringAsync();

                var ret = JsonConvert.DeserializeObject<TokenInfo>(text);

                ret.LoginDate = tm;

                return ret;
            }
        }

        public static async Task<TokenInfo> RefreshTokenInfoAsync(string refresh_token)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Hyperwave", "1.0"));
                client.Timeout = new TimeSpan(0, 0, 30);


                HttpContent content = new FormUrlEncodedContent(new Dictionary<string, string>()
                {
                    {"grant_type", "refresh_token" },
                    {"client_id", SSOConfiguration.ClientId },
                    {"refresh_token", refresh_token }
                });

                DateTime tm = DateTime.Now;

                HttpResponseMessage msg;

                try
                {
                    msg = await client.PostAsync("https://login.eveonline.com/v2/oauth/token", content);
                }
                catch (Exception)
                {
                    return null;
                }

                if (!msg.IsSuccessStatusCode)
                    return null;

                StreamContent stream = msg.Content as StreamContent;

                string text = await stream.ReadAsStringAsync();

                var ret = JsonConvert.DeserializeObject<TokenInfo>(text);

                ret.LoginDate = tm;

                return ret;
            }
        }

        public static async Task<CharacterInfo> GetCharacterInfoAsync(string token, CancellationToken cancel_token)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Hyperwave", "1.0"));


                HttpResponseMessage msg;

                try
                {
                    msg = await client.GetAsync("https://login.eveonline.com/oauth/verify", cancel_token);
                }
                catch (Exception)
                {
                    return null;
                }

                if (!msg.IsSuccessStatusCode)
                    return null;

                StreamContent stream = msg.Content as StreamContent;

                string text = await stream.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<CharacterInfo>(text);
            }
        }
        

        public static byte[] GetLicenseSigningCode()
        {
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Hyperwave.Auth.LicCheck.cer"))
            {
                byte[] ret = new byte[stream.Length];
                stream.Read(ret, 0, ret.Length);
                return ret;
            }
        }
    }

    [Flags]
    public enum AccessFlag
    {
        MailRead = 0,
        MailWrite = 1 << 0,
        ContactsRead = 1 << 1,
        ContactsWrite = 1 << 2,
        CalendarRead = 1 << 3,
        CalendarWrite = 1 << 4,
        MailSend = 1 << 5,
        WalletRead = 1 << 6
    }

    public enum TokenType
    {
        AccessToken,
        RefreshToken
    }
}
