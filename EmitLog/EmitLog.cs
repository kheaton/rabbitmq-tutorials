using RabbitMQ.Client;
using System;
using System.Linq;
using System.Text;

namespace EmitLog
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() 
            { 
                HostName = "localhost", 
                UserName = "rabbitmq", 
                Password = "rabbitmq",
            };

            using(var connection = factory.CreateConnection())
            {
                using(var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchange: "logs", type: "fanout");
                    var message = GetMessage(args);

                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: "logs",
                                         routingKey: string.Empty,
                                         basicProperties: null,
                                         body: body);

                    Console.WriteLine($" [x] Send {message}");
                }
            }

            Console.WriteLine("Press [enter] to exit.");
            Console.ReadLine();
        }

        private static string GetMessage(string[] args)
        {
            return args.Any() ? string.Join(" ", args) : "info: Hello World!";
        }
    }
}
