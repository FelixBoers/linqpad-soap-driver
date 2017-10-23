using System.CodeDom.Compiler;

namespace Driver
{
	public class CodeProvider
	{
		public static CodeDomProvider Default = CodeDomProvider.CreateProvider("CSharp");
	}
}
