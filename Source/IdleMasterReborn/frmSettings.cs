using System;
using System.Drawing;
using System.Windows.Forms;
using IdleMasterReborn.Properties;
using System.Threading;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace IdleMasterReborn
{
    public partial class frmSettings : Form
    {
        private bool _isLoadingTheme;
        private bool _wasHourBoostMode;
        private bool _behaviorChanged;

        public frmSettings()
        {
            InitializeComponent();
            WindowPositionManager.Attach(this);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (radIdleDefault.Checked)
            {
                Settings.Default.sort = "default";
            }
            if (radIdleLeastDrops.Checked)
            {
                Settings.Default.sort = "leastcards";
            }
            if (radIdleMostDrops.Checked)
            {
                Settings.Default.sort = "mostcards";
            }

            string selectedLanguage = cboLanguage.SelectedIndex <= 0 ? string.Empty : cboLanguage.Text;
            if (selectedLanguage != Settings.Default.language)
            {
                MessageBox.Show(localization.strings.please_restart);
                Settings.Default.language = selectedLanguage;
            }

            if (!_wasHourBoostMode || _behaviorChanged)
            {
                Settings.Default.OneThenMany = Settings.Default.OnlyOneGameIdle
                    = Settings.Default.fastMode = Settings.Default.IdlingModeWhitelist = Settings.Default.IdlingModeTargetHours = false;

                if (radFastMode.Checked)
                {
                    Settings.Default.fastMode = true;
                }
                else if (radOneThenMany.Checked)
                {
                    Settings.Default.OneThenMany = true;
                }
                else
                {
                    Settings.Default.OnlyOneGameIdle = !radManyThenOne.Checked;
                }
            }

            Settings.Default.minToTray = chkMinToTray.Checked;
            Settings.Default.ignoreclient = chkIgnoreClientStatus.Checked;
            Settings.Default.showUsername = chkShowUsername.Checked;
            Settings.Default.NoSleep = chkPreventSleep.Checked;
            Settings.Default.ShutdownWindowsOnDone = chkShutdown.Checked;
            Settings.Default.IdleOnlyPlayed = chkIdleOnlyPlayed.Checked;

            Settings.Default.Save();

            Close();
        }

        private void frmSettings_Load(object sender, EventArgs e)
        {
            if (cboLanguage.Items.Count == 0 || !Equals(cboLanguage.Items[0], localization.strings.theme_auto))
            {
                cboLanguage.Items.Insert(0, localization.strings.theme_auto);
            }

            if (!string.IsNullOrWhiteSpace(Settings.Default.language) && cboLanguage.Items.Contains(Settings.Default.language))
            {
                cboLanguage.SelectedItem = Settings.Default.language;
            }
            else
            {
                cboLanguage.SelectedIndex = 0;
            }

            switch (Settings.Default.sort)
            {
                case "leastcards":
                    radIdleLeastDrops.Checked = true;
                    break;
                case "mostcards":
                    radIdleMostDrops.Checked = true;
                    break;
                default:
                    break;
            }

            // Load translation
            this.Text = localization.strings.idle_master_settings;
            grpGeneral.Text = localization.strings.general;
            grpIdlingQuantity.Text = localization.strings.idling_behavior;
            grpPriority.Text = localization.strings.idling_order;
            btnOK.Text = localization.strings.accept;
            btnCancel.Text = localization.strings.cancel;
            ttHints.SetToolTip(btnAdvanced, localization.strings.advanced_auth);
            chkMinToTray.Text = localization.strings.minimize_to_tray;
            ttHints.SetToolTip(chkMinToTray, localization.strings.minimize_to_tray);
            chkIgnoreClientStatus.Text = localization.strings.ignore_client_status;
            ttHints.SetToolTip(chkIgnoreClientStatus, localization.strings.ignore_client_status);
            chkShowUsername.Text = localization.strings.show_username;
            ttHints.SetToolTip(chkShowUsername, localization.strings.show_username);
            radOneGameOnly.Text = localization.strings.idle_individual;
            ttHints.SetToolTip(radOneGameOnly, localization.strings.idle_individual);
            radManyThenOne.Text = localization.strings.idle_simultaneous;
            ttHints.SetToolTip(radManyThenOne, localization.strings.idle_simultaneous);
            radOneThenMany.Text = localization.strings.idle_onethenmany;
            ttHints.SetToolTip(radOneThenMany, localization.strings.idle_onethenmany);
            radIdleDefault.Text = localization.strings.order_default;
            ttHints.SetToolTip(radIdleDefault, localization.strings.order_default);
            radIdleMostDrops.Text = localization.strings.order_most;
            ttHints.SetToolTip(radIdleMostDrops, localization.strings.order_most);
            radIdleLeastDrops.Text = localization.strings.order_least;
            ttHints.SetToolTip(radIdleLeastDrops, localization.strings.order_least);
            _wasHourBoostMode = Settings.Default.IdlingModeWhitelist || Settings.Default.IdlingModeTargetHours;
            _behaviorChanged = false;
            radFastMode.Click += (s, ev) => _behaviorChanged = true;
            radOneGameOnly.Click += (s, ev) => _behaviorChanged = true;
            radManyThenOne.Click += (s, ev) => _behaviorChanged = true;
            radOneThenMany.Click += (s, ev) => _behaviorChanged = true;
            lblLanguage.Text = localization.strings.interface_language;
            lblTheme.Text = localization.strings.theme_label;
            _isLoadingTheme = true;
            cboTheme.Items.Clear();
            cboTheme.Items.Add(localization.strings.theme_auto);
            cboTheme.Items.Add(localization.strings.theme_light);
            cboTheme.Items.Add(localization.strings.theme_dark);
            if (string.Equals(Settings.Default.themeMode, "light", StringComparison.OrdinalIgnoreCase))
            {
                cboTheme.SelectedIndex = 1;
            }
            else if (string.Equals(Settings.Default.themeMode, "dark", StringComparison.OrdinalIgnoreCase))
            {
                cboTheme.SelectedIndex = 2;
            }
            else
            {
                cboTheme.SelectedIndex = 0;
            }
            _isLoadingTheme = false;
            ThemeManager.ApplyThemeSetting();
            chkPreventSleep.Text = localization.strings.prevent_sleep;
            ttHints.SetToolTip(chkPreventSleep, localization.strings.prevent_sleep);
            chkShutdown.Text = localization.strings.shutdown_when_done;
            ttHints.SetToolTip(chkShutdown, localization.strings.shutdown_when_done);
            chkIdleOnlyPlayed.Text = localization.strings.idle_only_played;
            ttHints.SetToolTip(chkIdleOnlyPlayed, localization.strings.idle_only_played);
            radFastMode.Text = localization.strings.fast_mode;
            ttHints.SetToolTip(radFastMode, localization.strings.fast_mode);
            linkLabelAppData.Text = localization.strings.browse_appdata;

            if (Settings.Default.fastMode)
            {
                radFastMode.Checked = true;
            }
            else if (Settings.Default.OneThenMany)
            {
                radOneThenMany.Checked = true;
            }
            else
            {
                radOneGameOnly.Checked = Settings.Default.OnlyOneGameIdle;
                radManyThenOne.Checked = !Settings.Default.OnlyOneGameIdle;
            }

            if (Settings.Default.minToTray)
            {
                chkMinToTray.Checked = true;
            }

            if (Settings.Default.ignoreclient)
            {
                chkIgnoreClientStatus.Checked = true;
            }

            if (Settings.Default.showUsername)
            {
                chkShowUsername.Checked = true;
            }

            if (Settings.Default.NoSleep)
            {
                chkPreventSleep.Checked = true;
            }

            if (Settings.Default.ShutdownWindowsOnDone)
            {
                chkShutdown.Checked = true;
            }

            if (Settings.Default.IdleOnlyPlayed)
            {
                chkIdleOnlyPlayed.Checked = true;
            }

            runtimeCustomThemeSettings();
        }


        private void runtimeCustomThemeSettings()
        {
            // Read settings
            var customTheme = Settings.Default.customTheme;
            var whiteIcons = Settings.Default.whiteIcons;

            // Set checkboxes (Not necessary, as the checkboxes are bound to the global setting)
            //darkThemeCheckBox.Checked = customTheme;
            //whiteIconsCheckBox.Checked = whiteIcons;

            if (customTheme)
            {
                // Custom theme colors (could be user selected, probably)
                Settings.Default.colorBgd = Color.FromArgb(38, 38, 38);
                Settings.Default.colorTxt = Color.FromArgb(196, 196, 196);
            }

            // Define colors
            Color colorBgd = customTheme ? Settings.Default.colorBgd : Settings.Default.colorBgdOriginal;
            Color colorTxt = customTheme ? Settings.Default.colorTxt : Settings.Default.colorTxtOriginal;

            // Define button style
            FlatStyle buttonStyle = customTheme ? FlatStyle.Flat : FlatStyle.Standard;

            // --------------------------
            // -- APPLY THEME SETTINGS --
            // --------------------------

            // Form colors
            this.BackColor = colorBgd;
            this.ForeColor = colorTxt;

            // Group title colors
            grpGeneral.ForeColor = grpIdlingQuantity.ForeColor = grpPriority.ForeColor = colorTxt;

            // Dropdowns
            cboLanguage.FlatStyle = cboTheme.FlatStyle = customTheme ? FlatStyle.Flat : FlatStyle.Standard;
            cboLanguage.BackColor = cboTheme.BackColor = colorBgd;
            cboLanguage.ForeColor = cboTheme.ForeColor = colorTxt;

            // Buttons
            btnOK.FlatStyle = btnCancel.FlatStyle = btnAdvanced.FlatStyle = buttonStyle;
            btnOK.BackColor = btnCancel.BackColor = btnAdvanced.BackColor = colorBgd;
            btnOK.ForeColor = btnCancel.ForeColor = btnAdvanced.ForeColor = colorTxt;

            // Link labels
            linkLabelAppData.LinkColor = customTheme ? Color.GhostWhite : Color.Blue;

            // Update the icon(s)
            runtimeWhiteIconsSettings();
            ThemeManager.ApplyVisualPolish(this, customTheme);
            Settings.Default.Save();
        }

        private void runtimeWhiteIconsSettings()
        {
            btnAdvanced.Image = Settings.Default.whiteIcons ? Resources.imgLock_w : Resources.imgLock;
        }

        private void btnAdvanced_Click(object sender, EventArgs e)
        {
            var frm = new frmSettingsAdvanced();
            frm.ShowDialog();
        }

        private void cboTheme_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isLoadingTheme)
            {
                return;
            }

            switch (cboTheme.SelectedIndex)
            {
                case 1:
                    Settings.Default.themeMode = "light";
                    break;
                case 2:
                    Settings.Default.themeMode = "dark";
                    break;
                default:
                    Settings.Default.themeMode = "auto";
                    break;
            }

            ThemeManager.ApplyThemeSetting();
            runtimeCustomThemeSettings();
        }

        private void chkShutdown_CheckedChanged(object sender, EventArgs e)
        {
            if (chkShutdown.Checked)
            {
                if (MessageBox.Show(localization.strings.shutdown_confirm_msg,
                                    localization.strings.shutdown_confirm_title, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    Settings.Default.ShutdownWindowsOnDone = chkShutdown.Checked;
                }
                else
                {
                    chkShutdown.Checked = false;
                }
            }
        }

        private void linkLabelSettings_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("explorer.exe", Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\IdleMasterReborn");
        }
    }
}
