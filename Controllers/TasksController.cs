using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Backend.Dtos;
using TaskManagement.Backend.Middleware;
using TaskManagement.Backend.Services;

namespace TaskManagement.Backend.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class TasksController : ControllerBase
{
    private readonly TaskService _taskService;

    public TasksController(TaskService taskService)
    {
        _taskService = taskService;
    }

    private int GetUserId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (claim == null) throw new UnauthorizedAccessException("User ID not found in token");
        return int.Parse(claim.Value);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoTaskDto>>> GetTasks(CancellationToken cancellationToken)
    {
        var tasks = await _taskService.GetTasksAsync(GetUserId(), cancellationToken);
        return Ok(tasks);
    }

    [HttpPost]
    [Idempotency]
    public async Task<ActionResult<TodoTaskDto>> CreateTask(CreateTaskDto dto, CancellationToken cancellationToken)
    {
        var task = await _taskService.CreateTaskAsync(GetUserId(), dto, cancellationToken);
        return CreatedAtAction(nameof(GetTasks), new { id = task.Id }, task);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<TodoTaskDto>> UpdateTask(int id, UpdateTaskDto dto, CancellationToken cancellationToken)
    {
        var task = await _taskService.UpdateTaskAsync(GetUserId(), id, dto, cancellationToken);
        if (task == null) return NotFound();
        return Ok(task);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteTask(int id, CancellationToken cancellationToken)
    {
        var result = await _taskService.DeleteTaskAsync(GetUserId(), id, cancellationToken);
        if (!result) return NotFound();
        return NoContent();
    }
}
