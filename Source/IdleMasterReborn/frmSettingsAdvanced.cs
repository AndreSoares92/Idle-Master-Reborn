using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using IdleMasterReborn.Properties;

namespace IdleMasterReborn
{
    public partial class frmSettingsAdvanced : Form
    {
        public frmSettingsAdvanced()
        {
            InitializeComponent();
            WindowPositionManager.Attach(this);
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            txtSessionID.PasswordChar = '\0';
            txtSteamLoginSecure.PasswordChar = '\0';
            txtSteamParental.PasswordChar = '\0';

            txtSessionID.Enabled = true;
            txtSteamLoginSecure.Enabled = true;
            txtSteamParental.Enabled = true;

            btnView.Visible = false;
        }

        private void frmSettingsAdvanced_Load(object sender, EventArgs e)
        {
            // Localize Form
            btnUpdate.Text = localization.strings.update;
            this.Text = localization.strings.auth_data;
            ttHelp.SetToolTip(btnView, localization.strings.cookie_warning);

            // Read settings
            var customTheme = Settings.Default.customTheme;
            var whiteIcons = Settings.Default.whiteIcons;

            // Define colors
            this.BackColor = customTheme ? Settings.Default.colorBgd : Settings.Default.colorBgdOriginal;
            this.ForeColor = customTheme ? Settings.Default.colorTxt : Settings.Default.colorTxtOriginal;

            // Buttons
            FlatStyle buttonStyle = customTheme ? FlatStyle.Flat : FlatStyle.Standard;
            btnView.FlatStyle = btnUpdate.FlatStyle = buttonStyle;
            btnView.BackColor = btnUpdate.BackColor = this.BackColor;
            btnView.ForeColor = btnUpdate.ForeColor = this.ForeColor;
            btnView.Image = customTheme ? Resources.imgView_w : Resources.imgView;
            ThemeManager.ApplyVisualPolish(this, customTheme);

            var sessionId = SecureStorage.Unprotect(Settings.Default.sessionid);
            if (!string.IsNullOrWhiteSpace(sessionId))
            {
                txtSessionID.Text = sessionId;
                txtSessionID.Enabled = false;
            }
            else
            {
                txtSessionID.PasswordChar = '\0';
            }

            var steamLoginSecure = SecureStorage.Unprotect(Settings.Default.steamLoginSecure);
            if (!string.IsNullOrWhiteSpace(steamLoginSecure))
            {
                txtSteamLoginSecure.Text = steamLoginSecure;
                txtSteamLoginSecure.Enabled = false;
            }
            else
            {
                txtSteamLoginSecure.PasswordChar = '\0';
            }

            var steamParental = SecureStorage.Unprotect(Settings.Default.steamparental);
            if (!string.IsNullOrWhiteSpace(steamParental))
            {
                txtSteamParental.Text = steamParental;
                txtSteamParental.Enabled = false;
            }
            else
            {
                txtSteamParental.PasswordChar = '\0';
                txtSteamParental.Text = localization.strings.not_required_hint;
            }

            if (txtSessionID.Enabled && txtSteamLoginSecure.Enabled && txtSteamParental.Enabled)
            {
                btnView.Visible = false;
            }

            btnUpdate.Enabled = false;
        }

        private void txtSessionID_TextChanged(object sender, EventArgs e)
        {
            btnUpdate.Enabled = true;
        }

        private void txtSteamLogin_TextChanged(object sender, EventArgs e)
        {
            btnUpdate.Enabled = true;
        }

        private void txtSteamParental_TextChanged(object sender, EventArgs e)
        {
            btnUpdate.Enabled = true;
        }

        private async Task CheckAndSave()
        {
            try
            {
                var parentalText = txtSteamParental.Text.Trim();
                if (parentalText == localization.strings.not_required_hint)
                {
                    parentalText = string.Empty;
                }

                Settings.Default.sessionid = SecureStorage.Protect(txtSessionID.Text.Trim());
                Settings.Default.steamLoginSecure = SecureStorage.Protect(txtSteamLoginSecure.Text.Trim());
                Settings.Default.myProfileURL = SteamProfile.GetSteamUrl();
                Settings.Default.steamparental = SecureStorage.Protect(parentalText);

                // Test if the cookie data is valid
                if (await CookieClient.IsLogined())
                {
                    Settings.Default.Save();
                    AccountManager.SaveCurrentAccount();
                    Close();
                    return;
                }
            }
            catch (Exception ex)
            {
                Logger.Exception(ex, "frmSettingsAdvanced -> CheckAndSave");
            }

            // Invalid cookie data, reset the form
            btnUpdate.Text = localization.strings.update;
            txtSessionID.Text = "";
            txtSteamLoginSecure.Text = "";
            txtSteamParental.Text = "";

            txtSessionID.PasswordChar = '\0';
            txtSteamLoginSecure.PasswordChar = '\0';
            txtSteamParental.PasswordChar = '\0';

            txtSessionID.Enabled = true;
            txtSteamLoginSecure.Enabled = true;
            txtSteamParental.Enabled = true;

            txtSessionID.Focus();

            MessageBox.Show(localization.strings.validate_failed);

            btnUpdate.Enabled = true;
        }

        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            btnUpdate.Enabled = false;
            txtSessionID.Enabled = false;
            txtSteamLoginSecure.Enabled = false;
            txtSteamParental.Enabled = false;

            btnUpdate.Text = localization.strings.validating;

            await CheckAndSave();
        }
    }
}
