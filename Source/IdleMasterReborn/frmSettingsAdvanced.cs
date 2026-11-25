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

        private void frmSettingsAdvanced_Load(object sender, EventArgs e)
        {
            // Localize Form
            btnUpdate.Text = localization.strings.update;
            this.Text = localization.strings.auth_data;

            // Read settings
            var customTheme = Settings.Default.customTheme;

            // Define colors
            this.BackColor = customTheme ? Settings.Default.colorBgd : Settings.Default.colorBgdOriginal;
            this.ForeColor = customTheme ? Settings.Default.colorTxt : Settings.Default.colorTxtOriginal;

            // Buttons & inputs
            btnUpdate.FlatStyle = customTheme ? FlatStyle.Flat : FlatStyle.Standard;
            btnUpdate.BackColor = this.BackColor;
            btnUpdate.ForeColor = this.ForeColor;

            Color inputBg = customTheme ? Color.FromArgb(42, 42, 42) : SystemColors.Window;
            txtSessionID.BackColor = txtSteamLoginSecure.BackColor = txtSteamParental.BackColor = inputBg;
            txtSessionID.ForeColor = txtSteamLoginSecure.ForeColor = txtSteamParental.ForeColor = this.ForeColor;

            ThemeManager.ApplyVisualPolish(this, customTheme);

            // Never expose saved cookies; this dialog is write-only for manual replacement.
            txtSessionID.Text = string.Empty;
            txtSteamLoginSecure.Text = string.Empty;
            txtSteamParental.Text = string.Empty;
            txtSessionID.PasswordChar = '*';
            txtSteamLoginSecure.PasswordChar = '*';
            txtSteamParental.PasswordChar = '\0';

            UpdateButtonState();
        }

        private void UpdateButtonState()
        {
            btnUpdate.Enabled = !string.IsNullOrWhiteSpace(txtSessionID.Text)
                && !string.IsNullOrWhiteSpace(txtSteamLoginSecure.Text);
        }

        private void txtSessionID_TextChanged(object sender, EventArgs e)
        {
            UpdateButtonState();
        }

        private void txtSteamLogin_TextChanged(object sender, EventArgs e)
        {
            UpdateButtonState();
        }

        private void txtSteamParental_TextChanged(object sender, EventArgs e)
        {
            UpdateButtonState();
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

            txtSessionID.PasswordChar = '*';
            txtSteamLoginSecure.PasswordChar = '*';
            txtSteamParental.PasswordChar = '\0';

            txtSessionID.Enabled = true;
            txtSteamLoginSecure.Enabled = true;
            txtSteamParental.Enabled = true;

            txtSessionID.Focus();

            MessageBox.Show(localization.strings.validate_failed);

            UpdateButtonState();
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
