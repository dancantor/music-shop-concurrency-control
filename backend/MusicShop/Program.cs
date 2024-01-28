using Microsoft.EntityFrameworkCore;
using MusicShop.Business.Service;
using MusicShop.Business.Service.Interfaces;
using MusicShop.ConcurrencyControl.Services;
using MusicShop.DataAccess.Context;
using MusicShop.DataAccess.Repository;
using MusicShop.DataAccess.Repository.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<EmployeeContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("EmployeeConnection")).UseLazyLoadingProxies());
builder.Services.AddDbContext<InstrumentContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("InstrumentConnection")).UseLazyLoadingProxies());

builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IMusicalInstrumentRepository, MusicalInstrumentRepository>();
builder.Services.AddScoped<IInstrumentPurchaseRepository, InstrumentPurchaseRepository>();

builder.Services.AddTransient<IEmployeeService, EmployeeService>();
builder.Services.AddTransient<IMusicalInstrumentService, MusicalInstrumentService>();
builder.Services.AddTransient<IInstrumentPurchaseService, InstrumentPurchaseService>();
builder.Services.AddSingleton<ConcurrencyControlService>();
builder.Services.AddSingleton<AbortTransactionService>();
builder.Services.AddHostedService<DeadlockDetectionService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.MapControllers();

app.Run();