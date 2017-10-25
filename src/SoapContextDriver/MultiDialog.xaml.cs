using System.Windows;

namespace SoapContextDriver
{
	public partial class MultiDialog
	{
	    public MultiDialog()
	    {
	        InitializeComponent();
        }

        public MultiDialog(MultiDialogViewModel viewModel)
            : this()
		{
			DataContext = viewModel;
		}

	    protected MultiDialogViewModel Model => DataContext as MultiDialogViewModel;
        
	    private void MultiDialog_OnLoaded(object sender, RoutedEventArgs e)
	    {
	        throw new System.NotImplementedException();
	    }
	}
}
