using System.ComponentModel.DataAnnotations;

namespace EnglishCards.WebApi.Models;

public class User
{
    public int Id { get; set; }

    [Required]
    [StringLength(40)]
    public string Name { get; set; } = string.Empty;

    [StringLength(100)]
    public string? Email { get; set; }


    public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
}
