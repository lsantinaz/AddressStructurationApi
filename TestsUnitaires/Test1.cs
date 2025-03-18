using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using System.Net;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http;

namespace TestsUnitaires;

[TestClass]
public class Test1
{
    [TestMethod]
    public async Task TestMethod1()
    {
        using var host = await new HostBuilder()
       .ConfigureWebHost(webBuilder =>
       {
           webBuilder
               .UseTestServer()
               .Configure(app =>
               {
                   app.UseRouting();
                   app.UseMiddleware<ApiKeyMiddleware>();
                   app.UseEndpoints(Endpoint =>
                   {
                       Endpoint.MapGet("/test", () =>
                       TypedResults.Text("hello test)"));
                   });  
               });
       })
       .StartAsync();

        var client = host.GetTestClient();
        var response = await client.GetAsync("/test");

        Assert.IsTrue(response.IsSuccessStatusCode);



    }


}
