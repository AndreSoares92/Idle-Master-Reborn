using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using IdleMasterReborn.Properties;

namespace IdleMasterReborn
{
    public class CookieClient : WebClient
    {
        internal CookieContainer Cookie = new CookieContainer();

        internal Uri ResponseUri;

        /// <summary>
        /// Instantiates a new <c>CookieClient</c> object.
        /// </summary>
        public CookieClient()
        {
            Cookie = GenerateCookies();
            Encoding = Encoding.UTF8;
        }

        /// <summary>
        /// Method <c>GetHttpAsync</c> will asynchronosly use a HTTP GET request (via <c>WebClient</c>) to the provided parameter <c>url</c>
        /// </summary>
        /// <param name="url">URL to send a GET request to</param>
        /// <param name="count">Number of retries to send a GET request</param>
        /// <returns>The HTML content of the HTTP GET response</returns>
        public static async Task<string> GetHttpAsync(string url, int count = 3)
        {
            while (true)
            {
                var client = new CookieClient();
                var content = string.Empty;

                try
                {
                    content = await client.DownloadStringTaskAsync(url);
                }
                catch (Exception ex)
                {
                    Logger.Exception(ex, "CookieClient -> GetHttpAsync, for url = " + url);
                }

                if (!string.IsNullOrWhiteSpace(content) || count == 0)
                    return content;

                count--;
            }
        }

