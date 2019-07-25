using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.Interfaces;
using Manager.Core.Views;
using Manager.Data;
using Manager.Services.Auth;
using Manager.Services.Commons;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using Microsoft.AspNetCore.Http;
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
    private readonly ServiceGeneric<SalaryScale> serviceSalaryScale;
    private readonly ServiceGeneric<MailLog> serviceMail;
    private readonly ServiceMailModel serviceMailModel;
    private readonly IServiceControlQueue serviceControlQueue;

    private string path;

    #region Constructor
    public ServiceRecommendation(DataContext context, DataContext contextLog, string pathToken, IServiceControlQueue _serviceControlQueue) : base(context)
    {
      try
      {
        serviceAuthentication = new ServiceAuthentication(context, contextLog);
        serviceRecommendation = new ServiceGeneric<Recommendation>(context);
        serviceRecommendationPerson = new ServiceGeneric<RecommendationPerson>(context);
        servicePerson = new ServiceGeneric<Person>(context);
        serviceSalaryScale = new ServiceGeneric<SalaryScale>(context);
        serviceMail = new ServiceGeneric<MailLog>(context);
        serviceMailModel = new ServiceMailModel(context);
        serviceLog = new ServiceLog(context, contextLog);
        serviceControlQueue = _serviceControlQueue;
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
    }
    public void SetUser(BaseUser user)
    {
      _user = user;
      serviceRecommendation._user = user;
      serviceRecommendationPerson._user = user;
      servicePerson._user = user;
      serviceSalaryScale._user = user;
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
        return recommendation._id;
      }
      catch (Exception e)
      {
        throw e;
      }
    }


    public void SetImage(string idBaseHelp, string url, string fileName, string attachmentid)
    {
      try
      {
        Recommendation basehelp = serviceRecommendation.GetFreeNewVersion(p => p._id == idBaseHelp).Result;
        basehelp.Image = url;
        serviceRecommendation.UpdateAccount(basehelp, null).Wait();
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
        recommendation.Skill = view.Skill;
        recommendation.Content = view.Content;
        recommendation.Image = view.Image;

        serviceRecommendation.Update(recommendation, null).Wait();

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
    public string NewRecommendationPerson(ViewCrudRecommendationPerson view)
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
        RecommendationPerson recommendationperson = serviceRecommendationPerson.InsertNewVersion(
          new RecommendationPerson()
          {
            _id = view._id,
            Recommendation = view.Recommendation,
            Content = view.Content == null ? null : view.Content.Replace("{Name}",view.Person.Name).Replace("{NameSend}",personsend.User.Name),
            Person = view.Person,
            Comments = view.Comments,
            _idColleague = _user._idUser,
            Date = DateTime.Now
          }).Result;

        Task.Run(() => Mail(person, view.Recommendation.Name));
        Task.Run(() => SendQueue(recommendationperson._id, person._id));
        Task.Run(() => LogSave(_user._idPerson, string.Format("Send reccomendation | {0}", recommendationperson._id)));

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
            _idRecommendation = p.Recommendation._id
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
            Content = p.Content
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
            _idRecommendation = p.Recommendation._id
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
        & p.StatusUser != EnumStatusUser.Disabled & p.StatusUser != EnumStatusUser.ErrorIntegration
        & p.User.Name.ToUpper().Contains(filter.ToUpper())).Result;
        return servicePerson.GetAllNewVersion(p => p.TypeUser > EnumTypeUser.Administrator
        & p.StatusUser != EnumStatusUser.Disabled & p.StatusUser != EnumStatusUser.ErrorIntegration
        & p.User.Name.ToUpper().Contains(filter.ToUpper()), count, count * (page - 1), "User.Name").Result
        .Select(x => x.GetViewListBase()).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    #endregion

    #region private

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
          From = new MailLogAddress("suporte@jmsoft.com.br", "Notificação do Analisa"),
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
    private void LogSave(string iduser, string local)
    {
      try
      {
        var user = servicePerson.GetAllNewVersion(p => p._id == iduser).Result.FirstOrDefault();
        var log = new ViewLog()
        {
          Description = "Access Recommendation",
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
