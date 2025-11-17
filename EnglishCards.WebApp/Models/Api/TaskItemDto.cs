namespace EnglishCards.WebApp.Models.Api
{
    public class TaskItemDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public DateTime? DueDate { get; set; }

        public int Status { get; set; }

        public int? DeckId { get; set; }

        public DeckDto? Deck { get; set; }

        public int? AssignedUserId { get; set; }

        public UserDto? AssignedUser { get; set; }
        public int? WordCardId { get; set; }

        
    }

    public class DeckDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
    }

    public class UserDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
    }
}
