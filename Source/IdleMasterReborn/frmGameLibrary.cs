using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using IdleMasterReborn.Properties;

namespace IdleMasterReborn
{
    public class frmGameLibrary : Form
    {
        private const int MaxSelectableGames = 30;
        private readonly frmMain mainForm;
        private readonly HashSet<int> _selectedAppIds = new HashSet<int>();
        private List<OwnedGame> _allGames = new List<OwnedGame>();
        private bool _isPopulating;
        private int _sortColumn = 0;
        private bool _sortAscending = true;

        private readonly Label lblSearch;
        private readonly TextBox txtSearch;
        private readonly Label lblSelectedCount;
        private readonly ListView lvGames;
        private readonly Button btnClear;
        private readonly Button btnStart;
        private readonly Button btnCancel;
        private readonly Label lblTargetHours;
        private readonly NumericUpDown nudTargetHours;
        private readonly Button btnFarmToTarget;

        public frmGameLibrary(frmMain parentForm)
        {
            mainForm = parentForm;

            Text = localization.strings.library_title;
            ClientSize = new Size(520, 480);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;

            lblSearch = new Label
            {
                AutoSize = true,
                Location = new Point(12, 15),
                Text = localization.strings.library_search
            };

            txtSearch = new TextBox
            {
                Location = new Point(75, 12),
                Size = new Size(265, 22)
            };
            txtSearch.TextChanged += (s, e) => PopulateList();

            lblSelectedCount = new Label
            {
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleRight,
                Location = new Point(348, 14),
                Size = new Size(160, 18)
            };

            lvGames = new ListView
            {
                Location = new Point(12, 42),
                Size = new Size(496, 350),
                View = View.Details,
                CheckBoxes = true,
                FullRowSelect = true,
                GridLines = false,
                MultiSelect = false
            };
            lvGames.Columns.Add(localization.strings.name, 300);
            lvGames.Columns.Add(localization.strings.hours, 80, HorizontalAlignment.Right);
            lvGames.Columns.Add("App ID", 85, HorizontalAlignment.Right);
            lvGames.ItemCheck += lvGames_ItemCheck;
            lvGames.ColumnClick += lvGames_ColumnClick;
            UpdateSortHeaders();

            lblTargetHours = new Label
            {
                AutoSize = true,
                Location = new Point(12, 405),
                Text = localization.strings.library_target_hours
            };

            nudTargetHours = new NumericUpDown
            {
                Location = new Point(195, 402),
                Size = new Size(70, 22),
                DecimalPlaces = 1,
                Increment = 0.5M,
                Minimum = 0.5M,
                Maximum = 10000M,
                Value = (decimal)Math.Max(0.5, Math.Min(10000.0, Settings.Default.TargetHours))
            };

            btnFarmToTarget = new Button
            {
                Location = new Point(275, 400),
                Size = new Size(233, 26),
                Text = localization.strings.library_start_target
            };
            btnFarmToTarget.Click += btnFarmToTarget_Click;

            btnClear = new Button
            {
                Location = new Point(12, 440),
                Size = new Size(110, 26),
                Text = localization.strings.library_clear
            };
            btnClear.Click += btnClear_Click;

            btnStart = new Button
            {
                Location = new Point(312, 440),
                Size = new Size(110, 26),
                Text = localization.strings.library_start
            };
            btnStart.Click += btnStart_Click;

            btnCancel = new Button
            {
                Location = new Point(428, 440),
                Size = new Size(80, 26),
                Text = localization.strings.cancel.Replace("&", "")
            };
            btnCancel.Click += (s, e) => Close();

            Controls.Add(lblSearch);
            Controls.Add(txtSearch);
            Controls.Add(lblSelectedCount);
            Controls.Add(lvGames);
            Controls.Add(lblTargetHours);
            Controls.Add(nudTargetHours);
            Controls.Add(btnFarmToTarget);
            Controls.Add(btnClear);
            Controls.Add(btnStart);
            Controls.Add(btnCancel);

            AcceptButton = btnStart;
            CancelButton = btnCancel;

            Load += frmGameLibrary_Load;
            WindowPositionManager.Attach(this);
        }

