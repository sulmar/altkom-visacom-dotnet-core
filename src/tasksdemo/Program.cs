using System;
using System.Threading;
using System.Threading.Tasks;

namespace tasksdemo
{
    class Program
    {
      //  static void Main(string[] args) => MainAsync(args).GetAwaiter().Result;

        static async Task Main(string[] args)
        {
            System.Console.WriteLine($"#{Thread.CurrentThread.ManagedThreadId} Hello Olsztyn .NET Core!!!");
        


            // Send(".NET Core");

            // Task t1 = new Task(() => Send(".NET Core"));
            // t1.Start();

            // Task<decimal> t1 = Task.Run(() => Calculate(100, 1.23m));
            // decimal result = t1.Result;
            // Task.Run(()=> Send($".NET Core {result}"));

            // decimal totalAmount = Calculate(100, 1.23m);
            // Send($"Total amount: {totalAmount}");

            // Task.Run(() => Calculate(100, 1.23m))
            //     .ContinueWith(t => Send($"Total amount: {t.Result}"));

            // CalculateAsync(100, 1.23m)
            //     .ContinueWith(t => SendAsync($"Total amount: {t.Result}"));

            decimal amount =  await CalculateAsync(100, 1.23m);
            await SendAsync($"Total amount: {amount}");

            System.Console.WriteLine($"#{Thread.CurrentThread.ManagedThreadId} Press any key to exit.");
            Console.ReadKey();

        
        }

        public static Task<decimal> CalculateAsync(decimal amount, decimal tax)
        {
            return Task.Run(()=>Calculate(amount, tax));
        }

        public static Task SendAsync(string message)
        {
            return Task.Run(()=>Send(message));
        }
        public static void Send(string message)
        {
            System.Console.WriteLine($"#{Thread.CurrentThread.ManagedThreadId} Sending {message}...");
            Thread.Sleep(TimeSpan.FromSeconds(5));
            System.Console.WriteLine($"#{Thread.CurrentThread.ManagedThreadId} Sent {message}.");
        }

        public static decimal Calculate(decimal amount, decimal tax)
        {
            System.Console.WriteLine($"#{Thread.CurrentThread.ManagedThreadId} Calculating...");
            Thread.Sleep(TimeSpan.FromSeconds(5));

            var result = amount * tax;
            System.Console.WriteLine($"#{Thread.CurrentThread.ManagedThreadId} Calculated.");

             return result;

        }
    }
}
