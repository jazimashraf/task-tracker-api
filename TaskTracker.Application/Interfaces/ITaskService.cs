using TaskTracker.Application.DTOs;

namespace TaskTracker.Application.Interfaces;

public interface ITaskService
{
    Task<TaskItemDto> CreateAsync(TaskItemCreateDto dto);
    Task<List<TaskItemDto>> GetAllAsync();
    Task<TaskItemDto?> GetByIdAsync(Guid id);
    Task<TaskItemDto?> UpdateAsync(Guid id, TaskItemUpdateDto dto);
    Task<bool> DeleteAsync(Guid id);
}