using TaskManagerAPI.Dtos;
using TaskManagerAPI.Models;

namespace TaskManagerAPI.Services;

public interface ITaskService
{
    Task<List<TaskDto>> GetAllAsync(int userId);
    Task<TaskDto?> GetByIdAsync(int userId, int id);

    Task<TodoTask> CreateAsync(int userId, CreateTaskDto dto);
    Task<bool> UpdateAsync(int userId, int id, UpdateTaskDto dto);
    Task<bool> DeleteAsync(int userId, int id);
}