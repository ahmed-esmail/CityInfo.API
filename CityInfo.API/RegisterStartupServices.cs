using Microsoft.AspNetCore.StaticFiles;

namespace CityInfo.API;

public static class RegisterStartupServices
{
  public static WebApplicationBuilder RegisterServices(this WebApplicationBuilder builder)
  {
    builder.Services.AddControllers((options) => { options.ReturnHttpNotAcceptable = true; })
      .AddXmlDataContractSerializerFormatters();
    builder.Services.AddSingleton<FileExtensionContentTypeProvider>();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    
    return builder;
  }
}