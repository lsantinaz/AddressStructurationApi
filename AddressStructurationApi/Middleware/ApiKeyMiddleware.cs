using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

public class ApiKeyMiddleware(RequestDelegate next, IConfiguration configuration)
{
    // Récupère les valeurs contenus dans le fichier appsettings.json
    private string _headerKeyName = configuration["headerKeyName"];
    private string _apikey = configuration["ApiKey"];

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task InvokeAsync(HttpContext context)
    {

        if (!context.Request.Headers.TryGetValue(_headerKeyName, out 
                var extractedApiKey))
        {
            // Retourne le code 401 Unauthorized + un message
            context.Response.StatusCode = 401;
            await context.Response.WriteAsJsonAsync(new {message = "Header key invalide" });
            return;
        }

        if (!_apikey.Equals(extractedApiKey))
        {
            // Retourne le code 401 Unauthorized + un message
            context.Response.StatusCode = 401;
            await context.Response.WriteAsJsonAsync(new {message="Unauthorized"});
            return;
        }

        // L'authentification s'est faite et passe à la prochaine étape
        await next(context);
    }
}
