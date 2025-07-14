using Project.DTOs;
using Microsoft.AspNetCore.Identity;

namespace Project.Interfaces
{
    public interface IUserService
    {
        Task RegisterAsync(RegisterDto registerDto);
        Task<IdentityResult> VerifyAndCompleteRegistrationAsync(VerifyCodeDto verifyCodeDto);
        Task ResendCodeAsync(string email);
    }
}