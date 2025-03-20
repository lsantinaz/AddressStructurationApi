using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;

using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder.Extensions;
using System.Net.NetworkInformation;
using Microsoft.Testing.Platform.Configurations;

namespace TestsUnitaires;

[TestClass]
public class ApiKeyMiddlewareTests
{
    private const string VALIDE_API_KEY = "temp";
    private const string INVALIDE_API_KEY = "654321";


    //public static IConfiguration InitConfiguration()
    //{
    //    var config = new ConfigurationBuilder()
    //       .AddJsonFile("appsettings.test.json")
    //        .AddEnvironmentVariables()
    //        .Build();
    //    return config;
    //}


    

    /// <summary>
    /// Cr�ation et configuration d'un client de test HTTP.
    /// Permet de simuler des appels API dans un environnement de test.
    /// </summary>
    /// <returns>Un client HTTP qui int�ragira avec le serveur de test</returns>
    private HttpClient CreateTestClient()
    {
        var handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
        };


        var host = new HostBuilder()
            .ConfigureWebHost(builder =>
            {
                builder
                       .UseConfiguration(new ConfigurationBuilder()
                            .AddJsonFile("appsettings.test.json")
                            .Build())    
                       .UseTestServer()
                       .ConfigureServices(app =>
                       {
                            
                       })
                       .Configure(app =>
                       {
                          app.UseMiddleware<ApiKeyMiddleware>();
                       });
            })
            .Start();


        return host.GetTestClient();
    }

 

    [TestMethod]
    public async Task TestWithEndpoint_ExpectedResponse()
    {
        
        var client = CreateTestClient();

        // Auth
        client.DefaultRequestHeaders.Add("X-API-Key", VALIDE_API_KEY);

        // Test GET
        var response = await client.GetAsync("/api/v1/test");

        Assert.IsTrue(response.IsSuccessStatusCode);
        var responseBody = await response.Content.ReadAsStringAsync();
        Assert.AreEqual("Hello Tests", responseBody);

        // Test POST
        // Envoie un JSON malformé (par exemple, une accolade manquante)
        var requestContent = new StringContent("{\"name\":\"John Doe\",\"rue_numero\":\"123\",\"npa_localite\":\"12345 City\",\"ISO20022\":\"true\"",
            System.Text.Encoding.UTF8, "application/json");
        var post = await client.PostAsync("/structuration", requestContent);
        response.EnsureSuccessStatusCode();

    }


        /// <summary>
        ///  Teste si le middleware retourne une réponse 200 OK avec une clé d'API valide.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
    public async Task Middleware_ReturnsOk_WhenApiKeysIsValid()
    {
        var client = CreateTestClient();
        client.DefaultRequestHeaders.Add("X-API-Key", VALIDE_API_KEY);
        client.BaseAddress = new Uri("https://localhost:44314");

        Console.WriteLine("Requête reçue");


        // Envoie un JSON malformé (par exemple, une accolade manquante)
        var requestContent = new StringContent("{\"name\":\"John Doe\",\"rue_numero\":\"123\",\"npa_localite\":\"12345 City\",\"ISO20022\":\"true\"",
            System.Text.Encoding.UTF8, "application/json");

        HttpResponseMessage response = await client.PostAsync("/api/v1/structuration", requestContent);
        //var response = await client.GetAsync("/swagger");
        
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
        client.DefaultRequestHeaders.Add("X-API-Key", INVALIDE_API_KEY);

        var response = await client.GetAsync("/api/v1/test");

        Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        var responseBody = await response.Content.ReadAsStringAsync();
        Assert.AreEqual("Api Key vide", responseBody);
    }

    

}
