using System;
using Microsoft.Azure.ServiceBus;

namespace ConsoleApp1
{
    internal static class Program
    {

        private const string ServiceBusConnectionStringForSend = "Endpoint=sb://centrictraining.servicebus.windows.net/;SharedAccessKeyName=Send;SharedAccessKey=MD5qth4+3KgkaNYgVpIqsqoBwIEXdXpq7/r19Z7OzIM=;";
        private const string ServiceBusTopic = "TestTopic";

        private static TopicClient _topicClient;

        static void Main()
        {
            Initialize();
            while (true)
            {
                Console.WriteLine("Hit any key to send a message.");
                Console.WriteLine("Hit escape to quit.");
                var key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.Escape:
                        return;
                    default:
                        SendMessage();
                        break;
                }
            }
        }

        private static void Initialize()
        {
            var client = new TopicClient(ServiceBusConnectionStringForSend, ServiceBusTopic);
            _topicClient = client;
        }


        private static void SendMessage()
        {
            //TODO: create meaningful payload
            string message = "todo, send customer";
            var brokeredMessage = new Message(System.Text.Encoding.Unicode.GetBytes(message));
            _topicClient.SendAsync(brokeredMessage).ConfigureAwait(false).GetAwaiter().GetResult();
        }
    }

    internal class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Geospatial Location { get; set; }
    }

    internal class Geospatial
    {
        public double Lat { get; set; }
        public double Lon { get; set; }
    }

    internal class Event
    {
        public Guid Id { get; set; }
        public int Sequence { get; set; }
        public DateTimeOffset Timestamp { get; set; } = DateTimeOffset.UtcNow;
    }
}
