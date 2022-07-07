namespace MailContainerTest.Types
{
    public class MakeMailTransferResult
    {
        //I extended a little bit the response

        public bool Success { get; set; }
        public string Error { get; set; }

        public static MakeMailTransferResult Failed(string error)
        {
            return new MakeMailTransferResult() { Success = false, Error = error};
        }

        public static MakeMailTransferResult Succeeded()
        {
            return new MakeMailTransferResult() { Success = true };
        }
    }
}
