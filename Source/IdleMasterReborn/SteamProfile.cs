using System;
using System.Net;
using System.Text;
using System.Xml;
using IdleMasterReborn.Properties;

namespace IdleMasterReborn
{
    internal class SteamProfile
    {
        internal static string GetSteamId()
        {
            var steamid = WebUtility.UrlDecode(SecureStorage.Unprotect(Settings.Default.steamLoginSecure));
            var index = steamid.IndexOfAny(new[] { '|' }, 0);
            return index != -1 ? steamid.Remove(index) : steamid;
        }

        internal static string GetAccessToken()
        {
            var raw = WebUtility.UrlDecode(SecureStorage.Unprotect(Settings.Default.steamLoginSecure));
            if (string.IsNullOrEmpty(raw))
            {
                return string.Empty;
            }

            int idx = raw.IndexOf("||", StringComparison.Ordinal);
            return idx != -1 ? raw.Substring(idx + 2) : string.Empty;
        }

        internal static string GetSteamUrl()
        {
            return "https://steamcommunity.com/profiles/" + GetSteamId();
        }

        internal static string GetDisplayName()
        {
            var steamUrl = GetSteamUrl();
            var userName = "User " + GetSteamId();
            try
            {
                var xmlRaw = new WebClient() { Encoding = Encoding.UTF8 }.DownloadString(string.Format("{0}/?xml=1", steamUrl));
                var xml = new XmlDocument();
                xml.LoadXml(xmlRaw);
                var nameNode = xml.SelectSingleNode("//steamID");
                if (nameNode != null)
                    userName = WebUtility.HtmlDecode(nameNode.InnerText);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Logger.Exception(ex, "frmMain -> GetSignedAs, for steamUrl = " + steamUrl);
            }
            return userName;
        }

        internal static string GetSignedAs()
        {
            return localization.strings.signed_in_as + " " + GetDisplayName();
        }
    }
}
