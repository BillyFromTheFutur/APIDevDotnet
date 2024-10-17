using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace API.Models.EntityFramework
{
    [Table("t_e_utilisateur_utl")]
    public class Utilisateur
    {
        [Key]
        [Column("utl_id")]
        public int UtilisateurId { get; set; }

        [Column("utl_nom")]
        [StringLength(50)]
        public string? Nom { get; set; }

        [Column("utl_prenom")]
        [StringLength(50)]
        public string? Prenom { get; set; }

        [Column("utl_mobile", TypeName = "character(10)")]
        [RegularExpression(@"^0[0-9]{9}$", ErrorMessage = "Veuillez mettre un numero de telephone valide !")]
        public string? Mobile { get; set; }

        [Column("utl_mail")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "La longueur d’un email doit être comprise entre 6 et 100 caractères."), EmailAddress, Required]
        public string Mail { get; set; } = null!;

        [Column("utl_pwd")]
        [StringLength(64, MinimumLength = 6, ErrorMessage = "Le mot de passe doit contenir entre 6 et 64 caractères.")]
        [RegularExpression("^(?=.*[A-Z])(?=.*\\d)(?=.*[\\W_])[A-Za-z\\d\\W_]{6,10}$", ErrorMessage = "Le mot de passe doit contenir entre 6 et 10 caractères, avec au moins 1 lettre majuscule, 1 chiffre et 1 caractère spécial.")]
        [Required]
        public required string Pwd { get; set; }

        [Column("utl_rue")]
        [StringLength(200)]
        public string? Rue { get; set; } 

        [Column("utl_cp", TypeName = "character(5)")]
        [RegularExpression(@"^\d{5}$", ErrorMessage = "Le champ doit contenir exactement 5 chiffres.")]
        public string? CodePostal { get; set; } 

        [Column("utl_ville")]
        [StringLength(50)]
        public string? Ville { get; set; }

        [Column("utl_pays")]
        [StringLength(50)]
        public string? Pays { get; set; }

        [Column("utl_latitude")]
        public float? Latitude { get; set; }

        [Column("utl_longitude")]
        public float? Longitude { get; set; }

        [Column("utl_datecreation")]
        public DateTime DateCreation { get; set; } // Valeur par défaut définie dans OnModelCreating

        public virtual ICollection<Notation> NotesUtilisateur { get; set; } = new List<Notation>();
    }
}
