using System.Configuration;

namespace MailContainerTest.Configuration
{
    public class MailConfiguration : IMailConfiguration
    {
        public string DataStoreType  => ConfigurationManager.AppSettings["DataStoreType"] ?? throw  new ArgumentNullException("DataStoreType setting is empty");
    }
}
