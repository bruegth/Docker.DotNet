#if !NETSTANDARD1_6
using System.Net;
#endif

using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System;
using System.Security.Authentication;

namespace Docker.DotNet.X509
{
    public class CertificateCredentials : Credentials
    {
        private readonly X509Certificate2 _certificate;

        public CertificateCredentials(X509Certificate2 clientCertificate)
        {
            _certificate = clientCertificate;
        }

        public Func<HttpRequestMessage, X509Certificate2, X509Chain, SslPolicyErrors, bool> ServerCertificateValidationCallback { get; set; }

        public override HttpMessageHandler GetHandler(HttpMessageHandler innerHandler)
        {
            var handler = (HttpClientHandler)innerHandler;

            handler.ClientCertificates.Add(_certificate);
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.CheckCertificateRevocationList = false;
            handler.AllowAutoRedirect = false;
            handler.UseProxy = false;
            handler.AllowAutoRedirect = true;
            handler.MaxAutomaticRedirections = 20;
            handler.Proxy = null;
            handler.SslProtocols = SslProtocols.Tls | SslProtocols.Tls11 | SslProtocols.Tls12;
            handler.ServerCertificateCustomValidationCallback += ServerCertificateValidationCallback;
            
            return handler;
        }

        public override bool IsTlsCredentials()
        {
            return true;
        }

        public override void Dispose()
        {
        }
    }
}