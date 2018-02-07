using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
//Sync first!
namespace EventReceiver
{
    internal static class Program
    {
        private const string ServiceBusConnectionStringForReceive = "Endpoint=sb://centrictraining.servicebus.windows.net/;SharedAccessKeyName=ListenAndSend;SharedAccessKey=x9rRnabgHu5PdVyabjNa5Lod6lWglpDgg24Fp9Way3I=";
        private const string ServiceBusTopic = "topic0";
        private const string ServiceBusSubscription = "subscription0";

        private static SubscriptionClient _subscriptionClient;

        static void Main()
        {
            Initialize();
            while (true)
            {
                Console.WriteLine("Hit 1 to start listening.");
                Console.WriteLine("Hit 2 to stop listening.");
                Console.WriteLine("Hit escape to quit.");
                var key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.D1:
                        Initialize();

                        break;
                    case ConsoleKey.D2:
                        Uninitialize();

                        break;
                    case ConsoleKey.Escape:
                        return;
                    default:

                        break;
                }
            }
        }

        private static void Initialize()
        {
            if (_subscriptionClient != null)
            {
                return;
            }

            var client = new SubscriptionClient(ServiceBusConnectionStringForReceive, ServiceBusTopic, ServiceBusSubscription, ReceiveMode.ReceiveAndDelete);
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 1,
                AutoComplete = false
            };

            client.RegisterMessageHandler(ProcessMessageAsync, messageHandlerOptions);
            _subscriptionClient = client;


        }

        private static void Uninitialize()
        {
            _subscriptionClient?.CloseAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            _subscriptionClient = null;
        }

        private static Task ProcessMessageAsync(Message message, CancellationToken cancellationTokenc)
        {
            string payload = System.Text.Encoding.Unicode.GetString(message.Body);
            //TODO: handle payload
            Console.WriteLine($"Received message, id:{message.MessageId}, body:{payload}");
            return Task.CompletedTask;
        }

        private static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs args)
        {
            Console.Error.WriteLine($"Received error:{args.Exception.Message}");
            return Task.CompletedTask;
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
}
