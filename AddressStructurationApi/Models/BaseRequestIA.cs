using System.Text.Json;

namespace AddressStructurationApi.Models
{
    /// <summary>
    /// 
    /// </summary>
    /// <example>
    /// {
    ///     "model": "llama3.1",
    ///     "messages": [
    ///         {
    ///             "role": "system", 
    ///             "content": "Répond uniquement en JSON. Tu dois structurer l'adresse avec les champs suivants : name(entreprise ou personne), NPA (4-5 chiffres), localite, rue et numéro. Si tu ne trouves pas de valeur, tu mets null"
    ///         },
    ///         {
    ///             "role": "user", 
    ///             "content": "structure cette adresse :AZ Informatique Sàrl, Pré de la claverie 22 2900 Porrentruy"
    ///         }
    ///     ],
    ///     "stream": false,
    ///     "format": {
    ///     "type": "object",
    ///     "properties": {
    ///         "name": {
    ///            "type": ["string", "null"]
    ///         },
    ///         "NPA": {
    ///            "type": ["number", "null"]
    ///         },
    ///         "localite": {
    ///            "type": ["string", "null"]
    ///         },
    ///         "rue": {
    ///            "type": ["string", "null"]
    ///         },
    ///         "numero": {
    ///            "type": ["number", "null"]
    ///         }
    ///     },
    ///     "required": [
    ///      "name",
    ///      "NPA",
    ///      "localite",
    ///      "rue",
    ///     "numero"
    ///    ]
    ///  }
    ///}
    /// 
    /// 
    /// </example>
    public abstract class BaseRequestIA
    {

        // Nom du modèle IA utilisé
        public string model { get; set; } = "llama3.1";

        // Indique si la réponse est streamer ou non
        public bool stream { get; set; } = false;

        // Liste des messages envoyés (contenant "role" et "content")
        public List<Message> messages { get; set; }
        
        public Format format { get; set; }

        public abstract void BuildRequest(string toStructure);

        /// <summary>
        /// Pour la mise en forme en JSON
        /// </summary>
        /// <returns></returns>
        public string toJson()
        {
            return JsonSerializer.Serialize(this);
        }

        /// <summary>
        /// Classe qui représente chaque message envoyé au modèle
        /// "role" : system ou user
        /// "content" : message
        /// </summary>
        public class Message
        {
            public string role { get; set; }
            public string content { get; set; }

        }

        /// <summary>
        /// 
        /// </summary>
        public class Format
        {
            public string type { get; set; }
            public object properties { get; set; }
            public List<string> required { get; set; }
        }
    }
}
