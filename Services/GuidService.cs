namespace ASPCoreEmpty.Services
{
    public class GuidService : IResponseFormatter
    {
        private Guid _guid = Guid.NewGuid();

        public async Task Format(HttpContext context, string content)
        {
            await context.Response.WriteAsync($"GUID: {_guid}\n{content}");
        }
    }
}
