using System.Windows.Controls;

namespace SoapContextDriver
{
	public class ConnectionLogger
	{
	    private readonly TextBox _box;

		public ConnectionLogger(TextBox box)
		{
			_box = box;
		}

		public void Clear()
		{
			_box.Text = "";
		}

		public void Error(string message)
		{
			Write("Error> ", message);
		}

		public void Warn(string message)
		{
			Write("Warning> ", message);
		}

		public void Write(string message)
		{
			Write("> ", message);
		}

	    private void Write(string tag, string message)
		{
			_box.Text += string.Concat(tag, message, "\n");
		}
	}
}