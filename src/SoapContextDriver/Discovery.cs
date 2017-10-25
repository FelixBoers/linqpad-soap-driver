using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Web.Services.Description;
using System.Web.Services.Discovery;
using System.Windows;

namespace SoapContextDriver
{
    public interface IDiscovery
    {
        IEnumerable<ServiceDescription> GetServices();
        DiscoveryClientDocumentCollection GetDocuments();
    }

    public class Discovery : IDiscovery
    {
	    private readonly string _uri;
	    private readonly ICredentials _credentials;
	    private DiscoveryClientDocumentCollection _documents;

        // Handle SSL cert errors (see http://stackoverflow.com/questions/777607/the-remote-certificate-is-invalid-according-to-the-validation-procedure-ple)
        static Discovery()
        {
            ServicePointManager.ServerCertificateValidationCallback = OnServerCertificateValidationCallback;
        }

		public Discovery(string uri, ICredentials credentials)
		{
			_uri = uri;
			_credentials = credentials;
		}

		public IEnumerable<ServiceDescription> GetServices()
		{
			return (
				from DictionaryEntry entry in GetDocuments()
				let description = entry.Value as ServiceDescription
				where description != null && description.Services.Count > 0
				select description
			);
		}

		public DiscoveryClientDocumentCollection GetDocuments()
		{
			return _documents ?? (_documents = DiscoverDocuments());
		}

	    private static bool OnServerCertificateValidationCallback(
            object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
	    {
	        if (sslPolicyErrors == SslPolicyErrors.None)
	            return true;

	        if ((sslPolicyErrors & SslPolicyErrors.RemoteCertificateChainErrors) <= 0)
	            return false;

	        var certStatus = chain.ChainStatus.Last();
	        var messageBoxText = $"{certStatus.StatusInformation.TrimEnd().TrimEnd('.')} (Code: {certStatus.Status}).{Environment.NewLine}{Environment.NewLine}Continue adding the connection?";
	        var messageBoxResult = MessageBox.Show(messageBoxText, "Certificate Validation Error", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes;
	        return messageBoxResult;
	    }

	    private DiscoveryClientDocumentCollection DiscoverDocuments()
		{
			var protocol = new DiscoveryClientProtocol {
				AllowAutoRedirect = true,
                
				Credentials = _credentials ?? CredentialCache.DefaultCredentials
			};

			protocol.DiscoverAny(_uri);
			protocol.ResolveAll();

			return protocol.Documents;			
		}
	}
}
