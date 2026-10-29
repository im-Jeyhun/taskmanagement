namespace TaskManagamanet.Entities
{
    public class User
    {
        public int UserId { get; set; }

        public string UserName { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string FatherName { get; set; } = string.Empty;

        public string PassHash { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public int SystemRoleId { get; set; }

        public int PositionId { get; set; }

        public int DepartmentId { get; set; }

        public int? SectionId { get; set; }

        public string? Role { get; set; }

        public int? ManagerUserId { get; set; }

        public bool IsActive { get; set; }

        public DateTime? LockoutUntil { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
