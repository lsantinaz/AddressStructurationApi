using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Caching.Memory;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Text.Json;

namespace AddressStructurationApi.Models
{
    /// <summary>
    /// Modèle de requête envoyé au modèle d'intelligence artificielle
    /// Norme ISO20022, sans détection/structuration de personne/entreprise
    /// </summary>
    public class RequestIAWithISO20022 : BaseRequestIA
    {
     
        /// <summary>
        /// 
        /// </summary>
        /// <param name="toStructure">chaine de caractère à structurer</param>
        public RequestIAWithISO20022(string toStructure)
        {
            BuildRequest(toStructure);          
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="toStructure">chaine de caractère à structurer</param>
        public override void BuildRequest(string toStructure)
        {
            // Sans détection 
            messages = new List<Message>
            {
                new Message
                {
                    role = "system",
                    content = "Répond uniquement en JSON. Tu dois structurer l'adresse avec les champs suivants : name(entreprise ou personne), NPA (4-5 chiffres), localite, rue et numéro. Si tu ne trouves pas de valeur, tu mets null"
                },
                new Message
                {
                    role = "user",
                    content = "Structure cette adresse : " + toStructure
                }
            };

            // Format 
            format = new Format
            {
                type = "object",
                properties = new
                {
                    name = new { type = new List<string> { "string", "null" } },
                    npa = new { type = new List<string> { "number", "null" } },
                    localite = new { type = new List<string> { "string", "null" } },
                    rue = new { type = new List<string> { "string", "null" } },
                    numero = new { type = new List<string> { "number", "null" } }
                },
                required = new List<string> { "name", "npa", "localite", "rue", "numero" }
            };
        }
    }

}
