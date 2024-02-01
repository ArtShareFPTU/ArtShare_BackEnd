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

builder.Services.AddDbContext<ArtShareContext>(options =>
    options.UseSqlServer("server=NhaPhan\\SQLEXPRESS;user=sa;password=1234567890;database=ArtShare;"));

builder.Services.AddControllers().AddOData(
    options => options.Select().Filter().OrderBy().Expand().Count().SetMaxTop(null).AddRouteComponents(
        "odata", GetEdmModel()));


// Add services to the container.

builder.Services.AddControllers();

builder.Services.DependencyInjectionConfiguration();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Logging.AddConsole();

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

app.Run();