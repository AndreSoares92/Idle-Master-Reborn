using System;
using System.Linq;
using System.Windows.Forms;
using IdleMasterExtended.Properties;

namespace IdleMasterExtended
{
    public partial class frmBlacklist : Form
    {
        public frmBlacklist()
        {
            InitializeComponent();
            WindowPositionManager.Attach(this);
        }

        private static string FormatAppEntry(string appId)
        {
            int id;
            OwnedGame game;
            if (int.TryParse(appId, out id) && SteamLibrary.CachedGames.TryGetValue(id, out game))
            {
                return string.Format("{0} ({1})", game.Name, id);
            }
            return appId;
        }

        private static string ExtractAppId(string itemText)
        {
            if (string.IsNullOrWhiteSpace(itemText))
            {
                return string.Empty;
            }

            int openIdx = itemText.LastIndexOf(" (", StringComparison.Ordinal);
            if (openIdx >= 0 && itemText.EndsWith(")", StringComparison.Ordinal))
            {
                string inner = itemText.Substring(openIdx + 2, itemText.Length - openIdx - 3);
                int parsed;
                if (int.TryParse(inner, out parsed))
                {
                    return parsed.ToString();
                }
            }
            return itemText.Trim();
        }

        public void SaveBlacklist()
        {
            Settings.Default.blacklist.Clear();
            var ids = lstBlacklist.Items.Cast<string>()
                .Select(ExtractAppId)
                .Where(id => !string.IsNullOrWhiteSpace(id))
                .Distinct()
                .ToArray();
            Settings.Default.blacklist.AddRange(ids);
            Settings.Default.Save();
        }

        private async void frmBlacklist_Load(object sender, EventArgs e)
        {
            // Localize form
            btnAdd.Text = localization.strings.add;
            btnSave.Text = localization.strings.save;
            this.Text = localization.strings.manage_blacklist;
            grpAdd.Text = localization.strings.add_game_blacklist;

            var savedIds = Settings.Default.blacklist.Cast<string>().Where(id => !string.IsNullOrWhiteSpace(id)).ToList();
            foreach (string id in savedIds)
            {
                lstBlacklist.Items.Add(FormatAppEntry(id));
            }

            if (Settings.Default.customTheme)
            {
                runtimeCustomThemeBlacklist(); // JN: Apply the dark theme
            }
            ThemeManager.ApplyVisualPolish(this, Settings.Default.customTheme);

            if (savedIds.Count > 0 && SteamLibrary.CachedGames.Count == 0)
            {
                await SteamLibrary.GetOwnedGamesAsync();
                if (!IsDisposed && SteamLibrary.CachedGames.Count > 0)
                {
                    for (int i = 0; i < lstBlacklist.Items.Count; i++)
                    {
                        string rawId = ExtractAppId(lstBlacklist.Items[i].ToString());
                        lstBlacklist.Items[i] = FormatAppEntry(rawId);
                    }
                }
            }
        }

        // Make everything dark to match the dark theme
        private void runtimeCustomThemeBlacklist()
        {
            System.Drawing.Color colorBgd = Settings.Default.colorBgd; // Dark gray (from Steam)
            System.Drawing.Color colorTxt = Settings.Default.colorTxt; // Light gray (from Steam)

            // Form
            this.BackColor = colorBgd;
            this.ForeColor = colorTxt;

            // Button
            btnAdd.FlatStyle = btnSave.FlatStyle = btnRemove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnAdd.BackColor = btnSave.BackColor = btnRemove.BackColor = colorBgd;
            btnAdd.ForeColor = btnSave.ForeColor = btnRemove.ForeColor = colorTxt;

            // List
            lstBlacklist.BackColor = colorBgd;
            lstBlacklist.ForeColor = colorTxt;

            // Group
            grpAdd.BackColor = colorBgd;
            grpAdd.ForeColor = colorTxt;

            // Textbox Appid
            txtAppid.BackColor = colorBgd;
            txtAppid.ForeColor = colorTxt;

            btnRemove.Image = Settings.Default.whiteIcons ? Resources.imgTrash_w : Resources.imgTrash;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveBlacklist();
            Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            int result;
            if (int.TryParse(txtAppid.Text.Trim(), out result))
            {
                string idStr = result.ToString();
                if (lstBlacklist.Items.Cast<string>().All(blApp => ExtractAppId(blApp) != idStr))
                    lstBlacklist.Items.Add(FormatAppEntry(idStr));
            }
            txtAppid.Text = string.Empty;
            txtAppid.Focus();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            lstBlacklist.Items.Remove(lstBlacklist.SelectedItem);
        }
    }
}
