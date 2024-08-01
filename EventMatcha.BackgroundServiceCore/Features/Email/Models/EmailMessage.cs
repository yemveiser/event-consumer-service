namespace EventMatcha.BackgroundServiceCore.Features.Email.Models
{
    public class EmailMessage
    {
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public Contact? Sender { get; set; }
        public Contact? Receiver { get; set; }
    }

    public class Contact
    {
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}
