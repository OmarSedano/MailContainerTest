using MailContainerTest.Types;

namespace MailContainerTest.Services
{
    public class MailContainerValidationService : IMailContainerValidationService
    {
        public bool IsValid(MakeMailTransferRequest mailTransferRequest, MailContainer mailContainer)
        {
            bool isValid = true;

            switch (mailTransferRequest.MailType)
            {
                case MailType.StandardLetter:
                    if (!mailContainer.AllowedMailType.HasFlag(AllowedMailType.LargeLetter))
                    {
                        isValid = false;
                    }
                    break;

                case MailType.LargeLetter:
                    if (!mailContainer.AllowedMailType.HasFlag(AllowedMailType.LargeLetter))
                    {
                        isValid = false;
                    }
                    else if (mailContainer.Capacity < mailTransferRequest.NumberOfMailItems)
                    {
                        isValid = false;
                    }
                    break;

                case MailType.SmallParcel:

                    if (!mailContainer.AllowedMailType.HasFlag(AllowedMailType.SmallParcel))
                    {
                        isValid = false;
                    }
                    else if (mailContainer.Status != MailContainerStatus.Operational)
                    {
                        isValid = false;
                    }
                    break;

                default:
                    isValid = false;
                    break;
            }

            return isValid;
        }
    }
}
