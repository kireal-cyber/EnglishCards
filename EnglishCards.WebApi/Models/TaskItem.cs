using System.ComponentModel.DataAnnotations;

namespace EnglishCards.WebApi.Models;

public enum TStatus
{
    New = 0,
    InProgress = 1,
    Done = 2,
    Canceled = 3
}

public class TaskItem
{
    public int Id { get; set; }

    [Required]
    [StringLength(120)]
    public string Title { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }

    public DateTime? DueDate { get; set; }

    public TStatus Status { get; set; } = TStatus.New;


    public int? DeckId { get; set; }
    public Deck? Deck { get; set; }

    public int? WordCardId { get; set; }
    public WordCard? WordCard { get; set; }


    public int? AssignedUserId { get; set; }
    public User? AssignedUser { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }
}
