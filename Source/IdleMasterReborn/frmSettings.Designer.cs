using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace IdleMasterReborn
{
    partial class frmSettings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSettings));
            this.grpGeneral = new System.Windows.Forms.GroupBox();
            this.chkIdleOnlyPlayed = new System.Windows.Forms.CheckBox();
            this.chkShutdown = new System.Windows.Forms.CheckBox();
            this.chkPreventSleep = new System.Windows.Forms.CheckBox();
            this.lblTheme = new System.Windows.Forms.Label();
            this.cboTheme = new System.Windows.Forms.ComboBox();
            this.cboLanguage = new System.Windows.Forms.ComboBox();
            this.lblLanguage = new System.Windows.Forms.Label();
            this.chkShowUsername = new System.Windows.Forms.CheckBox();
            this.chkIgnoreClientStatus = new System.Windows.Forms.CheckBox();
            this.chkMinToTray = new System.Windows.Forms.CheckBox();
            this.linkLabelAppData = new System.Windows.Forms.LinkLabel();
            this.grpPriority = new System.Windows.Forms.GroupBox();
            this.radIdleLeastDrops = new System.Windows.Forms.RadioButton();
            this.radIdleMostDrops = new System.Windows.Forms.RadioButton();
            this.radIdleDefault = new System.Windows.Forms.RadioButton();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.ttHints = new System.Windows.Forms.ToolTip(this.components);
            this.btnAdvanced = new System.Windows.Forms.Button();
            this.grpIdlingQuantity = new System.Windows.Forms.GroupBox();
            this.radFastMode = new System.Windows.Forms.RadioButton();
            this.radOneThenMany = new System.Windows.Forms.RadioButton();
            this.radManyThenOne = new System.Windows.Forms.RadioButton();
            this.radOneGameOnly = new System.Windows.Forms.RadioButton();
            this.lblReadMore = new System.Windows.Forms.Label();
            this.grpGeneral.SuspendLayout();
            this.grpPriority.SuspendLayout();
            this.grpIdlingQuantity.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpGeneral
            // 
            this.grpGeneral.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpGeneral.Controls.Add(this.chkIdleOnlyPlayed);
            this.grpGeneral.Controls.Add(this.chkShutdown);
            this.grpGeneral.Controls.Add(this.chkPreventSleep);
            this.grpGeneral.Controls.Add(this.lblTheme);
            this.grpGeneral.Controls.Add(this.cboTheme);
            this.grpGeneral.Controls.Add(this.cboLanguage);
            this.grpGeneral.Controls.Add(this.lblLanguage);
            this.grpGeneral.Controls.Add(this.chkShowUsername);
            this.grpGeneral.Controls.Add(this.chkIgnoreClientStatus);
            this.grpGeneral.Controls.Add(this.chkMinToTray);
            this.grpGeneral.Location = new System.Drawing.Point(12, 25);
            this.grpGeneral.Name = "grpGeneral";
            this.grpGeneral.Size = new System.Drawing.Size(510, 163);
            this.grpGeneral.TabIndex = 0;
            this.grpGeneral.TabStop = false;
            this.grpGeneral.Text = "General";
            this.grpGeneral.Enter += new System.EventHandler(this.grpGeneral_Enter);
            // 
            // chkIdleOnlyPlayed
            // 
            this.chkIdleOnlyPlayed.AutoSize = true;
            this.chkIdleOnlyPlayed.Location = new System.Drawing.Point(8, 138);
            this.chkIdleOnlyPlayed.Name = "chkIdleOnlyPlayed";
            this.chkIdleOnlyPlayed.Size = new System.Drawing.Size(133, 17);
            this.chkIdleOnlyPlayed.TabIndex = 32;
            this.chkIdleOnlyPlayed.Text = "Idle only played games";
            this.chkIdleOnlyPlayed.UseVisualStyleBackColor = true;
            // 
            // chkShutdown
            // 
            this.chkShutdown.AutoSize = true;
            this.chkShutdown.Location = new System.Drawing.Point(8, 114);
            this.chkShutdown.Name = "chkShutdown";
            this.chkShutdown.Size = new System.Drawing.Size(177, 17);
            this.chkShutdown.TabIndex = 31;
            this.chkShutdown.Text = "Shutdown Windows when done";
            this.chkShutdown.UseVisualStyleBackColor = true;
            this.chkShutdown.CheckedChanged += new System.EventHandler(this.chkShutdown_CheckedChanged);
            // 
            // chkPreventSleep
            // 
            this.chkPreventSleep.AutoSize = true;
            this.chkPreventSleep.Location = new System.Drawing.Point(8, 91);
            this.chkPreventSleep.Name = "chkPreventSleep";
            this.chkPreventSleep.Size = new System.Drawing.Size(163, 17);
            this.chkPreventSleep.TabIndex = 8;
            this.chkPreventSleep.Text = "Prevent Windows from Sleep";
            this.chkPreventSleep.UseVisualStyleBackColor = true;
            // 
            // lblTheme
            // 
            this.lblTheme.AutoSize = true;
            this.lblTheme.Location = new System.Drawing.Point(325, 64);
            this.lblTheme.Name = "lblTheme";
            this.lblTheme.Size = new System.Drawing.Size(43, 13);
            this.lblTheme.TabIndex = 6;
            this.lblTheme.Text = "Theme:";
            // 
            // cboTheme
            // 
            this.cboTheme.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTheme.FormattingEnabled = true;
            this.cboTheme.Location = new System.Drawing.Point(325, 84);
            this.cboTheme.Name = "cboTheme";
            this.cboTheme.Size = new System.Drawing.Size(175, 21);
            this.cboTheme.TabIndex = 7;
            this.cboTheme.SelectedIndexChanged += new System.EventHandler(this.cboTheme_SelectedIndexChanged);
            // 
            // cboLanguage
            // 
            this.cboLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLanguage.FormattingEnabled = true;
            this.cboLanguage.Items.AddRange(new object[] {
            "English",
            "Chinese (Simplified, China)",
            "Chinese (Traditional, China)",
            "Czech",
            "Dutch",
            "Finnish",
            "French",
            "German",
            "Greek",
            "Hungarian",
            "Italian",
            "Japanese",
            "Korean",
            "Norwegian",
            "Polish",
            "Portuguese",
            "Portuguese (Brazil)",
            "Romanian",
            "Russian",
            "Spanish",
            "Swedish",
            "Thai",
            "Turkish",
            "Ukrainian",
            "Croatian"});
            this.cboLanguage.Location = new System.Drawing.Point(325, 134);
            this.cboLanguage.Name = "cboLanguage";
            this.cboLanguage.Size = new System.Drawing.Size(175, 21);
            this.cboLanguage.TabIndex = 4;
            // 
            // lblLanguage
            // 
            this.lblLanguage.AutoSize = true;
            this.lblLanguage.Location = new System.Drawing.Point(325, 114);
            this.lblLanguage.Name = "lblLanguage";
            this.lblLanguage.Size = new System.Drawing.Size(103, 13);
            this.lblLanguage.TabIndex = 3;
            this.lblLanguage.Text = "Interface Language:";
            this.lblLanguage.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chkShowUsername
            // 
            this.chkShowUsername.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chkShowUsername.Location = new System.Drawing.Point(8, 66);
            this.chkShowUsername.Name = "chkShowUsername";
            this.chkShowUsername.Size = new System.Drawing.Size(379, 19);
            this.chkShowUsername.TabIndex = 2;
            this.chkShowUsername.Text = "Show Steam username of signed on user";
            this.chkShowUsername.UseVisualStyleBackColor = true;
            // 
            // chkIgnoreClientStatus
            // 
            this.chkIgnoreClientStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chkIgnoreClientStatus.Location = new System.Drawing.Point(8, 43);
            this.chkIgnoreClientStatus.Name = "chkIgnoreClientStatus";
            this.chkIgnoreClientStatus.Size = new System.Drawing.Size(379, 17);
            this.chkIgnoreClientStatus.TabIndex = 1;
            this.chkIgnoreClientStatus.Text = "Ignore Steam client status";
            this.chkIgnoreClientStatus.UseVisualStyleBackColor = true;
            // 
            // chkMinToTray
            // 
            this.chkMinToTray.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chkMinToTray.Location = new System.Drawing.Point(8, 20);
            this.chkMinToTray.Name = "chkMinToTray";
            this.chkMinToTray.Size = new System.Drawing.Size(379, 17);
            this.chkMinToTray.TabIndex = 0;
            this.chkMinToTray.Text = "Minimize Idle Master to system tray";
            this.chkMinToTray.UseVisualStyleBackColor = true;
            // 
            // linkLabelAppData
            // 
            this.linkLabelAppData.AutoSize = true;
            this.linkLabelAppData.Location = new System.Drawing.Point(43, 433);
            this.linkLabelAppData.Name = "linkLabelAppData";
            this.linkLabelAppData.Size = new System.Drawing.Size(120, 13);
            this.linkLabelAppData.TabIndex = 6;
            this.linkLabelAppData.TabStop = true;
            this.linkLabelAppData.Text = "Browse ApplicationData";
            this.linkLabelAppData.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelSettings_LinkClicked);
            // 
            // grpPriority
            // 
            this.grpPriority.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpPriority.Controls.Add(this.radIdleLeastDrops);
            this.grpPriority.Controls.Add(this.radIdleMostDrops);
            this.grpPriority.Controls.Add(this.radIdleDefault);
            this.grpPriority.Location = new System.Drawing.Point(12, 314);
            this.grpPriority.Name = "grpPriority";
            this.grpPriority.Size = new System.Drawing.Size(510, 96);
            this.grpPriority.TabIndex = 1;
            this.grpPriority.TabStop = false;
            this.grpPriority.Text = "Idling Order";
            // 
            // radIdleLeastDrops
            // 
            this.radIdleLeastDrops.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.radIdleLeastDrops.Location = new System.Drawing.Point(8, 65);
            this.radIdleLeastDrops.Name = "radIdleLeastDrops";
            this.radIdleLeastDrops.Size = new System.Drawing.Size(494, 17);
            this.radIdleLeastDrops.TabIndex = 2;
            this.radIdleLeastDrops.Text = "Prioritize games with the lowest number of available drops";
            this.radIdleLeastDrops.UseVisualStyleBackColor = true;
            // 
            // radIdleMostDrops
            // 
            this.radIdleMostDrops.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.radIdleMostDrops.Location = new System.Drawing.Point(8, 42);
            this.radIdleMostDrops.Name = "radIdleMostDrops";
            this.radIdleMostDrops.Size = new System.Drawing.Size(494, 17);
            this.radIdleMostDrops.TabIndex = 1;
            this.radIdleMostDrops.Text = "Prioritize games with the highest number of available drops";
            this.radIdleMostDrops.UseVisualStyleBackColor = true;
            // 
            // radIdleDefault
            // 
            this.radIdleDefault.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.radIdleDefault.Checked = true;
            this.radIdleDefault.Location = new System.Drawing.Point(8, 19);
            this.radIdleDefault.Name = "radIdleDefault";
            this.radIdleDefault.Size = new System.Drawing.Size(494, 17);
            this.radIdleDefault.TabIndex = 0;
            this.radIdleDefault.TabStop = true;
            this.radIdleDefault.Text = "Default (Alphabetical Order)";
            this.radIdleDefault.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(447, 428);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(366, 428);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "&Accept";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnAdvanced
            // 
            this.btnAdvanced.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAdvanced.Image = global::IdleMasterReborn.Properties.Resources.imgLock;
            this.btnAdvanced.Location = new System.Drawing.Point(12, 428);
            this.btnAdvanced.Name = "btnAdvanced";
            this.btnAdvanced.Size = new System.Drawing.Size(25, 23);
            this.btnAdvanced.TabIndex = 4;
            this.ttHints.SetToolTip(this.btnAdvanced, "Display advanced authentication information");
            this.btnAdvanced.UseVisualStyleBackColor = true;
            this.btnAdvanced.Click += new System.EventHandler(this.btnAdvanced_Click);
            // 
            // grpIdlingQuantity
            // 
            this.grpIdlingQuantity.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpIdlingQuantity.Controls.Add(this.radFastMode);
            this.grpIdlingQuantity.Controls.Add(this.radOneThenMany);
            this.grpIdlingQuantity.Controls.Add(this.radManyThenOne);
            this.grpIdlingQuantity.Controls.Add(this.radOneGameOnly);
            this.grpIdlingQuantity.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpIdlingQuantity.Location = new System.Drawing.Point(12, 193);
            this.grpIdlingQuantity.Margin = new System.Windows.Forms.Padding(2);
            this.grpIdlingQuantity.Name = "grpIdlingQuantity";
            this.grpIdlingQuantity.Padding = new System.Windows.Forms.Padding(2);
            this.grpIdlingQuantity.Size = new System.Drawing.Size(510, 116);
            this.grpIdlingQuantity.TabIndex = 5;
            this.grpIdlingQuantity.TabStop = false;
            this.grpIdlingQuantity.Text = "Idling Behavior";
            // 
            // radFastMode
            // 
            this.radFastMode.Checked = true;
            this.radFastMode.Location = new System.Drawing.Point(5, 18);
            this.radFastMode.Name = "radFastMode";
            this.radFastMode.Size = new System.Drawing.Size(496, 17);
            this.radFastMode.TabIndex = 7;
            this.radFastMode.TabStop = true;
            this.radFastMode.Text = "Fast mode (Recommended)";
            this.radFastMode.UseVisualStyleBackColor = true;
            // 
            // radOneThenMany
            // 
            this.radOneThenMany.Location = new System.Drawing.Point(5, 87);
            this.radOneThenMany.Name = "radOneThenMany";
            this.radOneThenMany.Size = new System.Drawing.Size(496, 17);
            this.radOneThenMany.TabIndex = 6;
            this.radOneThenMany.Text = "Idle games with more than 2 hours individually, then simultaneously";
            this.radOneThenMany.UseVisualStyleBackColor = true;
            // 
            // radManyThenOne
            // 
            this.radManyThenOne.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.radManyThenOne.Location = new System.Drawing.Point(5, 64);
            this.radManyThenOne.Name = "radManyThenOne";
            this.radManyThenOne.Size = new System.Drawing.Size(496, 17);
            this.radManyThenOne.TabIndex = 5;
            this.radManyThenOne.Text = "Idle games simultaneously up to 2 hours, then individually";
            this.radManyThenOne.UseVisualStyleBackColor = true;
            // 
            // radOneGameOnly
            // 
            this.radOneGameOnly.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.radOneGameOnly.Location = new System.Drawing.Point(5, 41);
            this.radOneGameOnly.Name = "radOneGameOnly";
            this.radOneGameOnly.Size = new System.Drawing.Size(496, 17);
            this.radOneGameOnly.TabIndex = 4;
            this.radOneGameOnly.Text = "Idle each game individually (Slow)";
            this.radOneGameOnly.UseVisualStyleBackColor = true;
            // 
            // lblReadMore
            // 
            this.lblReadMore.AutoSize = true;
            this.lblReadMore.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReadMore.Location = new System.Drawing.Point(53, 9);
            this.lblReadMore.Name = "lblReadMore";
            this.lblReadMore.Size = new System.Drawing.Size(211, 13);
            this.lblReadMore.TabIndex = 7;
            this.lblReadMore.Text = "Read more about the settings and features:";
            this.lblReadMore.Visible = false;
            // 
            // frmSettings
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(534, 463);
            this.Controls.Add(this.lblReadMore);
            this.Controls.Add(this.linkLabelAppData);
            this.Controls.Add(this.grpIdlingQuantity);
            this.Controls.Add(this.btnAdvanced);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.grpPriority);
            this.Controls.Add(this.grpGeneral);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Idle Master Reborn Settings";
            this.Load += new System.EventHandler(this.frmSettings_Load);
            this.grpGeneral.ResumeLayout(false);
            this.grpGeneral.PerformLayout();
            this.grpPriority.ResumeLayout(false);
            this.grpIdlingQuantity.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void grpGeneral_Enter(object sender, EventArgs e)
        {

        }

        #endregion

        private GroupBox grpGeneral;
        private CheckBox chkMinToTray;
        private GroupBox grpPriority;
        private RadioButton radIdleLeastDrops;
        private RadioButton radIdleMostDrops;
        private RadioButton radIdleDefault;
        private Button btnCancel;
        private Button btnOK;
        private Button btnAdvanced;
        private ToolTip ttHints;
        private CheckBox chkIgnoreClientStatus;
        private CheckBox chkShowUsername;
        private GroupBox grpIdlingQuantity;
        private RadioButton radManyThenOne;
        private RadioButton radOneGameOnly;
        private ComboBox cboLanguage;
        private Label lblLanguage;
        private RadioButton radOneThenMany;
        private Label lblTheme;
        private ComboBox cboTheme;
        private RadioButton radFastMode;
        private CheckBox chkPreventSleep;
        private CheckBox chkShutdown;
        private LinkLabel linkLabelAppData;
        private CheckBox chkIdleOnlyPlayed;
        private Label lblReadMore;
    }
}