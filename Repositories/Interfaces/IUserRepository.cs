using TaskManagamanet.Entities;

namespace TaskManagamanet.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByUsernameAsync(string username);
        Task<User?> GetByIdAsync(int userId);
        Task<bool> UpdatePasswordAsync(
            int userId,
            string passHash,
            bool isPassChangeRequired);


        Task<bool> ResetLoginAttemptsAsync(int userId);
        Task<bool> RegisterFailedLoginAsync(
            int userId,
            int maxAttempts,
            int lockoutMinutes);
    }
}
