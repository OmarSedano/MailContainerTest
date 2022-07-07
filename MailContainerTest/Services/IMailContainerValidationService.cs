using MailContainerTest.Types;

namespace MailContainerTest.Services;

public interface IMailContainerValidationService
{
    bool IsValid(MakeMailTransferRequest mailTransferRequest, MailContainer mailContainer);
}