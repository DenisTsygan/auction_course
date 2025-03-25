using System.Data;
using Npgsql;
using SothbeysKillerApi.Repository;
using SothbeysKillerApi.Repository.Interface;
using SothbeysKillerApi.Services;
using SothbeysKillerApi.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

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