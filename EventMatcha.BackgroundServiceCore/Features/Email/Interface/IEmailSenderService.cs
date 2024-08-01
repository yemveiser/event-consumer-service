using EventMatcha.BackgroundServiceCore.Features.Email.Models;

namespace EventMatcha.BackgroundServiceCore.Features.Email.Interface
{
    public interface IEmailSenderService
    {
        Task SendAsync(EmailMessage email);
    }
}
