using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using SevenZipExtractor;
using VCDiff.Decoders;
using VCDiff.Encoders;
using VCDiff.Includes;

namespace VersionChanger
{
	/// <summary>
	/// This class is responsible for updating/downgrading arma using this process:
	/// Stage 1: Unzip resource file to temp path
	/// Stage 2: Combine delta files with arma 3 files and output to another temp folder
	/// Stage 3: Replace all the files with the files generated in stage 4
	/// Stage 4: Delete all the generated files
	/// </summary>
	class Changer
	{

		private static int stage = 1;
		private static readonly string _unpackPath = Path.Combine(Path.GetTempPath(), @"Arma Delta Files");
		private static readonly string _armaTemp = Path.Combine(Path.GetTempPath(), @"Arma temp");



		/// <summary>
		/// Copy the resource files to downgrade from 1.84 to 1.80
		/// </summary>
		/// <param name="outputFolder">The main arma folder</param>
		internal static void Downgrade(string outputFolder)
		{
			DoProcess(@"Resources\files_180.7z", outputFolder);

		}

		/// <summary>
		/// Copy the resource files to upgrade from 1.80 to 1.84
		/// </summary>
		/// <param name="outputFolder">The main arma folder</param>
		internal static void Upgrade(string outputFolder)
		{
			DoProcess(@"Resources\files_186.7z", outputFolder);
		}


