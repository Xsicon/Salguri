using Salguri.Application.DTOS;
using Salguri.Application.Interfaces;
using Salguri.Domain.Models;
using Supabase.Gotrue;
using Supabase.Gotrue.Mfa;


//using Supabase.Gotrue;
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

        //public async Task<UserResponseDTO> RegisterAsync(UserRequestDTO request)
        //{
        //    Session? session = null;

        //    //    var options = new SignUpOptions
        //    //    {
        //    //        Data = new Dictionary<string, object>
        //    //{
        //    //    { "full_name", $"{request.FullName}" },
        //    //    { "app_id", "business" }
        //    //}
        //    //};
        //    try
        //    {
        //        var response = await _client.Auth.SignUp(request.Email!, request.Password!);
        //        await _client.From<SalguriUsers>().Insert(new SalguriUsers
        //        {
        //            Id = session?.User?.Id,
        //            FullName = request.FullName,
        //            //Email = request.Email,
        //            RoleId = null, // Default role for new users
        //            IsActive = false // New users are inactive until approved by admin
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        return new UserResponseDTO
        //        {
        //            Success = false,
        //            Message = ex.Message
        //        };

        //    }

        //    return new UserResponseDTO
        //    {
        //        Success = true,
        //        Message = "Registered Successfully. Please verify your email."
        //    };
        //}

        public async Task<UserResponseDTO> RegisterAsync(UserRequestDTO request)
        {
            try
            {
                var response = await _client.Auth.SignUp(request.Email!, request.Password!);

                var user = response?.User;

                await _client.From<Profiles>().Insert(new Profiles
                {
                    Id = Guid.Parse(user?.Id!),
                    FullName = request.FullName,
                    Phone = null,
                    AvatarUrl = null,
                    Role = null,
                    CreatedAt = DateTime.Now

                });

                return new UserResponseDTO
                {
                    Success = true,
                    Message = "Registered successfully. Please verify your email."
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


        public async Task<UserResponseDTO> LoginAsync(LoginRequestDTO request)
        {
            try
            {
                var session = await _client.Auth.SignIn(request.Email!, request.Password!);

                if (session == null || session.User == null)
                    throw new Exception("Invalid email or password.");

                Guid userId;
                if (!Guid.TryParse(session.User.Id, out userId))
                    throw new Exception("Invalid user ID format.");

                //var response = await _client
                //           .From<Profiles>()
                //           .Where(x => x.Id == userId)
                //           .Select(x => new object[] { x.RoleId!, x.IsActive! })
                //           .Get();

                //var user = response.Models.SingleOrDefault();

                //if (user == null)
                //    throw new Exception("User profile not found.");


                return new UserResponseDTO
                {
                    Success = true,
                    AccessToken = session.AccessToken!,
                    RefreshToken = session.RefreshToken!,
                    Email = session.User.Email,
                    Message = "Login Successfull"
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
        public async Task<UserResponseDTO> VerifyOtp(VerifyOtpRequest request)
        {
            try
            {
                var session = await _client.Auth.VerifyOTP(
                request.Email!,
                request.Otp!,
                Constants.EmailOtpType.Signup
            );
                return new UserResponseDTO
                {
                    Success = true,
                    Message = "Email verified successfully",
                    AccessToken = session?.AccessToken,
                    RefreshToken = session?.RefreshToken
                };
            }
            catch (Exception)
            {
                return new UserResponseDTO
                {
                    Success = false,
                    Message = "Invalid or expired OTP"
                };
            }


        }
        public async Task<UserResponseDTO> ResendOtp(ResendOtpRequestDTO request)
        {
            try
            {
                // Constructor requires email as a positional argument — not object initializer
                // new SignInWithPasswordlessEmailOptions(email) is the correct signature

                await _client.Auth.SignInWithOtp(
                    new SignInWithPasswordlessEmailOptions(request.Email!)
                );

                return new UserResponseDTO
                {
                    Success = true,
                    Message = "OTP resent successfully. Please check your email."
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

        public async Task<TotpEnrollResponseDTO> EnrollTotpAsync(string accessToken, string refreshToken)
        {
            try
            {
                // We need the user to be logged in to enroll TOTP
                // So we set the session using the access token from registration
                await _client.Auth.SetSession(accessToken, refreshToken);

                // Tell Supabase we want to enroll a TOTP factor
                // MfaEnrollParams specifies what type of MFA we want
                var enrollment = await _client.Auth.Enroll(new MfaEnrollParams
                {
                    // FactorType.Totp tells Supabase we want authenticator app
                    // not SMS or email OTP
                    FactorType = "totp",
                    FriendlyName = "Salguri"
                });

                // enrollment.TOTP contains the QR code and secret
                if (enrollment?.Totp == null)
                {
                    return new TotpEnrollResponseDTO
                    {
                        Success = false,
                        Message = "Failed to generate QR code"
                    };
                }

                return new TotpEnrollResponseDTO
                {
                    Success = true,
                    Message = "Scan QR code with your authenticator app",

                    // QrCode is a base64 image string
                    // Frontend displays this as an actual QR code image
                    QrCode = enrollment.Totp.QrCode,

                    // Secret is the manual entry key
                    // Some users prefer to type this instead of scanning
                    SecretKey = enrollment.Totp.Secret,

                    // Save this FactorId — needed when verifying the code
                    FactorId = enrollment.Id
                };
            }
            catch (Exception ex)
            {
                return new TotpEnrollResponseDTO
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        // Method 2: Verify TOTP — called when user enters 6 digit code from app
        public async Task<UserResponseDTO> VerifyTotpAsync(TotpVerifyRequestDTO request, string accessToken, string refreshToken)
        {
            try
            {
                // Set session so Supabase knows which user is verifying
                await _client.Auth.SetSession(accessToken, refreshToken);

                // ChallengeAndVerify combines challenge + verify into ONE call
                // No need to call Challenge() then Verify() separately anymore
                // MfaChallengeAndVerifyParams takes FactorId and Code together
                var result = await _client.Auth.ChallengeAndVerify(
                    new MfaChallengeAndVerifyParams
                    {
                        // FactorId identifies which TOTP device to verify against
                        FactorId = request.FactorId!,

                        // Code is the 6 digit number from authenticator app
                        Code = request.Code!
                    }
                );

                // result is a Session object — if null verification failed
                if (result == null)
                {
                    return new UserResponseDTO
                    {
                        Success = false,
                        Message = "Invalid code. Please try again."
                    };
                }

                return new UserResponseDTO
                {
                    Success = true,
                    Message = "Authenticator verified successfully."
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
