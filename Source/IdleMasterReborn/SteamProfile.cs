using System;
using System.Net;
using System.Text;
using System.Xml;
using Microsoft.Win32;
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

        internal static string GetActiveSteamClientId()
        {
            try
            {
                using (var key = Registry.CurrentUser.OpenSubKey(@"Software\Valve\Steam\ActiveProcess"))
                {
                    if (key == null)
                        return string.Empty;

                    object pidObj = key.GetValue("pid");
                    object activeUserObj = key.GetValue("ActiveUser");
                    if (pidObj == null || activeUserObj == null)
                        return string.Empty;

                    int pid = Convert.ToInt32(pidObj);
                    uint accountId = unchecked((uint)Convert.ToInt32(activeUserObj));
                    if (pid <= 0 || accountId == 0)
                        return string.Empty;

                    ulong steamId64 = 76561197960265728UL + (ulong)accountId;
                    return steamId64.ToString();
                }
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
