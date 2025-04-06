using AutoMapper;

namespace web_api.Services;

public abstract class BaseService(IHttpContextAccessor httpContextAccessor, IMapper mapper, IConfiguration configuration, IHostEnvironment env)
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly IConfiguration _configuration = configuration;
    private readonly IHostEnvironment _env = env;

    protected readonly IMapper _mapper = mapper;

    protected int UserId => GetUserIdFromClaims();

    private int GetUserIdFromClaims()
    {
        var isAuthenticated = _httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated;
        if (_env.IsProduction() && (!isAuthenticated.HasValue || !isAuthenticated.Value))
        {
            throw new UnauthorizedAccessException("User ID claim not found");
        }

        var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("userId");

        return int.Parse(userIdClaim?.Value ?? _configuration["DefaultUserId"] ?? throw new NotSupportedException("Login or set DefaultUserId"));
    }
}
