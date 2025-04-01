using AutoMapper;
using System.Security.Claims;

namespace web_api.Services;

public abstract class BaseService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    protected readonly IMapper _mapper;

    protected int UserId => GetUserIdFromClaims();

    protected BaseService(IHttpContextAccessor httpContextAccessor, IMapper mapper)
    {
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
    }

    private int GetUserIdFromClaims()
    {
        var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("userId");
        if (userIdClaim == null)
        {
            throw new UnauthorizedAccessException("User ID claim not found");
        }

        return int.Parse(userIdClaim.Value);
    }
}