        private async void frmGameLibrary_Load(object sender, EventArgs e)
        {
            ApplyTheme();

            foreach (string idStr in Settings.Default.whitelist)
            {
                int id;
                if (int.TryParse(idStr, out id) && id > 0)
                {
                    _selectedAppIds.Add(id);
                }
            }
            UpdateSelectedCountLabel();

            btnStart.Enabled = false;
            lvGames.Items.Add(new ListViewItem(localization.strings.library_loading));

            _allGames = await SteamLibrary.GetOwnedGamesAsync();
            btnStart.Enabled = true;
            PopulateList();
        }

        private void ApplyTheme()
        {
            bool customTheme = Settings.Default.customTheme;
            Color bg = customTheme ? Settings.Default.colorBgd : Settings.Default.colorBgdOriginal;
            Color txt = customTheme ? Settings.Default.colorTxt : Settings.Default.colorTxtOriginal;

            BackColor = bg;
            ForeColor = txt;
            txtSearch.BackColor = bg;
            txtSearch.ForeColor = txt;
            lvGames.BackColor = bg;
            lvGames.ForeColor = txt;
            nudTargetHours.BackColor = bg;
            nudTargetHours.ForeColor = txt;

            ThemeManager.ApplyVisualPolish(this, customTheme);
        }

        private void PopulateList()
        {
            _isPopulating = true;
            lvGames.BeginUpdate();
            lvGames.Items.Clear();

            string filter = (txtSearch.Text ?? string.Empty).Trim();
            var filtered = _allGames.Where(g =>
                string.IsNullOrEmpty(filter)
                || g.Name.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0
                || g.AppId.ToString().Contains(filter));

            IEnumerable<OwnedGame> sorted;
            switch (_sortColumn)
            {
                case 1:
                    sorted = _sortAscending
                        ? filtered.OrderBy(g => g.HoursPlayed).ThenBy(g => g.Name, StringComparer.CurrentCultureIgnoreCase)
                        : filtered.OrderByDescending(g => g.HoursPlayed).ThenBy(g => g.Name, StringComparer.CurrentCultureIgnoreCase);
                    break;
                case 2:
                    sorted = _sortAscending
                        ? filtered.OrderBy(g => g.AppId)
                        : filtered.OrderByDescending(g => g.AppId);
                    break;
                default:
                    sorted = _sortAscending
                        ? filtered.OrderBy(g => g.Name, StringComparer.CurrentCultureIgnoreCase)
                        : filtered.OrderByDescending(g => g.Name, StringComparer.CurrentCultureIgnoreCase);
                    break;
            }

            var list = sorted.ToList();
            if (list.Count == 0)
            {
                var emptyItem = new ListViewItem(localization.strings.library_empty);
                lvGames.Items.Add(emptyItem);
            }
            else
            {
                foreach (var game in list)
                {
                    var item = new ListViewItem(game.Name);
                    item.SubItems.Add(game.HoursPlayed.ToString("0.0"));
                    item.SubItems.Add(game.AppId.ToString());
                    item.Tag = game;
                    item.Checked = _selectedAppIds.Contains(game.AppId);
                    lvGames.Items.Add(item);
                }
            }

            lvGames.EndUpdate();
            _isPopulating = false;
            UpdateSelectedCountLabel();
        }

        private void lvGames_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (e.Column == _sortColumn)
            {
                _sortAscending = !_sortAscending;
            }
            else
            {
                _sortColumn = e.Column;
                _sortAscending = (e.Column != 1);
            }

            UpdateSortHeaders();
            PopulateList();
        }

