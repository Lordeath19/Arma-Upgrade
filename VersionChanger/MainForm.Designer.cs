using System;

namespace VersionChanger
{
	partial class MainForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

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
			this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
			this.armaBrowser = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.armaText = new System.Windows.Forms.TextBox();
			this.latestUpgrade = new System.Windows.Forms.Button();
			this.stableDowngrade = new System.Windows.Forms.Button();
			this.help = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.settingsButton = new System.Windows.Forms.Button();
			this.imageList1 = new System.Windows.Forms.ImageList(this.components);
			this.settingTooltip = new System.Windows.Forms.ToolTip(this.components);
			this.SuspendLayout();
			// 
			// armaBrowser
			// 
			this.armaBrowser.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.armaBrowser.Location = new System.Drawing.Point(268, 133);
			this.armaBrowser.Name = "armaBrowser";
			this.armaBrowser.Size = new System.Drawing.Size(52, 23);
			this.armaBrowser.TabIndex = 1;
			this.armaBrowser.Text = "Browse";
			this.armaBrowser.UseVisualStyleBackColor = true;
			this.armaBrowser.Click += new System.EventHandler(this.ArmaBrowser_Click);
			// 
			// label2
			// 
			this.label2.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(9, 119);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(105, 13);
			this.label2.TabIndex = 5;
			this.label2.Text = "Arma Main Folder";
			// 
			// armaText
			// 
			this.armaText.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.armaText.Location = new System.Drawing.Point(12, 135);
			this.armaText.Name = "armaText";
			this.armaText.Size = new System.Drawing.Size(250, 20);
			this.armaText.TabIndex = 0;
			this.armaText.Text = global::VersionChanger.Properties.Settings.Default.pathArma;
			this.armaText.TextChanged += new System.EventHandler(this.ArmaText_Changed);
			// 
			// latestUpgrade
			// 
			this.latestUpgrade.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.latestUpgrade.Location = new System.Drawing.Point(229, 162);
			this.latestUpgrade.Name = "latestUpgrade";
			this.latestUpgrade.Size = new System.Drawing.Size(91, 33);
			this.latestUpgrade.TabIndex = 7;
			this.latestUpgrade.Text = "Latest";
			this.latestUpgrade.UseVisualStyleBackColor = true;
			this.latestUpgrade.Click += new System.EventHandler(this.LatestUpgrade_Click);
			// 
			// stableDowngrade
			// 
			this.stableDowngrade.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.stableDowngrade.Location = new System.Drawing.Point(12, 162);
			this.stableDowngrade.Name = "stableDowngrade";
			this.stableDowngrade.Size = new System.Drawing.Size(91, 33);
			this.stableDowngrade.TabIndex = 6;
			this.stableDowngrade.Text = "Stable";
			this.stableDowngrade.UseVisualStyleBackColor = true;
			this.stableDowngrade.Click += new System.EventHandler(this.StableDowngrade_Click);
			// 
			// help
			// 
			this.help.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.help.Location = new System.Drawing.Point(12, 9);
			this.help.Name = "help";
			this.help.Size = new System.Drawing.Size(308, 31);
			this.help.TabIndex = 8;
			this.help.Text = "Select the main arma 3 folder.";
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(12, 29);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(308, 31);
			this.label1.TabIndex = 9;
			this.label1.Text = "Press Stable to switch to 1.80.";
			// 
			// label3
			// 
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.Location = new System.Drawing.Point(12, 50);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(308, 31);
			this.label3.TabIndex = 10;
			this.label3.Text = "Press Latest to switch to 1.86.";
			// 
			// label4
			// 
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.Location = new System.Drawing.Point(12, 71);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(308, 48);
			this.label4.TabIndex = 11;
			this.label4.Text = "Press Latest to specify custom version file.";
			// 
			// button1
			// 
			this.button1.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.button1.Location = new System.Drawing.Point(121, 162);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(91, 33);
			this.button1.TabIndex = 12;
			this.button1.Text = "Custom";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.CustomVersion_Click);
			// 
			// settingsButton
			// 
			this.settingsButton.BackgroundImage = global::VersionChanger.Properties.Resources.settings_icon;
			this.settingsButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.settingsButton.Location = new System.Drawing.Point(309, -1);
			this.settingsButton.Name = "settingsButton";
			this.settingsButton.Size = new System.Drawing.Size(23, 23);
			this.settingsButton.TabIndex = 13;
			this.settingTooltip.SetToolTip(this.settingsButton, "Settings");
			this.settingsButton.UseVisualStyleBackColor = true;
			this.settingsButton.Click += new System.EventHandler(this.SettingsButton_Click);
			// 
			// imageList1
			// 
			this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
			this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
			this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(332, 207);
			this.Controls.Add(this.settingsButton);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.help);
			this.Controls.Add(this.stableDowngrade);
			this.Controls.Add(this.latestUpgrade);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.armaText);
			this.Controls.Add(this.armaBrowser);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.Name = "MainForm";
			this.ShowIcon = false;
			this.Text = "Arma Version Changer";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
		private System.Windows.Forms.Button armaBrowser;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox armaText;
		private System.Windows.Forms.Button latestUpgrade;
		private System.Windows.Forms.Button stableDowngrade;
		private System.Windows.Forms.Label help;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button settingsButton;
		private System.Windows.Forms.ImageList imageList1;
		private System.Windows.Forms.ToolTip settingTooltip;
	}
}

