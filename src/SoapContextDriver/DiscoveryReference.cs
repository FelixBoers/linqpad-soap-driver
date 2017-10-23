using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;

namespace SoapContextDriver
{
	public class DiscoveryReference
	{
		public CodeCompileUnit CodeDom { get; set; }
		public CodeDomProvider CodeProvider { get; set; }
		public List<DiscoveryBinding> Bindings { get; }

	    private readonly List<string> _errors = new List<string>();
	    private readonly List<string> _warnings = new List<string>();

		public DiscoveryReference()
		{
			Bindings = new List<DiscoveryBinding>();
		}

		public bool HasErrors => Errors.Any();
	    public bool HasWarnings => Warnings.Any();

	    public IEnumerable<string> Errors => _errors;

	    public IEnumerable<string> Warnings => _warnings;

	    internal void Error(string message)
		{
			_errors.Add(message);
		}

		internal void Warn(string message)
		{
			_warnings.Add(message);
		}
	}
}