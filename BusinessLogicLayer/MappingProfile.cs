using AutoMapper;
using ModelLayer.BussinessObject;
using ModelLayer.DTOS.Request.Order;
using ModelLayer.DTOS.Response;
using ModelLayer.Enum;

namespace BusinessLogicLayer;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateOrder, Order>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
            .ForMember(dest => dest.CreateDate, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => OderStatus.Unpaid.ToString()))
            .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => "PayPal")) ;

        CreateMap<ArtworkRespone, Artwork>();

    }
    
}