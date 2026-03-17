using Salguri.Application.DTOS;

namespace Salguri.Application.Interfaces
{
    public interface IAuthService
    {
        Task<UserResponseDTO> RegisterAsync(UserRequestDTO request);
        Task<UserResponseDTO> LoginAsync(LoginRequestDTO request);
        Task<UserResponseDTO> VerifyOtp(VerifyOtpRequest request);
        Task<UserResponseDTO> ResendOtp(ResendOtpRequestDTO request);
        Task<TotpEnrollResponseDTO> EnrollTotpAsync(string accessToken, string refreshToken);
        Task<UserResponseDTO> VerifyTotpAsync(TotpVerifyRequestDTO request, string accessToken,string refreshToken);
    }
}
