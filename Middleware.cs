﻿namespace ASPCoreEmpty
{
    // Creating a class-based Middleware
    public class Middleware
    {
        private RequestDelegate _next;

        public Middleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Method == HttpMethods.Get && context.Request.Query["prueba"] == "true")
            {
                if (!context.Response.HasStarted)
                {
                    context.Response.ContentType = "text/plain";
                }
                await context.Response.WriteAsync("\n\nMiddleware (Class-based):\nBEFORE the main response body or '_next() callback'");
            }
            await _next(context);
        }
    }
}
