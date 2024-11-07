using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Application_Webassembly_Blazo.Models
{

    public class Notation
    {
        public int UtilisateurId { get; set; }

        public int SerieId { get; set; }

        [Range(0, 5, ErrorMessage = "Value for {0} must be between {1} and {2}."), Required]
        public required int Note { get; set; }

        [ForeignKey(nameof(UtilisateurId))]
        [InverseProperty(nameof(Utilisateur.NotesUtilisateur))]
        public virtual Utilisateur UtilisateurNotant { get; set; } = null!;

        [ForeignKey(nameof(SerieId))]
        [InverseProperty(nameof(Serie.NotesSerie))]
        public virtual Serie SerieNotee { get; set; } = null!;
    }
}
