using BusinessLogicLayer.IService;
using BusinessLogicLayer.Service;
using DataAccessLayer.BussinessObject.IRepository;
using DataAccessLayer.BussinessObject.Repository;

using Microsoft.Extensions.DependencyInjection;

namespace BusinessLogicLayer;

public static class DependencyInjection
{
    public static IServiceCollection DependencyInjectionConfiguration(this IServiceCollection services)
    {
        //Add DI Container
        //Repository
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<IArtworkRepository, ArtworkRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<ICommentRepository, CommentRepository>();
        services.AddScoped<IFollowRepository, FollowRepository>();
        services.AddScoped<ILikeRepository, LikeRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<ITagRepository, TagRepository>();
        services.AddScoped<IInboxRepository, InboxRepository>();
        services.AddScoped<IOrderDetailRepository, OderDetailRepository>();

        //Service
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IArtworkService, ArtworkService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<ICommentService, CommentService>();
        services.AddScoped<IFollowService, FollowService>();
        services.AddScoped<ILikeService, LikeService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<ITagService, TagService>();
        services.AddScoped<IInboxService, InboxService>();


        //AUTOMAPPER
        services.AddAutoMapper(typeof(MappingProfile).Assembly);

        return services;
    }
}