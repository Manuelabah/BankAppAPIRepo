using BankAPPAPICore.Business.Utilities;
using MediatR;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog(); // Add this line to use Serilog for logging
builder.Services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog());

// Add services to the container.
var assembly = AppDomain.CurrentDomain.Load("BankAPPAPICore.Business");
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assembly));
//builder.Services.AddMediatR(typeof(Program).Assembly);
builder.Services.AddTransient< ICacheService, CacheService > ();
builder.Services.AddMemoryCache();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
