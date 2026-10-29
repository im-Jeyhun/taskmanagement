namespace TaskManagamanet.DTOs.Task
{
    public sealed class CreateTaskRequest
    {

        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public DateTime? DueAt { get; set; }

        public int? EstimatedDurationMinutes { get; set; }

    }
}
