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
using System.Net.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AddressStructurationApi.Controllers
{

    [ApiController]
    [Route("api/v1/[controller]")]
    public class StructurationController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Constructeur d'une instance HttpClient 
        /// </summary>
        /// <param name="httpClientFactory">Factory d'un client http</param>
        public StructurationController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }


        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Structuration request)
        {
            // Récupération des champs du Body
            string tostructure = request.tostructure;
            bool? ISO20022 = request.ISO20022;

            try
            {
                return Ok(await structurationWithIA(tostructure));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new {erreur = ex.Message});
            }

            
            
        }

        /// <summary>
        /// Cette fonction appel le modèle IA avec le prompt inscrit et retourne la réponse 
        /// complète du modèle en format JSON
        /// </summary>
        /// <param name="request">chaine de caractère à structurer</param>
        /// <returns>La réponse JSON du modèle IA</returns>
        private async Task<object> CallModelIA(string request)
        {
            var url = "http://10.11.9.27:11434/api/chat";

            // Modèle de requête envoyé à l'IA

            var contenu = new StringContent(request, Encoding.UTF8, "application/json");

            // POST de la requête
            var reponse = await _httpClient.PostAsync(url, contenu);

            if (reponse.IsSuccessStatusCode)
            {
                // Réponse du modèle IA en JSON
                var responseModelIA = await reponse.Content.ReadAsStringAsync();
               
                return JsonSerializer.Deserialize<object>(responseModelIA);
            }

            return StatusCode((int)reponse.StatusCode, await reponse.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// Fonction qui recoit en paramètre un JSON et qui permet de parcourir ce JSON
        /// pour obtenir précisemment un contenu
        /// </summary>
        /// <param name="jsonResponse">Réponse JSON obtenu par le modèle IA</param>
        /// <returns>Un JSON, réponse des champs structuré obtenu par l'IA </returns>
        private string GetContentMessageOfModelIA(string jsonResponse)
        {
            using var jsonDoc = JsonDocument.Parse(jsonResponse);
            return jsonDoc.RootElement.GetProperty("message")
                                      .GetProperty("content")
                                      .ToString();

        }

        /// <summary>
        /// Méthode finale de structuration qui nous retourne le JSON final
        /// </summary>
        /// <param name="tostructure"></param>
        /// <returns>Le JSON structuré final</returns>
        private async Task<JsonElement> structurationWithIA(string tostructure)
        {
            /** Structuration de l'adresse **/
            // nouveau modèle de requête, convertit directement en JSON valide
            var payload = new RequestIAWithISO20022(tostructure);
            string jsonPayLoad = payload.toJson();

            // Envoie la requête au modèle IA
            var responseModelIA = await CallModelIA(jsonPayLoad);

            // Obtient uniquement les champs structurés, et non toute la réponse
            var response = GetContentMessageOfModelIA(JsonSerializer.Serialize(responseModelIA));

            return JsonSerializer.Deserialize<JsonElement>(response);
        }
    }
}
