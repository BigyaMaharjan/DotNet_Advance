using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared.Models;
using System.Text;
using System.Text.Json;

namespace InventoryService
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "OrderQueue",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    var order = JsonSerializer.Deserialize<Order>(message);

                    Console.WriteLine($"[InventoryService] Order Received: {order.OrderId}");
                    Console.WriteLine($"Updating inventory for product: {order.ProductName}, Quantity: {order.Quantity}");
                };

                channel.BasicConsume(queue: "OrderQueue",
                                     autoAck: true,
                                     consumer: consumer);

                Console.WriteLine("Inventory Service is waiting for orders...");
                Console.ReadLine();
            }
        }
    }
}