using Microsoft.AspNetCore.Identity.Data;
using TaskManagamanet.DTOs.Auth;
using LoginRequest = TaskManagamanet.DTOs.Auth.LoginRequest;

namespace TaskManagamanet.Services.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponse> LoginAsync(
            LoginRequest request);

        Task<RefreshTokenResponse> RefreshAsync(

            RefreshTokenRequest request);

        Task LogoutAsync(
            int userId);

        Task ChangePasswordAsync(
            int userId,
            ChangePasswordRequest request);
    }
}
