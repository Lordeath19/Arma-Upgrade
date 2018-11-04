using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VersionChanger
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();
		}

		private void LatestBrowser_Click(object sender, EventArgs e)
		{
			if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
			{
				latestText.Text = folderBrowserDialog1.SelectedPath;
			}
		}

		private void StableBrowser_Click(object sender, EventArgs e)
		{
			if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
			{
				stableText.Text = folderBrowserDialog1.SelectedPath;
			}

		}

		private void ArmaBrowser_Click(object sender, EventArgs e)
		{
			if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
			{
				armaText.Text = folderBrowserDialog1.SelectedPath;
			}
		}

		private void LatestUpgrade_Click(object sender, EventArgs e)
		{
			if (Properties.Resources.hashLatest != HashGen.GetHash(latestText.Text))
			{
				MessageBox.Show("The backup folder is wrong, either some files were changed or files were removed", "Incorrect Backup", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			Changer.Change(latestText.Text, armaText.Text);
		}

		private void StableDowngrade_Click(object sender, EventArgs e)
		{
			if (Properties.Resources.hashLatest != HashGen.GetHash(latestText.Text))
			{
				MessageBox.Show("The backup folder is wrong, either some files were changed or files were removed", "Incorrect Backup", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			Changer.Change(stableText.Text, armaText.Text);
		}
	}
}
