using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using IdleMasterReborn.Properties;
using Microsoft.Win32;

namespace IdleMasterReborn
{
    internal static class ThemeManager
    {
        private const string PersonalizeKey = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize";
        private const int DwmwaUseImmersiveDarkModeBefore20H1 = 19;
        private const int DwmwaUseImmersiveDarkMode = 20;

        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

        internal static bool IsWindowsDarkTheme()
        {
            try
            {
                object value = Registry.GetValue(PersonalizeKey, "AppsUseLightTheme", 1);
                if (value is int)
                {
                    return (int)value == 0;
                }
            }
            catch
            {
            }

            return false;
        }

        internal static void ApplyThemeSetting()
        {
            string mode = Settings.Default.themeMode;
            bool isDark;

            if (string.Equals(mode, "dark", StringComparison.OrdinalIgnoreCase))
            {
                isDark = true;
            }
            else if (string.Equals(mode, "light", StringComparison.OrdinalIgnoreCase))
            {
                isDark = false;
            }
            else
            {
                isDark = IsWindowsDarkTheme();
            }

            Settings.Default.customTheme = isDark;
            Settings.Default.whiteIcons = isDark;
        }

        internal static void ApplyTitleBarTheme(Form form, bool isDark)
        {
            if (form == null || !form.IsHandleCreated)
            {
                return;
            }

            try
            {
                int useImmersiveDarkMode = isDark ? 1 : 0;
                if (DwmSetWindowAttribute(form.Handle, DwmwaUseImmersiveDarkMode, ref useImmersiveDarkMode, sizeof(int)) != 0)
                {
                    DwmSetWindowAttribute(form.Handle, DwmwaUseImmersiveDarkModeBefore20H1, ref useImmersiveDarkMode, sizeof(int));
                }
            }
            catch
            {
            }
        }

        internal static ToolStripRenderer CreateMenuRenderer(bool isDark)
        {
            if (isDark)
            {
                return new DarkMenuRenderer();
            }
            return new ToolStripProfessionalRenderer();
        }

        internal static void ApplyVisualPolish(Form form, bool isDark)
        {
            if (form == null)
            {
                return;
            }

            ApplyTitleBarTheme(form, isDark);

            if (form.Font.Name != "Segoe UI")
            {
                form.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular);
            }

            StyleControls(form.Controls, isDark);
        }

        private static void StyleControls(Control.ControlCollection controls, bool isDark)
        {
            foreach (Control ctrl in controls)
            {
                var btn = ctrl as Button;
                if (btn != null)
                {
                    btn.Paint -= DarkDisabledButton_Paint;
                    if (isDark)
                    {
                        btn.FlatStyle = FlatStyle.Flat;
                        btn.BackColor = Color.FromArgb(48, 48, 48);
                        btn.ForeColor = Settings.Default.colorTxt;
                        btn.FlatAppearance.BorderColor = Color.FromArgb(80, 80, 80);
                        btn.FlatAppearance.BorderSize = 1;
                        btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(66, 66, 66);
                        btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(82, 82, 82);
                        btn.Paint += DarkDisabledButton_Paint;
                    }
                    else
                    {
                        btn.FlatStyle = FlatStyle.Standard;
                        btn.UseVisualStyleBackColor = true;
                        btn.ForeColor = Settings.Default.colorTxtOriginal;
                    }
                }

                var txt = ctrl as TextBox;
                if (txt != null)
                {
                    txt.BorderStyle = BorderStyle.FixedSingle;
                    if (isDark)
                    {
                        txt.BackColor = Color.FromArgb(45, 45, 45);
                        txt.ForeColor = Settings.Default.colorTxt;
                    }
                    else
                    {
                        txt.BackColor = SystemColors.Window;
                        txt.ForeColor = Settings.Default.colorTxtOriginal;
                    }
                }

                var nud = ctrl as NumericUpDown;
                if (nud != null)
                {
                    if (isDark)
                    {
                        nud.BackColor = Color.FromArgb(45, 45, 45);
                        nud.ForeColor = Settings.Default.colorTxt;
                    }
                    else
                    {
                        nud.BackColor = SystemColors.Window;
                        nud.ForeColor = Settings.Default.colorTxtOriginal;
                    }
                }

                var cbo = ctrl as ComboBox;
                if (cbo != null)
                {
                    if (isDark)
                    {
                        cbo.FlatStyle = FlatStyle.Flat;
                        cbo.BackColor = Color.FromArgb(45, 45, 45);
                        cbo.ForeColor = Settings.Default.colorTxt;
                    }
                    else
                    {
                        cbo.FlatStyle = FlatStyle.Standard;
                        cbo.BackColor = SystemColors.Window;
                        cbo.ForeColor = Settings.Default.colorTxtOriginal;
                    }
                }

                var lst = ctrl as ListBox;
                if (lst != null)
                {
                    lst.BorderStyle = BorderStyle.FixedSingle;
                    if (isDark)
                    {
                        lst.BackColor = Color.FromArgb(32, 32, 32);
                        lst.ForeColor = Settings.Default.colorTxt;
                    }
                    else
                    {
                        lst.BackColor = SystemColors.Window;
                        lst.ForeColor = Settings.Default.colorTxtOriginal;
                    }
                }

                if (ctrl.HasChildren)
                {
                    StyleControls(ctrl.Controls, isDark);
                }
            }
        }

