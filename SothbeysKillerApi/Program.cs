using SothbeysKillerApi.Repository;
using SothbeysKillerApi.Services;
using SothbeysKillerApi.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Storage in memory (1,2)
builder.Services.AddSingleton<IAuctionService, AuctionService>();//1
builder.Services.AddSingleton<ILotService, DefaultLotService>();//2

//builder.Services.AddTransient<IAuctionRepository, DbAuctionRepository>();

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