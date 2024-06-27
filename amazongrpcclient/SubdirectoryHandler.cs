using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace amazongrpcclient
{
    /// <summary>
    /// A delegating handler that will add a subdirectory to the URI of gRPC requests.
    /// </summary>
    public class SubdirectoryHandler : DelegatingHandler
    {
        private readonly string _subdirectory;

        public SubdirectoryHandler(HttpMessageHandler innerHandler, string subdirectory) : base(innerHandler)
        {
            _subdirectory = subdirectory;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var url = $"{request.RequestUri.Scheme}://{request.RequestUri.Host}{_subdirectory}{request.RequestUri.AbsolutePath}";
            request.RequestUri = new Uri(url, UriKind.Absolute);
            return base.SendAsync(request, cancellationToken);
        }
    }
}
