namespace TaskManagamanet.DTOs.Auth
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Username { get; set; }
        public string RoleName { get; set; }
        public string PositionName { get; set; }
        public string DepartamentName { get; set; }

        public bool IsPassChangeRequired { get; set; }
    }
}
