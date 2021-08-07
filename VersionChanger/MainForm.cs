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


			//If there is no file selected for the stable/latest, set it to the default
			Properties.Settings.Default.pathStable = File.Exists(Properties.Settings.Default.pathStable) ? @"Resources\files_180.7z" : Properties.Settings.Default.pathStable;
			Properties.Settings.Default.pathLatest = File.Exists(Properties.Settings.Default.pathLatest) ? @"Resources\files_188.7z" : Properties.Settings.Default.pathLatest;
			Properties.Settings.Default.Save();
			//For TST
			//ProgressWindow windowTest = new ProgressWindow(new Tuple<string, string, string>(@"H:\Games\Arma 3 1.80", @"H:\Games\Arma 3 Repack test\Arma 3", @"H:\Version Changer - Delta Arma 3 - 1.80 - Test"));
			//windowTest.ShowDialog();


			//Downgrade
			//ProgressWindow window = new ProgressWindow(new Tuple<string, string, string>(@"H:\Games\Arma 3 Repack test\Arma 3", @"H:\Games\Arma 3 1.84", @"H:\Version Changer - Delta Arma 3 - 1.80"));
			//window.ShowDialog();

			//Upgrade
			//window = new ProgressWindow(new Tuple<string, string, string>(@"H:\Games\Arma 3 1.84", @"H:\Games\Arma 3 Repack test\Arma 3", @"H:\Version Changer - Delta Arma 3 - 1.88"));
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
			//Clean up before and after yourself in case of unwanted additional hash/deltas
			Changer.Cleanup();

			//Default path for latest updated (in this case files_1.86)
			ProgressWindow window = new ProgressWindow(new Tuple<string, string>(@Properties.Settings.Default.pathLatest, armaText.Text));
			window.ShowDialog();

			Changer.Cleanup();
		}

		private void StableDowngrade_Click(object sender, EventArgs e)
		{
			//Clean up before and after yourself in case of unwanted additional hash/deltas
			Changer.Cleanup();

			//Default path for latest updated (in this case files_1.80)
			ProgressWindow window = new ProgressWindow(new Tuple<string, string>(@Properties.Settings.Default.pathStable, armaText.Text));
			window.ShowDialog();

			Changer.Cleanup();
		}


		private void ArmaText_Changed(object sender, EventArgs e)
		{
			//After the path changes in the path text box, save the update for later exectutions
			Properties.Settings.Default.pathArma = armaText.Text;
			Properties.Settings.Default.Save();
		}

		private void CustomVersion_Click(object sender, EventArgs e)
		{
			var filePath = string.Empty;

			using (OpenFileDialog openFileDialog = new OpenFileDialog())
			{
				//Open the dialog at the Resources folder (from the application start location
				openFileDialog.InitialDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"Resources");
				openFileDialog.Filter = "7z files (*.7z)|*.7z|All files (*.*)|*.*";
				openFileDialog.FilterIndex = 1;
				openFileDialog.RestoreDirectory = true;

				if (openFileDialog.ShowDialog() == DialogResult.OK)
				{
					//Get the path of specified file
					filePath = openFileDialog.FileName;

					//Clean up before and after yourself in case of unwanted additional hash/deltas
					Changer.Cleanup();

					//Path will be selected from the dialog
					ProgressWindow window = new ProgressWindow(new Tuple<string, string>(filePath, armaText.Text));
					window.ShowDialog();

					Changer.Cleanup();
				}
			}
		}

		private void SettingsButton_Click(object sender, EventArgs e)
		{
			Settings settings = new Settings();
			settings.ShowDialog();
		}
	}
}
