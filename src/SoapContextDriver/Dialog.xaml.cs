using System;
using System.Linq;
using System.Net;
using System.Windows;

namespace SoapContextDriver
{
	public partial class Dialog
	{
	    private int _page;
	    private readonly ConnectionLogger _logger;
	    private readonly DiscoveryWorker _worker;

		public Dialog(ConnectionModel model)
		{
			DataContext = model;
			InitializeComponent();
			_worker = new DiscoveryWorker();
			_logger = new ConnectionLogger(LogBox);
		}

	    private ConnectionModel Model => DataContext as ConnectionModel;

	    private void Connect_Click(object sender, EventArgs e)
		{
			if (Uri.IsWellFormedUriString(Model.Uri, UriKind.Absolute))
			{
				SetVisiblePage(1);
				Connect();
			}
		}

	    private void Select_Click(object sender, EventArgs e)
		{
			SetVisiblePage(2);
		}

	    private void Back_Click(object sender, EventArgs e)
		{
			SetVisiblePage(_page - 1);
		}

	    private void Finish_Click(object sender, EventArgs e)
		{
			// Close window
			DialogResult = true;
		}

	    private void Connect()
		{
			SelectButton.IsEnabled = false;
			RestartButton.IsEnabled = false;
			Progress.IsIndeterminate = true;

			if (!_worker.Busy)
			{
				_logger.Clear();
				_logger.Write("Connecting to " + Model.Uri);

				_worker
					.Failure(Discovery_Failure)
					.Complete(Discovery_Connect)
					.Connect(Model.Uri, CredentialCache.DefaultCredentials);
			}
		}

	    private void Discovery_Failure(DiscoveryFailureEventArgs e)
		{
			_logger.Error(e.Reason);
			RestartButton.IsEnabled = true;
		}

	    private void Discovery_Connect(DiscoveryCompleteEventArgs e)
		{
			var reference = e.Reference;
			foreach (var warning in reference.Warnings) _logger.Warn(warning);		
			foreach (var error in reference.Errors) _logger.Error(error);

			if (reference.HasErrors)
				_logger.Write("Cannot create a connection.");
			else
			{
				_logger.Write("Click \"Next\" to continue.");
				SelectButton.IsEnabled = true;

				var reset = string.IsNullOrEmpty(Model.BindingName) ||
					!reference.Bindings.Any(b => b.Name == Model.BindingName);
				if (reset)
				{
					Model.BindingName = reference.Bindings.Count > 0
						? reference.Bindings[0].Name : "";
				}

				BindingBox.ItemsSource = reference.Bindings.Select(b => b.Name);
				BindingBox.SelectedItem = Model.BindingName;
			}

			RestartButton.IsEnabled = true;
			Progress.IsIndeterminate = false;
			LogBox.ScrollToEnd();

			if (!reference.HasErrors && !reference.HasWarnings)
			{
				SetVisiblePage(2);
			}		
		}

	    private Visibility GetPageVisibility(int which)
		{
		    return which == _page ? Visibility.Visible : Visibility.Hidden;
		}

	    private void SetVisiblePage(int p)
		{
			if (p < 0 || p > 2) return;
			_page = p;
			Page1.Visibility = GetPageVisibility(0);
			Page2.Visibility = GetPageVisibility(1);
			Page3.Visibility = GetPageVisibility(2);
		}
	}
}
