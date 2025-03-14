using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

public class ApiKeyMiddleware
{
    private readonly RequestDelegate _next;
    private const string API_KEY_HEADER_NAME = "X-API-Key";
    private readonly string _apiKey;

    /// <summary>
    /// Méthode qui vérifie la valeur de la clé d'API
    /// </summary>
    /// <param name="next">représente le prochain middleware</param>
    /// <param name="configuration">Accès au fichier de configuration appsettings.json</param>
    public ApiKeyMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        // contient la valeur de la clé
        _apiKey = configuration["ApiKey"];
    }

    /// <summary>
    /// Cette méthode est appelée pour chaque requête HTTP
    /// </summary>
    /// <param name="context">informations sur la requête en cours</param>
    /// <returns> Status 401 Unauthorized ou contiue la requête car valide</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        
        // Récupère les valeurs du header de la requête et test ensuite si la clé
        // correspond à celle présent dans appsettings.json
        if (!context.Request.Headers.TryGetValue(API_KEY_HEADER_NAME, out var extractedApiKey) || extractedApiKey != _apiKey)
        {
            context.Response.StatusCode = 401; // Unauthorized
            await context.Response.WriteAsync("Api Key invalide");
            return;
        }

        await _next(context);
    }
}
