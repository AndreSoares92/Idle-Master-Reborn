using System.ComponentModel;
using System.Windows.Forms;

namespace IdleMasterReborn
{
    partial class frmSettingsAdvanced
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSettingsAdvanced));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtSessionID = new System.Windows.Forms.TextBox();
            this.txtSteamLoginSecure = new System.Windows.Forms.TextBox();
            this.txtSteamParental = new System.Windows.Forms.TextBox();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.ttHelp = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(122, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "sessionid:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(12, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(122, 23);
            this.label2.TabIndex = 1;
            this.label2.Text = "steamLoginSecure:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(12, 73);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(122, 32);
            this.label3.TabIndex = 2;
            this.label3.Text = "steamparental / steamMachineAuth:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.ttHelp.SetToolTip(this.label3, "Check your browser to see if you have one of these cookies available. They are ty" +
        "pically not required.");
            // 
            // txtSessionID
            // 
            this.txtSessionID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSessionID.Location = new System.Drawing.Point(140, 20);
            this.txtSessionID.Name = "txtSessionID";
            this.txtSessionID.PasswordChar = '*';
            this.txtSessionID.Size = new System.Drawing.Size(275, 20);
            this.txtSessionID.TabIndex = 3;
            this.txtSessionID.TextChanged += new System.EventHandler(this.txtSessionID_TextChanged);
            // 
            // txtSteamLoginSecure
            // 
            this.txtSteamLoginSecure.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSteamLoginSecure.Location = new System.Drawing.Point(140, 46);
            this.txtSteamLoginSecure.Name = "txtSteamLoginSecure";
            this.txtSteamLoginSecure.PasswordChar = '*';
            this.txtSteamLoginSecure.Size = new System.Drawing.Size(275, 20);
            this.txtSteamLoginSecure.TabIndex = 4;
            this.txtSteamLoginSecure.TextChanged += new System.EventHandler(this.txtSteamLogin_TextChanged);
            // 
            // txtSteamParental
            // 
            this.txtSteamParental.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSteamParental.Location = new System.Drawing.Point(140, 74);
            this.txtSteamParental.Name = "txtSteamParental";
            this.txtSteamParental.Size = new System.Drawing.Size(275, 20);
            this.txtSteamParental.TabIndex = 5;
            this.txtSteamParental.TextChanged += new System.EventHandler(this.txtSteamParental_TextChanged);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUpdate.Location = new System.Drawing.Point(340, 113);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 6;
            this.btnUpdate.Text = "&Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // ttHelp
            // 
            this.ttHelp.AutoPopDelay = 9000;
            this.ttHelp.InitialDelay = 500;
            this.ttHelp.ReshowDelay = 100;
            // 
            // frmSettingsAdvanced
            // 
            this.AcceptButton = this.btnUpdate;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(430, 148);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.txtSteamParental);
            this.Controls.Add(this.txtSteamLoginSecure);
            this.Controls.Add(this.txtSessionID);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmSettingsAdvanced";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Steam Authentication Data";
            this.Load += new System.EventHandler(this.frmSettingsAdvanced_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private TextBox txtSessionID;
        private TextBox txtSteamLoginSecure;
        private TextBox txtSteamParental;
        private Button btnUpdate;
        private ToolTip ttHelp;
    }
}