﻿namespace EventMatcha.BackgroundService.Models
{
    public class MessageInQueue
    {
        public string Id { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Otp {  get; set; } = string.Empty;
    }
}
