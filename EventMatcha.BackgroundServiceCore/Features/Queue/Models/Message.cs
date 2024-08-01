namespace EventMatcha.BackgroundServiceCore.Features.Queue.Models
{
    public class Message
    {
        public string Id { get; set; } = string.Empty;
        public string Content { get; set; } =  string.Empty;
        public DateTimeOffset EnqueuedTime { get; set; }
    }
}
