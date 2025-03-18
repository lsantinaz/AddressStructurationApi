using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;

using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder.Extensions;

namespace TestsUnitaires;

[TestClass]
public class ApiKeyMiddlewareTests
{
    private const string VALIDE_API_KEY = "temp";
    private const string INVALIDE_API_KEY = "654321";
    /*
    /// <summary>
    /// Cr�ation et configuration d'un client de test HTTP.
    /// Permet de simuler des appels API dans un environnement de test.
    /// </summary>
    /// <returns>Un client HTTP qui int�ragira avec le serveur de test</returns>
    private HttpClient CreateTestClient()
    {
        var host = new HostBuilder()
            .ConfigureWebHost(builder =>
            {
                builder.UseTestServer()

                       .Configure(app =>
                       {
                           app.UseMiddleware<ApiKeyMiddleware>();
                           app.Run(async context =>
                           {
                               context.Response.StatusCode = 200;
                               await context.Response.WriteAsync("test");
                           });
                       });
            })
            .Start();

        return host.GetTestClient();
    }*/

    [TestMethod]
    public async Task temptemptemp()
    {
        using var host = await new HostBuilder()
         .ConfigureWebHost(webBuilder =>
         {
             webBuilder
                 .UseTestServer()
                 .UseUrls("https://localhost:44314")
                 .ConfigureServices(services =>
                 {
                     // Charge la configuration à partir de appsettings.json
                     var configuration = new ConfigurationBuilder()
                         .SetBasePath(Directory.GetCurrentDirectory())  // L'emplacement du fichier appsettings.json
                         .AddJsonFile("appsettings.json")  // Charge le fichier appsettings.json
                         .Build();
                     services.AddSingleton<IConfiguration>(configuration);
                 })
                 .Configure(app =>
                 {
                     app.UseHttpsRedirection();
                     app.UseRouting();
                     app.UseMiddleware<ApiKeyMiddleware>();
                 });
         })
         .StartAsync();


        var response = await host.GetTestClient().GetAsync("/");

        Assert.AreNotEqual(HttpStatusCode.NotFound, response.StatusCode);

    }
    /*
    /// <summary>
    ///  Teste si le middleware retourne une réponse 200 OK avec une clé d'API valide.
    /// </summary>
    /// <returns></returns>
    [TestMethod]
    public async Task Middleware_ReturnsOk_WhenApiKeysIsValid()
    {
        var client = CreateTestClient();
        client.DefaultRequestHeaders.Add("X-API-Key", VALIDE_API_KEY);

        var response = await client.GetAsync("/");

        response.EnsureSuccessStatusCode();

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        var responseBody = await response.Content.ReadAsStringAsync();
        Assert.AreEqual("test", responseBody);

    }

    /// <summary>
    /// Teste si le middleware retourne une réponse 401 Unauthorized avec une clé d'API invalide.
    /// </summary>
    /// <returns></returns>
    [TestMethod]
    public async Task Middleware_ReturnsUnauthorized_WhenApiKeyIsInvalid()
    {
        var client = CreateTestClient();
        client.DefaultRequestHeaders.Add("X-API-Key", INVALIDE_API_KEY);

        var response = await client.GetAsync("/");

        Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        var responseBody = await response.Content.ReadAsStringAsync();
        Assert.AreEqual("Unauthorized", responseBody);
    }

    /// <summary>
    /// Teste si le middleware retourne une réponse 401 Unauthorized lorsque la clé d'API est absente.
    /// </summary>
    /// <returns></returns>
    [TestMethod]
    public async Task Middleware_ReturnsUnauthorized_WhenApiKeyIsMissing()
    {
        var client = CreateTestClient();

        var response = await client.GetAsync("/");

        Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        var responseBody = await response.Content.ReadAsStringAsync();
        Assert.AreEqual("Api Key vide", responseBody);
    }

    */

}
