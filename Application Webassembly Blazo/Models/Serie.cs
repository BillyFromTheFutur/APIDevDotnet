using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Application_Webassembly_Blazo.Models
{
    public partial class Serie
    {
        public int SerieId { get; set; }

        [StringLength(100), Required]
        public required string Titre { get; set; }

        public string? Resume { get; set; }

        public int? NbSaisons { get; set; }

        public int? NbEpisodes { get; set; }

        public int? AnneeCreation { get; set; }

        [StringLength(50)]
        public string? Network { get; set; }

        public virtual ICollection<Notation>? NotesSerie { get; }

    }
}
