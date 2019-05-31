// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.NetKit.ProfileService.Utility.HttpClient
{
    public class HttpClientOptions
    {
        public HttpClientOptions(string notificationServiceApiUrl, string messagingServiceApiUrl, string paymentServiceApiUrl)
        {
            NotificationServiceApiUrl = notificationServiceApiUrl;
            MessagingServiceApiUrl = messagingServiceApiUrl;
            PaymentServiceApiUrl = paymentServiceApiUrl;
        }

        public string NotificationServiceApiUrl { get; set; }
        public string MessagingServiceApiUrl { get; set; }
        public string PaymentServiceApiUrl { get; set; }
    }
}