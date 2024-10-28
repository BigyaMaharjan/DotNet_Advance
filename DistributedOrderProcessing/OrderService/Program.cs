using RabbitMQ.Client;
using Shared.Models;
using System.Text;
using System.Text.Json;

namespace OrderService
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

                while (true)
                {
                    Console.WriteLine("Enter 'exit' to stop the application.");
                    Console.Write("Enter product name: ");
                    string productName = Console.ReadLine();

                    if (productName.Equals("exit", StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine("Exiting application...");
                        break;
                    }

                    Console.Write("Enter quantity: ");
                    if (!int.TryParse(Console.ReadLine(), out int quantity))
                    {
                        Console.WriteLine("Invalid quantity. Please enter a valid number.");
                        continue; 
                    }

                    var order = new Order
                    {
                        OrderId = Guid.NewGuid(),
                        ProductName = productName,
                        Quantity = quantity
                    };

                    var message = JsonSerializer.Serialize(order);
                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: "",
                                         routingKey: "OrderQueue",
                                         basicProperties: null,
                                         body: body);

                    Console.WriteLine($"[x] Order Placed: {order.OrderId}, Product: {order.ProductName}, Quantity: {order.Quantity}");
                }
            }
        }

    }
}