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

namespace VersionChanger
{
	public partial class Settings : Form
	{
		private static bool changed;

		public Settings()
		{
			InitializeComponent();

			textLatest.Text = Properties.Settings.Default.pathLatest;
			textStable.Text = Properties.Settings.Default.pathStable;
			changed = false;
			

		}

		private void LatestResource_Click(object sender, EventArgs e)
		{
			var filePath = string.Empty;

			using (OpenFileDialog openFileDialog = new OpenFileDialog())
			{
				//Open the dialog at the Resources folder (from the application start location
				openFileDialog.InitialDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources");
				openFileDialog.Filter = "7z files (*.7z)|*.7z|All files (*.*)|*.*";
				openFileDialog.FilterIndex = 1;
				openFileDialog.RestoreDirectory = true;

				if (openFileDialog.ShowDialog() == DialogResult.OK)
				{
					//Get the path of specified file
					textLatest.Text = openFileDialog.FileName;


				}
			}
		}


		private void StableResource_Click(object sender, EventArgs e)
		{
			var filePath = string.Empty;

			using (OpenFileDialog openFileDialog = new OpenFileDialog())
			{
				//Open the dialog at the Resources folder (from the application start location
				openFileDialog.InitialDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources");
				openFileDialog.Filter = "7z files (*.7z)|*.7z|All files (*.*)|*.*";
				openFileDialog.FilterIndex = 1;
				openFileDialog.RestoreDirectory = true;

				if (openFileDialog.ShowDialog() == DialogResult.OK)
				{
					//Get the path of specified file
					textStable.Text = openFileDialog.FileName;


				}
			}
		}


		private void SaveButton_Click(object sender, EventArgs e)
		{

			Properties.Settings.Default.pathLatest = textLatest.Text;
			Properties.Settings.Default.pathStable = textStable.Text;
			Properties.Settings.Default.Save();
			changed = false;
		}


		private void FormClosingEvent(object sender, FormClosingEventArgs e)
		{
			DialogResult result = DialogResult.No;

			if (changed)
				result = MessageBox.Show("You have unsaved changes, do you wish to save?", "Discard", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

			if (result == DialogResult.Yes)
			{
				SaveButton_Click(sender, e);
				DialogResult = DialogResult.OK;
			}

			else if (result == DialogResult.Cancel)
				e.Cancel = true;

			else
				DialogResult = DialogResult.OK;
		}

		private void TextLatest_TextChanged(object sender, EventArgs e)
		{
			changed = true;
		}

		private void TextStable_TextChanged(object sender, EventArgs e)
		{
			changed = true;
		}
	}
}
