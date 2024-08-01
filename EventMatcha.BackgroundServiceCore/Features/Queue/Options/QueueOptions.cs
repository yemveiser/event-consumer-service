namespace EventMatcha.BackgroundServiceCore.Features.Queue.Options
{
    public class QueueOptions
    {
        public static string Section = "QueueOptions";
        public string ConnectionString { get; set; } = string.Empty;
    }
}
