using FluentValidation;
using EventMatcha.BackgroundServiceCore.Features.Email.Models;

namespace EventMatcha.BackgroundServiceCore.Features.Email.Validators
{
    public class EmailMessageValidator : AbstractValidator<EmailMessage>
    {
        public EmailMessageValidator()
        {
            RuleFor(x => x.Subject).NotEmpty();
            RuleFor(x => x.Body).NotEmpty();

            RuleFor(x => x.Sender).NotNull();
            RuleFor(x => x.Sender.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Sender.Name).NotEmpty();

            RuleFor(x => x.Receiver).NotNull();
            RuleFor(x => x.Receiver.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Receiver.Name).NotEmpty();
        }
    }
}
