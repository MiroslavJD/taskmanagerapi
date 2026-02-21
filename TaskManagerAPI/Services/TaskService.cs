using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Data;
using TaskManagerAPI.Dtos;
using TaskManagerAPI.Models;

namespace TaskManagerAPI.Services;

public class TaskService : ITaskService
{
    private readonly AppDbContext _db;

    public TaskService(AppDbContext db) => _db = db;

    public async Task<List<TaskDto>> GetAllAsync(int userId)
    {
        return await _db.Tasks
            .Where(t => t.UserId == userId)
            .OrderByDescending(t => t.Id)
            .Select(t => new TaskDto
            {
                Id = t.Id,
                Title = t.Title,
                IsDone = t.IsDone,
                CreatedAtUtc = t.CreatedAtUtc
            })
            .ToListAsync();
    }
    public async Task<TaskDto?> GetByIdAsync(int userId, int id)
    {
        return await _db.Tasks
            .Where(t => t.UserId == userId && t.Id == id)
            .Select(t => new TaskDto
            {
                Id = t.Id,
                Title = t.Title,
                IsDone = t.IsDone,
                CreatedAtUtc = t.CreatedAtUtc
            })
            .FirstOrDefaultAsync();
    }

    public async Task<TodoTask> CreateAsync(int userId, CreateTaskDto dto)
    {
        var task = new TodoTask
        {
            Title = dto.Title.Trim(),
            IsDone = false,
            CreatedAtUtc = DateTime.UtcNow,
            UserId = userId
        };

        _db.Tasks.Add(task);
        await _db.SaveChangesAsync();
        return task;
    }

    public async Task<bool> UpdateAsync(int userId, int id, UpdateTaskDto dto)
    {
        var task = await _db.Tasks.FirstOrDefaultAsync(t => t.UserId == userId && t.Id == id);
        if (task is null) return false;

        task.Title = dto.Title.Trim();
        task.IsDone = dto.IsDone;

        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int userId, int id)
    {
        var task = await _db.Tasks.FirstOrDefaultAsync(t => t.UserId == userId && t.Id == id);
        if (task is null) return false;

        _db.Tasks.Remove(task);
        await _db.SaveChangesAsync();
        return true;
    }
}