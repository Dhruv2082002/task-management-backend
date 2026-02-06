using Microsoft.EntityFrameworkCore;
using TaskManagement.Backend.Data;
using TaskManagement.Backend.Dtos;
using TaskManagement.Backend.Models;

namespace TaskManagement.Backend.Services;

public class TaskService
{
    private readonly AppDbContext _context;

    public TaskService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TodoTaskDto>> GetTasksAsync(int userId)
    {
        return await _context.TodoTasks
            .Where(t => t.UserId == userId)
            .Select(t => new TodoTaskDto(t.Id, t.Title, t.Description, t.IsCompleted, t.DueDate, t.UserId))
            .ToListAsync();
    }

    public async Task<TodoTaskDto> CreateTaskAsync(int userId, CreateTaskDto dto)
    {
        var task = new TodoTask
        {
            Title = dto.Title,
            Description = dto.Description,
            DueDate = dto.DueDate,
            UserId = userId,
            IsCompleted = false
        };

        _context.TodoTasks.Add(task);
        await _context.SaveChangesAsync();

        return new TodoTaskDto(task.Id, task.Title, task.Description, task.IsCompleted, task.DueDate, task.UserId);
    }

    public async Task<TodoTaskDto?> UpdateTaskAsync(int userId, int taskId, UpdateTaskDto dto)
    {
        var task = await _context.TodoTasks.FirstOrDefaultAsync(t => t.Id == taskId && t.UserId == userId);
        if (task == null) return null;

        task.Title = dto.Title;
        task.Description = dto.Description;
        task.IsCompleted = dto.IsCompleted;
        task.DueDate = dto.DueDate;

        await _context.SaveChangesAsync();

        return new TodoTaskDto(task.Id, task.Title, task.Description, task.IsCompleted, task.DueDate, task.UserId);
    }

    public async Task<bool> DeleteTaskAsync(int userId, int taskId)
    {
        var task = await _context.TodoTasks.FirstOrDefaultAsync(t => t.Id == taskId && t.UserId == userId);
        if (task == null) return false;

        _context.TodoTasks.Remove(task);
        await _context.SaveChangesAsync();
        return true;
    }
}
