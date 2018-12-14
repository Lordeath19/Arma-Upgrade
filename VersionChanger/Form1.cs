using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using VCDiff.Decoders;
using VCDiff.Encoders;
using VCDiff.Includes;

namespace VersionChanger
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();
			//Downgrade
			//GetDifferentFiles(@"H:\Games\Arma 3 1.80", @"H:\Games\Arma 3 1.84", @"H:\Version Changer - Delta Arma 3 - 1.80");


			//Upgrade

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
