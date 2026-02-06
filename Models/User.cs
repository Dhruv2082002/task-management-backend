using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Backend.Models;

public class User
{
    public int Id { get; set; }
    
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    public string PasswordHash { get; set; } = string.Empty;
    
    public string Name { get; set; } = string.Empty;
    
    public ICollection<TodoTask> Tasks { get; set; } = new List<TodoTask>();
}
