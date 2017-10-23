using System;
using System.CodeDom.Compiler;
using System.Linq;
using System.Net;
using System.Reflection;

namespace SoapContextDriver
{
	public class ProxyBuilder
	{
	    private readonly Discovery _discovery;

		public ProxyBuilder(string url)
		{
			_discovery = new Discovery(url, CredentialCache.DefaultCredentials);
		}

		public Proxy Build(AssemblyName assemblyName, string nameSpace)
		{
			var description = _discovery.GetServices().First();
			var assembly = BuildAssembly(assemblyName, nameSpace);

			return new Proxy {
				Namespace = nameSpace,
				Description = description,
				Assembly = assembly
			};
		}

	    private Assembly BuildAssembly(AssemblyName assemblyName, string nameSpace)
		{
			var codeProvider = CodeProvider.Default;
			var reference = new DiscoveryCompiler(_discovery, codeProvider)
				.GenerateReference(nameSpace);

			var options = new CompilerParameters(
				"System.dll System.Core.dll System.Xml.dll System.Web.Services.dll".Split(),
				assemblyName.CodeBase, true);
			var results = codeProvider.CompileAssemblyFromDom(options, new[] {reference.CodeDom});

			if (results.Errors.Count > 0)
				throw new Exception("Cannot compile service proxy: " +
					results.Errors[0].ErrorText + " (line " + results.Errors[0].Line + ")");

			return results.CompiledAssembly;
		}
	}
}