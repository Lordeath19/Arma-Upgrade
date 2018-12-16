using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace VersionChanger
{
	public partial class ProgressWindow : Form
	{
		private BackgroundWorker bgWorker;

		public ProgressWindow(Tuple<string, string> args)
		{
			InitializeComponent();
			bgWorker = new BackgroundWorker
			{
				WorkerReportsProgress = true,
			};
			bgWorker.DoWork += Changer.DoProcess;
			bgWorker.ProgressChanged += ProgressEvent;

			bgWorker.RunWorkerAsync(args);

		}

		internal void Clear()
		{
			progressOverall.Value = 1;
		}

		internal void ProgressEvent(object sender, ProgressChangedEventArgs e)
		{
			progressCurrent.Value = (e.UserState as int?) ?? 0;
			progressOverall.Value = e.ProgressPercentage;
		}
	}
}
