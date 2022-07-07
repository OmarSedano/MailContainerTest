using MailContainerTest.Data;
using MailContainerTest.Types;
using MailContainerTest.Configuration;

namespace MailContainerTest.Services
{
    public class MailTransferService : IMailTransferService
    {
        private readonly IMailContainerDataStoreStrategyService _dataStoreStrategyService;
        private readonly IMailConfiguration _mailConfiguration;
        private readonly IMailContainerValidationService _mailContainerValidationService;

        public MailTransferService(IMailContainerDataStoreStrategyService dataStoreStrategyService, IMailConfiguration mailConfiguration, IMailContainerValidationService mailContainerValidationService)
        {
            _dataStoreStrategyService = dataStoreStrategyService;
            _mailConfiguration = mailConfiguration;
            _mailContainerValidationService = mailContainerValidationService;
        }

        public MakeMailTransferResult MakeMailTransfer(MakeMailTransferRequest request)
        {
            var mailContainerDataStore = _dataStoreStrategyService.GetDataStore(_mailConfiguration.DataStoreType);
            var mailContainer = mailContainerDataStore.GetMailContainer(request.SourceMailContainerNumber);
            if (mailContainer == null)
            {
                return MakeMailTransferResult.Failed($"MailContainer is null for mailContainerNumber '{request.SourceMailContainerNumber}'");
            }

            var isValidMail = _mailContainerValidationService.IsValid(request, mailContainer);
            if (!isValidMail)
            {
                return MakeMailTransferResult.Failed($"Mail request is not valid");
            }

            mailContainer.Capacity -= request.NumberOfMailItems;
            mailContainerDataStore.UpdateMailContainer(mailContainer);

            return MakeMailTransferResult.Succeeded();
        }
    }
}
