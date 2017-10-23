using System.Collections.Generic;
using LINQPad.Extensibility.DataContext;

namespace SoapContextDriver
{
	public class Schema
	{
		public string TypeName { get; set; }
		public List<ExplorerItem> Entities { get; set; }
	}
}