using System.Collections.Generic;
using System.Xml.Linq;
using LINQPad.Extensibility.DataContext;

namespace SoapContextDriver
{
	public class ConnectionModel
	{
	    private readonly IConnectionInfo _connectionInfo;

	    public ConnectionModel(IConnectionInfo connectionInfo,
			IEnumerable<string> knownUris = null)
		{
			_connectionInfo = connectionInfo;
			KnownUris = knownUris ?? new string[0];
		}

	    private XElement DriverData => _connectionInfo.DriverData;

	    public bool Persist
		{
			get { return _connectionInfo.Persist; }
			set { _connectionInfo.Persist = value; }
		}

		public string Uri
		{
			get { return (string)DriverData.Element ("Uri") ?? ""; }
			set { DriverData.SetElementValue ("Uri", value); }
		}

		public string BindingName
		{
			get { return (string) DriverData.Element("Binding") ?? ""; }
			set { DriverData.SetElementValue ("Binding", value); }
		}

		public IEnumerable<string> KnownUris { get; }
	}
}
