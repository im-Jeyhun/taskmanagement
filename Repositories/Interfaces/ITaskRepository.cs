using TaskManagamanet.DTOs.Task;

namespace TaskManagamanet.Repositories.Interfaces
{
    public interface ITaskRepository
    {
        Task<long?> CreateTaskAsync(CreateTaskRequest createTaskRequest);
    }
}
