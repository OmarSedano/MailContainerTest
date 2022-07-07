namespace MailContainerTest.Data
{
    public class MailContainerDataStoreStrategyService : IMailContainerDataStoreStrategyService
    {
        public const string BackupStoreType = "Backup";

        public IMailContainerDataStore GetDataStore(string dataStoreType)
        {
            if (dataStoreType.Equals(BackupStoreType))
            {
                return new BackupMailContainerDataStore();
            }

            return new MailContainerDataStore();
        }
    }
}
