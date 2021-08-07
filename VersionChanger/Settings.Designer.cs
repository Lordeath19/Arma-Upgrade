namespace VersionChanger
{
	partial class Settings
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
			this.textLatest = new System.Windows.Forms.TextBox();
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.buttonLatest = new System.Windows.Forms.Button();
			this.buttonStable = new System.Windows.Forms.Button();
			this.textStable = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// textLatest
			// 
			this.textLatest.Location = new System.Drawing.Point(12, 38);
			this.textLatest.Name = "textLatest";
			this.textLatest.Size = new System.Drawing.Size(172, 20);
			this.textLatest.TabIndex = 0;
			this.textLatest.TextChanged += new System.EventHandler(this.TextLatest_TextChanged);
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(12, 124);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 2;
			this.button1.Text = "Save";
			this.button1.UseVisualStyleBackColor = true;
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(190, 124);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(75, 23);
			this.button2.TabIndex = 3;
			this.button2.Text = "Cancel";
			this.button2.UseVisualStyleBackColor = true;
			// 
			// buttonLatest
			// 
			this.buttonLatest.Location = new System.Drawing.Point(190, 36);
			this.buttonLatest.Name = "buttonLatest";
			this.buttonLatest.Size = new System.Drawing.Size(75, 23);
			this.buttonLatest.TabIndex = 4;
			this.buttonLatest.Text = "Browse";
			this.buttonLatest.UseVisualStyleBackColor = true;
			this.buttonLatest.Click += new System.EventHandler(this.LatestResource_Click);
			// 
			// buttonStable
			// 
			this.buttonStable.Location = new System.Drawing.Point(190, 84);
			this.buttonStable.Name = "buttonStable";
			this.buttonStable.Size = new System.Drawing.Size(75, 23);
			this.buttonStable.TabIndex = 6;
			this.buttonStable.Text = "Browse";
			this.buttonStable.UseVisualStyleBackColor = true;
			this.buttonStable.Click += new System.EventHandler(this.StableResource_Click);
			// 
			// textStable
			// 
			this.textStable.Location = new System.Drawing.Point(12, 86);
			this.textStable.Name = "textStable";
			this.textStable.Size = new System.Drawing.Size(172, 20);
			this.textStable.TabIndex = 5;
			this.textStable.TextChanged += new System.EventHandler(this.TextStable_TextChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(13, 19);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(96, 13);
			this.label1.TabIndex = 7;
			this.label1.Text = "Latest resource file";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(13, 70);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(97, 13);
			this.label2.TabIndex = 8;
			this.label2.Text = "Stable resource file";
			// 
			// Settings
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(277, 159);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.buttonStable);
			this.Controls.Add(this.textStable);
			this.Controls.Add(this.buttonLatest);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.textLatest);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.Name = "Settings";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = "Settings";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormClosingEvent);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox textLatest;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button buttonLatest;
		private System.Windows.Forms.Button buttonStable;
		private System.Windows.Forms.TextBox textStable;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
	}
}