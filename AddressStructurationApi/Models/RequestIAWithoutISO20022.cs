using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AddressStructurationApi.Models
{
    public class RequestIAWithoutISO20022 : BaseRequestIA
    {
        public RequestIAWithoutISO20022(string toStructure)
        {
            BuildRequest(toStructure);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="toStructure"></param>
        public override void BuildRequest(string toStructure)
        {
            // Avec détection
            messages = new List<Message>
            {
                new Message
                {
                    role = "system",
                    content = "Répond uniquement en JSON. Tu dois détecter si il s'agit d'une personne ou d'une societe. Si c'est une personne, tu structures le nom et le prenom, le champs societe_entreprise est à false. Si c'est une societe, remplis uniquement le champs societe, et met nom et prenom à false. Tu dois ensuite structurer l'adresse avec les champs suivants : NPA (4-5 chiffres), localite, rue et numéro. Si tu ne trouves pas de valeur, tu mets null"
                },
                new Message
                {
                    role = "user",
                    content = "détecter et ensuite structurer cette adresse : " + toStructure
                }
            };

            // Format 
            format = new Format
            {
                type = "object",
                properties = new
                {
                    nom = new { type = new List<string> { "string", "null" } },
                    prenom = new { type = new List<string> { "string", "null" } },
                    societe = new { type = new List<string> { "string", "null" } },
                    npa = new { type = new List<string> { "number", "null" } },
                    localite = new { type = new List<string> { "string", "null" } },
                    rue = new { type = new List<string> { "string", "null" } },
                    numero = new { type = new List<string> { "number", "null" } }
                },
                required = new List<string> { "nom", "prenom", "societe", "NPA", "localite", "rue", "numero" }
            };

        }
    }

}
