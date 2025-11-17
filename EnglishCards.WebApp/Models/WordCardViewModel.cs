using System.ComponentModel.DataAnnotations;

namespace EnglishCards.WebApp.Models
{
    public class WordCardViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Слово обязательно")]
        [StringLength(100)]
        public string EnglishWord { get; set; } = string.Empty;

        [StringLength(100)]
        public string? Transcription { get; set; }

        [Required(ErrorMessage = "Перевод обязателен")]
        [StringLength(200)]
        public string Translation { get; set; } = string.Empty;

        [StringLength(300)]
        public string? ExampleSentence { get; set; }

        [StringLength(200)]
        public string? PronunciationUrl { get; set; }

        [Required(ErrorMessage = "Нужно выбрать тему")]
        public int DeckId { get; set; }

       
        public DeckViewModel? Deck { get; set; }
    }
}
