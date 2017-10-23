using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using LINQPad.Extensibility.DataContext;

namespace SoapContextDriver
{
	public class SoapContextDriver : DynamicDataContextDriver
	{
		public override string Name => "WCF Data Services (SOAP)";

	    public override string Author => "github.com/flex87";

	    public override string GetConnectionDescription(IConnectionInfo connectionInfo)
		{
			var model = new ConnectionModel(connectionInfo);
			var uri = new Uri(model.Uri);
			var host = uri.Port == 80
				? uri.Host
				: string.Concat(uri.Host, ':', uri.Port);
			return $"{model.BindingName} ({host})";
		}

		public override bool AreRepositoriesEquivalent (IConnectionInfo r1, IConnectionInfo r2)
		{
			var m1 = new ConnectionModel(r1);
			var m2 = new ConnectionModel(r2);
			return Equals(m1.Uri, m2.Uri) && Equals(m1.BindingName, m2.BindingName);
		}

		public override IEnumerable<string> GetAssembliesToAdd (IConnectionInfo info)
		{
			// We need the following assembly for compiliation and autocompletion:
			return new [] { "System.Web.Services.dll" };
		}

		public override IEnumerable<string> GetNamespacesToAdd (IConnectionInfo info)
		{
			// Import the commonly used namespaces as a courtesy to the user:
			return new [] { "System.Web.Services" };
		}

		public override bool ShowConnectionDialog(IConnectionInfo connectionInfo, bool isNewConnection)
		{
			var model = new ConnectionModel(connectionInfo,
				new ConnectionHistoryReader(GetHistoryPath()).Read());
			return new Dialog(model).ShowDialog() == true;
		}

		public override List<ExplorerItem> GetSchemaAndBuildAssembly(IConnectionInfo connectionInfo, AssemblyName assemblyToBuild, ref string nameSpace, ref string typeName)
		{
			var model = new ConnectionModel(connectionInfo);
			var proxy = new ProxyBuilder(model.Uri)
				.Build(assemblyToBuild, nameSpace);

			var schema = new SchemaBuilder()
				.Build(proxy.Description, model.BindingName, proxy.Assembly);

			nameSpace = proxy.Namespace;
			typeName = schema.TypeName;

			new ConnectionHistoryWriter(GetHistoryPath())
				.Append(model.Uri);

			return schema.Entities;
		}

	    private string GetHistoryPath()
		{
			return Path.Combine(GetDriverFolder(), "uri.history");
		}
	}
}
