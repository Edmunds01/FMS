using AutoMapper;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;

namespace web_api.Services;

public abstract class BaseService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _configuration;
    private readonly IHostEnvironment _env;

    protected readonly IMapper _mapper;

    protected int UserId => GetUserIdFromClaims();

    protected BaseService(IHttpContextAccessor httpContextAccessor, IMapper mapper, IConfiguration configuration, IHostEnvironment env)
    {
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
        _configuration = configuration;
        _env = env;
    }

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
