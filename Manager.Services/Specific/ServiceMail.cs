using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.Interfaces;
using Manager.Data;
using Manager.Services.Auth;
using Manager.Services.Commons;
using Microsoft.AspNetCore.Http;
using System;

namespace Manager.Services.Specific
{
  public class ServiceMail : Repository<MailMessage>, IServiceMail
  {
    private readonly ServiceAuthentication serviceAuthentication;
    private readonly ServiceMailMessage serviceMailMessage;
    private readonly ServiceMailModel serviceMailModel;
    private readonly ServiceGeneric<MailLog> serviceMail;
    private readonly ServiceGeneric<Person> servicePerson;
    private readonly ServiceSendGrid serviceSendGrid;

    #region Constructor
    public ServiceMail(DataContext context) : base(context)
    {
      try
      {
        serviceAuthentication = new ServiceAuthentication(context);
        serviceMail = new ServiceGeneric<MailLog>(context);
        serviceMailMessage = new ServiceMailMessage(context);
        serviceMailModel = new ServiceMailModel(context);
        servicePerson = new ServiceGeneric<Person>(context);
        serviceSendGrid = new ServiceSendGrid(context);
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public void SetUser(IHttpContextAccessor contextAccessor)
    {
      User(contextAccessor);
      serviceMail._user = _user;
      serviceMailMessage._user = _user;
      serviceMailModel._user = _user;
      servicePerson._user = _user;
      serviceSendGrid.SetUser(_user);
    }
    public void SetUser(BaseUser user)
    {
      serviceMail._user = user;
      serviceMailMessage._user = user;
      serviceMailModel._user = user;
      servicePerson._user = user;
      serviceSendGrid.SetUser(user);
    }
    #endregion

    #region Mail
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
        throw e;
      }
    }
    #endregion

  }
}