        private void UpdateSortHeaders()
        {
            string arrow = _sortAscending ? " ▲" : " ▼";
            lvGames.Columns[0].Text = localization.strings.name + (_sortColumn == 0 ? arrow : string.Empty);
            lvGames.Columns[1].Text = localization.strings.hours + (_sortColumn == 1 ? arrow : string.Empty);
            lvGames.Columns[2].Text = "App ID" + (_sortColumn == 2 ? arrow : string.Empty);
        }

        private void lvGames_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (_isPopulating)
            {
                return;
            }

            var item = lvGames.Items[e.Index];
            var game = item.Tag as OwnedGame;
            if (game == null)
            {
                e.NewValue = CheckState.Unchecked;
                return;
            }

            if (e.NewValue == CheckState.Checked)
            {
                if (_selectedAppIds.Count >= MaxSelectableGames && !_selectedAppIds.Contains(game.AppId))
                {
                    e.NewValue = CheckState.Unchecked;
                    MessageBox.Show(
                        string.Format(localization.strings.library_limit_reached, MaxSelectableGames),
                        Text,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    return;
                }
                _selectedAppIds.Add(game.AppId);
            }
            else
            {
                _selectedAppIds.Remove(game.AppId);
            }

            UpdateSelectedCountLabel();
        }

        private void UpdateSelectedCountLabel()
        {
            lblSelectedCount.Text = string.Format(localization.strings.library_selected_count, _selectedAppIds.Count, MaxSelectableGames);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            _selectedAppIds.Clear();
            PopulateList();
        }

        private async void btnStart_Click(object sender, EventArgs e)
        {
            Settings.Default.whitelist.Clear();
            foreach (int id in _selectedAppIds.Take(MaxSelectableGames))
            {
                Settings.Default.whitelist.Add(id.ToString());
            }

            Settings.Default.IdlingModeTargetHours = false;
            Settings.Default.IdlingModeWhitelist = true;
            Settings.Default.fastMode = false;
            Settings.Default.OneThenMany = false;
            Settings.Default.OnlyOneGameIdle = false;
            Settings.Default.Save();

            mainForm.BeginNewIdleSession();
            mainForm.AllBadges.Clear();
            await mainForm.LoadBadgesAsync();

            if (_selectedAppIds.Count == 1)
            {
                int onlyId = _selectedAppIds.First();
                var badge = mainForm.AllBadges.FirstOrDefault(b => b.AppId == onlyId);
                if (badge != null)
                {
                    mainForm.StartSoloIdle(badge);
                }
            }
            else if (_selectedAppIds.Count > 1)
            {
                mainForm.StartMultipleIdle();
            }

            mainForm.DisableCardDropCheckTimer();
            mainForm.UpdateStateInfo();

            DialogResult = DialogResult.OK;
            Close();
        }

        private async void btnFarmToTarget_Click(object sender, EventArgs e)
        {
            double target = (double)nudTargetHours.Value;
            bool hasPending = _allGames.Any(g =>
                g.HoursPlayed < target
                && !Settings.Default.blacklist.Contains(g.AppId.ToString()));

            if (!hasPending)
            {
                MessageBox.Show(
                    string.Format(localization.strings.library_no_games_below_target, target.ToString("0.0")),
                    Text,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            Settings.Default.TargetHours = target;
            Settings.Default.IdlingModeTargetHours = true;
            Settings.Default.IdlingModeWhitelist = false;
            Settings.Default.fastMode = false;
            Settings.Default.OneThenMany = false;
            Settings.Default.OnlyOneGameIdle = false;
            Settings.Default.Save();

            mainForm.BeginNewIdleSession();
            mainForm.AllBadges.Clear();
            await mainForm.LoadBadgesAsync();

            if (mainForm.CanIdleBadges.Count() == 1)
            {
                mainForm.StartSoloIdle(mainForm.CanIdleBadges.First());
            }
            else if (mainForm.CanIdleBadges.Any())
            {
                mainForm.StartMultipleIdle();
            }

            mainForm.TimeLeft = 300;
            mainForm.EnableCardDropCheckTimer();
            mainForm.UpdateStateInfo();

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