        private static void DarkDisabledButton_Paint(object sender, PaintEventArgs e)
        {
            var btn = sender as Button;
            if (btn == null || btn.Enabled || string.IsNullOrEmpty(btn.Text))
            {
                return;
            }

            var rect = btn.ClientRectangle;
            using (var bgBrush = new SolidBrush(Color.FromArgb(40, 40, 40)))
            using (var borderPen = new Pen(Color.FromArgb(65, 65, 65)))
            {
                e.Graphics.FillRectangle(bgBrush, rect);
                e.Graphics.DrawRectangle(borderPen, 0, 0, rect.Width - 1, rect.Height - 1);
            }

            TextRenderer.DrawText(
                e.Graphics,
                btn.Text,
                btn.Font,
                rect,
                Color.FromArgb(135, 135, 135),
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        private sealed class DarkMenuRenderer : ToolStripProfessionalRenderer
        {
            public DarkMenuRenderer()
                : base(new DarkMenuColorTable())
            {
            }

            protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
            {
                if (e != null && e.Item != null)
                {
                    e.TextColor = e.Item.Enabled ? Color.FromArgb(220, 220, 220) : Color.FromArgb(120, 120, 120);
                }
                base.OnRenderItemText(e);
            }

            protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
            {
                if (e != null)
                {
                    e.ArrowColor = (e.Item == null || e.Item.Enabled) ? Color.FromArgb(220, 220, 220) : Color.FromArgb(120, 120, 120);
                }
                base.OnRenderArrow(e);
            }

            protected override void OnRenderItemCheck(ToolStripItemImageRenderEventArgs e)
            {
                if (e == null)
                {
                    return;
                }

                var g = e.Graphics;
                var rect = new Rectangle(e.ImageRectangle.Left, e.ImageRectangle.Top, e.ImageRectangle.Width - 1, e.ImageRectangle.Height - 1);
                using (var bgBrush = new SolidBrush(Color.FromArgb(62, 62, 66)))
                using (var borderPen = new Pen(Color.FromArgb(95, 95, 102)))
                using (var checkPen = new Pen(Color.FromArgb(126, 166, 75), 1.8F))
                {
                    g.FillRectangle(bgBrush, rect);
                    g.DrawRectangle(borderPen, rect);

                    var prevMode = g.SmoothingMode;
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    var p1 = new Point(rect.Left + 3, rect.Top + rect.Height / 2);
                    var p2 = new Point(rect.Left + rect.Width / 2 - 1, rect.Bottom - 4);
                    var p3 = new Point(rect.Right - 3, rect.Top + 4);
                    g.DrawLines(checkPen, new Point[] { p1, p2, p3 });
                    g.SmoothingMode = prevMode;
                }
            }
        }

        private sealed class DarkMenuColorTable : ProfessionalColorTable
        {
            private static readonly Color BgDark = Color.FromArgb(38, 38, 38);
            private static readonly Color BgHover = Color.FromArgb(62, 62, 66);
            private static readonly Color BgPressed = Color.FromArgb(74, 74, 79);
            private static readonly Color BorderDark = Color.FromArgb(74, 74, 79);

            public override Color MenuStripGradientBegin { get { return BgDark; } }
            public override Color MenuStripGradientEnd { get { return BgDark; } }
            public override Color MenuItemSelected { get { return BgHover; } }
            public override Color MenuItemSelectedGradientBegin { get { return BgHover; } }
            public override Color MenuItemSelectedGradientEnd { get { return BgHover; } }
            public override Color MenuItemPressedGradientBegin { get { return BgPressed; } }
            public override Color MenuItemPressedGradientEnd { get { return BgPressed; } }
            public override Color MenuItemPressedGradientMiddle { get { return BgPressed; } }
            public override Color MenuItemBorder { get { return BorderDark; } }
            public override Color MenuBorder { get { return BorderDark; } }
            public override Color CheckBackground { get { return BgHover; } }
            public override Color CheckPressedBackground { get { return BgHover; } }
            public override Color CheckSelectedBackground { get { return BgHover; } }
            public override Color ToolStripDropDownBackground { get { return BgDark; } }
            public override Color ImageMarginGradientBegin { get { return BgDark; } }
            public override Color ImageMarginGradientMiddle { get { return BgDark; } }
            public override Color ImageMarginGradientEnd { get { return BgDark; } }
            public override Color SeparatorDark { get { return BorderDark; } }
            public override Color SeparatorLight { get { return BgDark; } }
        }
    }
}
