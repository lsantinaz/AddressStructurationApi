using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Threading.Tasks;

namespace TestsUnitaires;

[TestClass]
public class ApiTests
{
    private readonly WebApplicationFactory<Program> _factory;

    public ApiTests()
    {
        // Cr�e un WebApplicationFactory pour initialiser l'application dans un environnement de test
        _factory = new WebApplicationFactory<Program>();
    }

    [TestMethod]
    public async Task Test_Api_Endpoint_Returns_OK()
    {
        // Cr�e un client HTTP pour envoyer des requ�tes au serveur de test
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Add("X-API-Key", "temp");

        // Envoie une requ�te GET � l'endpoint /api/v1/structuration
        var response = await client.GetAsync("/api/v1/structuration");

        // V�rifie si la r�ponse HTTP est 200 OK
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    }

    [TestMethod]
    public async Task Test_Api_Endpoint_Returns_NotFound_For_Invalid_Route()
    {
        // Cr�e un client HTTP pour envoyer des requ�tes au serveur de test
        var client = _factory.CreateClient();

        // Envoie une requ�te GET � une route inexistante
        var response = await client.GetAsync("/api/v1/invalidroute");

        // V�rifie si la r�ponse HTTP est 404 Not Found
        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
    }
}
