using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Manager.Web
{
  public class ErrorHandlingMiddleware
  {
    private readonly RequestDelegate _next;
    public ErrorHandlingMiddleware(RequestDelegate next)
    {
      _next = next;
    }
    public async Task Invoke(HttpContext context)
    {
      try
      {
        await _next(context);
      }
      catch (Exception ex)
      {
        await HandleServiceExceptionAsync(context, ex);
      }
    }
    private static Task HandleServiceExceptionAsync(HttpContext context, Exception exception)
    {
      var code = HttpStatusCode.BadRequest;
      var result = JsonConvert.SerializeObject(new { error = exception.Message });
      context.Response.ContentType = "application/json";
      context.Response.StatusCode = (int)code;
      return context.Response.WriteAsync(result);
    }
  }
}
