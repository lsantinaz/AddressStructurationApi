using Microsoft.AspNetCore.Mvc;

namespace AddressStructurationApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
         // Méthode GET de test
        [HttpGet]
        public ActionResult<string> Get()
        {
            return "test";
        }
    }
}
