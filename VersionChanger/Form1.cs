using PBOSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PBOSharp.Objects;
using System.Diagnostics;

namespace VersionChanger
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();
			PBOSharpClient pboClient = new PBOSharpClient();
			
			//Create the pbo file 
			FileStream fileStream = new FileStream(@"C:\Users\User\Documents\GitHub\Arma-Upgrade\VersionChanger\missions_f_epa_video_new.pbo", FileMode.Create, FileAccess.Write);
			PBO pbo = pboClient.AnalyzePBO(@"C:\Users\User\Documents\GitHub\Arma-Upgrade\VersionChanger\missions_f_epa_video.pbo");

			PBOReader configReader = new PBOReader(new FileStream(@"C:\Users\User\Documents\GitHub\Arma-Upgrade\VersionChanger\config.bin", FileMode.Open, FileAccess.Read), pboClient);
			for (int i = 0; i < pbo.Files.Count; i++)
			{
				PBOFile item = pbo.Files[i];
				if (item.FileNameShort.Equals("config.bin"))
				{
					item = new PBOFile(item.FileName, item.FileNameShort, item.PackingMethod, (int)configReader.BaseStream.Length, item.Reserved, item.Timestamp, (int)configReader.BaseStream.Length, 0, configReader);
				}

				pbo.Files[i] = item;
			}
			
			
			//Write the file content
			PBOWriter writer = new PBOWriter(fileStream, pboClient);
			writer.WritePBO(pbo);

			fileStream.Close();
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
