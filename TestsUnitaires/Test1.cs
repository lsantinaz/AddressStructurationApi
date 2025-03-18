using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using System.Net;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Builder.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TestsUnitaires;

[TestClass]
public class Test1
{

    private IHost CreateHost()
    {
        return new HostBuilder()
            .ConfigureWebHost(webBuilder =>
            {
                webBuilder
                    .UseTestServer()
                    .UseUrls("https://localhost:44314") // Définir l'URL de test avec HTTPS
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
                        app.UseHttpsRedirection(); // Si tu veux forcer HTTPS
                        app.UseRouting();
                        app.UseMiddleware<ApiKeyMiddleware>();
                    });
            })
            .Start();
    }

    [TestMethod]
    public async Task Should_ReturnSuccess_When_PostRequestToStructuration()
    {
        // Arrange
        var host = CreateHost();
        var client = host.GetTestClient();

        // Préparer le contenu de la requête (ici on utilise un JSON, selon ton API)
        var requestContent = new StringContent("{\"name\":\"John Doe\",\"rue_numero\":\"123\",\"npa_localite\":\"12345 City\"}",
            System.Text.Encoding.UTF8, "application/json");

        // Act
        var response = await client.PostAsync("/api/v1/structuration", requestContent);

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode); // Vérifie le statut HTTP attendu
        var responseContent = await response.Content.ReadAsStringAsync();
        Assert.IsTrue(responseContent.Contains("success"));
    }

    [TestMethod]
    public async Task Should_ReturnBadRequest_When_InvalidJson()
    {
        // Arrange
        var host = CreateHost();
        var client = host.GetTestClient();

        // Envoie un JSON malformé (par exemple, une accolade manquante)
        var requestContent = new StringContent("{\"name\":\"John Doe\",\"rue_numero\":\"123\",\"npa_localite\":\"12345 City\"",
            System.Text.Encoding.UTF8, "application/json");

        // Act
        var response = await client.PostAsync("/api/v1/structuration", requestContent);

        // Assert
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
    }

}
