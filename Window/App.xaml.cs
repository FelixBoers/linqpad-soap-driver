using System.Windows;
using Driver;

namespace TestWindow
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
	    protected override void OnStartup(StartupEventArgs e)
	    {
	        base.OnStartup(e);
	        var model = new ConnectionModel(new ConnectionInfo())
	        {
	            Uri = "http://localhost:6543/Service1.svc?wsdl"
            };

	        new Dialog(model).ShowDialog();
        }
	}
}
