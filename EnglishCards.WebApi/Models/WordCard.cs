
namespace EnglishCards.WebApi.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.Text.Json.Serialization;

    public class WordCard
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string EnglishWord { get; set; } = string.Empty;

        [StringLength(100)]
        public string? Transcription { get; set; }

        [Required]
        [StringLength(200)]
        public string Translation { get; set; } = string.Empty;

        [StringLength(300)]
        public string? ExampleSentence { get; set; }

        [StringLength(200)]
        public string? PronunciationUrl { get; set; }

        [Required]
        public int DeckId { get; set; }

        [JsonIgnore]
        public Deck? Deck { get; set; }
    }
}
