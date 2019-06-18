using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Reflection;

namespace devices.api
{

    
    public static class MyMiddlewareExtensions 
    {
        public static IApplicationBuilder UseMyMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<MyMiddleware>();

            return app;
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