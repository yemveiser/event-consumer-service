using EventMatcha.BackgroundServiceCore.Features.Queue.Models;

namespace EventMatcha.BackgroundServiceCore.Features.Queue.Interface
{
    public interface IQueueService
    {
        Task EnqueueMessageAsync(Message message);
        Task<Message> DequeueMessageAsync();
    }
}
