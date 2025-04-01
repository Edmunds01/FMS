using AutoMapper;

namespace web_api.AutoMapper;

public class AccountProfile : Profile
{
    public AccountProfile()
    {
        CreateMap<Dtos.Account, Models.Account>()
            .ForMember(dest => dest.InitialBalance, opt => opt.MapFrom(src => src.Balance));

        CreateMap<Models.Account, Dtos.Account>();
    }
}
