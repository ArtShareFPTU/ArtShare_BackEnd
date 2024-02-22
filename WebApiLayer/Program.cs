using BusinessLogicLayer;
using DataAccessLayer;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using ModelLayer.BussinessObject;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ArtShareContext>(options =>
    options.UseSqlServer("server=NhaPhan\\SQLEXPRESS;user=sa;password=1234567890;database=ArtShare;"));


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


app.Run();