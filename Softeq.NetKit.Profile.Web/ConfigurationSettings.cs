namespace Softeq.NetKit.Profile.Web
{
    public static class ConfigurationSettings
    {
        public static readonly string SQLConnectionStringName = "DefaultConnection";
        public static readonly string IdentityApiSecret = "Identity:ApiSecret";
        public static readonly string IdentityAuthority = "Identity:Authority";
        public static readonly string AzureStorageContentStorageHost = "AzureStorage:ContentStorageHost";
        public static readonly string AzureStorageUserPhotoContainerName = "AzureStorage:UserPhotoContainerName";
        public static readonly string AzureStorageUserPhotoSize = "AzureStorage:UserPhotoSize";
        public static readonly string AzureStorageTempContainerName = "AzureStorage:TempContainerName";
        public static readonly string AzureStorageContainerAccessTokenTimeToLive = "AzureStorage:ContainerAccessToken:TimeToLive";
        public static readonly string AzureStorageConnectionString = "AzureStorage:ConnectionString";
        public static readonly string AzureServiceBusConnectionString = "AzureServiceBus:ConnectionString";
        public static readonly string AzureServiceBusTopicClientName = "AzureServiceBus:TopicClientName";
        public static readonly string AzureServiceBusSubscriptionClientName = "AzureServiceBus:SubscriptionClientName";
        public static readonly string AzureServiceBusTimeToLive = "AzureServiceBus:TimeToLive";
        public static readonly string AzureServiceBusEventPublisherId = "AzureServiceBus:EventPublisherId";
        public static readonly string DependenciesPaymentServiceApiUrl = "Dependencies:PaymentService:ApiUrl";
        public static readonly string DependenciesNotificationServiceApiUrl = "Dependencies:NotificationService:ApiUrl";
        public static readonly string DependenciesMessagingServiceApiUrl = "Dependencies:MessagingService:ApiUrl";
    }
}
