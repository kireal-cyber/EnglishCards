using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EnglishCards.WebApi.Models
{
    public class Deck
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(300)]
        public string? Description { get; set; }

        public ICollection<WordCard> WordCards { get; set; } = new List<WordCard>();
    }
}
