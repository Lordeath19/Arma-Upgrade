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
		

		private void ArmaBrowser_Click(object sender, EventArgs e)
		{
			if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
			{
				armaText.Text = folderBrowserDialog1.SelectedPath;

			}
		}

		private void LatestUpgrade_Click(object sender, EventArgs e)
		{
			Changer.Upgrade(armaText.Text);
		}

		private void StableDowngrade_Click(object sender, EventArgs e)
		{
			Changer.Downgrade(armaText.Text);
		}


		private void ArmaText_Changed(object sender, EventArgs e)
		{
			Properties.Settings.Default.pathArma = armaText.Text;
			Properties.Settings.Default.Save();
		}
	}
}
