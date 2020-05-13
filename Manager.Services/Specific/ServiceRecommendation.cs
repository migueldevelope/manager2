using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.Interfaces;
using Manager.Core.Views;
using Manager.Data;
using Manager.Services.Auth;
using Manager.Services.Commons;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.BusinessView;
using Manager.Views.Enumns;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Manager.Services.Specific
{
  public class ServiceRecommendation : Repository<Recommendation>, IServiceRecommendation
  {

    private readonly ServiceAuthentication serviceAuthentication;
    private readonly ServiceLog serviceLog;
    private readonly ServiceGeneric<Recommendation> serviceRecommendation;
    private readonly ServiceGeneric<RecommendationPerson> serviceRecommendationPerson;
    private readonly ServiceGeneric<Person> servicePerson;
    private readonly ServiceGeneric<Skill> serviceSkill;
    private readonly ServiceGeneric<SalaryScale> serviceSalaryScale;
    private readonly ServiceGeneric<MailLog> serviceMail;
    private readonly ServiceMailModel serviceMailModel;
    private readonly ServiceGeneric<Account> serviceAccount;

    private readonly IServiceControlQueue serviceControlQueue;

    private string path;

    #region Constructor
    public ServiceRecommendation(DataContext context, DataContext contextLog, string pathToken, IServiceControlQueue _serviceControlQueue) : base(context)
    {
      try
      {
        serviceAuthentication = new ServiceAuthentication(context, contextLog, _serviceControlQueue, pathToken);
        serviceRecommendation = new ServiceGeneric<Recommendation>(context);
        serviceRecommendationPerson = new ServiceGeneric<RecommendationPerson>(context);
        servicePerson = new ServiceGeneric<Person>(context);
        serviceSalaryScale = new ServiceGeneric<SalaryScale>(context);
        serviceMail = new ServiceGeneric<MailLog>(context);
        serviceMailModel = new ServiceMailModel(context);
        serviceAccount = new ServiceGeneric<Account>(context);
        serviceSkill = new ServiceGeneric<Skill>(context);
        serviceLog = new ServiceLog(context, contextLog);

        serviceControlQueue = _serviceControlQueue;
        path = pathToken;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public void SetUser(IHttpContextAccessor contextAccessor)
    {
      User(contextAccessor);
      serviceRecommendation._user = _user;
      serviceRecommendationPerson._user = _user;
      servicePerson._user = _user;
      serviceSalaryScale._user = _user;
      serviceMail._user = _user;
      serviceLog._user = _user;
      serviceMailModel._user = _user;
      serviceMailModel.SetUser(_user);

    }
    public void SetUser(BaseUser user)
    {
      _user = user;
      serviceRecommendation._user = user;
      serviceRecommendationPerson._user = user;
      servicePerson._user = user;
      serviceSalaryScale._user = user;
      serviceMail._user = user;
      serviceLog._user = user;
      serviceMailModel._user = user;
      serviceMailModel.SetUser(_user);
    }
    #endregion

    #region Recommendation
    public string Delete(string id)
    {
      try
      {
        Recommendation item = serviceRecommendation.GetNewVersion(p => p._id == id).Result;
        item.Status = EnumStatus.Disabled;
        serviceRecommendation.Update(item, null).Wait();
        return "Recommendation deleted!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public string New(ViewCrudRecommendation view)
    {
      try
      {
        Recommendation recommendation = serviceRecommendation.InsertNewVersion(new Recommendation()
        {
          _id = view._id,
          Name = view.Name,
          Image = view.Image,
          Content = view.Content,
          Skill = view.Skill
        }).Result;

        Task.Run(() => AddRecommendationsAccounts(recommendation));

        return recommendation._id;
      }
      catch (Exception e)
      {
        throw e;
      }
    }


    public void SetImage(string idrecommendation, string url, string fileName, string attachmentid)
    {
      try
      {
        Recommendation recommendation = serviceRecommendation.GetNewVersion(p => p._id == idrecommendation).Result;
        if(recommendation == null)
        {
          recommendation = serviceRecommendation.GetNewVersion(p => p._id == idrecommendation).Result;
          recommendation.Image = url;
          serviceRecommendation.Update(recommendation, null).Wait();
        }
        else
        {
          recommendation.Image = url;
          serviceRecommendation.Update(recommendation, null).Wait();
        }
          

        
        //Task.Run(() => SynchronizeRecommendationsAsyncUpdate());
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string Update(ViewCrudRecommendation view)
    {
      try
      {
        Recommendation recommendation = serviceRecommendation.GetNewVersion(p => p._id == view._id).Result;

        recommendation.Name = view.Name;
        recommendation.Image = view.Image;
        recommendation.Skill = view.Skill;
        recommendation.Content = view.Content;

        serviceRecommendation.Update(recommendation, null).Wait();

        //Task.Run(() => SynchronizeRecommendationsAsync());
        //Task.Run(() => SynchronizeRecommendationsAsyncUpdate());

        return "Recommendation altered!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public ViewCrudRecommendation Get(string id)
    {
      try
      {
        return serviceRecommendation.GetNewVersion(p => p._id == id).Result.GetViewCrud();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewListRecommendation> List(ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        List<ViewListRecommendation> detail = serviceRecommendation.GetAllNewVersion(p => p.Name.ToUpper().Contains(filter.ToUpper()), count, count * (page - 1), "Name").Result
          .Select(x => new ViewListRecommendation()
          {
            _id = x._id,
            Name = x.Name,
            Image = x.Image
          }).ToList();
        total = serviceRecommendation.CountNewVersion(p => p.Name.ToUpper().Contains(filter.ToUpper())).Result;
        return detail;
      }
      catch (Exception e)
      {
        throw e;
      }
    }


    #endregion

    #region RecommendationPerson
    public string ReadRecommendationPerson(string idperson)
    {
      try
      {
        var recommendations = serviceRecommendationPerson.GetAllNewVersion(p => p.Person._id == idperson).Result;
        foreach(var recommendation in recommendations)
        {
          recommendation.Read = true;
          var i = serviceRecommendationPerson.Update(recommendation, null);
        }

        return "read";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string NewRecommendationPerson(ViewCrudRecommendationPerson view, string plataform)
    {
      try
      {
        var personsend = servicePerson.GetNewVersion(p => p.User._id == _user._idUser).Result;

        var verifyperson = serviceRecommendationPerson.GetNewVersion(p => p.Person._id == view.Person._id
        & p.Recommendation._id == view.Recommendation._id & p._idColleague == _user._idUser).Result;
        if (verifyperson != null)
          return "already recommendation to person";

        DateTime dateStart = DateTime.Parse(DateTime.Now.Year + "-" + DateTime.Now.Date.Month + "-01");
        DateTime dateEnd = dateStart.AddMonths(1).AddDays(-1);

        var verifycount = serviceRecommendationPerson.CountNewVersion(p => p._idColleague == _user._idUser
         && p.Date >= dateStart && p.Date < dateEnd).Result;
        if (verifycount == 3)
          return "many recommendation month";

        var person = servicePerson.GetNewVersion(p => p._id == view.Person._id).Result;
        var recommendation = serviceRecommendation.GetNewVersion(p => p._id == view.Recommendation._id).Result;
        RecommendationPerson recommendationperson = serviceRecommendationPerson.InsertNewVersion(
          new RecommendationPerson()
          {
            _id = view._id,
            Recommendation = view.Recommendation,
            Content = recommendation.Content == null ? null : recommendation.Content.Replace("{Name}", view.Person.Name).Replace("{NameSend}", personsend.User.Name),
            Person = view.Person,
            Comments = view.Comments,
            _idColleague = _user._idUser,
            Date = DateTime.Now,
            Read = false
          }).Result;

        Task.Run(() => Mail(person, view.Recommendation.Name));
        Task.Run(() => SendQueue(recommendationperson._id, person._id));
        Task.Run(() => LogSave(_user._idPerson, string.Format("Send reccomendation | {0}", recommendationperson._id),plataform));

        return "RecommendationPerson added!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewListRecommendationPerson> ListRecommendationPerson(string idrecommendation, ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        List<ViewListRecommendationPerson> detail = serviceRecommendationPerson.GetAllNewVersion(p => p.Recommendation._id == idrecommendation && p.Person.Name.ToUpper().Contains(filter.ToUpper()), count, count * (page - 1), "Person.Name").Result
          .Select(p => new ViewListRecommendationPerson
          {
            _id = p._id,
            Image = p.Recommendation.Image,
            NamePerson = p.Person.Name,
            _idPerson = p.Person._id,
            NameRecommendation = p.Recommendation.Name,
            _idRecommendation = p.Recommendation._id,
            Read = p.Read
          }).ToList();
        total = serviceRecommendationPerson.CountNewVersion(p => p.Recommendation._id == idrecommendation && p.Person.Name.ToUpper().Contains(filter.ToUpper())).Result;
        return detail;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewListRecommendationPersonId> ListRecommendationPersonId(string idperson, ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        List<ViewListRecommendationPersonId> detail = serviceRecommendationPerson.GetAllNewVersion(p => p.Person._id == idperson && p.Person.Name.ToUpper().Contains(filter.ToUpper()), count, count * (page - 1), "Person.Name").Result
          .Select(p => new ViewListRecommendationPersonId
          {
            _id = p._id,
            Image = p.Recommendation.Image,
            NameRecommendation = p.Recommendation.Name,
            Content = p.Content,
            Comments = p.Comments,
            Read = p.Read
          }).ToList();
        total = serviceRecommendationPerson.CountNewVersion(p => p.Person._id == idperson && p.Person.Name.ToUpper().Contains(filter.ToUpper())).Result;
        return detail;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewListRecommendationPerson> ListRecommendationPerson(ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        List<ViewListRecommendationPerson> detail = serviceRecommendationPerson.GetAllNewVersion(p => p.Person.Name.ToUpper().Contains(filter.ToUpper()), count, count * (page - 1), "Person.Name").Result
          .Select(p => new ViewListRecommendationPerson
          {
            _id = p._id,
            Image = p.Recommendation.Image,
            NamePerson = p.Person.Name,
            _idPerson = p.Person._id,
            NameRecommendation = p.Recommendation.Name,
            _idRecommendation = p.Recommendation._id,
            Read = p.Read
          }).ToList();
        total = serviceRecommendationPerson.CountNewVersion(p => p.Person.Name.ToUpper().Contains(filter.ToUpper())).Result;
        return detail;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewListPersonBase> ListPerson(ref long total, int count, int page, string filter)
    {
      try
      {
        total = servicePerson.CountNewVersion(p => p.TypeUser > EnumTypeUser.Administrator
        & p.StatusUser != EnumStatusUser.Disabled
        & p.User._id != _user._idUser
        & p.User.Name.ToUpper().Contains(filter.ToUpper())).Result;
        return servicePerson.GetAllNewVersion(p => p.TypeUser > EnumTypeUser.Administrator
        & p.StatusUser != EnumStatusUser.Disabled
        & p.User._id != _user._idUser
        & p.User.Name.ToUpper().Contains(filter.ToUpper()), count, count * (page - 1), "User.Name").Result
        .Select(x => x.GetViewListBase()).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewExportRecommendation> ExportRecommendation(ViewFilterIdAndDate filter)
    {
      try
      {
        List<ViewExportRecommendation> result = new List<ViewExportRecommendation>();

        foreach (var rows in filter.Persons)
        {
          var recommendations = serviceRecommendationPerson.GetAllNewVersion(p => p.Person._id == rows._id
          && p.Date >= filter.Date.Begin && p.Date <= filter.Date.End).Result;

          foreach (var item in recommendations)
          {
            var colleague = servicePerson.GetFreeNewVersion(p => p.User._id == item._idColleague).Result;
            if (filter.Persons.Where(p => p._id == item.Person._id).Count() > 0)

              result.Add(new ViewExportRecommendation
              {
                Name = item.Person.Name,
                NameRecommendation = item.Recommendation.Name,
                Comments = item.Comments,
                NameColleague = colleague?.User.Name
              });
          }
        }

        return result;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    #endregion

    #region private

    public string AddRecommendationsAccounts(Recommendation recommendation)
    {
      try
      {
        // Identificação da conta raiz do ANALISA
        var idresolution = "5b6c4f47d9090156f08775aa";
        List<Account> accounts = serviceAccount.GetAllFreeNewVersion(p => p._id != idresolution).Result;
        Recommendation local;
        foreach (Account account in accounts)
        {
          local = new Recommendation()
          {
            Template = recommendation._id,
            Content = recommendation.Content,
            Name = recommendation.Name,
            Image = recommendation.Image,
            Skill = serviceSkill.GetFreeNewVersion(p => p._idAccount == account._id & p.Template == recommendation.Skill._id).Result.GetViewList(),
            Status = recommendation.Status,
            _idAccount = account._id,
            _id = ObjectId.GenerateNewId().ToString()
          };
          Recommendation result = serviceRecommendation.InsertFreeNewVersion(local).Result;
        }
        return "Paramter added!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public void SynchronizeRecommendationsAsync()
    {
      try
      {
        // Identificação da conta raiz do ANALISA
        var idresolution = "5b6c4f47d9090156f08775aa";

        List<Account> accounts = serviceAccount.GetAllFreeNewVersion(p => p._id != idresolution).Result;

        // Recommendation
        foreach (Recommendation recommendation in serviceRecommendation.GetAllFreeNewVersion(p => p._idAccount == idresolution).Result)
        {
          Recommendation local;
          foreach (Account accountRecommendation in accounts)
          {
            local = serviceRecommendation.GetFreeNewVersion(p => p._idAccount == accountRecommendation._idAccount && p.Template == recommendation._id).Result;
            if (local == null)
            {
              local = new Recommendation()
              {
                Template = recommendation._id,
                Content = recommendation.Content,
                Name = recommendation.Name,
                Image = recommendation.Image,
                Skill = serviceSkill.GetFreeNewVersion(p => p._idAccount == accountRecommendation._id & p.Template == recommendation.Skill._id).Result.GetViewList(),
                Status = recommendation.Status,
                _idAccount = accountRecommendation._id,
                _id = ObjectId.GenerateNewId().ToString()
              };
              Recommendation result = serviceRecommendation.InsertFreeNewVersion(local).Result;
            }
          }
        }
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public void SynchronizeRecommendationsAsyncUpdate()
    {
      try
      {
        // Identificação da conta raiz do ANALISA
        var idresolution = "5b6c4f47d9090156f08775aa";

        List<Account> accounts = serviceAccount.GetAllFreeNewVersion(p => p._id != idresolution).Result;

        // Recommendation
        foreach (Recommendation recommendation in serviceRecommendation.GetAllFreeNewVersion(p => p._idAccount == idresolution).Result)
        {
          Recommendation local;
          foreach (Account accountRecommendation in accounts)
          {
            local = serviceRecommendation.GetFreeNewVersion(p => p._idAccount == accountRecommendation._idAccount && p.Template == recommendation._id).Result;
            if (local != null)
            {
              local.Name = recommendation.Name;
              local.Image = recommendation.Image;
              var item = serviceRecommendation.UpdateAccount(local, null);
            }
          }
        }
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    private void Mail(Person person, string skill)
    {
      try
      {
        var model = serviceMailModel.Recommendation(path);
        if (model.StatusMail == EnumStatus.Disabled)
          return;

        string managername = "";
        try
        {
          managername = servicePerson.GetAllNewVersion(p => p.User._id == _user._idUser).Result.FirstOrDefault().User.Name;
        }
        catch (Exception)
        {

        }

        var body = model.Message.Replace("{Person}", person.User.Name).Replace("{Link}", model.Link).Replace("{Manager}", managername).Replace("{Company}", person.Company.Name).Replace("{Occupation}", person.Occupation.Name).Replace("{Type}", skill);
        var sendMail = new MailLog
        {
          From = new MailLogAddress("sucessocliente@fluidstate.com.br", "Suporte ao Cliente | Fluid"),
          To = new List<MailLogAddress>(){
                        new MailLogAddress(person.User.Mail, person.User.Name)
                    },
          Priority = EnumPriorityMail.Low,
          _idPerson = person._id,
          NamePerson = person.User.Name,
          Body = body,
          StatusMail = EnumStatusMail.Sended,
          Included = DateTime.Now,
          Subject = model.Subject
        };
        var mailObj = serviceMail.InsertNewVersion(sendMail).Result;
        var token = SendMail(path, person, mailObj._id.ToString());
      }
      catch (Exception e)
      {
        throw e;
      }
    }


    private string SendMail(string link, Person person, string idmail)
    {
      try
      {
        string token = serviceAuthentication.AuthenticationMail(person);
        using (var client = new HttpClient())
        {
          client.BaseAddress = new Uri(link.Substring(0, link.Length - 1) + ":5201/");
          client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
          var resultMail = client.PostAsync("sendmail/" + idmail, null).Result;
          return token;
        }
      }
      catch (Exception e)
      {
        throw e;
      }
    }


    private void SendQueue(string id, string idperson)
    {
      try
      {
        var data = new ViewCrudMaturityRegister
        {
          _idPerson = idperson,
          TypeMaturity = EnumTypeMaturity.Recommendation,
          _idRegister = id,
          Date = DateTime.Now,
          _idAccount = _user._idAccount
        };

        serviceControlQueue.SendMessageAsync(JsonConvert.SerializeObject(data));
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    private void LogSave(string iduser, string local, string plataform)
    {
      try
      {
        var user = servicePerson.GetAllNewVersion(p => p._id == iduser).Result.FirstOrDefault();
        var log = new ViewLog()
        {
          Description = "Access Recommendation " + plataform,
          Local = local,
          _idPerson = user._id
        };
        serviceLog.NewLog(log);
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    #endregion
  }
}
