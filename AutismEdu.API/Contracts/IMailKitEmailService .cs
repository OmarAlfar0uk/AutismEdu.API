namespace AutismEdu.API.Contracts
{
    public interface IMailKitEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
    }
}
