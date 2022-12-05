using ASPCoreEmpty.Services;

namespace ASPCoreEmpty
{
    public class CustomMiddleware2
    {
        private IResponseFormatter _formatter;
        private RequestDelegate _next;

        public CustomMiddleware2(RequestDelegate next, IResponseFormatter formatter)
        {
            _next = next;
            _formatter = formatter;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path == "/middleware-formatter-2")
            {
                await _formatter.Format(context, "Soy CustomMiddleware2: Html Formatter");
            }
            else
            {
                await _next(context);
            }
        }
    }
}
