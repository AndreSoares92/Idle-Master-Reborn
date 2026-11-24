using System;
using System.Windows.Forms;

namespace IdleMasterExtended
{
    public partial class frmStatistics : Form
    {
        private Statistics statistics;
        public frmStatistics(Statistics statistics)
        {
            InitializeComponent();
            this.statistics = statistics;
            WindowPositionManager.Attach(this);
        }

        private void frmStatistics_Load(object sender, EventArgs e)
        {
            // Localize Form
            this.Text = localization.strings.statistics.Replace("&", "");
            btnOK.Text = localization.strings.accept;
            lblSessionHeader.Text = localization.strings.this_session + ":";
            lblTotalHeader.Text = localization.strings.total + ":";

            TimeSpan sessionMinutesIdled = TimeSpan.FromMinutes(statistics.getSessionMinutesIdled());
            TimeSpan totalMinutesIdled = TimeSpan.FromMinutes(Properties.Settings.Default.totalMinutesIdled);

            int sessionHoursIdled = (sessionMinutesIdled.Days * 24) + sessionMinutesIdled.Hours;
            int totalHoursIdled = (totalMinutesIdled.Days * 24) + totalMinutesIdled.Hours;

            //Session
            if (sessionHoursIdled > 0)
            {
                lblSessionTime.Text = String.Format("{0} {1}, {2} {3} {4}",
                        sessionHoursIdled,
                        sessionHoursIdled == 1 ? localization.strings.stats_hour : localization.strings.stats_hours,
                        sessionMinutesIdled.Minutes,
                        sessionMinutesIdled.Minutes == 1 ? localization.strings.stats_minute : localization.strings.stats_minutes,
                        localization.strings.stats_idled);
            }
            else
            {
                lblSessionTime.Text = String.Format("{0} {1} {2}",
                        sessionMinutesIdled.Minutes,
                        sessionMinutesIdled.Minutes == 1 ? localization.strings.stats_minute : localization.strings.stats_minutes,
                        localization.strings.stats_idled);
            }
            lblSessionCards.Text = statistics.getSessionCardIdled().ToString() + " " + localization.strings.stats_cards_idled;

            //Total
            if (totalHoursIdled > 0)
            {
                lblTotalTime.Text = String.Format("{0} {1}, {2} {3} {4}",
                    totalHoursIdled,
                    totalHoursIdled == 1 ? localization.strings.stats_hour : localization.strings.stats_hours,
                    totalMinutesIdled.Minutes,
                    totalMinutesIdled.Minutes == 1 ? localization.strings.stats_minute : localization.strings.stats_minutes,
                    localization.strings.stats_idled);
            }
            else
            {
                lblTotalTime.Text = String.Format("{0} {1} {2}",
                    totalMinutesIdled.Minutes,
                    totalMinutesIdled.Minutes == 1 ? localization.strings.stats_minute : localization.strings.stats_minutes,
                    localization.strings.stats_idled);
            }
            lblTotalCards.Text = Properties.Settings.Default.totalCardIdled.ToString() + " " + localization.strings.stats_cards_idled;

            if (Properties.Settings.Default.customTheme)
            {
                runtimeCustomThemeStatistics(); // JN: Apply the dark theme
            }
            ThemeManager.ApplyVisualPolish(this, Properties.Settings.Default.customTheme);
        }

        // Make everything dark to match the dark theme
        private void runtimeCustomThemeStatistics()
        {
            System.Drawing.Color colorBgd = Properties.Settings.Default.colorBgd; // Dark gray (from Steam)
            System.Drawing.Color colorTxt = Properties.Settings.Default.colorTxt; // Light gray (from Steam)

            // Form
            this.BackColor = colorBgd;
            this.ForeColor = colorTxt;

            // Button
            btnOK.FlatStyle = FlatStyle.Flat; // Flat style to customize buttons
            btnOK.BackColor = colorBgd;
            btnOK.ForeColor = colorTxt;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
