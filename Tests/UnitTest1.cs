using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net.Http;

namespace Tests
{
    [TestClass]
    public class UnitTest1
    {
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



    }
}
