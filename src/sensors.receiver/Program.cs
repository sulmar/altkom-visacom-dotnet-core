using System;
using Microsoft.AspNetCore.SignalR.Client;
using System.Threading.Tasks;
using production.models;

namespace sensors.receiver
{
    class Program
    {
        static async Task Main(string[] args)
        {
          
            Console.BackgroundColor = ConsoleColor.Green;
            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine("Signal-R Receiver!");

            const string url = "http://localhost:5000/hubs/sensors";

            // dotnet add package Microsoft.AspNetCore.SignalR.Client

            HubConnection connection = new HubConnectionBuilder()
                .WithUrl(url)
                .Build();

            System.Console.WriteLine("Connecting...");
            await connection.StartAsync();
            System.Console.WriteLine("Connected.");

            connection.On<Measure>("Measured", 
                measure => System.Console.WriteLine($"Received {measure.Temperature}"));

            System.Console.WriteLine("Press enter key to exit.");
            Console.ReadLine();

            Console.ResetColor();

        }
    }
}
