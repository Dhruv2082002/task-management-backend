using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Backend.Dtos;

public record TodoTaskDto(int Id, string Title, string Description, bool IsCompleted, DateTime? DueDate, int UserId);
public record CreateTaskDto([Required] string Title, string Description, DateTime? DueDate);
public record UpdateTaskDto([Required] string Title, string Description, bool IsCompleted, DateTime? DueDate);
