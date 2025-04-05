using AutoMapper;

namespace web_api.AutoMapper;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<Dtos.NewCategory, Models.Category>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => (byte)src.Type))
            .ForMember(dest => dest.UserId, opt => opt.Ignore());

        CreateMap<Models.Category, Dtos.Category>()
            .ForMember(dest => dest.ShowDeleteButton, opt => opt.MapFrom(_ => true))
            .ForMember(dest => dest.SumOfTransactions, opt => opt.Ignore())
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => (Dtos.CategoryType)src.Type));
    }
}
