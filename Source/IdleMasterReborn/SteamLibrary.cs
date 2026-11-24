using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using HtmlAgilityPack;
using IdleMasterReborn.Properties;

namespace IdleMasterReborn
{
    public class OwnedGame
    {
        public int AppId { get; set; }
        public string Name { get; set; }
        public double HoursPlayed { get; set; }
    }

    internal static class SteamLibrary
    {
        internal static readonly Dictionary<int, OwnedGame> CachedGames = new Dictionary<int, OwnedGame>();

        internal static async Task<List<OwnedGame>> GetOwnedGamesAsync()
        {
            var games = await FetchFromPlayerServiceAsync();
            if (games.Count == 0)
            {
                games = await FetchFromProfileHtmlAsync();
            }

            CachedGames.Clear();
            foreach (var g in games)
            {
                if (!CachedGames.ContainsKey(g.AppId))
                {
                    CachedGames[g.AppId] = g;
                }
            }

            return games.OrderBy(g => g.Name, StringComparer.CurrentCultureIgnoreCase).ToList();
        }

        private static async Task<List<OwnedGame>> FetchFromPlayerServiceAsync()
        {
            var result = new List<OwnedGame>();
            try
            {
                string token = SteamProfile.GetAccessToken();
                string steamId = SteamProfile.GetSteamId();
                if (string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(steamId))
                {
                    return result;
                }

                string url = string.Format(
                    "https://api.steampowered.com/IPlayerService/GetOwnedGames/v1/?access_token={0}&steamid={1}&include_appinfo=1&include_played_free_games=1&format=json",
                    Uri.EscapeDataString(token),
                    Uri.EscapeDataString(steamId));

                using (var client = new WebClient { Encoding = Encoding.UTF8 })
                {
                    string json = await client.DownloadStringTaskAsync(url);
                    var serializer = new JavaScriptSerializer { MaxJsonLength = int.MaxValue };
                    var root = serializer.DeserializeObject(json) as Dictionary<string, object>;
                    if (root != null && root.ContainsKey("response"))
                    {
                        var response = root["response"] as Dictionary<string, object>;
                        if (response != null && response.ContainsKey("games"))
                        {
                            ParseGamesArray(response["games"] as IEnumerable, result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Exception(ex, "SteamLibrary -> FetchFromPlayerServiceAsync");
            }
            return result;
        }

        private static async Task<List<OwnedGame>> FetchFromProfileHtmlAsync()
        {
            var result = new List<OwnedGame>();
            try
            {
                if (string.IsNullOrWhiteSpace(Settings.Default.myProfileURL))
                {
                    return result;
                }

                string html = await CookieClient.GetHttpAsync(Settings.Default.myProfileURL + "/games/?tab=all");
                if (string.IsNullOrWhiteSpace(html))
                {
                    return result;
                }

                var doc = new HtmlDocument();
                doc.LoadHtml(html);

                var configNode = doc.DocumentNode.SelectSingleNode("//*[@data-profile-gameslist]");
                if (configNode != null)
                {
                    string encodedJson = configNode.GetAttributeValue("data-profile-gameslist", string.Empty);
                    if (!string.IsNullOrWhiteSpace(encodedJson))
                    {
                        string decoded = WebUtility.HtmlDecode(encodedJson);
                        var serializer = new JavaScriptSerializer { MaxJsonLength = int.MaxValue };
                        var root = serializer.DeserializeObject(decoded) as Dictionary<string, object>;
                        if (root != null && root.ContainsKey("rgGames"))
                        {
                            ParseGamesArray(root["rgGames"] as IEnumerable, result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Exception(ex, "SteamLibrary -> FetchFromProfileHtmlAsync");
            }
            return result;
        }

        private static void ParseGamesArray(IEnumerable gamesArray, List<OwnedGame> output)
        {
            if (gamesArray == null)
            {
                return;
            }

            foreach (var item in gamesArray)
            {
                var dict = item as Dictionary<string, object>;
                if (dict == null || !dict.ContainsKey("appid"))
                {
                    continue;
                }

                int appId;
                if (!int.TryParse(Convert.ToString(dict["appid"], CultureInfo.InvariantCulture), out appId) || appId <= 0)
                {
                    continue;
                }

                string name = dict.ContainsKey("name") ? Convert.ToString(dict["name"]) : ("App ID: " + appId);
                double hours = 0;
                if (dict.ContainsKey("playtime_forever"))
                {
                    double minutes = 0;
                    if (double.TryParse(Convert.ToString(dict["playtime_forever"], CultureInfo.InvariantCulture), NumberStyles.Any, CultureInfo.InvariantCulture, out minutes))
                    {
                        hours = Math.Round(minutes / 60.0, 1);
                    }
                }
                else if (dict.ContainsKey("hours_forever"))
                {
                    string hoursStr = Convert.ToString(dict["hours_forever"], CultureInfo.InvariantCulture).Replace(",", string.Empty);
                    double parsedHours;
                    if (double.TryParse(hoursStr, NumberStyles.Any, CultureInfo.InvariantCulture, out parsedHours))
                    {
                        hours = Math.Round(parsedHours, 1);
                    }
                }

                output.Add(new OwnedGame
                {
                    AppId = appId,
                    Name = string.IsNullOrWhiteSpace(name) ? ("App ID: " + appId) : name,
                    HoursPlayed = hours
                });
            }
        }
    }
}
