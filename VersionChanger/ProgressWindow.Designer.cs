﻿namespace VersionChanger
{
	partial class ProgressWindow
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
			this.progressOverall = new System.Windows.Forms.ProgressBar();
			this.progressCurrent = new System.Windows.Forms.ProgressBar();
			this.labelOverall = new System.Windows.Forms.Label();
			this.labelCurrent = new System.Windows.Forms.Label();
			this.cancelButton = new System.Windows.Forms.Button();
			this.pauseButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// progressOverall
			// 
			this.progressOverall.Location = new System.Drawing.Point(11, 85);
			this.progressOverall.Name = "progressOverall";
			this.progressOverall.Size = new System.Drawing.Size(303, 23);
			this.progressOverall.TabIndex = 0;
			// 
			// progressCurrent
			// 
			this.progressCurrent.Location = new System.Drawing.Point(11, 25);
			this.progressCurrent.Name = "progressCurrent";
			this.progressCurrent.Size = new System.Drawing.Size(303, 23);
			this.progressCurrent.TabIndex = 1;
			// 
			// labelOverall
			// 
			this.labelOverall.AutoSize = true;
			this.labelOverall.Location = new System.Drawing.Point(128, 69);
			this.labelOverall.Name = "labelOverall";
			this.labelOverall.Size = new System.Drawing.Size(65, 13);
			this.labelOverall.TabIndex = 2;
			this.labelOverall.Text = "Stage 1 of 3";
			// 
			// labelCurrent
			// 
			this.labelCurrent.AutoSize = true;
			this.labelCurrent.Location = new System.Drawing.Point(128, 9);
			this.labelCurrent.Name = "labelCurrent";
			this.labelCurrent.Size = new System.Drawing.Size(81, 13);
			this.labelCurrent.TabIndex = 3;
			this.labelCurrent.Text = "Processing file: ";
			// 
			// cancelButton
			// 
			this.cancelButton.Location = new System.Drawing.Point(213, 133);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(101, 29);
			this.cancelButton.TabIndex = 4;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			// 
			// pauseButton
			// 
			this.pauseButton.Location = new System.Drawing.Point(11, 133);
			this.pauseButton.Name = "pauseButton";
			this.pauseButton.Size = new System.Drawing.Size(101, 29);
			this.pauseButton.TabIndex = 5;
			this.pauseButton.Text = "Pause";
			this.pauseButton.UseVisualStyleBackColor = true;
			// 
			// ProgressWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(327, 174);
			this.Controls.Add(this.pauseButton);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.labelCurrent);
			this.Controls.Add(this.labelOverall);
			this.Controls.Add(this.progressCurrent);
			this.Controls.Add(this.progressOverall);
			this.Name = "ProgressWindow";
			this.Text = "Progress";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ProgressBar progressOverall;
		private System.Windows.Forms.ProgressBar progressCurrent;
		private System.Windows.Forms.Label labelOverall;
		private System.Windows.Forms.Label labelCurrent;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Button pauseButton;
	}
}