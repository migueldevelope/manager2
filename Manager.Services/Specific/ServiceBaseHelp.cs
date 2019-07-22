using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.Interfaces;
using Manager.Data;
using Manager.Services.Commons;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manager.Services.Specific
{
  public class ServiceBaseHelp : Repository<BaseHelp>, IServiceBaseHelp
  {
    private readonly ServiceGeneric<BaseHelp> serviceBaseHelp;
    private readonly IQueueClient queueClient;

    #region Constructor
    public ServiceBaseHelp(DataContext context, string serviceBusConnectionString, string queueName) : base(context)
    {
      try
      {
        queueClient = new QueueClient(serviceBusConnectionString, queueName);
        serviceBaseHelp = new ServiceGeneric<BaseHelp>(context);
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public void SetUser(IHttpContextAccessor contextAccessor)
    {
      User(contextAccessor);
      serviceBaseHelp._user = _user;
    }
    public void SetUser(BaseUser user)
    {
      _user = user;
      serviceBaseHelp._user = user;
    }
    #endregion

    #region BaseHelp
    public string Delete(string id)
    {
      try
      {
        BaseHelp item = serviceBaseHelp.GetNewVersion(p => p._id == id).Result;
        item.Status = EnumStatus.Disabled;
        serviceBaseHelp.Update(item, null).Wait();
        return "BaseHelp deleted!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public void SetAttachment(string idBaseHelp, string url, string fileName, string attachmentid)
    {
      try
      {
        BaseHelp basehelp = serviceBaseHelp.GetNewVersion(p => p._id == idBaseHelp).Result;
        basehelp.Attachment = new ViewCrudAttachmentField { Url = url, Name = fileName, _idAttachment = attachmentid };
        serviceBaseHelp.Update(basehelp, null).Wait();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string New(ViewCrudBaseHelp view)
    {
      try
      {
        BaseHelp basehelp = serviceBaseHelp.InsertNewVersion(new BaseHelp()
        {
          _id = view._id,
          Name = view.Name,
        }).Result;
        return "BaseHelp added!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public string UpdateLink(string link, string id)
    {
      try
      {
        BaseHelp basehelp = serviceBaseHelp.GetNewVersion(p => p._id == id).Result;
        basehelp.AccessLink = link;

        serviceBaseHelp.Update(basehelp, null).Wait();
        return "BaseHelp altered!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string Update(ViewCrudBaseHelp view)
    {
      try
      {
        BaseHelp basehelp = serviceBaseHelp.GetNewVersion(p => p._id == view._id).Result;

        basehelp.Name = view.Name;
        basehelp.Content = view.Content;
        basehelp.AccessLink = view.AccessLink;
        basehelp.Employee = view.Employee;
        basehelp.Infra = view.Infra;
        basehelp.Manager = view.Manager;

        serviceBaseHelp.Update(basehelp, null).Wait();


        return "BaseHelp altered!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string Count(string id)
    {
      try
      {
        var data = new ViewIdAccount() { _id = id, _idAccount = _user._idAccount };
        SendMessageAsync(JsonConvert.SerializeObject(data));
        return "count";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public ViewCrudBaseHelp Get(string id)
    {
      try
      {
        return serviceBaseHelp.GetNewVersion(p => p._id == id).Result.GetViewCrud();
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public ViewCrudBaseHelp GetByText(string text)
    {
      try
      {
        return serviceBaseHelp.GetNewVersion(p => (p.Name.ToLower() == text.ToLower() || p.Content.ToLower() == text.ToLower())).Result.GetViewCrud();
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public List<ViewListBaseHelp> List(ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        List<ViewListBaseHelp> detail = serviceBaseHelp.GetAllNewVersion(p => p.Name.ToUpper().Contains(filter.ToUpper()), count, count * (page - 1), "Name").Result
          .Select(x => new ViewListBaseHelp()
          {
            _id = x._id,
            Name = x.Name,
            AccessCount = x.AccessCount,
            Content = x.Content
          }).ToList();
        total = serviceBaseHelp.CountNewVersion(p => p.Name.ToUpper().Contains(filter.ToUpper())).Result;
        return detail;
      }
      catch (Exception e)
      {
        throw e;
      }
    }


    #endregion

    #region " ... private ... "

    private void SendMessageAsync(dynamic view)
    {
      try
      {
        var message = new Message(Encoding.UTF8.GetBytes(view));
        queueClient.SendAsync(message);
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public void RegisterOnMessageHandlerAndReceiveMesssages()
    {
      try
      {
        var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
        {
          MaxConcurrentCalls = 1,
          AutoComplete = false
        };

        queueClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    private async Task ProcessMessagesAsync(Message message, CancellationToken token)
    {
      var view = JsonConvert.DeserializeObject<ViewIdAccount>(Encoding.UTF8.GetString(message.Body));
      SetUser(new BaseUser()
      {
        _idAccount = view._idAccount
      });

      BaseHelp basehelp = serviceBaseHelp.GetNewVersion(p => p._id == view._id).Result;
      basehelp.AccessCount += 1;
      serviceBaseHelp.Update(basehelp, null).Wait();

      await queueClient.CompleteAsync(message.SystemProperties.LockToken);
    }

    private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
    {
      var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
      return Task.CompletedTask;
    }

    #endregion

  }
}
