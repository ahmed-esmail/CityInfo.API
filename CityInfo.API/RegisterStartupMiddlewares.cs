namespace CityInfo.API;

public static class RegisterStartupMiddlewares
{
  public static WebApplication SetupMiddleware(this WebApplication app)
  {
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

    return app;
  }
}