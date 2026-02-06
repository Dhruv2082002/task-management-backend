using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Backend.Models;

public class TodoTask
{
    public int Id { get; set; }
    
    [Required]
    public string Title { get; set; } = string.Empty;
    
    public string Description { get; set; } = string.Empty;
    
    public bool IsCompleted { get; set; }
    
    public DateTime? DueDate { get; set; }
    
    public int UserId { get; set; }
    public User? User { get; set; }
}
