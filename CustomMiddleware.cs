using ASPCoreEmpty.Services;

namespace ASPCoreEmpty
{
    public class CustomMiddleware
    {
        private IResponseFormatter _formatter;
        private RequestDelegate _next;

        public CustomMiddleware(RequestDelegate next, IResponseFormatter formatter)
        {
            _next = next;
            _formatter = formatter;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path == "/middleware-formatter")
            {
                await _formatter.Format(context, "Soy CustomMiddleware: Html Formatter");
            }
            else
            {
                await _next(context);
            }
        }
    }
}
