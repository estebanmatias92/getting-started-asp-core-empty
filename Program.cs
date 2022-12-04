using ASPCoreEmpty;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Adding a Lamda-based Mioddleware and modifying the HttpResponse Body BEFORE the _next callback is called
app.Use(async (context, next) =>
{
    if ((context.Request.Method == HttpMethods.Get) && (context.Request.Query["prueba"] == "true"))
    {
        context.Response.ContentType = "text/plain";
        await context.Response.WriteAsync($"\n\nMiddleware (Lambda-based):\nBEFORE the main response body or '_next() callback'");
    }
    await next();
});

// Modifying HttpResponse AFTER the "_next()" callback is called
app.Use(async (context, next) =>
{
    await next();
    await context.Response.WriteAsync($"\n\nMiddleware (Lambda-based):\nAFTER the main response body or '_next() callback'\nStatus Code: {context.Response.StatusCode}");
});

// Adding a Class-based Middleware
app.UseMiddleware<Middleware>();

/*
    Main
 */
//app.MapGet("/", () => $"\nHola Matias!\nBienvenido a ASP\n\n");
var utils = new Utils();

app.MapGet("/", () => utils.Greetings());

app.Run();

/*
    End Main
 */

// Testing Custom class creation and invocation
namespace ASPCoreEmpty
{
    public class Utils
    {
        public string Greetings()
        {
            string name = "Response Body";
            return $"\n\n\n\tMain {name} or _next() callback!\n";
        }
    }
}
