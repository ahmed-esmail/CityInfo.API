using CityInfo.API;

WebApplication.CreateBuilder(args)
  .RegisterServices().Build().SetupMiddleware().Run();
  