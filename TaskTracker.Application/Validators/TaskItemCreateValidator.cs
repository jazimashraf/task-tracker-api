using FluentValidation;
using TaskTracker.Application.DTOs;

namespace TaskTracker.Application.Validators;

public class TaskItemCreateValidator : AbstractValidator<TaskItemCreateDto>
{
    public TaskItemCreateValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(100);
    }
}