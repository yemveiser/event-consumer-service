namespace EventMatcha.BackgroundService.Interfaces
{
    public interface IEmailExecutorService 
    {
        Task ProcessQueueMessagesAsync();
    }
}
