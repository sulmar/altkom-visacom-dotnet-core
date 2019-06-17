using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Reflection;

namespace devices.api
{

    public interface ICommand 
    {
        string Execute();
    }

    public class SendCommand : ICommand
    {
        public string Execute()
        {
            return "Send Command";
        } 
    }

    public class PrintCommand : ICommand
    {
        public string Execute()
        {
            return "Print Command";
        }
    }

    public static class MyMiddlewareExtensions 
    {
        public static IApplicationBuilder UseMyMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<MyMiddleware>();

            return app;
        }
    }
    
    public class CommandMiddleware
    {
       private readonly RequestDelegate next;

        public CommandMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string commandname = context.Request.Path.ToString();

            commandname = commandname.TrimStart('/');

            if (commandname.EndsWith("Command"))
            {
                Type type = Type.GetType($"devices.api.{commandname}");

                if (type == null)
                    throw new NotSupportedException();
                
                // Create instance
                ICommand command = (ICommand) Activator.CreateInstance(type);

                string result = command.Execute();

                await context.Response.WriteAsync(result);

            }

           
            // await next.Invoke(context);
        }
    }

    public class MyMiddleware
    {
        private readonly RequestDelegate next;
        public MyMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            context.Response.Headers.Add("x-version", "1.1");

            await next.Invoke(context);
        }

    }

}