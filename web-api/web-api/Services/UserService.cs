using AutoMapper;
using web_api.Repository.Interfaces;
using web_api.Services.Interfaces;

namespace web_api.Services;

public class UserService(
    IUserRepository userRepository,
    IHttpContextAccessor httpContextAccessor,
    IConfiguration configuration,
    IHostEnvironment env,
    IMapper mapper
    ) : BaseService(httpContextAccessor, mapper, configuration, env), IUserService
{
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<string> GetUserEmailAsync()
    {
        var user = await _userRepository.GetByIdStrictAsync(UserId);

        return user.Email;
    }

    public async Task ChangeUserPasswordAsync(string oldPassword, string newPassword)
    {
        var user = await _userRepository.GetByIdAsync(UserId);

        if (user != null && Helper.PasswordHelper.ComparePasswords(user.PasswordHash, oldPassword))
        {
            user.PasswordHash = Helper.PasswordHelper.GenerateVarbinary64FromPassword(newPassword);
            await _userRepository.SaveChangesAsync();
            return;
        }

        throw new NotSupportedException("User not found or password is incorrect");
    }
}
