namespace MINT.EShop.Core.Interfaces
{
    public interface IEmailSender
    {
        Task SendVerificationCodeAsync(string targetEmail, string code);
    }
}
