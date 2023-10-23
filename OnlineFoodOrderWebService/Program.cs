using Microsoft.EntityFrameworkCore;
using OnlineFoodOrderDALCrossPlatform.Models;
using OnlineFoodOrderDALCrossPlatform;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<CustomerRepository>(new CustomerRepository(new OnlineFoodOrderDBContext(new DbContextOptions<OnlineFoodOrderDBContext>())));
builder.Services.AddSingleton<AdminRepository>(new AdminRepository(new OnlineFoodOrderDBContext(new DbContextOptions<OnlineFoodOrderDBContext>())));
builder.Services.AddSingleton<CommonRepository>(new CommonRepository(new OnlineFoodOrderDBContext(new DbContextOptions<OnlineFoodOrderDBContext>())));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
