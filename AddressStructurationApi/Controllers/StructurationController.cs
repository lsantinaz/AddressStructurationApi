using AddressStructurationApi.Models;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;
using System.Text.Json.Nodes;

namespace AddressStructurationApi.Controllers
{

    [ApiController]
    public class StructurationController : Controller
    {

        private static readonly HttpClient client = new HttpClient();

        [HttpPost]
        [Route("api/v1/structuration")]
        public ActionResult<string> Post([FromBody] NoStructured request)
        {
            //Si le JSON ne correspond pas au modèle on retourne une erreur 400 Bad Request
            if (request == null)
            {
                HttpContext.Response.StatusCode = 400;
                return BadRequest("JSON Invalide");
            }

            /** Structuration de l'adresse **/
            
            // En cas de norme ISO 20022
            if (request.ISO20022)
            {
                
            } else
            {
                
            }

            return "test";
        }
    }
    
}
