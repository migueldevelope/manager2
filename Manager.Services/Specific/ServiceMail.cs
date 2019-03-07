using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.Interfaces;
using Manager.Core.Views;
using Manager.Data;
using Manager.Services.Auth;
using Manager.Services.Commons;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;

namespace Manager.Services.Specific
{
  public class ServiceMail : Repository<MailMessage>, IServiceMail
  {
    private readonly ServiceAuthentication serviceAuthentication;
    private readonly ServiceMailMessage mailMessageService;
    private readonly ServiceMailModel mailModelService;
    private readonly ServiceGeneric<MailLog> mailService;
    private readonly ServiceGeneric<Person> personService;
    private readonly ServiceSendGrid serviceSendGrid;
    private new readonly DataContext _context;

    public BaseUser user { get => _user; set => user = _user; }

    public ServiceMail(DataContext context)
      : base(context)
    {
      try
      {
        _context = context;
        mailMessageService = new ServiceMailMessage(context);
        mailModelService = new ServiceMailModel(context);
        mailService = new ServiceGeneric<MailLog>(context);
        personService = new ServiceGeneric<Person>(context);
        serviceSendGrid = new ServiceSendGrid(context);
        serviceAuthentication = new ServiceAuthentication(context);
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string SendMail(string link, string mail, string password, string idmail)
    {
      try
      {
        //ViewPerson view = serviceAuthentication.AuthenticationMail(person);
        //using (var client = new HttpClient())
        //{
        //  client.BaseAddress = new Uri(link);
        //  //var data = new
        //  //{
        //  //  Mail = mail,
        //  //  Password = password
        //  //};
        //  ////Authentication
        //  //var json = JsonConvert.SerializeObject(data);
        //  //var content = new StringContent(json);
        //  //content.Headers.ContentType.MediaType = "application/json";
        //  //client.DefaultRequestHeaders.Add("ContentType", "application/json");
        //  //var result = client.PostAsync("manager/authentication/encrypt", content).Result;

        //  //var resultContent = result.Content.ReadAsStringAsync().Result;
        //  //var auth = JsonConvert.DeserializeObject<ViewPerson>(resultContent);
        //  client.DefaultRequestHeaders.Add("Authorization", "Bearer " + view.Token);
        //  var resultMail = client.PostAsync("mail/sendmail/" + idmail, null);
        //  return view.Token;
        //}
        throw new Exception("Not Implemented!");
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public void SetUser(IHttpContextAccessor contextAccessor)
    {
      User(contextAccessor);
      mailMessageService._user = _user;
      mailModelService._user = _user;
      mailService._user = _user;
      personService._user = _user;
      serviceSendGrid._user = _user;
    }

  }
}
