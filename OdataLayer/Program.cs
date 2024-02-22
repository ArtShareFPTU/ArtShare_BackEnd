using BusinessLogicLayer;
using DataAccessLayer;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using ModelLayer.BussinessObject;

var builder = WebApplication.CreateBuilder(args);

static IEdmModel GetEdmModel()
{
    var modelBuilder = new ODataConventionModelBuilder();
    modelBuilder.EntitySet<Account>("Account");
    modelBuilder.EntitySet<Artwork>("Artwork");
    modelBuilder.EntitySet<ArtworkTag>("ArtworkTag");
    modelBuilder.EntitySet<ArtworkCategory>("ArtworkCategory");
    modelBuilder.EntitySet<Category>("Category");
    modelBuilder.EntitySet<Comment>("Comment");
    modelBuilder.EntitySet<Follow>("Follow");
    modelBuilder.EntitySet<Like>("Like");
    modelBuilder.EntitySet<Order>("Order");
    modelBuilder.EntitySet<OrderDetail>("OrderDetail");
    modelBuilder.EntitySet<Tag>("Tag");
    return modelBuilder.GetEdmModel();
}

builder.Services.AddControllers().AddOData(
    options => options.Select().Filter().OrderBy().Expand().Count().SetMaxTop(null).AddRouteComponents(
        "odata", GetEdmModel()));


// Add services to the container.

builder.Services.AddControllers();

builder.Services.DependencyInjectionConfiguration();

var app = builder.Build();

app.UseRouting();
app.UseEndpoints(endpoints => endpoints.MapControllers());


app.Run();