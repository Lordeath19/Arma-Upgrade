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
			this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
			this.stableBrowser = new System.Windows.Forms.Button();
			this.armaBrowser = new System.Windows.Forms.Button();
			this.stableText = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.armaText = new System.Windows.Forms.TextBox();
			this.latestUpgrade = new System.Windows.Forms.Button();
			this.stableDowngrade = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.latestText = new System.Windows.Forms.TextBox();
			this.latestBrowser = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// stableBrowser
			// 
			this.stableBrowser.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.stableBrowser.Location = new System.Drawing.Point(267, 82);
			this.stableBrowser.Name = "stableBrowser";
			this.stableBrowser.Size = new System.Drawing.Size(52, 23);
			this.stableBrowser.TabIndex = 0;
			this.stableBrowser.Text = "Browse";
			this.stableBrowser.UseVisualStyleBackColor = true;
			this.stableBrowser.Click += new System.EventHandler(this.StableBrowser_Click);
			// 
			// armaBrowser
			// 
			this.armaBrowser.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.armaBrowser.Location = new System.Drawing.Point(267, 26);
			this.armaBrowser.Name = "armaBrowser";
			this.armaBrowser.Size = new System.Drawing.Size(52, 23);
			this.armaBrowser.TabIndex = 1;
			this.armaBrowser.Text = "Browse";
			this.armaBrowser.UseVisualStyleBackColor = true;
			this.armaBrowser.Click += new System.EventHandler(this.ArmaBrowser_Click);
			// 
			// stableText
			// 
			this.stableText.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.stableText.Location = new System.Drawing.Point(11, 84);
			this.stableText.Name = "stableText";
			this.stableText.Size = new System.Drawing.Size(250, 20);
			this.stableText.TabIndex = 2;
			// 
			// label1
			// 
			this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(8, 68);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(91, 13);
			this.label1.TabIndex = 3;
			this.label1.Text = "1.80 Patch Folder";
			// 
			// label2
			// 
			this.label2.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(8, 12);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(89, 13);
			this.label2.TabIndex = 5;
			this.label2.Text = "Arma Main Folder";
			// 
			// armaText
			// 
			this.armaText.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.armaText.Location = new System.Drawing.Point(11, 28);
			this.armaText.Name = "armaText";
			this.armaText.Size = new System.Drawing.Size(250, 20);
			this.armaText.TabIndex = 4;
			// 
			// latestUpgrade
			// 
			this.latestUpgrade.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.latestUpgrade.Location = new System.Drawing.Point(228, 180);
			this.latestUpgrade.Name = "latestUpgrade";
			this.latestUpgrade.Size = new System.Drawing.Size(91, 33);
			this.latestUpgrade.TabIndex = 6;
			this.latestUpgrade.Text = "Latest";
			this.latestUpgrade.UseVisualStyleBackColor = true;
			this.latestUpgrade.Click += new System.EventHandler(this.LatestUpgrade_Click);
			// 
			// stableDowngrade
			// 
			this.stableDowngrade.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.stableDowngrade.Location = new System.Drawing.Point(12, 180);
			this.stableDowngrade.Name = "stableDowngrade";
			this.stableDowngrade.Size = new System.Drawing.Size(91, 33);
			this.stableDowngrade.TabIndex = 7;
			this.stableDowngrade.Text = "Stable";
			this.stableDowngrade.UseVisualStyleBackColor = true;
			this.stableDowngrade.Click += new System.EventHandler(this.StableDowngrade_Click);
			// 
			// label3
			// 
			this.label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(8, 124);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(100, 13);
			this.label3.TabIndex = 10;
			this.label3.Text = "1.84 Backup Folder";
			// 
			// latestText
			// 
			this.latestText.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.latestText.Location = new System.Drawing.Point(11, 140);
			this.latestText.Name = "latestText";
			this.latestText.Size = new System.Drawing.Size(250, 20);
			this.latestText.TabIndex = 9;
			// 
			// latestBrowser
			// 
			this.latestBrowser.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.latestBrowser.Location = new System.Drawing.Point(267, 138);
			this.latestBrowser.Name = "latestBrowser";
			this.latestBrowser.Size = new System.Drawing.Size(52, 23);
			this.latestBrowser.TabIndex = 8;
			this.latestBrowser.Text = "Browse";
			this.latestBrowser.UseVisualStyleBackColor = true;
			this.latestBrowser.Click += new System.EventHandler(this.LatestBrowser_Click);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(332, 225);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.latestText);
			this.Controls.Add(this.latestBrowser);
			this.Controls.Add(this.stableDowngrade);
			this.Controls.Add(this.latestUpgrade);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.armaText);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.stableText);
			this.Controls.Add(this.armaBrowser);
			this.Controls.Add(this.stableBrowser);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.Name = "MainForm";
			this.Text = "Arma Version Changer";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
		private System.Windows.Forms.Button stableBrowser;
		private System.Windows.Forms.Button armaBrowser;
		private System.Windows.Forms.TextBox stableText;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox armaText;
		private System.Windows.Forms.Button latestUpgrade;
		private System.Windows.Forms.Button stableDowngrade;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox latestText;
		private System.Windows.Forms.Button latestBrowser;
	}
}

