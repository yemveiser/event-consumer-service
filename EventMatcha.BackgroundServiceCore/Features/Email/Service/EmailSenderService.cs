using FluentEmail.Core;
using FluentValidation;
using FluentValidation.Results;
using EventMatcha.BackgroundServiceCore.Features.Email.Interface;
using EventMatcha.BackgroundServiceCore.Features.Email.Models;

namespace EventMatcha.BackgroundServiceCore.Features.Email.Service
{
    public class EmailSenderService : IEmailSenderService
    {
        private readonly IFluentEmail _fluentEmail;
        private readonly IValidator<EmailMessage> _validator;

        public EmailSenderService(IFluentEmail fluentEmail, IValidator<EmailMessage> validator)
        {
            _fluentEmail = fluentEmail;
            _validator = validator;
        }

        public async Task SendAsync(EmailMessage email)
        {
            //  var validationResult = await _validator.ValidateAsync((IValidationContext)email);

            ValidationResult validationResult = await _validator.ValidateAsync(email);


            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    Console.WriteLine($"Validation error: {error.ErrorMessage}");
                }
            }
            else
            {
                await _fluentEmail
                              .To(email?.Receiver?.Email)
                              .Subject(email?.Subject)
                              .Body(email?.Body, true)
                              .SetFrom(email?.Sender?.Email, email?.Sender?.Name)
                              .SendAsync();
            }


        }


    }
}
