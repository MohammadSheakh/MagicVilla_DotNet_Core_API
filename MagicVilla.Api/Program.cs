using MagicVilla.Api.Database;
using MagicVilla.Api.Helper.AutoMapper;
using MagicVilla.Api.Helper.Logger;
using MagicVilla.Api.Modules.Villas.Services.Repository;
using MagicVilla.Api.Modules.Villas.Services.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//----------------- Log ----
Log.Logger = new LoggerConfiguration().MinimumLevel.Information()
    .WriteTo.File("Log/VillaLogs.txt", rollingInterval: RollingInterval.Day).CreateLogger();

builder.Host.UseSerilog(); //⚫ instead of built in logging .. use Serilog .. 

//----------------- Auto Mapper Service ----
builder.Services.AddAutoMapper(typeof(MappingConfig));

//---------------- VillaRepository ----------
builder.Services.AddScoped<IVillaRepository, VillaRepository>(); // for dependency injection

//builder.Services.AddControllers();
builder.Services.AddControllers(
    option =>
    {
        //⚫option.ReturnHttpNotAcceptable = true;
    }
    ).AddNewtonsoftJson();

// ⚫ ----------------- Database Service ----
builder.Services.AddDbContext<ApplicationDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultSQLConnection"));
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//---------------------- ⚫ we have to register service into container 
// as Logging is not a built in container service .. we have to register it
// maximum lifetime -> AddSingleton - AddScope - AddTransient

// custom service 
builder.Services.AddSingleton<ILogging, Logging>(); // <Interface, Implementation of that>

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
