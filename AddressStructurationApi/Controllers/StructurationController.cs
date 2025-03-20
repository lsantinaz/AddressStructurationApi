using AddressStructurationApi.Models;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;
using System.Text.Json.Nodes;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Data;
using System.Xml.Linq;

namespace AddressStructurationApi.Controllers
{

    [ApiController]
    [Route("api/v1/[controller]")]
    public class StructurationController : ControllerBase
    {

        [HttpPost]
        public ActionResult<string> Post([FromBody] Structuration request)
        {
            // Déclaration du JSON final
            var jsonStructured = new { };

            /** Structuration de l'adresse **/
            // En cas de norme ISO 20022
            if (request.ISO20022 ==  false)
            {
                
                // En cas de Swissdec
                // Nécessite la structuration du nom/prénom ou la détection d'une société/entreprise
                // Interaction avec le modèle IA 
               
            }

            // Structuration du reste de l'adresse


            // Retourne l'adresse structuré 

            return Ok();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Structuration()
        {

        }
    }  
    
}
