using Azure.Storage.Queues.Models;
using Azure.Storage.Queues;
using EventMatcha.BackgroundService.Interfaces;
using EventMatcha.BackgroundServiceCore.Features.Email.Models;
using EventMatcha.BackgroundServiceCore.Features.Queue.Interface;
using EventMatcha.BackgroundServiceCore.MessageTemplates;
using Newtonsoft.Json;
using EventMatcha.BackgroundService.Models;
using EventMatcha.BackgroundServiceCore.Features.Email.Interface;
using EventMatcha.BackgroundServiceCore.Features.Email.Options;

namespace EventMatcha.BackgroundService.Services
{
    public class EmailExecutorService : IEmailExecutorService
    {
        private readonly IQueueService _queueService;
        private readonly IEmailSenderService _emailSenderService;
        private readonly QueueClient _queueClient;
        private readonly ILogger<EmailExecutorService> _logger;
        private readonly SMSLive247Options _emailOptions;
        public EmailExecutorService(
            IQueueService queueService,
            IEmailSenderService emailSenderService,
            QueueClient queueClient,
            ILogger<EmailExecutorService> logger,
            SMSLive247Options emailOptions)
        {
            _emailSenderService = emailSenderService;
            _queueService = queueService;
            _queueClient=queueClient;
            _logger = logger;
            _emailOptions=emailOptions;

        }

        public void WriteLog(string LogMessage)
        {
            _logger.LogInformation($"{DateTime.Now:yyyy-mm-dd hh:mm:ss tt} {LogMessage}");
        }
        public async Task ProcessQueueMessagesAsync()
        {
           
            try
            {
                QueueMessage[] retrievedMessages = await _queueClient.ReceiveMessagesAsync();
                if (retrievedMessages.Length == 0)
                {
                    WriteLog("There is no message in the queue");

                }
                WriteLog("There are "  + retrievedMessages.Length +" in the azure queue");
                MessageTemplateServices template = new MessageTemplateServices();
                string senderEmail = _emailOptions.SenderEmail;
                string senderName = _emailOptions.SenderName;
                var emailppp = _emailOptions.Port;
                string subject = "User Registration";

                var SenderDetails = new Contact { Email = senderEmail, Name = senderName };

                foreach (var newMessage in retrievedMessages)
                {

                    var queueMessage = JsonConvert.DeserializeObject<MessageInQueue>(newMessage.MessageText);
                    var ReceiverDetails = new Contact { Email = queueMessage.Email, Name = queueMessage.Name };

                    string receiverEmail = queueMessage.Email;
                    string receiverName = queueMessage.Name;
                    string otp = queueMessage.Otp;
                    DateTime otpExpiredOn = DateTime.Now.AddMinutes(30);
                    string emailContent = "";
                    if (string.IsNullOrEmpty(otp))
                    {
                         emailContent = template.GenerateUserRegistrationConfirmationEmail(
                             senderEmail, 
                             senderName,
                             receiverEmail,
                             receiverName,
                             subject);

                    }
                    else
                    {
                         emailContent = template.GenerateUserRegistrationEmail(
                             senderEmail, 
                             senderName, 
                             receiverEmail, 
                             receiverName, 
                             otp,
                             otpExpiredOn,
                             subject);

                    }

                    var emailMessage = new EmailMessage
                    {
                        Subject = subject,
                        Body = emailContent,
                        Sender = SenderDetails,
                        Receiver = ReceiverDetails,
                    };

                    await _emailSenderService.SendAsync(emailMessage);
                    WriteLog("Email sent successfully to  " + queueMessage.Email);
                    await _queueClient.DeleteMessageAsync(newMessage.MessageId, newMessage.PopReceipt);

                }        

            }
            catch (ArgumentException ex)
            {
                WriteLog("inside try and catch " + ex.Message);

            }
            catch (Exception ex)
            {
            
                WriteLog("other exceptions {ex.Message} ");
            }
            
           }
    }
}
