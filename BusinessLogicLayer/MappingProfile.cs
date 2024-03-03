using AutoMapper;
using ModelLayer.BussinessObject;
using ModelLayer.DTOS.Request.Account;
using ModelLayer.DTOS.Request.Order;
using ModelLayer.DTOS.Response;
using ModelLayer.DTOS.Response.Account;
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

        CreateMap<Artwork, ArtworkRespone>()
            .ForMember(c => c.Id, opt => opt.MapFrom(a => a.Id))
            .ForMember(c => c.AccountId, opt => opt.MapFrom(a => a.AccountId))
            .ForMember(c => c.Title, opt => opt.MapFrom(a => a.Title))
            .ForMember(c => c.Description, opt => opt.MapFrom(a => a.Description))
            .ForMember(c => c.Url, opt => opt.MapFrom(a => a.Url))
            .ForMember(c => c.Likes, opt => opt.MapFrom(a => a.Likes))
            .ForMember(c => c.Fee, opt => opt.MapFrom(a => a.Fee))
            .ForMember(c => c.Status, opt => opt.MapFrom(a => a.Status));

        CreateMap<UpdateAccountRequest, Account>()
            .ForMember(c => c.FullName, opt => opt.MapFrom(a => a.FullName))
            .ForMember(c => c.Description, opt => opt.MapFrom(a => a.Description))
            .ForMember(c => c.UserName, opt => opt.MapFrom(a => a.UserName));

        CreateMap<Account, AccountResponse>()
            .ForMember(c => c.Id, opt => opt.MapFrom(a => a.Id))
            .ForMember(c => c.Email, opt => opt.MapFrom(a => a.Email))
            .ForMember(c => c.Password, opt => opt.MapFrom(a => a.Password))
            .ForMember(c => c.Description, opt => opt.MapFrom(a => a.Description))
            .ForMember(c => c.FullName, opt => opt.MapFrom(a => a.FullName))
            .ForMember(c => c.Birthday, opt => opt.MapFrom(a => a.Birthday))
            .ForMember(c => c.UserName, opt => opt.MapFrom(a => a.UserName))
            .ForMember(c => c.CreateDate, opt => opt.MapFrom(a => a.CreateDate))
            .ForMember(c => c.Avatar, opt => opt.MapFrom(a => a.Avatar))
            .ForMember(c => c.Status, opt => opt.MapFrom(a => a.Status))
            .ForMember(c => c.NumArtwork, opt => opt.MapFrom(a => a.Artworks.Count()))
            .ForMember(c => c.NumFollowers, opt => opt.MapFrom(a => a.FollowFollowers.Count()))
            .ForMember(c => c.NumFollowings, opt => opt.MapFrom(a => a.FollowArtists.Count()));

        CreateMap<CreateAccountRequest, Account>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
            .ForMember(c => c.FullName, opt => opt.MapFrom(a => a.FullName))
            .ForMember(c => c.Description, opt => opt.MapFrom(a => a.Description))
            .ForMember(c => c.UserName, opt => opt.MapFrom(a => a.UserName))
            .ForMember(c => c. Birthday, opt => opt.MapFrom(a => a.Birthday))
            .ForMember(c => c.Email, opt => opt.MapFrom(a => a.Email))
            .ForMember(c => c.Password, opt => opt.MapFrom(a => a.Password));

    }

}