using Salguri.Application.DTOS;

namespace Salguri.Application.Interfaces
{
    public interface IAuthService
    {
        Task<UserResponseDTO> RegisterAsync(UserRequestDTO request);
        Task<UserResponseDTO> LoginAsync(LoginRequestDTO request);
    }
}
