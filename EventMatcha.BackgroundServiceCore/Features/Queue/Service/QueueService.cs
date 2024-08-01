using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using EventMatcha.BackgroundServiceCore.Features.Queue.Interface;
using EventMatcha.BackgroundServiceCore.Features.Queue.Models;
using Newtonsoft.Json;

namespace EventMatcha.BackgroundServiceCore.Features.Queue.Service
{
    public class QueueService : IQueueService
    {
        private readonly QueueClient _queueClient;

        public QueueService(QueueClient queueClient)
        {
            _queueClient = queueClient;
        }

        public async Task EnqueueMessageAsync(Message message)
        {
            var messageText = JsonConvert.SerializeObject(message);
            await _queueClient.SendMessageAsync(messageText);
        }
        //separate read from delete below
        // read, send mail and dequeue
        public async Task<Message> DequeueMessageAsync()
        {
            QueueMessage[] retrievedMessage = await _queueClient.ReceiveMessagesAsync(maxMessages: 1);
            if (retrievedMessage.Length == 0)
            {
                return null;
            }

            var message = JsonConvert.DeserializeObject<Message>(retrievedMessage[0].MessageText);
            await _queueClient.DeleteMessageAsync(retrievedMessage[0].MessageId, retrievedMessage[0].PopReceipt);
            return message;
        }
    }
}
