using Azure.Storage.Queues;
using EventMatcha.BackgroundServiceCore.Features.Queue.Interface;
using EventMatcha.BackgroundServiceCore.Features.Queue.Service;
using EventMatcha.BackgroundServiceCore.Features.Queue.Options;
using Microsoft.Extensions.DependencyInjection;
using EventMatcha.BackgroundServiceCore.Features.Queue.Constant;
using EventMatcha.BackgroundServiceCore.Features.Email.Options;
using Microsoft.Extensions.Configuration;

namespace EventMatcha.BackgroundServiceCore.Features.Queue
{
    public static class QueueDependencyInjection
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
            services.ConfigureOptions<QueueOptions>(configuration.GetSection(QueueOptions.Section));
        }

        public static IServiceCollection AddQueueServices(this IServiceCollection services, IConfiguration configuration )
        {
            SetupOptions(services, configuration);
            var serviceProvider = services.BuildServiceProvider();  
            var queueOptions = serviceProvider.GetRequiredService<QueueOptions>();  
            var queueClient = new QueueClient(queueOptions.ConnectionString, QueueConstants.QueueName);
            queueClient.CreateIfNotExists();

            services.AddSingleton(queueClient);
            services.AddTransient<IQueueService, QueueService>();


            return services;
        }
    }
}
