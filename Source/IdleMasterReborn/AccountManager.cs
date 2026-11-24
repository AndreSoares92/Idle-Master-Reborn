using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using IdleMasterReborn.Properties;

namespace IdleMasterReborn
{
    public class SavedAccount
    {
        public string SteamId { get; set; }
        public string DisplayName { get; set; }
        public string SessionId { get; set; }
        public string SteamLoginSecure { get; set; }
        public string SteamParental { get; set; }
        public string SteamRememberLogin { get; set; }
        public string SteamMachineAuth { get; set; }
        public string ProfileUrl { get; set; }
    }

    internal static class AccountManager
    {
        private static readonly string AccountsFilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "IdleMasterReborn",
            "accounts.xml");

        internal static List<SavedAccount> LoadAccounts()
        {
            try
            {
                if (!File.Exists(AccountsFilePath))
                {
                    return new List<SavedAccount>();
                }

                var serializer = new XmlSerializer(typeof(List<SavedAccount>));
                using (var stream = File.OpenRead(AccountsFilePath))
                {
                    var list = serializer.Deserialize(stream) as List<SavedAccount>;
                    return list ?? new List<SavedAccount>();
                }
            }
            catch (Exception ex)
            {
                Logger.Exception(ex, "AccountManager -> LoadAccounts");
                return new List<SavedAccount>();
            }
        }

        internal static void SaveAccounts(List<SavedAccount> accounts)
        {
            try
            {
                var dir = Path.GetDirectoryName(AccountsFilePath);
                if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                var serializer = new XmlSerializer(typeof(List<SavedAccount>));
                using (var stream = File.Create(AccountsFilePath))
                {
                    serializer.Serialize(stream, accounts ?? new List<SavedAccount>());
                }
            }
            catch (Exception ex)
            {
                Logger.Exception(ex, "AccountManager -> SaveAccounts");
            }
        }

        internal static void SaveCurrentAccount()
        {
            try
            {
                string steamId = SteamProfile.GetSteamId();
                if (string.IsNullOrWhiteSpace(steamId))
                {
                    return;
                }

                var accounts = LoadAccounts();
                var existing = accounts.FirstOrDefault(a => a.SteamId == steamId);
                string displayName = SteamProfile.GetDisplayName();

                if (existing == null)
                {
                    existing = new SavedAccount { SteamId = steamId };
                    accounts.Add(existing);
                }

                existing.DisplayName = displayName;
                existing.SessionId = SecureStorage.Protect(SecureStorage.Unprotect(Settings.Default.sessionid));
                existing.SteamLoginSecure = SecureStorage.Protect(SecureStorage.Unprotect(Settings.Default.steamLoginSecure));
                existing.SteamParental = SecureStorage.Protect(SecureStorage.Unprotect(Settings.Default.steamparental));
                existing.SteamRememberLogin = SecureStorage.Protect(SecureStorage.Unprotect(Settings.Default.steamRememberLogin));
                existing.SteamMachineAuth = SecureStorage.Protect(SecureStorage.Unprotect(Settings.Default.steamMachineAuth));
                existing.ProfileUrl = Settings.Default.myProfileURL;

                SaveAccounts(accounts);
            }
            catch (Exception ex)
            {
                Logger.Exception(ex, "AccountManager -> SaveCurrentAccount");
            }
        }

        internal static void ApplyAccount(SavedAccount account)
        {
            if (account == null)
            {
                return;
            }

            Settings.Default.sessionid = SecureStorage.Protect(SecureStorage.Unprotect(account.SessionId));
            Settings.Default.steamLoginSecure = SecureStorage.Protect(SecureStorage.Unprotect(account.SteamLoginSecure));
            Settings.Default.steamparental = SecureStorage.Protect(SecureStorage.Unprotect(account.SteamParental));
            Settings.Default.steamRememberLogin = SecureStorage.Protect(SecureStorage.Unprotect(account.SteamRememberLogin));
            Settings.Default.steamMachineAuth = SecureStorage.Protect(SecureStorage.Unprotect(account.SteamMachineAuth));
            Settings.Default.myProfileURL = string.IsNullOrWhiteSpace(account.ProfileUrl)
                ? "https://steamcommunity.com/profiles/" + account.SteamId
                : account.ProfileUrl;
            Settings.Default.Save();
        }

        internal static void RemoveAccount(string steamId)
        {
            if (string.IsNullOrWhiteSpace(steamId))
            {
                return;
            }

            var accounts = LoadAccounts();
            accounts.RemoveAll(a => a.SteamId == steamId);
            SaveAccounts(accounts);
        }
    }
}
