using System.CodeDom.Compiler;

namespace SoapContextDriver
{
	public class CodeProvider
	{
		public static CodeDomProvider Default = CodeDomProvider.CreateProvider("CSharp");
	}
}
