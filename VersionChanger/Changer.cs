using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using PBOSharp;
using PBOSharp.Objects;
using SevenZipExtractor;

namespace VersionChanger
{
	class Changer
	{


		private static readonly string _zipPath = Path.Combine(Path.GetTempPath(), @"ArmaPatch.7z");

		/// <summary>
		/// Changes all pbos inside rootOut with the modifies files inside rootIn
		/// </summary>
		/// <param name="rootOut">Root path of the output folder, where the pbo files are stored</param>
		/// <param name="rootIn">Root path of the input folder, where the modified files are stored</param>
		internal static void ChangePBOs(string rootOut, string rootIn)
		{
			PBOSharpClient client = new PBOSharpClient();
			var everything = Directory.EnumerateFiles(rootIn, "*", SearchOption.AllDirectories);
			foreach (var item in everything)
			{
				var pbo = client.AnalyzePBO(item);//Input pbo
				var pboNew = client.AnalyzePBO(item.Replace(rootIn, rootOut));//Original pbo
				string newPath = pboNew.LongName.Replace(".pbo", "_new.pbo");//Output pbo file
				using (FileStream fs = new FileStream(newPath, FileMode.Create, FileAccess.Write))
				{
					for (int i = 0; i < pboNew.Files.Count; i++)
					{
						var currFile = pboNew.Files[i];//Original file inside pbo
						var pboFile = pbo.Files.Find(x => x.FileName.Equals(currFile.FileName));//Input file inside pbo
																								//Check if pbo file exists in the input pbo
						if (pboFile != null)
						{
							var configReader = pboFile.Reader;
							currFile = new PBOFile(currFile.FileName, currFile.FileNameShort, currFile.PackingMethod, pboFile.OriginalSize, pboFile.Reserved, currFile.Timestamp, pboFile.DataSize, pboFile.Offset, configReader);
						}

						pboNew.Files[i] = currFile;
					}

					PBOWriter writer = new PBOWriter(fs, client);
					writer.WritePBO(pboNew);
					writer.Close();
				}

				pboNew.Reader.Close();
				File.Replace(newPath, pboNew.LongName, null);
				File.Delete(newPath);
				/*

				//Create the pbo file 

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
				*/

			}
		}

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

		/// <summary>
		/// One time function, extracts all config.bin files from the pbos, this later gets inserted to the pbo in question to update/downgrade it
		/// </summary>
		/// <param name="armaPath">path of arma to grab the pbos from</param>
		/// <param name="outPath">output folder that will act as the root to all the created pbos containing config.bin files</param>
		public static void ExtractAllConfigs(string armaPath, string outPath)
		{
			PBOSharpClient client = new PBOSharpClient();
			var everything = Directory.EnumerateFiles(armaPath, "*.pbo", SearchOption.AllDirectories);
			foreach (var item in everything)
			{
				var pbo = client.AnalyzePBO(item);
				foreach (var pboFile in pbo.Files)
				{
					if (!pboFile.FileName.EndsWith("config.bin"))
						continue;

					string newPath = pbo.LongName.Replace(".pbo", "").Replace(armaPath, outPath);
					Directory.CreateDirectory(Path.Combine(newPath, pboFile.FileName.Replace(pboFile.FileNameShort, "")));
					using (FileStream fs = new FileStream(Path.Combine(newPath, pboFile.FileName), FileMode.Create, FileAccess.Write))
					{
						BinaryReader br = pboFile.Reader;
						br.BaseStream.Seek(pboFile.Offset, SeekOrigin.Begin);
						byte[] readStuff = br.ReadBytes(pboFile.DataSize);
						fs.Write(readStuff, 0, readStuff.Length);
					}

				}
				string pboFolder = item.Replace(armaPath, outPath).Replace(".pbo", "");
				client.PackPBO(pboFolder);
				try { Directory.Delete(pboFolder, true); }
				catch (DirectoryNotFoundException) { }
			}
		}

		public static void GetDifferentFiles(string path1, string path2, string output)
		{
			foreach (var item in Directory.EnumerateFiles(path1, "*", SearchOption.AllDirectories))
			{
				var file = new FileInfo(item);
				try
				{
					if (file.Length != new FileInfo(item.Replace(path1, path2)).Length)
					{
						Directory.CreateDirectory(file.DirectoryName.Replace(path1, output));
						File.Copy(item, item.Replace(path1, output));
						string newFileName = item.Replace(path1, output).Replace(Path.GetFileNameWithoutExtension(file.FullName), $"{Path.GetFileNameWithoutExtension(file.FullName)}(2)");
						File.Copy(item.Replace(path1, path2), newFileName);
					}
				}
				catch (IOException) { }
			}
		}
	}
}
