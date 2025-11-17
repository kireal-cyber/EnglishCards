using System.ComponentModel.DataAnnotations;

namespace EnglishCards.WebApp.Models
{
    public class DeckViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Название темы обязательно")]
        [StringLength(100, ErrorMessage = "Название не может быть длиннее 100 символов")]
        public string Name { get; set; } = string.Empty;

        [StringLength(300, ErrorMessage = "Описание не может быть длиннее 300 символов")]
        public string? Description { get; set; }
    }
}
