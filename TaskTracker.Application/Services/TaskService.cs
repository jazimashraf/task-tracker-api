using FluentValidation;
using TaskTracker.Application.DTOs;
using TaskTracker.Application.Exceptions;
using TaskTracker.Application.Interfaces;
using TaskTracker.Domain.Entities;
using TaskTracker.Domain.Enums;

namespace TaskTracker.Application.Services;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _repository;
    private readonly IValidator<TaskItemCreateDto> _createValidator;
    private readonly IValidator<TaskItemUpdateDto> _updateValidator;

    public TaskService(
        ITaskRepository repository,
        IValidator<TaskItemCreateDto> createValidator,
        IValidator<TaskItemUpdateDto> updateValidator)
    {
        _repository = repository;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    public async Task<TaskItemDto> CreateAsync(TaskItemCreateDto dto)
    {
        await _createValidator.ValidateAndThrowAsync(dto);

        ValidateBusinessRules(dto.Title, dto.Status);

        var task = new TaskItem
        {
            Id = Guid.NewGuid(),
            Title = dto.Title.Trim(),
            Description = dto.Description,
            Status = dto.Status,
            DueDate = dto.DueDate
        };

        await _repository.AddAsync(task);

        return MapToDto(task);
    }

    public async Task<List<TaskItemDto>> GetAllAsync()
    {
        var tasks = await _repository.GetAllAsync();
        return tasks.Select(x => MapToDto(x)).ToList();
    }

    public async Task<TaskItemDto?> GetByIdAsync(Guid id)
    {
        var task = await _repository.GetByIdAsync(id);
        return task == null ? null : MapToDto(task);
    }

    public async Task<TaskItemDto?> UpdateAsync(Guid id, TaskItemUpdateDto dto)
    {
        await _updateValidator.ValidateAndThrowAsync(dto);

        ValidateBusinessRules(dto.Title, dto.Status);

        var task = await _repository.GetByIdAsync(id, track: true);

        if (task == null)
            return null;

        task.Title = dto.Title.Trim();
        task.Description = dto.Description;
        task.Status = dto.Status;
        task.DueDate = dto.DueDate;

        await _repository.UpdateAsync(task);

        return MapToDto(task);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var task = await _repository.GetByIdAsync(id, track: true);

        if (task == null)
            return false;

        await _repository.DeleteAsync(task);

        return true;
    }

    private static void ValidateBusinessRules(string title, TaskItemStatus status)
    {
        if (status == TaskItemStatus.Done && string.IsNullOrWhiteSpace(title))
        {
            throw new BusinessRuleException("A task cannot be marked as Done when Title is empty or whitespace.");
        }
    }

    private static TaskItemDto MapToDto(TaskItem task)
    {
        return new TaskItemDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            Status = task.Status,
            DueDate = task.DueDate
        };
    }
}