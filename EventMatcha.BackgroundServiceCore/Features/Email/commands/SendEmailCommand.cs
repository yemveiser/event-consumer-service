using EventMatcha.BackgroundServiceCore.Features.Email.Models;
using MediatR;

namespace EventMatcha.BackgroundServiceCore.Features.Email.commands
{
    public class SendEmailCommand : IRequest
    {
        public EmailMessage Email { get; set; }

        public SendEmailCommand(EmailMessage email)
        {
            Email = email;
        }
    }
}
