using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CityInfo.API.Filters;

public class LogActionFilter : ActionFilterAttribute
{
  
  public override void OnActionExecuting(ActionExecutingContext filterContext)
  {
    Log("OnActionExecuting", filterContext.RouteData);       
  }

  public override void OnActionExecuted(ActionExecutedContext filterContext)
  {
    Log("OnActionExecuted", filterContext.RouteData);       
  }

  public override void OnResultExecuting(ResultExecutingContext filterContext)
  {
    Log("OnResultExecuting", filterContext.RouteData);       
  }

  public override void OnResultExecuted(ResultExecutedContext filterContext)
  {
    Log("OnResultExecuted", filterContext.RouteData);       
  }

  private void Log(string methodName, RouteData routeData)
  {
    var controllerName = routeData.Values["controller"];
    var actionName = routeData.Values["action"];
    var message = $"{methodName} controller:{controllerName} action:{actionName}";
    Debug.WriteLine(message, "Action Filter Log");
  }

}