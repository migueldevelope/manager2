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
  /// <summary>
  /// 
  /// </summary>
  [Produces("application/json")]
  [Route("manager/configurationnotifications")]
  public class ConfigurationNotificationsController : DefaultController
  {
    private readonly IServiceConfigurationNotifications service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_service"></param>
    /// <param name="contextAccessor"></param>
    public ConfigurationNotificationsController(IServiceConfigurationNotifications _service, IHttpContextAccessor contextAccessor) : base(contextAccessor)
    {
      service = _service;
      service.SetUser(contextAccessor);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="view"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("new")]
    public async Task<string> Post([FromBody]ConfigurationNotification view)
    {
      return await Task.Run(() =>service.New(view));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("list")]
    public async Task<List<ConfigurationNotification>> List( int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.List(ref total, count, page, filter);
      Response.Headers.Add("x-total-count", total.ToString());
      return await Task.Run(() =>result);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("get")]
    public async Task<ConfigurationNotification> List(string id)
    {
      return await Task.Run(() =>service.Get(id));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="view"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    [Route("update")]
    public async Task<string> Update([FromBody]ConfigurationNotification view)
    {
      return await Task.Run(() =>service.Update(view));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [Authorize]
    [HttpDelete]
    [Route("delete/{id}")]
    public async Task<string> Delete(string id)
    {
      return await Task.Run(() =>service.Remove(id));
    }

  }
}