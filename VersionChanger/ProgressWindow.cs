using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace VersionChanger
{
	public partial class ProgressWindow : Form
	{
		private BackgroundWorker bgWorker;
		private bool toggle;
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
			bgWorker.RunWorkerAsync(args);

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
			bgWorker.RunWorkerAsync(args);

		}

		public void Completed(object sender, RunWorkerCompletedEventArgs e)
		{
			Close();
		}

		internal void Clear()
		{
			progressOverall.Value = 1;
		}

		internal void ProgressEvent(object sender, ProgressChangedEventArgs e)
		{
			progressCurrent.Value = int.TryParse(e.UserState?.ToString(), out int test) ? test : 0;
			labelOverall.Text = !int.TryParse(e.UserState?.ToString(), out test) ? e.UserState?.ToString() ?? labelOverall.Text : labelOverall.Text;
			progressOverall.Value = e.ProgressPercentage;
		}

		private void CancelButton_Click(object sender, EventArgs e)
		{
			if (MessageBox.Show("Are you sure you want to cancel?\nOperations in progress will complete and exit", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				Changer.ResumeThread();
				bgWorker.CancelAsync();
			}
		}


		private void PauseButton_Click(object sender, EventArgs e)
		{
			if(!toggle)
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

	}
}
