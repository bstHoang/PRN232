using Microsoft.AspNetCore.Identity;
using Project.DTOs;
using Project.DTOs.Accounts;

namespace Project.Interfaces
{
    public interface IUserService
    {
        Task RegisterAsync(RegisterDto registerDto);
        Task<IdentityResult> VerifyAndCompleteRegistrationAsync(VerifyCodeDto verifyCodeDto);
        Task ResendCodeAsync(string email);
        Task<string> LoginAsync(LoginDto loginDto);
        Task<IEnumerable<AccountDto>> GetAllAccountsAsync();
        Task<AccountDto> GetAccountByIdAsync(string id);
    }
}