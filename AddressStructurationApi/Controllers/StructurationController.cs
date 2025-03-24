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
using System.Net;
using System.Net.Http.Headers;

namespace AddressStructurationApi.Controllers
{

    [ApiController]
    [Route("api/v1/[controller]")]
    public class StructurationController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private string URL_MODEL_IA = "http://10.11.9.27:11434/api/chat";
        private string URL_SWISS_POST_API = "https://webservices-int.post.ch:17023/IN_SYNSYN_EXT_INT/REST/v1/autocomplete4";
        private string USERNAME_SWISS_POST_API = "TU_196764_0102";
        private string PASSWORD_SWISS_POST_API = "NKf8KRSY";



        /// <summary>
        /// Constructeur d'une instance HttpClient qui permet d'utiliser ce service
        /// pour les appels aux différentes API externe (modèle et poste)
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
            bool ISO20022 = request.ISO20022;

            try
            {
                var structuredAddress = await structurationAddress(tostructure, ISO20022);
                var verificatedAddress = await verificationAddress(JsonSerializer.Serialize(structuredAddress));

                return Ok(verificatedAddress);
            }
            catch(Exception ex)
            {
                return StatusCode(500, new { erreur = ex.Message });
            }
        }

        /// <summary>
        /// Structuration de l'adresse à l'aide du modèle d'intelligence artificielle
        /// </summary>
        /// <param name="tostructure">Chaine de caractère à structurer</param>
        /// <param name="ISO20022">Boolean concernant la norme ISO20022</param>
        /// <returns>Le JSON structuré final</returns>
        private async Task<JsonElement> structurationAddress(string tostructure, bool ISO20022)
        {
            /** Structuration de l'adresse **/
            string requestToIA = "";

            if (ISO20022)
            {
                requestToIA = new RequestIAWithISO20022(tostructure).toJson();
            }
            else
            {
                requestToIA = new RequestIAWithoutISO20022(tostructure).toJson();
            }

            // Appel à l'API et réponse finale
            var responseOfModelIA = await CallModelIA(requestToIA);
            var responseFinal = GetContentMessageOfModelIA(JsonSerializer.Serialize(responseOfModelIA));

            return JsonSerializer.Deserialize<JsonElement>(responseFinal);
        }

        /// <summary>
        /// Méthode qui vérifie une adresse passée en paramètre.
        /// Utilisation du service Assistant d'adresse via Webservice de la Poste Suisse.
        /// Elle traite une adresse déjà structurée passée en paramètre (modèle IA) pour ensuite 
        /// envoyer les données à l'API de la Poste pour que celle-ci corrige les champs.
        /// </summary>
        /// <param name="responseOfModel">Chaine de caractère de la réponse structurée du modèle IA</param>
        /// <returns>la réponse JSON de l'API de la Poste</returns>
        private async Task<JsonElement> verificationAddress(string responseOfModel)
        {
            string npa = getPropertyOfIAModelResponse(JsonSerializer.Deserialize<JsonElement>(responseOfModel), 1);
            string localite = getPropertyOfIAModelResponse(JsonSerializer.Deserialize<JsonElement>(responseOfModel), 2);
            string rue = getPropertyOfIAModelResponse(JsonSerializer.Deserialize<JsonElement>(responseOfModel), 3);
            string numero = getPropertyOfIAModelResponse(JsonSerializer.Deserialize<JsonElement>(responseOfModel), 4);

            // Appel à l'API de la Poste et réponse finale
            var responseOfSwissPost = await CallSwissPostAPI(new RequestSwissPostAPI(npa, localite, rue, numero).toJson());
            var responseFinal = GetContentMessageOfSwissPostAPI(JsonSerializer.Serialize(responseOfSwissPost));

            return JsonSerializer.Deserialize<JsonElement>(responseFinal);
        }

        /// <summary>
        /// Fonction qui retourne une seul propriété d'un JSON passé en paramètre
        /// selon la valeur "order" passé en paramètre.
        /// Les valeurs définit sont celles que le modèle IA retourne (npa, localite, rue et numero).
        /// Cette méthode est utilisée pour la vérification de l'adresse.
        /// </summary>
        /// <param name="json">JSON à parcouri</param>
        /// <param name="order">INT, valeur correspondant à l'ordre de l'élément</param>
        /// <returns>Chaine de caractère de la valeur du JSON correspondante à l'ordre</returns>
        private string getPropertyOfIAModelResponse(JsonElement json, int order)
        {
            var values = new string[]
            {
                json.GetProperty("npa").ToString(),
                json.GetProperty("localite").ToString(),
                json.GetProperty("rue").ToString(),
                json.GetProperty("numero").ToString()
            };

            // Retourne la valeur à l'index du tableau (nécessite - 1)
            return values[order - 1];
        }

        /// <summary>
        /// Cette fonction appel le modèle IA avec le prompt inscrit et retourne la réponse 
        /// complète du modèle en format JSON
        /// </summary>
        /// <param name="request">chaine de caractère à structurer</param>
        /// <returns>La réponse JSON du modèle IA</returns>
        private async Task<object> CallModelIA(string request)
        {

            var contenu = new StringContent(request, Encoding.UTF8, "application/json");

            // réponse du POST
            var reponse = await _httpClient.PostAsync(URL_MODEL_IA, contenu);

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
        /// Appel de l'API de la poste
        /// </summary>
        /// <returns>un JSON contenant le npa, la localité, la rue, le numéro etc...</returns>
        private async Task<object> CallSwissPostAPI(string request)
        {

            // Ajouter l'en-tête "Authorization" avec la méthode Basic Auth
            string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{USERNAME_SWISS_POST_API}:{PASSWORD_SWISS_POST_API}"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
            
            var contenu = new StringContent(request, Encoding.UTF8, "application/json");
            
            // Obtient la réponse par méthode POST
            var reponse = await _httpClient.PostAsync(URL_SWISS_POST_API, contenu);
            
            // Test de succès et réponse retournée
            if (reponse.IsSuccessStatusCode)
            {

                var responseSwissPostApi = await reponse.Content.ReadAsStringAsync();

                return JsonSerializer.Deserialize<object>(responseSwissPostApi);
            }

            return StatusCode((int)reponse.StatusCode, await reponse.Content.ReadAsStringAsync());
        }

        private string GetContentMessageOfSwissPostAPI(string jsonResponse)
        {
            using var jsonDoc = JsonDocument.Parse(jsonResponse);
            return jsonDoc.RootElement.GetProperty("QueryAutoComplete4Result")
                                      .GetProperty("AutoCompleteResult")
                                      .ToString();
        }
    }
}
