using System.ComponentModel.DataAnnotations;

namespace AddressStructurationApi.Models
{
    /// <summary>
    /// Représente le modèle utilisé dans le contrôleur StructurationController
    /// </summary>
    public class Structuration
    {
        // Champs requis dans le JSON d'entré du corps de la requête
        [Required(ErrorMessage = "Le champ tostructure est oligatoire et ne peut pas être vide.")]
        public string tostructure { get; set; }

        [Required(ErrorMessage = "Le champs ISO20022 est obligatoire et ne peut pas être vide.")]
        public bool ISO20022 { get; set; }
    }
}
