using System.Collections.Generic;
using System.IO;

namespace SoapContextDriver
{
	public class ConnectionHistoryReader
	{
	    private readonly string _path;
		public ConnectionHistoryReader(string path)
		{
			_path = path;
		}

		public IEnumerable<string> Read()
		{
			if (!File.Exists(_path))
                return new string[0];

			using (var stream = new FileStream(_path, FileMode.Open))
				return ReadStream(new StreamReader(stream));
		}

	    private static IEnumerable<string> ReadStream(StreamReader reader)
		{
			var lines = new List<string>();
			while (!reader.EndOfStream)
				lines.Add(reader.ReadLine());
			return lines;
		}
	}
}