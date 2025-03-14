using Microsoft.AspNetCore.Mvc;

namespace AddressStructurationApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {

        // Méthode GET de test
        [HttpGet(Name = "GetTest")]
        public ActionResult<string> Get()
        {
            return "test";
        }
    }
}
