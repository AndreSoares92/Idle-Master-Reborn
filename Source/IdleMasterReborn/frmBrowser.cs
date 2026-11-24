using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using IdleMasterReborn.Properties;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;

namespace IdleMasterReborn
{
    public class frmBrowser : Form
    {
        private readonly WebView2 webView;
        private readonly Timer tmrCheckCookies;
        private bool _isCapturing;

        public frmBrowser()
        {
            Text = "Idle Master Reborn - Steam";
            ClientSize = new Size(920, 680);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;

            webView = new WebView2();
            webView.Dock = DockStyle.Fill;
            Controls.Add(webView);

            tmrCheckCookies = new Timer();
            tmrCheckCookies.Interval = 1000;
            tmrCheckCookies.Tick += async (s, e) => await CheckSteamCookiesAsync();

            Load += frmBrowser_Load;
            FormClosing += frmBrowser_FormClosing;
            WindowPositionManager.Attach(this);
        }

        private async void frmBrowser_Load(object sender, EventArgs e)
        {
            ThemeManager.ApplyTitleBarTheme(this, Settings.Default.customTheme);
            try
            {
                var userDataFolder = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "IdleMasterReborn",
                    "WebView2");

                var env = await CoreWebView2Environment.CreateAsync(null, userDataFolder);
                await webView.EnsureCoreWebView2Async(env);

                webView.CoreWebView2.CookieManager.DeleteAllCookies();
                webView.CoreWebView2.NavigationCompleted += async (s, args) => await CheckSteamCookiesAsync();

                tmrCheckCookies.Start();
                webView.CoreWebView2.Navigate("https://steamcommunity.com/login/home/?goto=");
            }
            catch (Exception ex)
            {
                Logger.Exception(ex, "frmBrowser -> frmBrowser_Load");
                tmrCheckCookies.Stop();
                Close();
                using (var fallback = new frmSettingsAdvanced())
                {
                    fallback.ShowDialog();
                }
            }
        }

        private async Task CheckSteamCookiesAsync()
        {
            if (_isCapturing || IsDisposed || webView.CoreWebView2 == null)
            {
                return;
            }

            _isCapturing = true;
            try
            {
                var cookies = await webView.CoreWebView2.CookieManager.GetCookiesAsync("https://steamcommunity.com");

                string sessionId = string.Empty;
                string steamLoginSecure = string.Empty;
                string steamParental = string.Empty;
                string steamRememberLogin = string.Empty;
                string steamMachineAuth = string.Empty;

                foreach (var cookie in cookies)
                {
                    if (cookie.Name == "sessionid")
                    {
                        sessionId = cookie.Value;
                    }
                    else if (cookie.Name == "steamLoginSecure")
                    {
                        steamLoginSecure = cookie.Value;
                    }
                    else if (cookie.Name == "steamparental")
                    {
                        steamParental = cookie.Value;
                    }
                    else if (cookie.Name == "steamRememberLogin")
                    {
                        steamRememberLogin = cookie.Value;
                    }
                    else if (cookie.Name.StartsWith("steamMachineAuth", StringComparison.OrdinalIgnoreCase))
                    {
                        steamMachineAuth = cookie.Value;
                    }
                }

                bool hasLoginToken = !string.IsNullOrWhiteSpace(steamLoginSecure)
                    && !string.Equals(steamLoginSecure, "deleted", StringComparison.OrdinalIgnoreCase)
                    && (steamLoginSecure.Contains("||") || steamLoginSecure.IndexOf("%7C%7C", StringComparison.OrdinalIgnoreCase) >= 0);

                if (!string.IsNullOrWhiteSpace(sessionId) && hasLoginToken)
                {
                    tmrCheckCookies.Stop();

                    Settings.Default.sessionid = SecureStorage.Protect(sessionId);
                    Settings.Default.steamLoginSecure = SecureStorage.Protect(steamLoginSecure);
                    Settings.Default.steamparental = SecureStorage.Protect(steamParental);
                    Settings.Default.steamRememberLogin = SecureStorage.Protect(steamRememberLogin);
                    Settings.Default.steamMachineAuth = SecureStorage.Protect(steamMachineAuth);
                    Settings.Default.myProfileURL = SteamProfile.GetSteamUrl();
                    Settings.Default.Save();
                    AccountManager.SaveCurrentAccount();

                    DialogResult = DialogResult.OK;
                    Close();
                }
            }
            catch (Exception ex)
            {
                Logger.Exception(ex, "frmBrowser -> CheckSteamCookiesAsync");
            }
            finally
            {
                _isCapturing = false;
            }
        }

        private void frmBrowser_FormClosing(object sender, FormClosingEventArgs e)
        {
            tmrCheckCookies.Stop();
            tmrCheckCookies.Dispose();
        }
    }
}
