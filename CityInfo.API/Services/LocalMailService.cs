using System.Diagnostics;

namespace CityInfo.API.Services;

public class LocalMailService : IMailService
{
  private readonly string _mailTo;
  private readonly string _mailFrom;
  
  public LocalMailService(IConfiguration config)
  {
    _mailTo = config.GetValue<string>("mailSettings:mailToAddress");
    _mailFrom = config.GetValue<string>("mailSettings:mailFromAddress");
  }
  

  public void Send(string subject, string message)
  {
    // send mail - output to debug window
    Debug.WriteLine($"Mail from {_mailFrom} to {_mailTo}, with {nameof(LocalMailService)}.");
    Debug.WriteLine($"Subject: {subject}");
    Debug.WriteLine($"Message: {message}");
  }
}