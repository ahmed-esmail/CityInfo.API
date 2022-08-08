using CityInfo.API.DbContexts;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace CityInfo.API;

public static class RegisterStartupServices
{
  public static WebApplicationBuilder RegisterServices(this WebApplicationBuilder builder)
  {
    Log.Logger = new LoggerConfiguration()
      .MinimumLevel.Debug().WriteTo.Console()
      .WriteTo.File("logs/cityinfo.txt", rollingInterval: RollingInterval.Day)
      .CreateLogger();

    builder.Services.AddControllers(options =>
      {
        options.ReturnHttpNotAcceptable = true;
      })
      .AddNewtonsoftJson()
      .AddXmlDataContractSerializerFormatters();
    builder.Host.UseSerilog();
#if DEBUG
    builder.Services.AddTransient<IMailService, LocalMailService>();
#else
    builder.Services.AddTransient<IMailService, CloudMailService>();
#endif
    builder.Services.AddSingleton<CitiesDataStore>();
    builder.Services.AddSingleton<FileExtensionContentTypeProvider>();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddScoped<ICityInfoRepository, CityInfoRepository>();
    builder.Services.AddDbContext<CityInfoContext>(
      option => option.UseSqlServer(builder.Configuration.GetConnectionString("CityInfoContext")
                                                      ?? throw new InvalidOperationException("Connection String for CityInfoContext Not Found")));
    builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    return builder;
  }
}