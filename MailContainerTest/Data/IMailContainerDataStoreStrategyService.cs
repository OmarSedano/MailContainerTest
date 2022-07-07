namespace MailContainerTest.Data
{
    public interface IMailContainerDataStoreStrategyService
    {
        public IMailContainerDataStore GetDataStore(string dataStoreType);
    }
}
