using AutoMapper;

namespace web_api.Services;

public abstract class BaseService(IHttpContextAccessor httpContextAccessor, IMapper mapper)
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    protected readonly IMapper _mapper = mapper;

    protected int UserId => GetUserIdFromClaims();

    private int GetUserIdFromClaims()
    {
        var isAuthenticated = _httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated;
        if (!isAuthenticated.HasValue || !isAuthenticated.Value)
        {
            throw new UnauthorizedAccessException("User ID claim not found");
        }

        var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("userId");

        return int.Parse(userIdClaim?.Value ?? throw new NotSupportedException("Login or set DefaultUserId"));
    }
}
