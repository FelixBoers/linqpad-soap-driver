using System;
using System.ComponentModel;
using System.Net;

namespace SoapContextDriver
{
	public class DiscoveryCompleteEventArgs : EventArgs
	{
		public DiscoveryReference Reference { get; set; }
	}

	public class DiscoveryFailureEventArgs : EventArgs
	{
		public string Reason { get; set; }
	}

	public class DiscoveryWorker
	{
	    private class DiscoveryResult
		{
			public DiscoveryReference Reference { get; set; }
			public string FailureReason { get; set; }
		}

	    private Action<DiscoveryCompleteEventArgs> _completeHandler;
	    private Action<DiscoveryFailureEventArgs> _failureHandler;
	    private readonly BackgroundWorker _worker = new BackgroundWorker();

		public DiscoveryWorker()
		{
			_worker.DoWork += OnDoWork;
			_worker.RunWorkerCompleted += OnRunWorkerCompleted;
		}

		public bool Busy => _worker.IsBusy;

	    public DiscoveryWorker Failure(Action<DiscoveryFailureEventArgs> failureHandler)
		{
			_failureHandler = failureHandler;
			return this;
		}

		public DiscoveryWorker Complete(Action<DiscoveryCompleteEventArgs> completeHandler)
		{
			_completeHandler = completeHandler;
			return this;
		}

		public void Connect(string uri, ICredentials credentials)
		{
			_worker.RunWorkerAsync(new Discovery(uri, credentials));
		}

	    private static void OnDoWork(object sender, DoWorkEventArgs args)
		{
			var discovery = args.Argument as Discovery;
			var result = new DiscoveryResult();
			args.Result = result;

		    if (discovery == null)
		        return;

		    try
		    {
		        result.Reference = new DiscoveryCompiler(discovery, CodeProvider.Default)
		            .GenerateReference("temp");
		    }
		    catch (InvalidOperationException ioe)
		    {
		        result.FailureReason = ioe.Message;
		    }
		}

	    private void OnRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			var result = (DiscoveryResult)e.Result;
			if (result.Reference != null && _completeHandler != null)
			{
				_completeHandler(new DiscoveryCompleteEventArgs {
					Reference = result.Reference
				});
			}
			else
            {
                _failureHandler?.Invoke(new DiscoveryFailureEventArgs
                {
                    Reason = result.FailureReason
                });
            }
        }
	}
}
