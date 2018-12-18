using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using SevenZip;
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

		private static int _stage = 0;//Curent stage of the version change
		private static readonly int _stages = 5;//Total stages (for progress calculation
		private static readonly string _unpackPath = Path.Combine(Path.GetTempPath(), @"Arma Delta Files");//Default unpack path for the zip
		private static readonly string _armaTemp = Path.Combine(Path.GetTempPath(), @"Arma temp");//Default path for building the arma files from deltas
		private static BackgroundWorker worker;//Background worker from progress window
		private static ManualResetEvent mre;//Pause and resume from progress window



		#region Control Functions

		/// <summary>
		/// Resumes all worker threads and allows them to modify/read files
		/// </summary>
		public static void ResumeThread()
		{
			mre.Set();
		}

		/// <summary>
		/// Pauses all worker threads and prevents them from modifying/reading files
		/// </summary>
		public static void PauseThread()
		{
			mre.Reset();
		}
		#endregion

		#region Delta switch functions



		/// <summary>
		/// Create delta file from 2 files
		/// </summary>
		/// <param name="modified">File that the delta will update to</param>
		/// <param name="original">File that the delta file will use as the base</param>
		/// <param name="output">Output path for the delta file</param>
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


		/// <summary>
		/// Function to assemble a file from delta and original and output it to the "output" file
		/// </summary>
		/// <param name="delta">Path to delta file</param>
		/// <param name="original">Path to original file</param>
		/// <param name="output">Path to output file</param>
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
		#endregion

		#region Replace Functions


		/// <summary>
		/// Main process that will call all functions in order
		/// </summary>
		/// <param name="sender">the background worker</param>
		/// <param name="args">
		/// 
		/// resourceName = Name of delta file zip
		/// outputFolder = Full path of target files
		/// 
		/// </param>
		public static void DoProcess(object sender, DoWorkEventArgs args)
		{
			string resourceName = (args.Argument as Tuple<string, string>).Item1;
			string outputFolder = (args.Argument as Tuple<string, string>).Item2;
			worker = sender as BackgroundWorker;
			mre = new ManualResetEvent(true);
			_stage = 0;
			bool loopAgain = true;
			while (loopAgain)
			{
				try
				{
					switch (_stage)
					{
						//First stage - Extract the delta files from the resource file
						case 0:
							ExtractZip(resourceName);
							if (worker.CancellationPending) goto case 4;
							worker.ReportProgress(100 * ++_stage / _stages, "Verifying Arma Version");
							mre.WaitOne();
							goto case 1;

						//Second stage - Verify that all files in the target folder are ready 
						//to get combined with the delta files
						case 1:
							if (!VerifyVersion(outputFolder, out string mismatch))
							{
								MessageBox.Show("Your arma version can not be changed due to this file not having the correct data\n\n" + mismatch, "Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
								loopAgain = false;
								break;
							}
							if (worker.CancellationPending) goto case 4;
							worker.ReportProgress(100 * ++_stage / _stages, "Making update files");
							mre.WaitOne();

							goto case 2;

						//Start combining files from the target folder and the delta files
						case 2:
							MergeDifferentFiles(_unpackPath, outputFolder, _armaTemp);
							if (worker.CancellationPending) goto case 4;
							worker.ReportProgress(100 * ++_stage / _stages, "Replacing arma files");
							mre.WaitOne();

							goto case 3;

						//Replace all files in the target folder with the combined files 
						case 3:
							ReplaceAll(outputFolder);
							if (worker.CancellationPending) goto case 4;
							worker.ReportProgress(100 * ++_stage / _stages, "Cleaning up");
							mre.WaitOne();

							goto case 4;

						//Cleanup Zip unpack path (where the raw delta files are)
						//Cleanup Arma temp path (where the combined files are built)
						case 4:
							Cleanup();
							if (worker.CancellationPending)
							{
								loopAgain = false;
								break;
							}
							worker.ReportProgress(100 * ++_stage / _stages);

							goto case 5;
						case 5:
							loopAgain = false;
							MessageBox.Show("Version has been changed", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
							break;

					}

				}
				//File doesn't exist/access denied/file in use
				catch (IOException e)
				{
					if (MessageBox.Show("Could not access the file\nDo you want to try again?\n\n" + e.Message, "Version change failed", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) == DialogResult.Cancel)
					{
						loopAgain = false;
					}
				}
				//File is set to read only
				catch (UnauthorizedAccessException e)
				{
					if (MessageBox.Show("The file is set to read only\nAnd can't be replaced\n\n" + e.Message, "Version change failed", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) == DialogResult.Cancel)
					{
						loopAgain = false;
					}
				}
				//Incorrect windows version
				catch (PlatformNotSupportedException e)
				{
					if (MessageBox.Show("The program is only able to run on windows 7+\n\n" + e.Message, "Version change failed", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) == DialogResult.Cancel)
					{
						loopAgain = false;
					}
				}
				//Error happened in one of the Parallel functions (MergeDifferentFiles/GetDifferentFiles)
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
				//Generic catch all exception to prevent the program from continuing
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
		/// Create diff files for original and updated and store it output (works in conjuction with GetDifferentFiles)
		/// </summary>
		/// <param name="path1">Delta version root</param>
		/// <param name="path2">Original version root</param>
		/// <param name="output">output root path for diff files</param>
		public static void MergeDifferentFiles(string path1, string path2, string output)
		{
			long length = 0;

			//Calculate total length in bytes to set as the 100% for the progress bar
			foreach (var item in Directory.EnumerateFiles(path1, "*", SearchOption.AllDirectories).Where(x => !x.Contains(".hash")))
			{
				string outputFile = $"{item.Replace(path1, path2)}";
				length += new FileInfo(outputFile).Length;
			}

			double done = 0;
			Parallel.ForEach(Directory.EnumerateFiles(path1, "*", SearchOption.AllDirectories).Where(x => !x.Contains(".hash")),
				new ParallelOptions { MaxDegreeOfParallelism = 5 },
				(item, state) =>
				{
					var file = new FileInfo(item);

					//Start building final file from delta

					Directory.CreateDirectory(file.DirectoryName.Replace(path1, output));
					Debug.WriteLine($"Writing out file: {item.Replace(path1, output)}");

					DoDecode(item, item.Replace(path1, path2), item.Replace(path1, output));
					Debug.WriteLine($"Finsihed writing out file: {item.Replace(path1, output)}");

					//Update progress with the total bytes 
					done += (double)100 * new FileInfo(item.Replace(path1, path2)).Length / length;
					if (done >= 100)
					{
						done = 100;
					}
					worker.ReportProgress((100 * _stage / _stages) + (int)done / _stages, (int)done);
					if (worker.CancellationPending) state.Break();
					mre.WaitOne();

				});
		}


		/// <summary>
		/// Iterate through all hash files in the _unpack path and verify if they exist and match the files in the outputFolder
		/// </summary>
		/// <param name="outputFolder">folder to check the hash values with</param>
		/// <param name="mismatch">output value (will be empty if all files match) that will contain mismatch file paths</param>
		/// <returns></returns>
		private static bool VerifyVersion(string outputFolder, out string mismatch)
		{
			mismatch = "";
			bool res = true;
			long length = 0;
			double done = 0;
			foreach (var item in Directory.EnumerateFiles(_unpackPath, "*.hash", SearchOption.AllDirectories))
			{
				//Calculate total length in bytes to set as the 100% for the progress bar
				string outputFile = $"{item.Replace(_unpackPath, outputFolder).Replace(".hash", "")}";

				if (!File.Exists(outputFile))
				{
					continue;
				}

				length += new FileInfo(outputFile).Length;
			}
			foreach (var item in Directory.EnumerateFiles(_unpackPath, "*.hash", SearchOption.AllDirectories))
			{
				string outputFile = $"{item.Replace(_unpackPath, outputFolder).Replace(".hash", "")}";
				if (!File.Exists(outputFile))
				{
					mismatch += outputFile;
					res = false;
					continue;
				}

				using (var md5 = MD5.Create())
				using (var streamInput = File.OpenRead(outputFile))
				{
					//Compare between the hash calculated from the stream and the hash read from the file
					var hash = md5.ComputeHash(streamInput);
					var hashFile = File.ReadAllBytes(item);
					if (!hash.SequenceEqual(hashFile))
					{
						Debug.WriteLine($"Not Item match {outputFile}");
						mismatch += outputFile + Environment.NewLine;
						res = false;
					}
					else
					{
						Debug.WriteLine($"Item match {outputFile}");
					}
				}

				//Update progress with the total bytes 
				done += (double)100 * new FileInfo(outputFile).Length / length;
				if (done >= 100)
				{
					done = 100;
				}
				worker.ReportProgress((100 * _stage / _stages) + (int)done / _stages, (int)done);
				if (worker.CancellationPending) return true;
				mre.WaitOne();

			};
			return res;
		}

		/// <summary>
		/// Replace all the files from the outputfolder  directory with the final files from the program
		/// </summary>
		/// <param name="outputFolder">directory to replace files in</param>
		private static void ReplaceAll(string outputFolder)
		{
			long length = Directory.EnumerateFiles(_armaTemp, "*", SearchOption.AllDirectories).Sum(x => new FileInfo(x).Length);
			double done = 0;
			bool forceCancelCanceled = false;
			foreach (var item in Directory.EnumerateFiles(_armaTemp, "*", SearchOption.AllDirectories))
			{
				//Replace the file
				File.Delete(item.Replace(_armaTemp, outputFolder));
				File.Move(item, item.Replace(_armaTemp, outputFolder));

				//Update progress with the total bytes 
				done += (double)100 * new FileInfo(item.Replace(_armaTemp, outputFolder)).Length / length;
				if (done >= 100)
				{
					done = 100;
				}

				worker.ReportProgress((100 * _stage / _stages) + (int)done / _stages, (int)done);

				//Make sure you user is certain he wants to cancel as cancelling this step *will* corrupt something in the game, and will prevent him from updating again (as the hash will change)
				if (worker.CancellationPending && !forceCancelCanceled)
				{
					if (MessageBox.Show("You have started replacing files\nIf you cancel now, arma might be corrupt.\nDo you wish to force cancel?", "CAUTION", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
					{
						return;
					}
					else
					{
						forceCancelCanceled = true;
					}
				}
				mre.WaitOne();


			};
		}


		/// <summary>
		/// Remove all temp folders
		/// </summary>
		public static void Cleanup()
		{
			try
			{
				if (Directory.Exists(_armaTemp)) Directory.Delete(_armaTemp, true);
				if (Directory.Exists(_unpackPath)) Directory.Delete(_unpackPath, true);
			}
			catch (Exception) { }
		}


		/// <summary>
		/// Extract resource to _zipPath
		/// </summary>
		/// <param name="resourceName">name of zip to extract</param>
		private static void ExtractZip(string resourceName)
		{
			//Get the dll path for 7z
			var sevenZipPath = Path.Combine(
				Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
				Environment.Is64BitProcess ? "x64" : "x86", "7z.dll");

			SevenZipBase.SetLibraryPath(sevenZipPath);


			var file = new SevenZipExtractor(resourceName);
			file.FileExtractionFinished += (sender, args) =>
			{
				//args.PercentDone returns a byte for some fucking reason, try to convert to an int
				int percentDone = int.TryParse(args.PercentDone.ToString(), out int test) ? test : 0;
				if (percentDone >= 100)
				{
					percentDone = 100;
				}
				worker.ReportProgress((100 * _stage / _stages) + percentDone / _stages, percentDone);
				if (worker.CancellationPending) args.Cancel = true;

				mre.WaitOne();

			};

			//Start extracting
			file.ExtractArchive(_unpackPath);

		}
		#endregion

		#region Setup Functions

		/// <summary>
		/// Create diff files for original and updated and store it output (works in conjuction with MergeDifferentFiles
		/// </summary>
		/// <param name="path1">Modified version root</param>
		/// <param name="path2">Original version root</param>
		/// <param name="output">output root path for diff files</param>
		public static void GetDifferentFiles(object sender, DoWorkEventArgs args)
		{
			string path1 = (args.Argument as Tuple<string, string, string>).Item1;
			string path2 = (args.Argument as Tuple<string, string, string>).Item2;
			string output = (args.Argument as Tuple<string, string, string>).Item3;
			worker = sender as BackgroundWorker;


			long length = 0;
			double done = 0;
			foreach (var item in Directory.EnumerateFiles(path1, "*", SearchOption.AllDirectories))
			{
				//Calculate total byte length for progress calculation
				var file = new FileInfo(item);

				try
				{
					if (file.Length != new FileInfo(item.Replace(path1, path2)).Length)
					{
						length += file.Length;
					}
				}
				catch (IOException) { }
			}

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
								//Try to create a hash file and delta file
								DoEncode(item, item.Replace(path1, path2), item.Replace(path1, output));
								DoHash(item.Replace(path1, path2), $"{item.Replace(path1, output)}.hash");
								Debug.WriteLine($"Finsihed writing out file: {item.Replace(path1, output)}");

							}
							catch (IOException e)
							{
								//Cleanup in case of failure
								Debug.WriteLine($"Failed writing out file: {item.Replace(path1, output)}\n\n{e.Message}");
								File.Delete(item.Replace(path1, output));
								File.Delete($"{item.Replace(path1, output)}.hash");
							}
							catch (OutOfMemoryException)
							{
								//Cleanup in case of failure
								Debug.WriteLine($"Failed writing out file: {item.Replace(path1, output)} ------- MEMORY");
								File.Delete(item.Replace(path1, output));
								File.Delete($"{item.Replace(path1, output)}.hash");

							}

						}

						//Update progress
						done += (double)100 * file.Length / length;
						if (done >= 100)
						{
							done = 100;
						}
						worker.ReportProgress((int)done);
						if (worker.CancellationPending) return;
						mre.WaitOne();

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
				if (worker.CancellationPending) return;
				mre.WaitOne();

			}

		}

		#endregion
	}
}
