using FluentAssertions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TaskTracker.Application.DTOs;
using TaskTracker.Application.Exceptions;
using TaskTracker.Application.Services;
using TaskTracker.Application.Validators;
using TaskTracker.Domain.Enums;
using TaskTracker.Infrastructure.Data;
using TaskTracker.Infrastructure.Repositories;

namespace TaskTracker.Tests;

public class TaskServiceTests
{
    private TaskService CreateService()
    {
        var options = new DbContextOptionsBuilder<TaskTrackerDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new TaskTrackerDbContext(options);

        return new TaskService(
            new TaskRepository(context),
            new TaskItemCreateValidator(),
            new TaskItemUpdateValidator());
    }

    [Fact]
    public async Task CreateAsync_WhenTitleIsEmpty_ShouldThrowValidationException()
    {
        var service = CreateService();

        var dto = new TaskItemCreateDto
        {
            Title = "",
            Status = TaskItemStatus.Todo
        };

        var act = async () => await service.CreateAsync(dto);

        await act.Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task CreateAsync_WhenStatusDoneAndTitleWhitespace_ShouldThrowBusinessRuleException()
    {
        var service = CreateService();

        var dto = new TaskItemCreateDto
        {
            Title = "   ",
            Status = TaskItemStatus.Done
        };

        var act = async () => await service.CreateAsync(dto);

        await act.Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task CreateAsync_WithValidTask_ShouldCreateSuccessfully()
    {
        var service = CreateService();

        var dto = new TaskItemCreateDto
        {
            Title = "Finish assignment",
            Description = "Build Task Tracker API",
            Status = TaskItemStatus.Todo
        };

        var result = await service.CreateAsync(dto);

        result.Id.Should().NotBeEmpty();
        result.Title.Should().Be("Finish assignment");
        result.Status.Should().Be(TaskItemStatus.Todo);
    }

    [Fact]
    public async Task UpdateAsync_WithValidTask_ShouldUpdateSuccessfully()
    {
        var service = CreateService();

        var created = await service.CreateAsync(new TaskItemCreateDto
        {
            Title = "Initial title",
            Status = TaskItemStatus.Todo
        });

        var updateDto = new TaskItemUpdateDto
        {
            Title = "Updated title",
            Status = TaskItemStatus.InProgress
        };

        var updated = await service.UpdateAsync(created.Id, updateDto);

        updated.Should().NotBeNull();
        updated!.Title.Should().Be("Updated title");
        updated.Status.Should().Be(TaskItemStatus.InProgress);
    }


}