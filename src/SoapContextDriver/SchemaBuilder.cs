using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Services;
using System.Web.Services.Description;
using LINQPad.Extensibility.DataContext;

namespace SoapContextDriver
{
	public class SchemaBuilder
	{
		public Schema Build(ServiceDescription description, string bindingName, Assembly assembly)
		{
			var binding = GetSoapBinding(description, bindingName);
			var serviceType = GetServiceType(binding, assembly);
			return new Schema {
				TypeName = serviceType.Name,
				Entities = BuildEntities(serviceType, binding)
			};
		}

	    private static Type GetServiceType(NamedItem soapBinding, Assembly assembly)
	    {
	        return (from type in assembly.GetTypes()
	            let bindingAttribute = type.GetCustomAttributes(typeof(WebServiceBindingAttribute), false)
	                .Cast<WebServiceBindingAttribute>()
	                .SingleOrDefault()
	            where bindingAttribute != null && bindingAttribute.Name == soapBinding.Name
	            select type).FirstOrDefault();

	        //var serviceTypes = new List<Type>();

			//// FIXME: Best way to match the service/binding to the implementing type
			////

			//foreach (var type in assembly.GetTypes())
			//{
			//    var bindingAttributes = type.GetCustomAttributes(typeof (WebServiceBindingAttribute), false);
			//    if (bindingAttributes.Length != 0) serviceTypes.Add(type);
			//}

			//var name = soapBinding.Type.Name;
			//return string.IsNullOrEmpty(name)
			//    ? new Type[0]
			//    : serviceTypes.Where(t => t.Name == name || t.Name.EndsWith(name));
	    }

	    private static List<ExplorerItem> BuildEntities(Type serviceType, Binding soapBinding)
	    {
	        return GetSoapOperations(soapBinding).Select(operation => CreateExplorerOperation(serviceType, operation)).ToList();
	    }

	    private static ExplorerItem CreateExplorerOperation(Type serviceType, OperationBinding operation)
		{
			var method = serviceType.GetMethod(operation.Name);
		    if (method == null)
                throw new InvalidOperationException($"Method {operation.Name} not found.");
		    var parameters = method.GetParameters();
		    var description = operation.DocumentationElement?.InnerText ?? "";
		    var item = new ExplorerItem(operation.Name, ExplorerItemKind.QueryableObject, ExplorerIcon.StoredProc)
		    {
		        ToolTipText = description,
		        DragText = GetMethodCallString(method.Name, from p in parameters select p.Name),
		        Children = (
		            from p in parameters
		            select new ExplorerItem(p.Name, ExplorerItemKind.Parameter, ExplorerIcon.Parameter)
		        ).ToList()
		    };

		    return item;
		}

	    private static Binding GetSoapBinding(ServiceDescription description, string bindingName)
		{
			var soapBindings = description.GetSoapBindings()
				.Where(binding => binding.Name == bindingName)
				.OrderByDescending(binding => binding.Type.Name);
			return soapBindings.FirstOrDefault();
		}

	    private static IEnumerable<OperationBinding> GetSoapOperations(Binding soapBinding)
		{
            return soapBinding.Operations.Cast<OperationBinding>()
				.OrderBy(o => o.Name).ToList();
		}

	    private static string GetMethodCallString(string methodName, IEnumerable<string> parameterNames)
		{
			return string.Concat(methodName, "(", string.Join(", ", parameterNames), ")");
		}
	}
}
