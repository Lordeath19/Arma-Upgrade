using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

namespace VersionChanger
{
	public partial class ProgressWindow : Form
	{
		private BackgroundWorker bgWorker;
		private bool toggle;//false = resume
							//true = pause

		public ProgressWindow(Tuple<string, string> args)
		{
			InitializeComponent();
			toggle = false;

			bgWorker = new BackgroundWorker
			{
				WorkerReportsProgress = true,
				WorkerSupportsCancellation = true
			};
			bgWorker.DoWork += Changer.DoProcess;
			bgWorker.RunWorkerCompleted += Completed;
			bgWorker.ProgressChanged += ProgressEvent;
			bgWorker.RunWorkerAsync(args);//Args will include the resource file and output folder

		}


		public ProgressWindow(Tuple<string, string, string> args)
		{
			InitializeComponent();
			toggle = false;
			bgWorker = new BackgroundWorker
			{
				WorkerReportsProgress = true,
			};
			bgWorker.DoWork += Changer.GetDifferentFiles;
			bgWorker.RunWorkerCompleted += Completed;
			bgWorker.ProgressChanged += ProgressEvent;
			bgWorker.RunWorkerAsync(args);//Args will include the modified path, original path, and output delta path

		}

		//When the back ground worker completes close the form
		public void Completed(object sender, RunWorkerCompletedEventArgs e)
		{
			Close();
		}


		/// <summary>
		/// Updated everytime progress occurs 
		/// </summary>
		/// <param name="e">Contains user state which basically wears 2 hats
		/// Int - percentage in the current progress bar
		/// String - New label to put to represent current stage
		/// </param>
		internal void ProgressEvent(object sender, ProgressChangedEventArgs e)
		{
			//Try to parse the user state (the current percentage) to an int, else 0
			progressCurrent.Value = int.TryParse(e.UserState?.ToString(), out int test) ? test : 0;

			//Try to parse the user state(if it's not an int it's a string) to a string, else keep it that way
			labelOverall.Text = !int.TryParse(e.UserState?.ToString(), out test) ? e.UserState?.ToString() ?? labelOverall.Text : labelOverall.Text;

			progressOverall.Value = e.ProgressPercentage;
		}


		private void CancelButton_Click(object sender, EventArgs e)
		{
			bool toggleCopy = toggle;

			if (!toggleCopy)
			{
				Changer.PauseThread();
				toggleCopy = true;
			}
			if (MessageBox.Show("Are you sure you want to cancel?\nOperations in progress will complete and exit", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				//In case thread is paused, resume it so it can stop gracefully
				Changer.ResumeThread();

				//Send cancel request to threads
				bgWorker.CancelAsync();

				DialogResult = DialogResult.OK;
			}
			else if (toggleCopy)
			{
				Changer.ResumeThread();
			}
		}


		private void PauseButton_Click(object sender, EventArgs e)
		{
			//Flip between pause and resume
			if (!toggle)
			{
				pauseButton.Text = "Resume";
				Changer.PauseThread();
			}
			else
			{
				pauseButton.Text = "Pause";
				Changer.ResumeThread();
			}
			toggle = !toggle;
		}

		private void ProgressWindow_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (DialogResult != DialogResult.OK)
			{
				CancelButton_Click(sender, e);
				if (DialogResult != DialogResult.OK)
					e.Cancel = true;
			}
		}
	}
}
