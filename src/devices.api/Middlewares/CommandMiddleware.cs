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
        private readonly int copies;

        public PrintCommand(int copies)
        {
            this.copies = copies;
        }

        public string Execute()
        {
            return $"Print Command copies {copies}";
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

                object[] args = {10};
                
                // Create instance
                ICommand command = (ICommand) Activator.CreateInstance(type, args);

                var parameters = context.Request.Query;

                foreach(var parameter in parameters)
                {
                    
                }


                string result = command.Execute();

                await context.Response.WriteAsync(result);

            }

           
            // await next.Invoke(context);
        }
    }





}