using ASPCoreEmpty;
using ASPCoreEmpty.Services;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Configure Middleware with options from FruitOptions.cs class
builder.Services.Configure<FruitOptions>(options =>
{
    options.Name = "Watermelon";
});

// Add services with dependency injection
builder.Services.AddSingleton<IResponseFormatter, HtmlResponseFormatter>();

// Building the app object with all the configuration
var app = builder.Build();

// Preparing endpoint to see Fruit Middleware in action
app.MapGet("/fruit", async (HttpContext context, IOptions<FruitOptions> FruitOptions) =>
{
    FruitOptions options = FruitOptions.Value;
    await context.Response.WriteAsync($"\nFruit Middleware options:\nName: {options.Name},\nColor: {options.Color}");
});

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

// Adding a Middleware Before the next() callback and dont call the next() callback to stop the flow of the pipeline right here
app.Use(async (context, next) =>
{
    if (context.Request.Path == "/short")
    {
        await context.Response.WriteAsync("\n\nMiddleware (lambda-based):\nShort-circuiting the http request pipeline");
    }
    else
    {
        await next();
    }
});

// Creating a Branching Middleware
app.Map("/branch", branch =>
{
    //branch.Use(async (HttpContext context, Func<Task> next) =>
    branch.Run(async (HttpContext context) =>
    {
        await context.Response.WriteAsync("\n\nMiddleware (branch):\nSeparated response from the request pipeline\nTerminal middleware cuz it is not invoking next callback from the request pipeline");
    });
    branch.Run(new Middleware().Invoke);
});

// Adding a Class-based Middleware
app.UseMiddleware<Middleware>();

// Adding Class-based Middleware with options configured
app.UseMiddleware<FruitMiddleware>();

// Adding CustomMiddleware
app.UseMiddleware<CustomMiddleware>();

// Dependency Injectin testing!!!!
//IResponseFormatter textFormatter = new TextResponseFormatter();
// Adding endpoint to test the above service
app.MapGet("/text-formatter", async (HttpContext context, IResponseFormatter formatter) => { await formatter.Format(context, "Soy un Text Formatter"); });
// Instantiating another service
//IResponseFormatter htmlFormatter = new HtmlResponseFormatter();
// Adding endpoint to test the above service
app.MapGet("/html-formatter", async (HttpContext context, IResponseFormatter formatter) =>
{
    await formatter.Format(context, "Soy un Html Formatter");
});

// Register new Endpoint calling service CustomEndpoint
app.MapGet("/custom-endpoint", CustomEndpoint.Endpoint);

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