        /// <summary>
        /// Method <c>IsLogined</c> will attempt to find out if the user has successfully logged in or not.
        /// </summary>
        /// <returns><c>true</c> if there was information available that confirms the login as successful, otherwise <c>false</c></returns>
        public static async Task<bool> IsLogined()
        {
            if (string.IsNullOrWhiteSpace(Settings.Default.myProfileURL))
            {
                return false;
            }

            var response = string.Empty;
            try
            {
                response = await GetHttpAsync(Settings.Default.myProfileURL);
            }
            catch (Exception ex)
            {
                Logger.Exception(ex, "CookieClient -> IsLogined, for url = " + Settings.Default.myProfileURL);
            }

            // Transient network error or timeout: do not wipe saved cookies.
            if (string.IsNullOrWhiteSpace(response))
            {
                return false;
            }

            if (HasAuthenticatedHeader(response))
            {
                return true;
            }

            // Access token likely expired; try silent refresh via steamRefresh_steam.
            if (await RefreshLoginToken())
            {
                try
                {
                    var refreshedResponse = await GetHttpAsync(Settings.Default.myProfileURL);
                    if (!string.IsNullOrWhiteSpace(refreshedResponse) && HasAuthenticatedHeader(refreshedResponse))
                    {
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    Logger.Exception(ex, "CookieClient -> IsLogined (post-refresh), for url = " + Settings.Default.myProfileURL);
                }
            }

            ClearCookies();
            return false;
        }

        private static bool HasAuthenticatedHeader(string html)
        {
            if (string.IsNullOrWhiteSpace(html))
                return false;

            var document = new HtmlDocument();
            document.LoadHtml(html);

            var accountNode = document.DocumentNode.SelectSingleNode("//span[@id='account_pulldown']")
                ?? document.DocumentNode.SelectSingleNode("//span[@class='notification_count']");
            return accountNode != null && !string.IsNullOrWhiteSpace(accountNode.InnerText);
        }

        public static async Task<bool> RefreshLoginToken()
        {
            string refreshToken = SecureStorage.Unprotect(Settings.Default.steamRememberLogin);
            if (string.IsNullOrWhiteSpace(refreshToken) || !IsTokenLikeCookie(refreshToken))
            {
                return false;
            }

            try
            {
                return await Task.Run(() => RefreshLoginTokenSync(refreshToken));
            }
            catch (Exception ex)
            {
                Logger.Exception(ex, "CookieClient -> RefreshLoginToken, for url = " + Settings.Default.myProfileURL);
                return false;
            }
        }

        private static bool RefreshLoginTokenSync(string refreshToken)
        {
            var jar = new CookieContainer();
            var loginBase = new Uri("https://login.steampowered.com");
            var communityBase = new Uri("https://steamcommunity.com");

            jar.Add(loginBase, new Cookie("steamRefresh_steam", refreshToken));
            string currentSessionId = SecureStorage.Unprotect(Settings.Default.sessionid);
            if (!string.IsNullOrWhiteSpace(currentSessionId))
            {
                jar.Add(communityBase, new Cookie("sessionid", currentSessionId));
            }

            string html = SendRefreshRequest(
                "https://login.steampowered.com/jwt/refresh?redir=https%3A%2F%2Fsteamcommunity.com",
                "GET",
                null,
                jar);

            var communityCookies = jar.GetCookies(communityBase);
            Cookie newLoginCookie = communityCookies["steamLoginSecure"];

            if (!IsValidLoginCookie(newLoginCookie) && !string.IsNullOrWhiteSpace(html))
            {
                var doc = new HtmlDocument();
                doc.LoadHtml(html);
                var form = doc.DocumentNode.SelectSingleNode("//form[contains(@action, 'settoken')]")
                    ?? doc.DocumentNode.SelectSingleNode("//form[@method='POST' or @method='post']");
                if (form != null)
                {
                    string action = WebUtility.HtmlDecode(form.GetAttributeValue("action", string.Empty));
                    if (!string.IsNullOrWhiteSpace(action))
                    {
                        var inputs = form.SelectNodes(".//input[@name]");
                        var pairs = new List<string>();
                        if (inputs != null)
                        {
                            foreach (var input in inputs)
                            {
                                string name = input.GetAttributeValue("name", string.Empty);
                                string val = WebUtility.HtmlDecode(input.GetAttributeValue("value", string.Empty));
                                if (!string.IsNullOrEmpty(name))
                                {
                                    pairs.Add(Uri.EscapeDataString(name) + "=" + Uri.EscapeDataString(val));
                                }
                            }
                        }

                        string postData = string.Join("&", pairs.ToArray());
                        SendRefreshRequest(action, "POST", postData, jar);
                        communityCookies = jar.GetCookies(communityBase);
                        newLoginCookie = communityCookies["steamLoginSecure"];
                    }
                }
            }

            if (!IsValidLoginCookie(newLoginCookie))
            {
                return false;
            }

            Settings.Default.steamLoginSecure = SecureStorage.Protect(newLoginCookie.Value);

            Cookie newSessionCookie = communityCookies["sessionid"];
            if (newSessionCookie != null && !string.IsNullOrWhiteSpace(newSessionCookie.Value))
            {
                Settings.Default.sessionid = SecureStorage.Protect(newSessionCookie.Value);
            }

            var loginCookies = jar.GetCookies(loginBase);
            Cookie rotatedRefresh = loginCookies["steamRefresh_steam"];
            if (rotatedRefresh != null && !string.IsNullOrWhiteSpace(rotatedRefresh.Value))
            {
                Settings.Default.steamRememberLogin = SecureStorage.Protect(rotatedRefresh.Value);
            }

            Settings.Default.myProfileURL = SteamProfile.GetSteamUrl();
            Settings.Default.Save();
            AccountManager.SaveCurrentAccount();
            return true;
        }

        private static string SendRefreshRequest(string url, string method, string postData, CookieContainer jar)
        {
            var req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = method;
            req.CookieContainer = jar;
            req.AllowAutoRedirect = true;
            req.Timeout = 15000;
            req.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36";

            if (method == "POST" && postData != null)
            {
                byte[] bytes = Encoding.UTF8.GetBytes(postData);
                req.ContentType = "application/x-www-form-urlencoded";
                req.ContentLength = bytes.Length;
                using (var reqStream = req.GetRequestStream())
                {
                    reqStream.Write(bytes, 0, bytes.Length);
                }
            }

            using (var resp = (HttpWebResponse)req.GetResponse())
            using (var stream = resp.GetResponseStream())
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }

        private static bool IsTokenLikeCookie(string value)
        {
            return !string.IsNullOrWhiteSpace(value)
                && (value.Contains("||") || value.IndexOf("%7C%7C", StringComparison.OrdinalIgnoreCase) >= 0);
        }

        private static bool IsValidLoginCookie(Cookie cookie)
        {
            return cookie != null
                && !string.IsNullOrWhiteSpace(cookie.Value)
                && !string.Equals(cookie.Value, "deleted", StringComparison.OrdinalIgnoreCase)
                && IsTokenLikeCookie(cookie.Value);
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            var request = base.GetWebRequest(address);

            if (request is HttpWebRequest)
                (request as HttpWebRequest).CookieContainer = Cookie;

            return request;
        }

        protected override WebResponse GetWebResponse(WebRequest request, System.IAsyncResult result)
        {
            try
            {
                var baseResponse = base.GetWebResponse(request);

                var cookies = (baseResponse as HttpWebResponse).Cookies;

                // If Steam marks steamLoginSecure as deleted, clear only the access cookie
                // so IsLogined() can attempt a silent refresh using steamRefresh_steam.
                Cookie loginCookie = cookies["steamLoginSecure"];
                if (loginCookie != null && loginCookie.Value == "deleted")
                {
                    Settings.Default.steamLoginSecure = string.Empty;
                    Settings.Default.Save();
                }

                ResponseUri = baseResponse.ResponseUri;

                return baseResponse;
            }
            catch (Exception ex)
            {
                Logger.Exception(ex, "CookieClient -> WebResponse = " + request);
            }

            return null;
        }

        private static void ClearCookies()
        {
            Settings.Default.sessionid = string.Empty;
            Settings.Default.steamLogin = string.Empty;
            Settings.Default.steamLoginSecure = string.Empty;
            Settings.Default.steamparental = string.Empty;
            Settings.Default.steamMachineAuth = string.Empty;
            Settings.Default.steamRememberLogin = string.Empty;
            Settings.Default.myProfileURL = string.Empty;

            Settings.Default.Save();
        }

        private static CookieContainer GenerateCookies()
        {
            var cookies = new CookieContainer();
            var target = new Uri("https://steamcommunity.com");

            cookies.Add(new Cookie("sessionid", SecureStorage.Unprotect(Settings.Default.sessionid)) { Domain = target.Host });
            cookies.Add(new Cookie("steamLoginSecure", SecureStorage.Unprotect(Settings.Default.steamLoginSecure)) { Domain = target.Host });
            cookies.Add(new Cookie("steamparental", SecureStorage.Unprotect(Settings.Default.steamparental)) { Domain = target.Host });

            string remember = SecureStorage.Unprotect(Settings.Default.steamRememberLogin);
            if (!IsTokenLikeCookie(remember))
            {
                cookies.Add(new Cookie("steamRememberLogin", remember) { Domain = target.Host });
            }

            cookies.Add(new Cookie(GetSteamMachineAuthCookieName(), SecureStorage.Unprotect(Settings.Default.steamMachineAuth)) { Domain = target.Host });

            return cookies;
        }

        private static string GetSteamMachineAuthCookieName()
        {
            var loginSecure = SecureStorage.Unprotect(Settings.Default.steamLoginSecure);
            if (!string.IsNullOrEmpty(loginSecure) && loginSecure.Length > 17)
                return string.Format("steamMachineAuth{0}", loginSecure.Substring(0, 17));

            return "steamMachineAuth";
        }
    }
}
