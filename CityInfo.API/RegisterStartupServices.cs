using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.StaticFiles;
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

    builder.Services.AddControllers(options => { options.ReturnHttpNotAcceptable = true; })
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

    return builder;
  }
}