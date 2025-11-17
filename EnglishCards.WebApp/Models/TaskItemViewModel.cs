using System.ComponentModel.DataAnnotations;

namespace EnglishCards.WebApp.Models
{
    
    public enum TaskStatusVm
    {
        New = 0,
        InProgress = 1,
        Done = 2,
        Canceled = 3
    }

    public class TaskItemViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(120)]
        public string Title { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        [Display(Name = "Дедлайн")]
        public DateTime? DueDate { get; set; }

        [Display(Name = "Статус")]
        public TaskStatusVm Status { get; set; } = TaskStatusVm.New;

        [Display(Name = "Тема")]
        public int? DeckId { get; set; }

        public string? DeckName { get; set; }

        [Display(Name = "Назначено пользователю")]
        public int? AssignedUserId { get; set; }

        public string? AssignedUserName { get; set; }

        
        public bool IsOverdue =>
            this.DueDate.HasValue
            && this.DueDate.Value < DateTime.UtcNow
            && this.Status != TaskStatusVm.Done;
    }
}
