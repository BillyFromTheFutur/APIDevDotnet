using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Application_Webassembly_Blazo.Models
{
    public partial class Utilisateur
    {
        public int UtilisateurId { get; set; }

        [StringLength(50)]
        public string? Nom { get; set; }

        [StringLength(50)]
        public string? Prenom { get; set; }

        [StringLength(10)]
        [RegularExpression(@"^0[0-9]{9}$", ErrorMessage = "Veuillez mettre un numero de telephone valide !")]
        public string? Mobile { get; set; }

        [StringLength(100, MinimumLength = 6, ErrorMessage = "La longueur d’un email doit être comprise entre 6 et 100 caractères."), EmailAddress, Required]
        public string Mail { get; set; } = null!;

        [StringLength(64, MinimumLength = 6, ErrorMessage = "Le mot de passe doit contenir entre 6 et 64 caractères.")]
        [RegularExpression("^(?=.*[A-Z])(?=.*\\d)(?=.*[\\W_])[A-Za-z\\d\\W_]{6,10}$", ErrorMessage = "Le mot de passe doit contenir entre 6 et 10 caractères, avec au moins 1 lettre majuscule, 1 chiffre et 1 caractère spécial.")]
        [Required]
        public required string Pwd { get; set; }

        [StringLength(200)]
        public string? Rue { get; set; }

        [RegularExpression(@"^\d{5}$", ErrorMessage = "Le champ doit contenir exactement 5 chiffres.")]
        public string? CodePostal { get; set; }

        [StringLength(50)]
        public string? Ville { get; set; }

        [StringLength(50)]
        public string? Pays { get; set; }

        public float? Latitude { get; set; }

        public float? Longitude { get; set; }

        public DateTime DateCreation { get; set; }

        public virtual ICollection<Notation>? NotesUtilisateur { get; }

    }
}
