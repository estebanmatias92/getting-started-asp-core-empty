using ASPCoreEmpty.Services;

namespace ASPCoreEmpty
{
    // New Service with depdency injection through HttpContext object
    public class CustomEndpoint
    {
        public static async Task Endpoint(HttpContext context)
        {
            IResponseFormatter formatter = context.RequestServices.GetRequiredService<IResponseFormatter>();

            await formatter.Format(context, "Soy la clase Custom Endpoint");
        }
    }
}
