using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using SevenZipExtractor;

namespace VersionChanger
{
	class Changer
	{


		private static readonly string _zipPath = Path.Combine(Path.GetTempPath(), @"ArmaPatch.7z");

		/// <summary>
		/// Copy the resource files to downgrade from 1.84 to 1.80
		/// </summary>
		/// <param name="outputFolder">The main arma folder</param>
		internal static void Downgrade(string outputFolder)
		{
			bool loopAgain = true;
			while (loopAgain)
			{
				try
				{
					CopyResource("VersionChanger.Resources.files_180.7z", outputFolder);

					loopAgain = false;
					MessageBox.Show("Version has been changed", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
				catch (IOException e)
				{
					if (MessageBox.Show("The file is currently in use\nDo you want to try again?\n\n" + e.Message, "Version change failed", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) == DialogResult.Cancel)
					{
						loopAgain = false;
					}
				}
				catch (UnauthorizedAccessException e)
				{
					if (MessageBox.Show("The file is set to read only\nAnd can't be replaced\n\n" + e.Message, "Version change failed", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) == DialogResult.Cancel)
					{
						loopAgain = false;
					}
				}
				catch (PlatformNotSupportedException e)
				{
					if (MessageBox.Show("The program is only able to run on windows 7+\n\n" + e.Message, "Version change failed", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) == DialogResult.Cancel)
					{
						loopAgain = false;
					}
				}
				catch (Exception e)
				{
					if (MessageBox.Show("An unknown error has occured\n\n" + e.Message, "Version change failed", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) == DialogResult.Cancel)
					{
						loopAgain = false;
					}
				}
			}
		}

		/// <summary>
		/// Copy the resource files to upgrade from 1.80 to 1.84
		/// </summary>
		/// <param name="outputFolder">The main arma folder</param>
		internal static void Upgrade(string outputFolder)
		{
			bool loopAgain = true;
			while (loopAgain)
			{
				try
				{
					CopyResource("VersionChanger.Resources.files_184.7z", outputFolder);

					loopAgain = false;
					MessageBox.Show("Version has been changed", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
				catch (IOException e)
				{
					if (MessageBox.Show("The file is currently in use\nDo you want to try again?\n\n" + e.Message, "Version change failed", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) == DialogResult.Cancel)
					{
						loopAgain = false;
					}
				}
				catch (UnauthorizedAccessException e)
				{
					if (MessageBox.Show("The file is set to read only\nAnd can't be replaced\n\n" + e.Message, "Version change failed", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) == DialogResult.Cancel)
					{
						loopAgain = false;
					}
				}
				catch (PlatformNotSupportedException e)
				{
					if (MessageBox.Show("The program is only able to run on windows 7+\n\n" + e.Message, "Version change failed", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) == DialogResult.Cancel)
					{
						loopAgain = false;
					}
				}
				catch(Exception e)
				{
					if (MessageBox.Show("An unknown error has occured\n\n" + e.Message, "Version change failed", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) == DialogResult.Cancel)
					{
						loopAgain = false;
					}
				}
			}
		}


		/// <summary>
		/// Function to copy resource by name to specified path
		/// </summary>
		/// <param name="resourceName">full resource name</param>
		/// <param name="outputFolder">obselute path including extension</param>
		private static void CopyResource(string resourceName, string outputFolder)
		{
			//Create file to read from in temporary folder
			using (var resource = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
			{

				using (var file = new FileStream(_zipPath, FileMode.Create, FileAccess.Write))
				{
					resource.CopyTo(file);
				}

			}

			//Use library to extract the content of the temporary file to the output folder
			using (ArchiveFile archiveFile = new ArchiveFile(_zipPath))
			{
				archiveFile.Extract(outputFolder, true);
			}

			//Delete temp file after completion
			File.Delete(_zipPath);

		}


		public static void LmaoGonnaFuckStuffUpIThink(string pboNameIGuess)
		{
			byte[] fileBytes = File.ReadAllBytes(pboNameIGuess);
			char[] sb = Encoding.ASCII.GetChars(fileBytes);

			string s = new string(sb);
			s = s.Replace('\0', ' ');
			Debug.WriteLine(s);
			//File.WriteAllText(outputFilename, sb.ToString());
		}
	}
}
