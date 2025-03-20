using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

public class ApiKeyMiddleware(RequestDelegate next, IConfiguration configuration)
{

    private string _headerKeyName = configuration["headerKeyName"];
    private string _apikey = configuration["ApiKey"];

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Headers.TryGetValue(_headerKeyName, out
                var extractedApiKey))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsJsonAsync(new {message = "Header key invalide" });
            return;
        }

        if (!_apikey.Equals(extractedApiKey))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsJsonAsync(new {message="Unauthorized"});
            return;
        }
        await next(context);
    }
}
