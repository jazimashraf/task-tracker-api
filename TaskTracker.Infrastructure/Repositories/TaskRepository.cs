using Microsoft.EntityFrameworkCore;
using TaskTracker.Application.Interfaces;
using TaskTracker.Domain.Entities;
using TaskTracker.Infrastructure.Data;

namespace TaskTracker.Infrastructure.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly TaskTrackerDbContext _context;

    public TaskRepository(TaskTrackerDbContext context)
    {
        _context = context;
    }

    public async Task<TaskItem> AddAsync(TaskItem task)
    {
        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();
        return task;
    }

    public async Task<List<TaskItem>> GetAllAsync()
    {
        return await _context.Tasks.AsNoTracking().ToListAsync();
    }

    public async Task<TaskItem?> GetByIdAsync(Guid id, bool track = false)
    {
        var query = _context.Tasks.AsQueryable();
        if (!track)
        {
            query = query.AsNoTracking();
        }
        return await query.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task UpdateAsync(TaskItem task)
    {
        _context.Tasks.Update(task);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(TaskItem task)
    {
        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();
    }
}
