using System.Data;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using SothbeysKillerApi.Contexts;
using SothbeysKillerApi.ExceptionHandlers;
using SothbeysKillerApi.Repository;
using SothbeysKillerApi.Repository.Interface;
using SothbeysKillerApi.Services;
using SothbeysKillerApi.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<LotDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DB"));
});
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
builder.Services.AddScoped<IDbConnection>(sp =>
    new NpgsqlConnection("Server=localhost;Port=5432;Database=auction_db;Username=postgres;Password=123456")
);

//Storage in db (1,2,3,4)
builder.Services.AddTransient<IAuctionService, DbAuctionService>();//1 
builder.Services.AddTransient<IAuctionRepository, DbAuctionRepository>();//2
builder.Services.AddTransient<ILotService, DefaultLotService>();//3
builder.Services.AddTransient<ILotRepository, DbLotRepository>();//4 db
//builder.Services.AddSingleton<ILotRepository, InMemoryLotRepository>();//4 memory

builder.Services.AddExceptionHandler<EntityExceptionNullreferenceHandler>();
builder.Services.AddExceptionHandler<LotExceptionValidationHandler>();
builder.Services.AddExceptionHandler<ServerExceptionsHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseExceptionHandler();

app.MapControllers();

app.Run();