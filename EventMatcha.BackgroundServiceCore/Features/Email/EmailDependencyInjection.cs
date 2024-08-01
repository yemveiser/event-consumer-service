using Microsoft.Extensions.DependencyInjection;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using FluentValidation;
using FluentValidation.Validators;
using EventMatcha.BackgroundServiceCore.Features.Email.Validators;
using EventMatcha.BackgroundServiceCore.Features.Email.Interface;
using EventMatcha.BackgroundServiceCore.Features.Email.Options;
using EventMatcha.BackgroundServiceCore.Features.Email.Models;
using EventMatcha.BackgroundServiceCore.Features.Email.Service;

namespace EventMatcha.BackgroundServiceCore.Features.Email
{
    public static class SmsDependencyInjection
    {
        public static IServiceCollection ConfigureOptions<TOption>(this IServiceCollection services,
    IConfiguration configuration) where TOption : class, new()
        {
            ArgumentNullException.ThrowIfNull(services);

            ArgumentNullException.ThrowIfNull(configuration);

            var option = new TOption();
            configuration.Bind(option);
            services.AddSingleton(option);

            return services;
        }
        private static void SetupOptions(IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureOptions<SMSLive247Options>(configuration.GetSection(SMSLive247Options.Section));
        }

        public static IServiceCollection AddEmailServices(this IServiceCollection services, IConfiguration configuration)
        {
            SetupOptions(services, configuration);
            var serviceProvier = services.BuildServiceProvider();
            var emailOptions = serviceProvier.GetRequiredService<SMSLive247Options>();

            services.AddFluentEmail(emailOptions.Username, "Example Service")
                    .AddSmtpSender(new SmtpClient(emailOptions.SmtpServer)
                    {
                        Port = emailOptions.Port,
                        Credentials = new System.Net.NetworkCredential(emailOptions.Username, emailOptions.Password)
                    });
            services.AddTransient<IEmailSenderService, EmailSenderService>();
            services.AddSingleton<IValidator<EmailMessage>, EmailMessageValidator>();

            return services;
        }
    }
}
