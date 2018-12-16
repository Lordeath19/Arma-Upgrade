using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
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
	/// Stage 2: Check hash values to determine whether to change the version
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
			if (MessageBox.Show("Are you sure you want to downgrade to version 1.80?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
				DoProcess(@"Resources\files_180.7z", outputFolder);

		}

		/// <summary>
		/// Copy the resource files to upgrade from 1.80 to 1.84
		/// </summary>
		/// <param name="outputFolder">The main arma folder</param>
		internal static void Upgrade(string outputFolder)
		{
			if (MessageBox.Show("Are you sure you want to upgrade to version 1.86?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
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
							if (!VerifyVersion(outputFolder, out string mismatch))
							{
								MessageBox.Show("Your arma version can not be changed due to this file not having the correct data\n\n" + mismatch, "Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
								loopAgain = false;
								break;
							}
							stage++;
							goto case 3;
						case 3:
							MergeDifferentFiles(_unpackPath, outputFolder, _armaTemp);
							stage++;
							goto case 4;
						case 4:
							ReplaceAll(_armaTemp, outputFolder);
							stage++;
							goto case 5;
						case 5:
							Cleanup();
							goto case 6;
						case 6:
							loopAgain = false;
							MessageBox.Show("Version has been changed", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
							break;

					}
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

		private static bool VerifyVersion(string outputFolder, out string mismatch)
		{
			mismatch = "";
			bool res = true;
			foreach (var item in Directory.EnumerateFiles(_unpackPath, "*.hash", SearchOption.AllDirectories))
			{
				string outputFile = $"{item.Replace(_unpackPath, outputFolder).Replace(".hash", "")}";
				using (var md5 = MD5.Create())
				using (var streamInput = File.OpenRead(outputFile))
				{
					var hash = md5.ComputeHash(streamInput);
					var hashFile = File.ReadAllBytes(item);
					if (!hash.SequenceEqual(hashFile))
					{
						Debug.WriteLine($"Not Item match {outputFile}");
						mismatch += item;
						res = false;
					}
					else
					{
						Debug.WriteLine($"Item match {outputFile}");
					}

				}
			};
			return res;
		}

		private static void ReplaceAll(string armaTemp, string outputFolder)
		{
			Parallel.ForEach(Directory.EnumerateFiles(armaTemp, "*", SearchOption.AllDirectories),
				new ParallelOptions { MaxDegreeOfParallelism = 5 },
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
				VCDiffResult result = coder.Encode(true, true); //encodes with no checksum and not interleaved
				if (result != VCDiffResult.SUCCESS)
				{
					Debug.WriteLine($"Something got fucked up, how to check please help");
					//error was not able to encode properly
				}

				if (outputS.Length <= 5)
				{
					outputS.Close();
					File.Delete(output);
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
			Parallel.ForEach(Directory.EnumerateFiles(path1, "*", SearchOption.AllDirectories).Where(x=>!x.Contains(".hash")),
				new ParallelOptions { MaxDegreeOfParallelism = 5 },
				(item) =>
				{
					var file = new FileInfo(item);

					Directory.CreateDirectory(file.DirectoryName.Replace(path1, output));
					Debug.WriteLine($"Writing out file: {item.Replace(path1, output)}");

					DoDecode(item, item.Replace(path1, path2), item.Replace(path1, output));
					Debug.WriteLine($"Finsihed writing out file: {item.Replace(path1, output)}");

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
								DoHash(item.Replace(path1, path2), $"{item.Replace(path1, output)}.hash");
								Debug.WriteLine($"Finsihed writing out file: {item.Replace(path1, output)}");

							}
							catch (IOException e)
							{
								Debug.WriteLine($"Failed writing out file: {item.Replace(path1, output)}\n\n{e.Message}");
							}
							catch (OutOfMemoryException)
							{
								Debug.WriteLine($"Failed writing out file: {item.Replace(path1, output)} ------- MEMORY");
								File.Delete(item.Replace(path1, output));

							}

						}
					}
					catch (IOException) { }
				});
		}

		/// <summary>
		/// Hash the file and output the result
		/// </summary>
		/// <param name="fileToHash">Full path to file</param>
		/// <param name="outputHashFile">Full path to output file</param>
		private static void DoHash(string fileToHash, string outputHashFile)
		{
			using (var md5 = MD5.Create())
			using (var stream = File.OpenRead(fileToHash))
			using (var streamOut = File.OpenWrite(outputHashFile))
			{
				var hash = md5.ComputeHash(stream);
				streamOut.Write(hash, 0, hash.Length);
			}

		}
	}
}