		private static void DoProcess(string resourceName, string outputFolder)
		{
			bool loopAgain = true;
			while (loopAgain)
			{
				try
				{
					switch (stage)
					{
						case 1:
							ExtractZip(resourceName);
							stage++;
							goto case 2;
						case 2:
							MergeDifferentFiles(_unpackPath, outputFolder, _armaTemp);
							stage++;
							goto case 3;
						case 3:
							ReplaceAll(_armaTemp, outputFolder);
							stage++;
							goto case 4;
						case 4:
							Cleanup();
							break;
					}
					loopAgain = false;
					MessageBox.Show("Version has been changed", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
				catch (IOException e)
				{
					if (MessageBox.Show("Could not access the file\nDo you want to try again?\n\n" + e.Message, "Version change failed", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) == DialogResult.Cancel)
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

				catch (AggregateException es)
				{
					string totalExceptions = "";
					foreach (var e in es.InnerExceptions)
					{
						totalExceptions += $"{e.Message}\n\n";
					}
					if (MessageBox.Show("An unknown error has occured\n\n" + totalExceptions, "Version change failed", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) == DialogResult.Cancel)
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

		private static void ReplaceAll(string armaTemp, string outputFolder)
		{
			Parallel.ForEach(Directory.EnumerateFiles(armaTemp, "*", SearchOption.AllDirectories),
				new ParallelOptions { MaxDegreeOfParallelism = 4 },
				(item) =>
				{
					File.Replace(item, item.Replace(armaTemp, outputFolder), null);
				});
		}

		private static void Cleanup()
		{
			Directory.Delete(_armaTemp, true);
			Directory.Delete(_unpackPath, true);
		}

		/// <summary>
		/// Extract resource to _zipPath
		/// </summary>
		/// <param name="resourceName">name of zip to extract</param>
		private static void ExtractZip(string resourceName)
		{
			using (ArchiveFile archiveFile = new ArchiveFile(resourceName))
			{
				archiveFile.Extract(_unpackPath, true);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="modified"></param>
		/// <param name="original"></param>
		/// <param name="output"></param>
		private static void DoEncode(string modified, string original, string output)
		{
			using (FileStream outputS = new FileStream(output, FileMode.CreateNew, FileAccess.Write))
			using (FileStream dictS = new FileStream(original, FileMode.Open, FileAccess.Read))
			using (FileStream targetS = new FileStream(modified, FileMode.Open, FileAccess.Read))
			{
				VCCoder coder = new VCCoder(dictS, targetS, outputS);
				VCDiffResult result = coder.Encode(); //encodes with no checksum and not interleaved
				if (result != VCDiffResult.SUCCESS)
				{
					//error was not able to encode properly
				}
			}
		}

		private static void DoDecode(string delta, string original, string output)
		{
			using (FileStream outputS = new FileStream(output, FileMode.Create, FileAccess.Write))
			using (FileStream dictS = new FileStream(original, FileMode.Open, FileAccess.Read))
			using (FileStream targetS = new FileStream(delta, FileMode.Open, FileAccess.Read))
			{
				VCDecoder decoder = new VCDecoder(dictS, targetS, outputS);

				//You must call decoder.Start() first. The header of the delta file must be available before calling decoder.Start()

				VCDiffResult result = decoder.Start();

				if (result != VCDiffResult.SUCCESS)
				{
					//error abort
				}

				result = decoder.Decode(out long bytesWritten);

				if (result != VCDiffResult.SUCCESS)
				{
					//error decoding
				}

				//if success bytesWritten will contain the number of bytes that were decoded
			}
		}


		/// <summary>
		/// Create diff files for original and updated and store it output (works in conjuction with GetDifferentFiles)
		/// </summary>
		/// <param name="path1">Delta version root</param>
		/// <param name="path2">Original version root</param>
		/// <param name="output">output root path for diff files</param>
		public static void MergeDifferentFiles(string path1, string path2, string output)
		{
			Parallel.ForEach(Directory.EnumerateFiles(path1, "*", SearchOption.AllDirectories),
				new ParallelOptions { MaxDegreeOfParallelism = 1 },
				(item) =>
				{
					var file = new FileInfo(item);

					Directory.CreateDirectory(file.DirectoryName.Replace(path1, output));
					Debug.WriteLine($"Writing out file: {item.Replace(path1, output)}");

					DoDecode(item, item.Replace(path1, path2), item.Replace(path1, output));
					Debug.WriteLine($"Finsihed writing out file: {item.Replace(path1, output)}");

					try
					{
						if (File.Exists(item.Replace(path1, output))
						&& new FileInfo(item.Replace(path1, output)).Length == 0)
						{
							File.Delete(item.Replace(path1, output));
						}
					}
					catch (Exception)
					{ }
				});
		}


		/// <summary>
		/// Create diff files for original and updated and store it output (works in conjuction with MergeDifferentFiles
		/// </summary>
		/// <param name="path1">Modified version root</param>
		/// <param name="path2">Original version root</param>
		/// <param name="output">output root path for diff files</param>
		public static void GetDifferentFiles(string path1, string path2, string output)
		{
			Parallel.ForEach(Directory.EnumerateFiles(path1, "*", SearchOption.AllDirectories),
				new ParallelOptions { MaxDegreeOfParallelism = 5 },
				(item) =>
				{
					var file = new FileInfo(item);
					try
					{
						if (file.Length != new FileInfo(item.Replace(path1, path2)).Length)
						{
							Directory.CreateDirectory(file.DirectoryName.Replace(path1, output));
							Debug.WriteLine($"Writing out file: {item.Replace(path1, output)}");
							try
							{
								DoEncode(item, item.Replace(path1, path2), item.Replace(path1, output));
								Debug.WriteLine($"Finsihed writing out file: {item.Replace(path1, output)}");

							}
							catch (IOException)
							{
								Debug.WriteLine($"Failed writing out file: {item.Replace(path1, output)}");
							}
							catch (OutOfMemoryException)
							{
								Debug.WriteLine($"Failed writing out file: {item.Replace(path1, output)} ------- MEMORY");
							}

						}
					}
					catch (IOException) { }

					try
					{
						if (File.Exists(item.Replace(path1, output))
						&& new FileInfo(item.Replace(path1, output)).Length == 0)
						{
							File.Delete(item.Replace(path1, output));
						}
					}
					catch (Exception)
					{ }
				});
		}


	}
}
