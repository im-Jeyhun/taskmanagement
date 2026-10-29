using TaskManagamanet.Entities;

namespace TaskManagamanet.Repositories.Interfaces
{
    public interface ITokenRepository
    {
        Task CreateAsync(
       int userId,
       string tokenHash,
       DateTime expiresAt);

        Task<UserToken?> GetActiveByHashAsync(
            string tokenHash);

        Task<bool> RevokeAsync(long tokenId);

        Task RevokeAllForUserAsync(int userId);
    }
}