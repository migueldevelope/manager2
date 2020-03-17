using Manager.Core.Business;
using Manager.Core.Interfaces;
using Manager.Core.Views;
using Manager.Data;
using Manager.Services.Commons;
using MongoDB.Driver.Linq;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Manager.Core.Base;
using Manager.Services.Auth;
using Manager.Views.Enumns;
using Manager.Views.BusinessView;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using System.Text;
using Newtonsoft.Json;
using Manager.Views.BusinessList;
using MongoDB.Bson;

namespace Manager.Services.Specific
{
  public class ServiceAutoManager : Repository<AutoManager>, IServiceAutoManager
  {

    private readonly ServiceAuthentication serviceAuthentication;
    private readonly ServiceGeneric<AutoManager> serviceAutoManager;
    private readonly ServiceGeneric<MailLog> serviceMailLog;
    private readonly ServiceGeneric<MailMessage> serviceMailMessage;
    private readonly ServiceMailModel serviceMailModel;
    //private readonly ServiceGeneric<Person> servicePerson;
    private readonly IServicePerson servicePerson;
    private readonly ServiceWorkflow serviceWorkflow;
    private readonly IQueueClient queueClient;
    private readonly string path;

    #region Constructor
    public ServiceAutoManager(DataContext context, DataContext contextLog, IServiceControlQueue serviceControlQueue, IServicePerson _servicePerson, string _path) : base(context)
    {
      try
      {
        queueClient = new QueueClient(serviceControlQueue.ServiceBusConnectionString(), "structmanager");
        serviceAuthentication = new ServiceAuthentication(context, contextLog, serviceControlQueue, _path);
        serviceAutoManager = new ServiceGeneric<AutoManager>(context);
        serviceMailLog = new ServiceGeneric<MailLog>(contextLog);
        serviceMailModel = new ServiceMailModel(context);
        serviceMailMessage = new ServiceGeneric<MailMessage>(contextLog);
        //servicePerson = new ServiceGeneric<Person>(context);
        servicePerson = _servicePerson;
        serviceWorkflow = new ServiceWorkflow(context, contextLog, serviceControlQueue, _path);
        path = _path;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public void SetUser(IHttpContextAccessor contextAccessor)
    {
      User(contextAccessor);
      serviceAutoManager._user = _user;
      serviceMailLog._user = _user;
      serviceMailMessage._user = _user;
      serviceMailModel.SetUser(_user);
      if (servicePerson != null)
        servicePerson.SetUser(_user);
      serviceWorkflow.SetUser(_user);
    }
    public void SetUser(BaseUser user)
    {
      _user = user;
      serviceAutoManager._user = user;
      serviceMailLog._user = user;
      serviceMailMessage._user = user;
      serviceMailModel.SetUser(user);
      if(servicePerson != null)
        servicePerson.SetUser(user);
      serviceWorkflow.SetUser(user);
    }
    #endregion

    #region AutoManager
    public List<ViewAutoManagerPerson> List(string idManager, ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        int skip = (count * (page - 1));
        if (filter == null)
          return null;

        if (filter != string.Empty)
        {
          var listEnd = ListEnd(idManager, filter);
          total = listEnd.Count();

          return listEnd.Skip(skip).Take(count).ToList();
        }
        else
          return ListOpen(idManager, ref total, count, page, filter);
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public List<ViewAutoManagerPerson> ListOpen(string idManager, ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        int skip = (count * (page - 1));
        List<ViewAutoManagerPerson> list = (from person in servicePerson.GetAllNewVersion(p => p.Status == EnumStatus.Enabled).Result
                    where person.TypeUser != EnumTypeUser.Support && person.TypeUser != EnumTypeUser.Administrator && person.StatusUser != EnumStatusUser.Disabled && person.Manager == null && person.StatusUser != EnumStatusUser.Disabled && person._id != idManager
                    select person).ToList().Select(person => new ViewAutoManagerPerson { IdPerson = person._id, NamePerson = person.User.Name, Status = EnumStatusAutoManagerView.Open }).Skip(skip).Take(count).ToList();

        total = servicePerson.CountNewVersion(person => person.TypeUser != EnumTypeUser.Support && person.TypeUser != EnumTypeUser.Administrator && person.StatusUser != EnumStatusUser.Disabled && person.Manager == null && person.StatusUser != EnumStatusUser.Disabled && person._id != idManager).Result;
   
        if (list.Count > 0)
          list.FirstOrDefault().total = total;

        return list;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public List<ViewAutoManagerPerson> ListEnd(string idManager, string filter)
    {
      try
      {
        List<ViewAutoManagerPerson> result = new List<ViewAutoManagerPerson>();
        foreach (var item in servicePerson.GetAllNewVersion(p => p.Manager._id != idManager && p._id != idManager && p.TypeJourney != EnumTypeJourney.OutOfJourney && p.User.Name.ToUpper().Contains(filter.ToUpper())).Result.ToList())
        {
          ViewAutoManagerPerson view = new ViewAutoManagerPerson();
          long exists = serviceAutoManager.CountNewVersion(p => p.Person._id == item._id && p.Requestor._id == idManager && p.StatusAutoManager == EnumStatusAutoManager.Requested).Result;
          long existsManager = servicePerson.CountNewVersion(p => p._id == item._id && p.Manager._id != null).Result;
          view.IdPerson = item._id;
          view.NamePerson = item.User.Name;
          if (exists > 0)
            view.Status = EnumStatusAutoManagerView.Requestor;
          else if (existsManager > 0)
            view.Status = EnumStatusAutoManagerView.Manager;
          else
            view.Status = EnumStatusAutoManagerView.Open;
          result.Add(view);
        }
        return result;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public void SetManagerPerson(ViewManager view, string idPerson)
    {
      try
      {
        var person = servicePerson.GetNewVersion(p => p._id == idPerson).Result;
        var manager = servicePerson.GetNewVersion(p => p._id == view.IdManager).Result;
        if (view.Status == EnumStatusAutoManagerView.Open)
        {
          person.Manager = new BaseFields() { _id = manager._id, Mail = manager.User.Mail, Name = manager.User.Name };
          var exists = servicePerson.CountNewVersion(p => p.Manager._id == view.IdManager).Result;
          //if (exists == 0 & manager.TypeUser == EnumTypeUser.Employee)
          //{
          //  manager.TypeUser = EnumTypeUser.Manager;
          //  servicePerson.Update(manager, null).Wait();
          //}

          person.Manager = servicePerson.UpdateManager(person, manager._id, null);
          
        }
        else
        {
          var viewFlow = new ViewFlow
          {
            IdPerson = idPerson,
            Type = EnumTypeFlow.Manager
          };
          var auto = new AutoManager
          {
            Person = person.GetViewListBase(),
            Requestor = manager.GetViewListBase(),
            StatusAutoManager = EnumStatusAutoManager.Requested,
            Status = EnumStatus.Enabled,
            OpenDate = DateTime.Now,
            Workflow = serviceWorkflow.NewFlow(viewFlow).Select(p => p._id).ToList()
          };
          serviceAutoManager.InsertNewVersion(auto).Wait();
          //searsh model mail database
          var model = serviceMailModel.AutoManager(path);
          if (model.StatusMail == EnumStatus.Disabled)
            return;

          var url = path + "manager/automanager/" + person._id.ToString() + "/approved/" + manager._id.ToString();
          var message = new MailMessage
          {
            Type = EnumTypeMailMessage.Put,
            Name = model.Name,
            Url = url,
            Body = " { '_idWorkFlow': '" + auto.Workflow + "' } "
          };
          var idMessageApv = serviceMailMessage.InsertNewVersion(message).Result._id;
          var requestor = servicePerson.GetAllNewVersion(p => p._id == auto.Requestor._id).Result.FirstOrDefault();
          var body = model.Message.Replace("{Person}", serviceWorkflow.GetNewVersion(p => p._id == auto.Workflow.FirstOrDefault()).Result.Requestor.Name).Replace("{Manager}", requestor.User.Name);
          body = body.Replace("{Requestor}", auto.Requestor.Name);
          body = body.Replace("{Employee}", person.User.Name);
          // approved link
          body = body.Replace("{Approved}", model.Link + "genericmessage/" + idMessageApv.ToString());
          url = path + "manager/automanager/" + person._id.ToString() + "/disapproved/" + manager._id.ToString();
          //disapproved link
          message.Url = url;
          message._id = ObjectId.GenerateNewId().ToString();
          var idMessageDis = serviceMailMessage.InsertNewVersion(message).Result._id;
          body = body.Replace("{Disapproved}", model.Link + "genericmessage/" + idMessageDis.ToString());
          var sendMail = new MailLog
          {
            From = new MailLogAddress("noreplay@fluidstate.com.br", "Suporte ao Cliente | Fluid"),
            To = new List<MailLogAddress>(){
                  new MailLogAddress(requestor.User.Mail, requestor.User.Name)
              },
            Priority = EnumPriorityMail.Low,
            _idPerson = person._id,
            NamePerson = person.User.Name,
            Body = body,
            StatusMail = EnumStatusMail.Sended,
            Included = DateTime.Now,
            Subject = model.Subject,
          };
          serviceMailLog.InsertNewVersion(sendMail).Wait();
          var messageApv = serviceMailMessage.GetAllNewVersion(p => p._id == idMessageApv).Result.FirstOrDefault();
          var messageDis = serviceMailMessage.GetAllNewVersion(p => p._id == idMessageDis).Result.FirstOrDefault();
          var token = SendMail(path, person, sendMail._id.ToString());
          messageApv.Token = token;
          messageApv.Name = "automanagerapproved";
          serviceMailMessage.Update(messageApv, null).Wait();
          messageDis.Token = token;
          messageDis.Name = "automanagerdisapproved";
          serviceMailMessage.Update(messageDis, null).Wait();
        };
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public string Disapproved(ViewWorkflow view, string idPerson, string idManager)
    {
      try
      {
        var auto = serviceAutoManager.GetNewVersion(p => p.Person._id == idPerson & p.Requestor._id == idManager & p.StatusAutoManager == EnumStatusAutoManager.Requested).Result;
        if (auto == null)
          return "realized";

        var list = new List<string>();
        foreach (var item in auto.Workflow)
          list.Add(serviceWorkflow.Disapproved(view)._id);

        auto.Workflow = list;
        auto.StatusAutoManager = EnumStatusAutoManager.Disapproved;
        serviceAutoManager.Update(auto, null).Wait();

        return "disapproved";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public string Approved(ViewWorkflow view, string idPerson, string idManager)
    {
      try
      {
        var auto = serviceAutoManager.GetAllNewVersion(p => p.Person._id == idPerson & p.Requestor._id == idManager & p.StatusAutoManager == EnumStatusAutoManager.Requested).Result.FirstOrDefault();
        if (auto == null)
          return "realized";

        var list = new List<string>();
        foreach (var item in auto.Workflow)
          list.Add(serviceWorkflow.Approved(view)._id);

        auto.Workflow = list;
        auto.StatusAutoManager = EnumStatusAutoManager.Approved;
        var manager = servicePerson.GetNewVersion(p => p._id == idManager).Result;
        var person = servicePerson.GetNewVersion(p => p._id == idPerson).Result;
        person.Manager = new BaseFields() { _id = manager._id, Mail = manager.User.Mail, Name = manager.User.Name };
        servicePerson.UpdateManager(person, manager._id, null);
        //if (manager.TypeUser == EnumTypeUser.Employee)
        //{
        //  manager.TypeUser = EnumTypeUser.Manager;
        //  servicePerson.Update(manager, null).Wait();
        //}
        serviceAutoManager.Update(auto, null).Wait();

        return "approved";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public void Canceled(string idPerson, string idManager)
    {
      try
      {
        var auto = serviceAutoManager.GetAllNewVersion(p => p.Person._id == idPerson & p.Requestor._id == idManager & p.StatusAutoManager == EnumStatusAutoManager.Requested).Result.FirstOrDefault();
        auto.StatusAutoManager = EnumStatusAutoManager.Canceled;
        serviceAutoManager.Update(auto, null).Wait();
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public List<ViewAutoManager> ListApproved(string idManager)
    {
      try
      {
        var listauto = serviceAutoManager.GetAllNewVersion();
        List<ViewAutoManager> list = new List<ViewAutoManager>();
        foreach (var auto in listauto)
        {
          var item = serviceWorkflow.GetNewVersion(p => auto.Workflow.Contains(p._id) & p.StatusWorkflow == EnumWorkflow.Open
          & p.Requestor._id == idManager).Result;

          if (item != null)
            list.Add(new ViewAutoManager()
            {
              IdWorkflow = item._id,
              IdRequestor = auto.Requestor._id,
              NameRequestor = auto.Requestor.Name,
              IdPerson = auto.Person._id.ToString(),
              NamePerson = auto.Person.Name
            });
        }

        return list;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public void DeleteManager(string idPerson)
    {
      try
      {
        var person = servicePerson.GetNewVersion(p => p._id == idPerson).Result;
        person.Manager = null;
        servicePerson.UpdateManager(person, null, person._id);
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    #endregion

    #region private

    private string SendMail(string link, Person person, string idmail)
    {
      try
      {
        string token = serviceAuthentication.AuthenticationMail(person);
        using (var client = new HttpClient())
        {
          client.BaseAddress = new Uri(link.Substring(0, link.Length - 1) + ":5201/");
          client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
          var resultMail = client.PostAsync("sendmail/" + idmail, null);
          return token;
        }
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    #endregion

  }
}
