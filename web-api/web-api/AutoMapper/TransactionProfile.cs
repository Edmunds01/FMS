using AutoMapper;

namespace web_api.AutoMapper;

public class TransactionProfile : Profile
{
    public TransactionProfile()
    {
        CreateMap<Dtos.Transaction, Models.Transaction>()
            .ForMember(dest => dest.UserId, opt => opt.Ignore())
            .ForMember(dest => dest.TransactionId, opt => opt.MapFrom(src => src.TransactionId == null ? 0 : src.TransactionId));

        CreateMap<Models.Transaction, Dtos.Transaction>();
    }
}
