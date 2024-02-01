using AutoMapper;
using ModelLayer.BussinessObject;
using ModelLayer.DTOS.Request.Order;

namespace BusinessLogicLayer;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateOrder, Order>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
            .ForMember(dest => dest.CreateDate, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => "PayPal"))
            .ForMember(dest => dest.OrderDetails, opt => opt.MapFrom(src => src.OrderDetails));
        ;
    }
    
}