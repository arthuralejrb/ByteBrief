using ByteBrief.Worker;
using ByteBrief.Worker.data;
using ByteBrief.Worker.scrapers;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ByteBriefContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddHttpClient<BleepingComputerScraper>();
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
