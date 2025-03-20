using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace AddressStructurationApi.Models
{
    /// <summary>
    /// Modèle de requête envoyé au modèle d'intelligence artificielle
    /// Norme ISO20022, sans détection/structuration de personne/entreprise
    /// </summary>
    public class RequestIAWithISO20022
    {
        // Nom du modèle IA utilisé
        public string model { get; set; }

        // Liste des messages envoyés (contenant "role" et "content")
        public List<Message> messages { get; set; }

        // Indique si la réponse est streamer ou non
        public bool stream {  get; set; }

        // Format d'attente des réponses structurés avec des types pour chaque champs
        public Format format { get; set; }
        
        // Constructeur de la requête 
        public RequestIAWithISO20022(string tostructure)
        {
            model = "llama3.1";
            stream = false;
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
                    content = tostructure
                }
            };

            format = new Format
            {
                type = "object",
                properties = new Properties()
                {
                    name = new Field { type = new List<string> { "string", "null" } },
                    npa = new Field { type = new List<string> { "number", "null" } },
                    localite = new Field { type = new List<string> { "string", "null" } },
                    rue = new Field { type = new List<string> { "string", "null" } },
                    numero = new Field { type = new List<string> { "number", "null" } }
                },
                required = new List<string> 
                { 
                    nameof(Properties.name),
                    nameof(Properties.npa),
                    nameof(Properties.localite),
                    nameof(Properties.rue),
                    nameof(Properties.numero),
                }
            };
        }

        /// <summary>
        /// Pour la mise en forme en JSON
        /// </summary>
        /// <returns></returns>
        public string toJson()
        {
            return JsonSerializer.Serialize(this);
        }
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
    /// Classe qui contient les propriétés du format attendu pour la réponse du modèle
    /// Inclue également les types et les champs requis
    /// </summary>
    public class Format
    {
        public string type { get; set; }
        public Properties properties { get; set; }
        public List<string> required { get; set; } // Liste des propriétés requies
    }

    /// <summary>
    /// Classe qui décrit les différents champs attendu dans la réponse finale soit 
    ///     - name : personne ou société
    ///     - NPA : code postale d'une localité
    ///     - localite : nom d'une localité
    ///     - rue : nom d'une rue
    ///     - numero : numéro d'une rue
    /// </summary>
    public class Properties
    {
        public Field name { get; set; }
        public Field npa { get; set; }
        public Field localite { get; set; }
        public Field rue { get; set; }
        public Field numero { get; set; }
    }

    /// <summary>
    /// Classe qui définit les types dans les champs, soit : 
    ///     - string
    ///     - null
    ///     - boolean
    /// </summary>
    public class Field
    {
        public List<string> type {  get; set; }
    }
}
