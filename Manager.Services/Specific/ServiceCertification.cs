using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.BusinessModel;
using Manager.Core.Interfaces;
using Manager.Core.Views;
using Manager.Data;
using Manager.Services.Auth;
using Manager.Services.Commons;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
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
#pragma warning disable 1998
  public class ServiceCertification : Repository<Certification>, IServiceCertification
  {
    private readonly ServiceAuthentication serviceAuthentication;
    private readonly ServiceGeneric<Certification> serviceCertification;
    private readonly ServiceGeneric<CertificationPerson> serviceCertificationPerson;
    private readonly ServiceLog serviceLog;
    private readonly ServiceLogMessages serviceLogMessages;
    private readonly ServiceGeneric<MailLog> serviceMail;
    private readonly ServiceMailModel serviceMailModel;
    private readonly ServiceGeneric<Monitoring> serviceMonitoring;
    private readonly ServiceGeneric<Occupation> serviceOccupation;
    private readonly ServiceGeneric<Group> serviceGroup;
    private readonly ServiceGeneric<Company> serviceCompany;
    private readonly ServiceGeneric<Parameter> serviceParameter;
    private readonly ServiceGeneric<Person> servicePerson;
    private readonly ServiceGeneric<Questions> serviceQuestions;
    private readonly ServiceGeneric<TextDefault> serviceTextDefault;
    private readonly IServiceControlQueue serviceControlQueue;

    public string path;

    #region Constructor
    public ServiceCertification(DataContext context, DataContext contextLog, string pathToken, IServiceControlQueue _serviceControlQueue) : base(context)
    {
      try
      {
        serviceAuthentication = new ServiceAuthentication(context, contextLog);
        serviceCertification = new ServiceGeneric<Certification>(context);
        serviceCertificationPerson = new ServiceGeneric<CertificationPerson>(context);
        serviceLog = new ServiceLog(context, contextLog);
        serviceLogMessages = new ServiceLogMessages(context);
        serviceMail = new ServiceGeneric<MailLog>(contextLog);
        serviceMailModel = new ServiceMailModel(context);
        serviceMonitoring = new ServiceGeneric<Monitoring>(context);
        serviceOccupation = new ServiceGeneric<Occupation>(context);
        serviceGroup = new ServiceGeneric<Group>(context);
        serviceCompany = new ServiceGeneric<Company>(context);
        serviceParameter = new ServiceGeneric<Parameter>(context);
        servicePerson = new ServiceGeneric<Person>(context);
        serviceQuestions = new ServiceGeneric<Questions>(context);
        serviceTextDefault = new ServiceGeneric<TextDefault>(context);
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
      serviceCertification._user = _user;
      serviceCertificationPerson._user = _user;
      serviceLog.SetUser(_user);
      serviceLogMessages.SetUser(_user);
      serviceMail._user = _user;
      serviceMailModel.SetUser(_user);
      serviceMonitoring._user = _user;
      serviceOccupation._user = _user;
      serviceGroup._user = _user;
      serviceCompany._user = _user;
      serviceParameter._user = _user;
      servicePerson._user = _user;
      serviceQuestions._user = _user;
      serviceTextDefault._user = _user;
    }
    public void SetUser(BaseUser user)
    {
      _user = user;
      serviceCertification._user = user;
      serviceCertificationPerson._user = user;
      serviceLog.SetUser(user);
      serviceLogMessages.SetUser(user);
      serviceMail._user = user;
      serviceMailModel.SetUser(user);
      serviceMonitoring._user = user;
      serviceOccupation._user = user;
      serviceGroup._user = user;
      serviceCompany._user = user;
      serviceParameter._user = user;
      servicePerson._user = user;
      serviceQuestions._user = user;
      serviceTextDefault._user = user;
    }
    #endregion

    #region private

    private void SendQueue(string id, string idperson)
    {
      try
      {
        var data = new ViewCrudMaturityRegister
        {
          _idPerson = idperson,
          TypeMaturity = EnumTypeMaturity.Certification,
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

    private class ViewCertificationComparer : IEqualityComparer<ViewListCertificationPerson>
    {
      public bool Equals(ViewListCertificationPerson x, ViewListCertificationPerson y)
      {
        if (x._id == y._id & x.Manager == y.Manager)
        {
          return true;
        }
        else
        {
          return false;
        }
      }

      public int GetHashCode(ViewListCertificationPerson obj)
      {
        return obj._id.GetHashCode();
      }
    }

    private Certification Load(Certification certification)
    {
      try
      {
        var companyname = servicePerson.GetNewVersion(p => p._id == certification.Person._id).Result.Company.Name;
        certification.Questions = new List<ViewListCertificationQuestions>();

        var itens = new List<CertificationQuestions>();


        foreach (var item in serviceQuestions.GetAllNewVersion(p => p.TypeQuestion == EnumTypeQuestion.Text & p.TypeRotine == EnumTypeRotine.Certification).Result.ToList())
        {
          certification.Questions.Add(new ViewListCertificationQuestions()
          {
            Question =
            new ViewCrudQuestions()
            {
              _id = item._id,
              Content = item.Content.Replace("{company_name}", item.Company.Name)
              .Replace("{employee_name}", certification.Person.Name).Replace("{item_name}", certification.CertificationItem.Name),
              Name = item.Name,
              Order = item.Order,
              TypeQuestion = item.TypeQuestion,
              TypeRotine = item.TypeRotine
            },
            _id = ObjectId.GenerateNewId().ToString()
          });
        }

        var text = serviceTextDefault.GetAllNewVersion(p => p.TypeText == EnumTypeText.Certification).Result.FirstOrDefault();
        if (text != null)
          certification.TextDefault = text.Content.Replace("{company_name}", companyname).Replace("{employee_name}", certification.Person.Name)
            .Replace("{manager_name}", certification.Person.NameManager).Replace("{item_name}", certification.CertificationItem.Name);

        return certification;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    private Certification LoadMap(Certification certification)
    {
      try
      {
        var companyname = servicePerson.GetNewVersion(p => p._id == certification.Person._id).Result.Company.Name;

        certification.Questions = new List<ViewListCertificationQuestions>();

        var itens = new List<ViewListCertificationQuestions>();


        foreach (var item in serviceQuestions.GetAllNewVersion(p => p.TypeRotine == EnumTypeRotine.Certification).Result.OrderBy(p => p.Order).ToList())
        {
          certification.Questions.Add(new ViewListCertificationQuestions()
          {
            Question =
            new ViewCrudQuestions()
            {
              _id = item._id,
              Content = item.Content.Replace("{company_name}", companyname)
              .Replace("{employee_name}", certification.Person.Name).Replace("{item_name}", certification.CertificationItem.Name),
              Name = item.Name,
              Order = item.Order,
              TypeQuestion = item.TypeQuestion,
              TypeRotine = item.TypeRotine
            },
            _id = ObjectId.GenerateNewId().ToString()
          });
        }

        var text = serviceTextDefault.GetAllNewVersion(p => p.TypeText == EnumTypeText.Certification).Result.FirstOrDefault();
        if (text != null)
          certification.TextDefault = text.Content.Replace("{company_name}", companyname).Replace("{employee_name}", certification.Person.Name)
            .Replace("{manager_name}", certification.Person.NameManager).Replace("{item_name}", certification.CertificationItem.Name);

        return certification;
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
          Description = "Access Certification ",
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
    //send mail
    private void Mail(Person person, BaseFields guest)
    {
      try
      {
        //searsh model mail database
        var model = serviceMailModel.Certification(path);
        if (model.StatusMail == EnumStatus.Disabled)
          return;

        var body = model.Message.Replace("{Person}", person.User.Name).Replace("{Link}", model.Link).Replace("{Manager}", person.Manager.Name).Replace("{Company}", person.Company.Name).Replace("{Occupation}", person.Occupation.Name).Replace("{Guest}", guest.Name);
        var sendMail = new MailLog
        {
          From = new MailLogAddress("suporte@jmsoft.com.br", "Notificação do Analisa"),
          To = new List<MailLogAddress>(){
                        new MailLogAddress(guest.Mail, guest.Name)
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
    private void MailApproved(Person person, string skillName)
    {
      try
      {
        //searsh model mail database
        var model = serviceMailModel.CertificationApproval(path);
        if (model.StatusMail == EnumStatus.Disabled)
          return;

        var body = model.Message.Replace("{Manager}", person.Manager?.Name)
                                .Replace("{Skill}", skillName)
                                .Replace("{Person}", person.User.Name);
        var sendMail = new MailLog
        {
          From = new MailLogAddress("suporte@jmsoft.com.br", "Notificação do Analisa"),
          To = new List<MailLogAddress>(){
                        new MailLogAddress(person.Manager.Mail, person.Manager.Name)
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
    private void MailApprovedPerson(Person person, string skillName)
    {
      try
      {
        //searsh model mail database
        var model = serviceMailModel.CertificationApprovalPerson(path);
        if (model.StatusMail == EnumStatus.Disabled)
          return;

        var body = model.Message.Replace("{Person}", person.User.Name)
                                .Replace("{Skill}", skillName);
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
    private void MailDisapproved(Person person, string skillName)
    {
      try
      {
        //searsh model mail database
        var model = serviceMailModel.CertificationDisapproval(path);
        if (model.StatusMail == EnumStatus.Disabled)
          return;

        var body = model.Message.Replace("{Manager}", person.Manager?.Name)
                                .Replace("{Skill}", skillName)
                                .Replace("{Person}", person.User.Name);
        var sendMail = new MailLog
        {
          From = new MailLogAddress("suporte@jmsoft.com.br", "Notificação do Analisa"),
          To = new List<MailLogAddress>(){
                        new MailLogAddress(person.Manager.Mail, person.Manager.Name)
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
          client.BaseAddress = new Uri(link);
          client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
          var resultMail = client.PostAsync("mail/sendmail/" + idmail, null).Result;
          return token;
        }
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    #endregion

    #region certification
    public void SetAttachment(string idcertification, string url, string fileName, string attachmentid)
    {
      try
      {
        var certification = serviceCertification.GetAllNewVersion(p => p._id == idcertification).Result.FirstOrDefault();

        if (certification.Attachments == null)
        {
          certification.Attachments = new List<ViewCrudAttachmentField>();
        }
        certification.Attachments.Add(new ViewCrudAttachmentField { Url = url, Name = fileName, _idAttachment = attachmentid });
        serviceCertification.Update(certification, null).Wait();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string DeleteCertification(string idcertification)
    {
      try
      {

        var certification = serviceCertification.GetAllNewVersion(p => p.Person._id == idcertification).Result.FirstOrDefault();
        Task.Run(() => LogSave(_user._idPerson, string.Format("Delete | ", idcertification)));
        if (certification == null)
          return "Certification deleted!";

        certification.Status = EnumStatus.Disabled;
        serviceCertification.Update(certification, null).Wait();
        return "Certification deleted!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string DeletePerson(string idcertification, string idcertificationperson)
    {
      try
      {
        var certification = serviceCertification.GetAllNewVersion(p => p._id == idcertification).Result.FirstOrDefault();
        foreach (var item in certification.ListPersons)
        {
          if (item._id == idcertificationperson)
          {
            certification.ListPersons.Remove(item);
            serviceCertification.Update(certification, null).Wait();
            return "remove";
          }
        }

        return "not found";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string UpdateStatusCertification(ViewCrudCertificationPersonStatus viewcertification, string idperson)
    {
      try
      {
        var certification = serviceCertification.GetAllNewVersion(p => p._id == viewcertification._idCertification).Result.FirstOrDefault();
      var person = servicePerson.GetNewVersion(p => p._id == certification.Person._id).Result;

        foreach (var item in certification.ListPersons)
        {
          if (item._id == idperson)
          {
            item.StatusCertificationPerson = viewcertification.StatusCertificationPerson;
            item.Comments = viewcertification.Comments;
            item.DateApprovation = DateTime.Now;

            var certPerson = serviceCertificationPerson.GetNewVersion(p => p._id == item._id).Result;
            certPerson.StatusCertificationPerson = item.StatusCertificationPerson;
            certPerson.Comments = item.Comments;
            certPerson.DateApprovation = item.DateApprovation;

            serviceCertificationPerson.Update(certPerson, null).Wait();

            serviceCertification.Update(certification, null).Wait();
            var open = certification.ListPersons.Where(p => p.StatusCertificationPerson == EnumStatusCertificationPerson.Wait).Count();
            if (open == 0)
            {
              var disapproved = certification.ListPersons.Where(p => p.StatusCertificationPerson == EnumStatusCertificationPerson.Disapproved).Count();
              if (disapproved > 0)
                certification.StatusCertification = EnumStatusCertification.Disaproved;
              else
                certification.StatusCertification = EnumStatusCertification.Approved;

              certification.DateEnd = DateTime.Now;

              serviceCertification.Update(certification, null).Wait();

              if (certification.StatusCertification == EnumStatusCertification.Approved)
              {
                Task.Run(() => MailApproved(person, certification.CertificationItem.Name));
                Task.Run(() => MailApprovedPerson(person, certification.CertificationItem.Name));
                Task.Run(() => SendQueue(certification._id, certification.Person._id));
              }
            }

            return "update";

          }
        }

        return "not found";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewListCertificationPerson> ListCertificationsWaitPerson(string idperson, ref long total, string filter, int count, int page)
    {
      try
      {
        int skip = (count * (page - 1));

        var person = servicePerson.GetAllNewVersion(p => p._id == idperson).Result.FirstOrDefault();
        List<ViewListCertificationPerson> list = new List<ViewListCertificationPerson>();

        //load certification guest
        foreach (var item in serviceCertificationPerson.GetAllNewVersion(p => p.StatusCertificationPerson == EnumStatusCertificationPerson.Wait & p.Person._id == idperson).Result.ToList())
        {
          var certification = serviceCertification.GetAllNewVersion(
          p => p.ListPersons.Contains(item.GetViewList())).Result.FirstOrDefault();
          if (certification != null)
          {
            list.Add(new ViewListCertificationPerson()
            {
              _id = certification._id,
              Name = certification.Person.Name,
              NameItem = certification.CertificationItem.Name,
              Manager = false
            });
          }

        };

        //load certification manager
        foreach (var item in serviceCertification.GetAllNewVersion(p => p.Person._idManager == idperson & p.StatusCertification == EnumStatusCertification.Wait).Result.ToList())
        {
          list.Add(new ViewListCertificationPerson()
          {
            _id = item._id,
            Name = item.Person.Name,
            NameItem = item.CertificationItem.Name,
            Manager = true
          });
        };


        var result = list.OrderBy(p => p.Name).Skip(skip).Take(count).ToList();
        result = result.Distinct(new ViewCertificationComparer()).ToList();
        total = result.Count();

        return result;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewListCertificationItem> ListCertificationPerson(string idperson, ref long total, string filter, int count, int page)
    {
      try
      {
        int skip = (count * (page - 1));

        var result = serviceCertification.GetAllNewVersion(p => p.Person._id == idperson & p.StatusCertification == EnumStatusCertification.Approved
        & p.CertificationItem.Name.ToUpper().Contains(filter.ToUpper())).Result.OrderBy(p => p.CertificationItem.Name).Skip(skip).Take(count)
        .Select(p => new ViewListCertificationItem
        {
          _id = p.CertificationItem._id,
          Name = p.CertificationItem.Name,
          Concept = p.CertificationItem.Concept,
          IdItem = p.CertificationItem.IdItem,
          ItemCertification = p.CertificationItem.ItemCertification,
          ItemCertificationView = p.CertificationItem.ItemCertification == EnumItemCertification.SkillCompanyHard ? EnumItemCertificationView.Company :
          p.CertificationItem.ItemCertification == EnumItemCertification.SkillCompanySoft ? EnumItemCertificationView.Company :
          p.CertificationItem.ItemCertification == EnumItemCertification.SkillGroupHard ? EnumItemCertificationView.Hard :
          p.CertificationItem.ItemCertification == EnumItemCertification.SkillOccupationHard ? EnumItemCertificationView.Hard : EnumItemCertificationView.Soft
        }).ToList();

        total = serviceCertification.CountNewVersion(p => p.Person._id == idperson & p.StatusCertification == EnumStatusCertification.Approved).Result;
        return result;
      }
      catch (Exception e)
      {
        throw e;
      }
    }


    public List<ViewListCertification> ListEnded(ref long total, string filter, int count, int page)
    {
      try
      {

        int skip = (count * (page - 1));
        var detail = serviceCertification.GetAllNewVersion(p => p.Person.Name.ToUpper().Contains(filter.ToUpper())).Result.OrderBy(p => p.Person.Name).Skip(skip).Take(count).ToList();
        total = serviceCertification.CountNewVersion(p => p.Person.Name.ToUpper().Contains(filter.ToUpper())).Result;

        return detail.Select(p => new ViewListCertification()
        {
          _id = p._id,
          Name = p.Person.Name,
          _idPerson = p.Person._id,
          StatusCertification = p.StatusCertification
        }).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewListPersonBase> ListPersons(string idcertification, ref long total, string filter, int count, int page)
    {
      try
      {
        var certificaiton = serviceCertification.GetAllNewVersion(p => p._id == idcertification).Result.FirstOrDefault();

        var outros = servicePerson.GetAllNewVersion(p => p.TypeUser != EnumTypeUser.Support & p.StatusUser != EnumStatusUser.Disabled
        & p.StatusUser != EnumStatusUser.ErrorIntegration & p.TypeUser != EnumTypeUser.Administrator
        ).Result.ToList();
        List<ViewListPersonBase> details = new List<ViewListPersonBase>();
        foreach (var item in outros)
          details.Add(item.GetViewListBase());

        var detail = details.Where(p => p.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(o => o.Name).ToList();

        var listExclud = serviceCertification.GetAllNewVersion(p => p._id == idcertification).Result.FirstOrDefault().ListPersons;
        foreach (var item in listExclud)
        {
          detail.RemoveAll(p => p._id == item._id);
        }
        detail.RemoveAll(p => p._id == certificaiton.Person._id);

        total = detail.Count();

        return detail;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public ViewListCertificationProfile GetProfile(string idperson)
    {
      try
      {
        var person = servicePerson.GetNewVersion(p => p._id == idperson).Result;
        var occupation = serviceOccupation.GetNewVersion(p => p._id == person.Occupation._id).Result;
        var group = serviceGroup.GetNewVersion(p => p._id == occupation.Group._id).Result;
        var company = serviceCompany.GetNewVersion(p => p._id == group.Company._id).Result;

        var remove = serviceCertification.GetAllNewVersion(p => p.Person._id == idperson
        & p.StatusCertification != EnumStatusCertification.Disaproved
        & p.StatusCertification != EnumStatusCertification.Open).Result.ToList();


        var view = new ViewListCertificationProfile
        {
          ItemSkill = new List<ViewListCertificationItem>()
        };

        foreach (var item in company.Skills)
        {
          if (remove.Where(p => p.CertificationItem.IdItem == item._id).Count() == 0)
          {
            view.ItemSkill.Add(new ViewListCertificationItem()
            {
              _id = ObjectId.GenerateNewId().ToString(),
              Concept = item.Concept,
              IdItem = item._id,
              Name = item.Name,
              ItemCertification = item.TypeSkill == EnumTypeSkill.Hard ? EnumItemCertification.SkillCompanyHard : EnumItemCertification.SkillCompanySoft
            });
          }
        }

        foreach (var item in group.Skills)
        {
          if (remove.Where(p => p.CertificationItem.IdItem == item._id).Count() == 0)
          {
            view.ItemSkill.Add(new ViewListCertificationItem()
            {
              _id = ObjectId.GenerateNewId().ToString(),
              Concept = item.Concept,
              IdItem = item._id,
              Name = item.Name,
              ItemCertification = item.TypeSkill == EnumTypeSkill.Hard ? EnumItemCertification.SkillGroupHard : EnumItemCertification.SkillGroupSoft
            });
          }
        }

        foreach (var item in occupation.Skills)
        {
          if (remove.Where(p => p.CertificationItem.IdItem == item._id).Count() == 0)
          {
            view.ItemSkill.Add(new ViewListCertificationItem()
            {
              _id = ObjectId.GenerateNewId().ToString(),
              Concept = item.Concept,
              IdItem = item._id,
              Name = item.Name,
              ItemCertification = item.TypeSkill == EnumTypeSkill.Hard ? EnumItemCertification.SkillOccupationHard : EnumItemCertification.SkillOccupationSoft
            });
          }

        }

        view.ItemActivitie = new List<ViewListCertificationItem>();
        foreach (var item in occupation.Activities)
        {
          if (remove.Where(p => p.CertificationItem.IdItem == item._id).Count() == 0)
          {
            view.ItemActivitie.Add(new ViewListCertificationItem()
            {
              _id = ObjectId.GenerateNewId().ToString(),
              Concept = null,
              IdItem = item._id,
              Name = item.Name,
              ItemCertification = EnumItemCertification.Activitie
            });
          }
        }

        var text = serviceTextDefault.GetAllNewVersion(p => p.TypeText == EnumTypeText.CertificationHead).Result.FirstOrDefault();
        if (text != null)
          view.TextDefault = text.Content.Replace("{company_name}", person.Company.Name).Replace("{employee_name}", person.User.Name)
            .Replace("{manager_name}", person.Manager.Name);

        view.ItemActivitie = view.ItemActivitie.OrderBy(o => o.Name).ToList();
        view.ItemSkill = view.ItemSkill.OrderBy(o => o.Name).ToList();

        return view;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public ViewCrudCertification NewCertification(ViewListCertificationItem item, string idperson)
    {
      try
      {
        Person person = servicePerson.GetAllNewVersion(p => p._id == idperson).Result.FirstOrDefault();
        item._id = ObjectId.GenerateNewId().ToString();

        Certification certification = new Certification()
        {
          CertificationItem = new ViewListCertificationItem()
          {
            _id = item._id,
            IdItem = item.IdItem,
            Concept = item.Concept,
            Name = item.Name,
            ItemCertification = item.ItemCertification
          },
          Person = person.GetViewListBaseManager(),
          Status = EnumStatus.Enabled,
          ListPersons = new List<ViewCrudCertificationPerson>(),
          StatusCertification = EnumStatusCertification.Open,
          Attachments = new List<ViewCrudAttachmentField>()
        };

        certification = LoadMap(certification);

        certification = serviceCertification.InsertNewVersion(certification).Result;
        Task.Run(() => LogSave(_user._idPerson, string.Format("Start new process | {0}", certification._id)));
        return new ViewCrudCertification()
        {
          _id = certification._id,
          Name = certification.Person.Name,
          _idPerson = certification.Person._id,
          TextDefault = certification.TextDefault,
          StatusCertification = certification.StatusCertification,
          CertificationItem = certification.CertificationItem,
          Attachments = certification.Attachments,
          ListPersons = certification.ListPersons,
          Questions = certification.Questions
        };
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string AddPerson(string idcertification, ViewListPersonBase person)
    {
      try
      {
        var certification = serviceCertification.GetFreeNewVersion(p => p._id == idcertification).Result;
        var companyname = servicePerson.GetNewVersion(p => p._id == certification.Person._id).Result.Company.Name;

        var text = serviceTextDefault.GetAllNewVersion(p => p.TypeText == EnumTypeText.CertificationPerson).Result.FirstOrDefault();
        if (text != null)
          text.Content = text.Content.Replace("{company_name}", companyname).Replace("{employee_name}", certification.Person.Name)
            .Replace("{manager_name}", certification.Person.NameManager).Replace("{item_name}", certification.CertificationItem.Name);

        var textEnd = serviceTextDefault.GetAllNewVersion(p => p.TypeText == EnumTypeText.CertificationPersonEnd).Result.FirstOrDefault();
        if (textEnd != null)
          textEnd.Content = textEnd.Content.Replace("{company_name}", companyname).Replace("{employee_name}", certification.Person.Name)
            .Replace("{manager_name}", certification.Person.NameManager).Replace("{item_name}", certification.CertificationItem.Name);


        var cerPerson = new CertificationPerson()
        {
          Person = person,
          Status = EnumStatus.Enabled,
          StatusCertificationPerson = EnumStatusCertificationPerson.Wait,
          Comments = null,
          TextDefault = text.Content,
          TextDefaultEnd = textEnd.Content
        };

        cerPerson = serviceCertificationPerson.InsertNewVersion(cerPerson).Result;
        certification.ListPersons.Add(cerPerson.GetViewList());
        serviceCertification.Update(certification, null).Wait();

        return "ok";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string ApprovedCertification(string idcertification, ViewCrudCertificationPerson view)
    {
      try
      {
        var certification = serviceCertification.GetAllNewVersion(p => p._id == idcertification).Result.FirstOrDefault();

        foreach (var item in certification.ListPersons)
        {
          if (item._id == view._id)
          {
            item.StatusCertificationPerson = view.StatusCertificationPerson;
            item.Comments = view.Comments;
          }
          var cerPerson = serviceCertificationPerson.GetNewVersion(p => p._id == item._id).Result;
          cerPerson.StatusCertificationPerson = item.StatusCertificationPerson;
          cerPerson.Comments = item.Comments;
          serviceCertificationPerson.Update(cerPerson, null).Wait();
          serviceCertification.Update(certification, null).Wait();
          Task.Run(() => LogSave(_user._idPerson, string.Format("Certification approved | {0}", certification._id)));
          return "Certification approved!";
        }
        return "Certification not found!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string UpdateCertification(ViewCrudCertification view, string idperson, string idmonitoring)
    {
      try
      {
        var certification = serviceCertification.GetNewVersion(p => p._id == view._id).Result;
        var person = servicePerson.GetNewVersion(p => p._id == certification.Person._id).Result;

        certification.StatusCertification = view.StatusCertification;
        certification.TextDefault = view.TextDefault;
        foreach (var question in certification.Questions)
        {
          var item = view.Questions.Where(p => p.Question._id == question.Question._id).FirstOrDefault();
          question.Answer = item.Answer;
        };

        if (certification.StatusCertification == EnumStatusCertification.Wait)
        {
          var monitoring = serviceMonitoring.GetAllNewVersion(p => p._id == idmonitoring & p.StatusMonitoring == EnumStatusMonitoring.Show).Result.FirstOrDefault();
          if (monitoring != null)
          {
            serviceMonitoring.Delete(idmonitoring, true);
          }
        }

        if (certification.StatusCertification == EnumStatusCertification.Wait)
        {
          certification.DateBegin = DateTime.Now;
          foreach (var item in certification.ListPersons)
          {
            try
            {
              var personItem = servicePerson.GetNewVersion(p => p._id == item._id).Result;

              Task.Run(() => Mail(person, new BaseFields() { Name = item.Name, Mail = personItem.User.Mail }));
            }
            catch (Exception)
            {

            }

          }
        }

        serviceCertification.Update(certification, null).Wait();
        return "Certification altered!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }


    public ViewCrudCertification CertificationsWaitPerson(string idcertification)
    {
      try
      {
        Certification certification = serviceCertification.GetNewVersion(p => p._id == idcertification).Result;
        return new ViewCrudCertification()
        {
          _id = certification._id,
          Name = certification.Person.Name,
          _idPerson = certification.Person._id,
          TextDefault = certification.TextDefault,
          StatusCertification = certification.StatusCertification,
          CertificationItem = new ViewListCertificationItem()
          {
            _id = certification.CertificationItem._id,
            Name = certification.CertificationItem.Name,
            ItemCertification = certification.CertificationItem.ItemCertification,
            Concept = certification.CertificationItem.Concept,
            IdItem = certification.CertificationItem.IdItem
          },
          Attachments = certification.Attachments.Select(p => new ViewCrudAttachmentField() { _idAttachment = p._idAttachment, Name = p.Name, Url = p.Url }).ToList(),
          ListPersons = certification.ListPersons.Select(p => new ViewCrudCertificationPerson()
          {
            _id = p._id,
            Name = p.Name,
            Comments = p.Comments,
            StatusCertificationPerson = p.StatusCertificationPerson,
            TextDefault = p.TextDefault,
            TextDefaultEnd = p.TextDefaultEnd
          }).ToList(),
          Questions = certification.Questions.Select(p => new ViewListCertificationQuestions()
          {
            Answer = p.Answer,
            Question = new ViewCrudQuestions()
            {
              _id = p.Question._id,
              Name = p.Question.Name,
              Content = p.Question.Content,
              Order = p.Question.Order,
              TypeQuestion = p.Question.TypeQuestion,
              TypeRotine = p.Question.TypeRotine
            }
          }).ToList()
        };
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    #endregion

  }
#pragma warning restore 1998
}
