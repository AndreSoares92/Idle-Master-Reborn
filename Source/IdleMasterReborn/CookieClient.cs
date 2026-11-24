using System;
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
            var document = new HtmlDocument();

            try
            {
                var response = await GetHttpAsync(Settings.Default.myProfileURL);
                document.LoadHtml(response);
            }
            catch (Exception ex)
            {
                Logger.Exception(ex, "CookieClient -> IsLogined, for url = " + Settings.Default.myProfileURL);
            }

            // Check Steam profile header for an authenticated session (account pulldown or notification count).
            var accountNode = document.DocumentNode.SelectSingleNode("//span[@id='account_pulldown']")
                ?? document.DocumentNode.SelectSingleNode("//span[@class='notification_count']");

            if (accountNode == null || string.IsNullOrWhiteSpace(accountNode.InnerText))
            {
                // No authenticated header node available --> not logged in properly.
                ClearCookies();
                return false;
            }
            else
            {
                return true;
            }
        }

        public static async Task<bool> RefreshLoginToken()
        {
            var client = new CookieClient();
            var target = new Uri("https://login.steampowered.com");

            client.Cookie.Add(new Cookie("steamRefresh_steam", SecureStorage.Unprotect(Settings.Default.steamLoginSecure)) { Domain = target.Host });

            try
            {
                var response = await client.DownloadStringTaskAsync("https://login.steampowered.com/jwt/refresh?redir=https%3A%2F%2Fsteamcommunity.com");
                return !string.IsNullOrWhiteSpace(response);
            }
            catch (Exception ex)
            {
                Logger.Exception(ex, "CookieClient -> RefreshToken, for url = " + Settings.Default.myProfileURL);
            }

            return false;
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

                // Check, if cookie should be deleted. This means that sessionID is now invalid and user has to log in again.
                // Maybe this shoud be done other way (authenticate exception), but because of shared settings and timers in frmMain...
                Cookie loginCookie = cookies["steamLoginSecure"];
                if (loginCookie != null && loginCookie.Value == "deleted")
                {
                    ClearCookies();
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
            cookies.Add(new Cookie("steamRememberLogin", SecureStorage.Unprotect(Settings.Default.steamRememberLogin)) { Domain = target.Host });
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
