namespace playground.Features.Email
{
    public interface IEmailService
    {
        Task SendEmailAsync(string emailAddress, string subject, string message);

        Task SendEmailFluentAsync(string emailAddress, string subject);
    }
}
