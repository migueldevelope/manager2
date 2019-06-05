using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Manager.Core.Business;
using Manager.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Manager.Controllers
{
  [Produces("application/json")]
  [Route("configurationnotifications")]
  public class ConfigurationNotificationsController : DefaultController
  {
    private readonly IServiceConfigurationNotifications service;

    public ConfigurationNotificationsController(IServiceConfigurationNotifications _service, IHttpContextAccessor contextAccessor) : base(contextAccessor)
    {
      service = _service;
      service.SetUser(contextAccessor);
    }

    [HttpPost]
    [Route("new")]
    public async Task<string> Post([FromBody]ConfigurationNotification view)
    {
      return await service.New(view);
    }

    [Authorize]
    [HttpGet]
    [Route("list")]
    public async Task<List<ConfigurationNotification>> List( int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.List(ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return await result;
    }

    [Authorize]
    [HttpGet]
    [Route("get")]
    public async Task<ConfigurationNotification> List(string id)
    {
      return await service.Get(id);
    }

    [Authorize]
    [HttpPut]
    [Route("update")]
    public async Task<string> Update([FromBody]ConfigurationNotification view)
    {
      return await service.Update(view);
    }

    [Authorize]
    [HttpDelete]
    [Route("delete/{id}")]
    public async Task<string> Delete(string id)
    {
      return await service.Remove(id);
    }

  }
}