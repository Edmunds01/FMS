using AutoMapper;

namespace web_api.AutoMapper;

public class AccountProfile : Profile
{
    public AccountProfile()
    {
        CreateMap<Dtos.Account, Models.Account>()
            .ForMember(dest => dest.InitialBalance, opt => opt.MapFrom(src => src.Balance));

        CreateMap<Dtos.NewAccount, Models.Account>()
            .ForMember(dest => dest.InitialBalance, opt => opt.MapFrom(src => src.Balance))
            .ForMember(dest => dest.AccountId, opt => opt.MapFrom(_ => 0));
    }
}
