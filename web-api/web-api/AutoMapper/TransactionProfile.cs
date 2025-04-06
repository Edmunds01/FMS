using AutoMapper;

namespace web_api.AutoMapper;

public class TransactionProfile : Profile
{
    public TransactionProfile()
    {
        CreateMap<Dtos.Transaction, Models.Transaction>()
            .ForMember(dest => dest.UserId, opt => opt.Ignore());
        
        CreateMap<Dtos.NewTransaction, Models.Transaction>()
            .ForMember(dest => dest.UserId, opt => opt.Ignore());

        CreateMap<Models.Transaction, Dtos.Transaction>();
    }
}
