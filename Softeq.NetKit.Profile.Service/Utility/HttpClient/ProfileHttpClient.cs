// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Net.Http;
using Softeq.HttpClient.Extension;
using Softeq.HttpClient.Extension.Utility;

namespace Softeq.NetKit.ProfileService.Utility.HttpClient
{
    public class ProfileHttpClient : RestHttpClientBase
    {
        public ProfileHttpClient(IHttpClientFactory httpClientFactory, string clientName, IExceptionHandler exceptionHandler)
            : base(httpClientFactory, clientName, exceptionHandler)
        {
        }

        protected override string ApiUrl { get; }
    }
}