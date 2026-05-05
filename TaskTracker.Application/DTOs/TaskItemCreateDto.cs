using TaskTracker.Domain.Enums;

namespace TaskTracker.Application.DTOs;

public class TaskItemCreateDto
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public TaskItemStatus Status { get; set; } = TaskItemStatus.Todo;
    public DateTime? DueDate { get; set; }
}