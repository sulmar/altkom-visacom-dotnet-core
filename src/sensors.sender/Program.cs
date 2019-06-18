using System;
using Microsoft.AspNetCore.SignalR.Client;
using System.Threading.Tasks;
using production.models;
using Bogus;

namespace sensors.sender
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine("Signal-R Sender!");

            const string url = "http://localhost:5000/hubs/sensors";

            // dotnet add package Microsoft.AspNetCore.SignalR.Client

            HubConnection connection = new HubConnectionBuilder()
                .WithUrl(url)
                .Build();

            System.Console.WriteLine("Connecting...");
            await connection.StartAsync();
            System.Console.WriteLine("Connected.");


            while(true)
            {
                 // dotnet add package Bogus
                var faker = new Faker<Measure>()
                    .RuleFor(p => p.DeviceId, "temp-001")
                    .RuleFor(p => p.Temperature, f=>f.Random.Float(20, 40));
                    
                Measure measure = faker.Generate();

               
                // Measure measure = new Measure
                // {
                //     DeviceId = "temp-001",
                //     Temperature = 26.04f
                // };

                await connection.SendAsync("MeasureAdded", measure);
                System.Console.WriteLine($"Sent {measure.Temperature}");

                await Task.Delay(TimeSpan.FromSeconds(1));
            }

            System.Console.WriteLine("Press enter key to exit.");
            Console.ReadLine();

            Console.ResetColor();
        }
    }
}
