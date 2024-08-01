using EventMatcha.BackgroundServiceCore.Features.SMS.Models;

namespace EventMatcha.BackgroundServiceCore.Features.SMS.Interface
{
    public interface ISmsService
    {
        Task SendSmsAsync(string toPhoneNumber, string message);
    }
}
