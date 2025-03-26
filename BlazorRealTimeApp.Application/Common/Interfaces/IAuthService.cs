using BlazorRealTimeApp.Application.Models;

namespace BlazorRealTimeApp.Application.Common.Interfaces
{
    public interface IAuthService
    {
        Task<bool> LoginAsync(LoginRequest request);
    }
}
