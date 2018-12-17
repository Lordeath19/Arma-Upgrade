using System;
using System.Windows.Forms;

namespace VersionChanger
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();
			//Downgrade
			//ProgressWindow window = new ProgressWindow(new Tuple<string, string, string>(@"H:\Games\Arma 3 1.80", @"H:\Games\Arma 3 1.84", @"H:\Version Changer - Delta Arma 3 - 1.80"));
			//window.ShowDialog();

			//Upgrade
			//window = new ProgressWindow(new Tuple<string, string, string>(@"H:\Games\Arma 3 1.84", @"H:\Games\Arma 3 1.80", @"H:\Version Changer - Delta Arma 3 - 1.86"));
			//window.ShowDialog();
		

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
			Changer.Cleanup();

			ProgressWindow window = new ProgressWindow(new Tuple<string, string>(@"Resources\files_186.7z", armaText.Text));
			window.ShowDialog();

			Changer.Cleanup();
		}

		private void StableDowngrade_Click(object sender, EventArgs e)
		{
			Changer.Cleanup();

			ProgressWindow window = new ProgressWindow(new Tuple<string, string>(@"Resources\files_180.7z", armaText.Text));
			window.ShowDialog();

			Changer.Cleanup();
		}


		private void ArmaText_Changed(object sender, EventArgs e)
		{
			Properties.Settings.Default.pathArma = armaText.Text;
			Properties.Settings.Default.Save();
		}





	}
}
