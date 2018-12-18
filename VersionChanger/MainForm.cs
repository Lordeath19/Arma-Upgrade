using System;
using System.IO;
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

		private void CustomVersion_Click(object sender, EventArgs e)
		{
			var filePath = string.Empty;

			using (OpenFileDialog openFileDialog = new OpenFileDialog())
			{
				openFileDialog.InitialDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"Resources");
				openFileDialog.Filter = "7z files (*.7z)|*.7z|All files (*.*)|*.*";
				openFileDialog.FilterIndex = 1;
				openFileDialog.RestoreDirectory = true;

				if (openFileDialog.ShowDialog() == DialogResult.OK)
				{
					//Get the path of specified file
					filePath = openFileDialog.FileName;

					Changer.Cleanup();

					ProgressWindow window = new ProgressWindow(new Tuple<string, string>(filePath, armaText.Text));
					window.ShowDialog();

					Changer.Cleanup();
				}
			}
		}
	}
}
