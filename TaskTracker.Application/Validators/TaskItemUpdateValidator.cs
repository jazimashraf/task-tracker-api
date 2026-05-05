using FluentValidation;
using TaskTracker.Application.DTOs;

namespace TaskTracker.Application.Validators;

public class TaskItemUpdateValidator : AbstractValidator<TaskItemUpdateDto>
{
    public TaskItemUpdateValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(100);
    }
}