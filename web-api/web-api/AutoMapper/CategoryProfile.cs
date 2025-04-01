using AutoMapper;

namespace web_api.AutoMapper;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<Dtos.Category, Models.Category>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => (byte)src.Type))
            .ForMember(dest => dest.UserId, opt => opt.Ignore());

        CreateMap<Models.Account, Dtos.Account>();
    }
}
