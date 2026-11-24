using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace IdleMasterExtended
{
    public class UpdateCheckResult
    {
        public bool Success { get; set; }
        public bool UpdateAvailable { get; set; }
        public Version CurrentVersion { get; set; }
        public Version LatestVersion { get; set; }
        public string LatestTag { get; set; }
        public string ReleaseUrl { get; set; }
        public string ErrorMessage { get; set; }
    }

    internal static class UpdateChecker
    {
        public const string RepoUrl = "https://github.com/AndreSoares92/Idle-Master-Reborn";
        public const string ReleasesUrl = "https://github.com/AndreSoares92/Idle-Master-Reborn/releases";
        public const string LatestReleaseApiUrl = "https://api.github.com/repos/AndreSoares92/Idle-Master-Reborn/releases/latest";

        public static Version GetCurrentVersion()
        {
            var v = Assembly.GetExecutingAssembly().GetName().Version;
            if (v == null)
                return new Version(1, 0, 0);

            return new Version(v.Major, v.Minor, v.Build < 0 ? 0 : v.Build);
        }

        public static async Task<UpdateCheckResult> CheckForUpdatesAsync()
        {
            var currentVersion = GetCurrentVersion();
            var result = new UpdateCheckResult
            {
                CurrentVersion = currentVersion,
                ReleaseUrl = ReleasesUrl
            };

            try
            {
                string json = await Task.Run(() => FetchLatestReleaseJson(currentVersion));
                var serializer = new JavaScriptSerializer();
                var data = serializer.DeserializeObject(json) as Dictionary<string, object>;
                if (data == null)
                {
                    result.Success = false;
                    result.ErrorMessage = "Invalid response from GitHub.";
                    return result;
                }

                string tag = data.ContainsKey("tag_name") && data["tag_name"] != null
                    ? data["tag_name"].ToString().Trim()
                    : string.Empty;
                string htmlUrl = data.ContainsKey("html_url") && data["html_url"] != null
                    ? data["html_url"].ToString().Trim()
                    : ReleasesUrl;

                if (!string.IsNullOrEmpty(htmlUrl))
                    result.ReleaseUrl = htmlUrl;

                result.LatestTag = tag;

                Version latestVersion;
                if (!TryParseTagVersion(tag, out latestVersion))
                {
                    result.Success = false;
                    result.ErrorMessage = "Could not parse release version.";
                    return result;
                }

                result.LatestVersion = latestVersion;
                result.UpdateAvailable = latestVersion > currentVersion;
                result.Success = true;
                return result;
            }
            catch (WebException ex)
            {
                var httpResponse = ex.Response as HttpWebResponse;
                if (httpResponse != null && httpResponse.StatusCode == HttpStatusCode.NotFound)
                {
                    result.Success = true;
                    result.UpdateAvailable = false;
                    result.LatestVersion = currentVersion;
                    result.LatestTag = "v" + currentVersion.ToString(3);
                    return result;
                }

                result.Success = false;
                result.ErrorMessage = ex.Message;
                return result;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = ex.Message;
                return result;
            }
        }

        private static string FetchLatestReleaseJson(Version currentVersion)
        {
            var request = (HttpWebRequest)WebRequest.Create(LatestReleaseApiUrl);
            request.Method = "GET";
            request.UserAgent = "IdleMasterReborn/" + currentVersion.ToString(3);
            request.Accept = "application/vnd.github+json";
            request.Timeout = 10000;

            using (var response = (HttpWebResponse)request.GetResponse())
            using (var stream = response.GetResponseStream())
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        private static bool TryParseTagVersion(string tag, out Version version)
        {
            version = null;
            if (string.IsNullOrEmpty(tag))
                return false;

            string cleaned = tag.Trim();
            if (cleaned.StartsWith("v", StringComparison.OrdinalIgnoreCase))
                cleaned = cleaned.Substring(1);

            int dashIdx = cleaned.IndexOf('-');
            if (dashIdx >= 0)
                cleaned = cleaned.Substring(0, dashIdx);

            string[] parts = cleaned.Split('.');
            if (parts.Length == 1)
                cleaned = cleaned + ".0.0";
            else if (parts.Length == 2)
                cleaned = cleaned + ".0";

            Version parsed;
            if (!Version.TryParse(cleaned, out parsed))
                return false;

            version = new Version(parsed.Major, parsed.Minor, parsed.Build < 0 ? 0 : parsed.Build);
            return true;
        }
    }
}
