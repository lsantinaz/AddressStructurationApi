using Microsoft.AspNetCore.Mvc;

namespace AddressStructurationApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class TestController : Controller
    {
         // Méthode GET de test
        [HttpGet]
        public ActionResult<string> Get()
        {
            return "test";
        }
    }
}
