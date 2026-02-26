using Salguri.Application.DTOS;
using Salguri.Application.Interfaces;
using Salguri.Domain.Models;
using Supabase.Gotrue;
using Client = Supabase.Client;


namespace Salguri.Infrastructure.Services.AuthService
{
    public class SupabaseAuthService : IAuthService
    {
        private readonly Client _client;

        public SupabaseAuthService(Client client)
        {
            _client = client;
        }

        public async Task<UserResponseDTO> RegisterAsync(UserRequestDTO request)
        {
            Session? session = null;

            var options = new SignUpOptions
            {
                Data = new Dictionary<string, object>
        {
            { "full_name", $"{request.FullName}" },
            { "app_id", "business" }
        }
            };
            try
            {
                session = await _client.Auth.SignUp(request.Email!, request.Password!, options);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                return new UserResponseDTO
                {
                    Success = false,
                    Message = ex.Message.ToString()
                };

            }

            return new UserResponseDTO
            {
                Success = true,
                Message = "Registered Successfully. Please verify your email."
            };
        }




        public async Task<UserResponseDTO> LoginAsync(LoginRequestDTO request)
        {
            try
            {
                var session = await _client.Auth.SignIn(request.Email!, request.Password!);

                if (session == null || session.User == null)
                    throw new Exception("Invalid email or password.");

                var response = await _client
                           .From<SalguriUsers>()
                           .Where(x => x.Id == session.User.Id)
                           .Select(x => new object[] { x.RoleId!, x.IsActive! })
                           .Get();

                var user = response.Models.FirstOrDefault();

                if (user == null)
                    throw new Exception("User profile not found.");


                return new UserResponseDTO
                {
                    Success = true,
                    AccessToken = session.AccessToken!,
                    IsActive = user?.IsActive ?? false,
                    Message = user?.IsActive ?? false
                        ? "Login successful."
                        : "Your account is pending admin approval."
                };
            }
            catch (Exception ex)
            {
                return new UserResponseDTO
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

    }
}
