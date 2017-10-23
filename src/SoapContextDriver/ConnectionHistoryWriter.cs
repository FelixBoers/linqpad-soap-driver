using System;
using System.IO;
using System.Linq;

namespace SoapContextDriver
{
	public class ConnectionHistoryWriter
	{
	    private readonly string _path;

		public ConnectionHistoryWriter(string path)
		{
			_path = path;
		}

		public void Append(string item)
		{
			var history = new ConnectionHistoryReader(_path).Read().ToList();
		    if (history.Any(uri => uri.Equals(item, StringComparison.OrdinalIgnoreCase)))
				return;

			using (var stream = new FileStream(_path, FileMode.OpenOrCreate, FileAccess.Write))
			{
				var writer = new StreamWriter(stream);
				foreach (var value in history) writer.WriteLine(value);
				writer.WriteLine(item);
				writer.Flush();
			}
		}
	}
}
