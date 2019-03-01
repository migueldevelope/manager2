using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.Enumns;
using Manager.Core.Interfaces;
using Manager.Core.Views;
using Manager.Data;
using Manager.Services.Commons;
using Manager.Views.Enumns;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Manager.Services.Specific
{
#pragma warning disable 1998
  public class ServiceCertification : Repository<Certification>, IServiceCertification
  {
    private readonly ServiceGeneric<Certification> certificationService;
    private readonly ServiceGeneric<CertificationPerson> certificationPersonService;
    private readonly ServiceGeneric<Person> personService;
    private readonly ServiceLog logService;
    private readonly ServiceMailModel mailModelService;
    private readonly ServiceGeneric<Questions> questionsService;
    private readonly ServiceGeneric<MailMessage> mailMessageService;
    private readonly ServiceGeneric<MailLog> mailService;
    private readonly ServiceGeneric<Parameter> parameterService;
    private readonly ServiceGeneric<TextDefault> textDefaultService;
    private readonly ServiceGeneric<Occupation> occupationService;
    private readonly ServiceLogMessages logMessagesService;
    public string path;

    public ServiceCertification(DataContext context, string pathToken)
      : base(context)
    {
      try
      {
        certificationService = new ServiceGeneric<Certification>(context);
        certificationPersonService = new ServiceGeneric<CertificationPerson>(context);
        textDefaultService = new ServiceGeneric<TextDefault>(context);
        personService = new ServiceGeneric<Person>(context);
        logService = new ServiceLog(_context);
        mailModelService = new ServiceMailModel(context);
        questionsService = new ServiceGeneric<Questions>(context);
        mailMessageService = new ServiceGeneric<MailMessage>(context);
        mailService = new ServiceGeneric<MailLog>(context);
        parameterService = new ServiceGeneric<Parameter>(context);
        logMessagesService = new ServiceLogMessages(context);
        occupationService = new ServiceGeneric<Occupation>(context);
        path = pathToken;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    private void NewOnZero()
    {
      try
      {
        var on = certificationService.GetAuthentication(p => p.Status == EnumStatus.Disabled).Count();
        if (on == 0)
        {
          var person = personService.GetAll().FirstOrDefault();
          var zero = certificationService.Insert(new Certification() { Person = person, Status = EnumStatus.Disabled, StatusCertification = EnumStatusCertification.Approved });
        }
      }
      catch (Exception)
      {
        throw;
      }
    }

    private Certification Load(Certification certification)
    {
      try
      {
        certification.Questions = new List<CertificationQuestions>();

        var itens = new List<CertificationQuestions>();


        foreach (var item in questionsService.GetAll(p => p.TypeQuestion == EnumTypeQuestion.Text & p.TypeRotine == EnumTypeRotine.Certification).ToList())
        {
          certification.Questions.Add(new CertificationQuestions()
          {
            Question =
            new Questions()
            {
              _id = item._id,
              _idAccount = item._idAccount,
              Company = item.Company,
              Content = item.Content.Replace("{company_name}", certification.Person.Company.Name)
              .Replace("{employee_name}", certification.Person.User.Name).Replace("{item_name}", certification.CertificationItem.Name),
              Name = item.Name,
              Order = item.Order,
              Status = item.Status,
              Template = item.Template,
              TypeQuestion = item.TypeQuestion,
              TypeRotine = item.TypeRotine
            },
            _idAccount = item._idAccount,
            _id = ObjectId.GenerateNewId().ToString()
          });
        }

        var text = textDefaultService.GetAll(p => p.TypeText == EnumTypeText.Certification).FirstOrDefault();
        if (text != null)
          certification.TextDefault = text.Content.Replace("{company_name}", certification.Person.Company.Name).Replace("{employee_name}", certification.Person.User.Name)
            .Replace("{manager_name}", certification.Person.Manager.Name).Replace("{item_name}", certification.CertificationItem.Name);

        return certification;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    private Certification LoadMap(Certification certification)
    {
      try
      {
        certification.Questions = new List<CertificationQuestions>();

        var itens = new List<CertificationQuestions>();


        foreach (var item in questionsService.GetAll(p => p.TypeRotine == EnumTypeRotine.Certification).OrderBy(p => p.Order).ToList())
        {
          certification.Questions.Add(new CertificationQuestions()
          {
            Question =
            new Questions()
            {
              _id = item._id,
              _idAccount = item._idAccount,
              Company = item.Company,
              Content = item.Content.Replace("{company_name}", certification.Person.Company.Name)
              .Replace("{employee_name}", certification.Person.User.Name).Replace("{item_name}", certification.CertificationItem.Name),
              Name = item.Name,
              Order = item.Order,
              Status = item.Status,
              Template = item.Template,
              TypeQuestion = item.TypeQuestion,
              TypeRotine = item.TypeRotine
            },
            _idAccount = item._idAccount,
            _id = ObjectId.GenerateNewId().ToString()
          });
        }

        var text = textDefaultService.GetAll(p => p.TypeText == EnumTypeText.Certification).FirstOrDefault();
        if (text != null)
          certification.TextDefault = text.Content.Replace("{company_name}", certification.Person.Company.Name).Replace("{employee_name}", certification.Person.User.Name)
            .Replace("{manager_name}", certification.Person.Manager.Name).Replace("{item_name}", certification.CertificationItem.Name);

        return certification;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public async void LogSave(string iduser, string local)
    {
      try
      {
        var user = personService.GetAll(p => p._id == iduser).FirstOrDefault();
        var log = new ViewLog()
        {
          Description = "Access Certification ",
          Local = local,
          Person = user
        };
        logService.NewLog(log);
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public void SetUser(IHttpContextAccessor contextAccessor)
    {
      User(contextAccessor);
      personService._user = _user;
      certificationService._user = _user;
      logService._user = _user;
      mailModelService._user = _user;
      mailMessageService._user = _user;
      mailService._user = _user;
      questionsService._user = _user;
      textDefaultService._user = _user;
      parameterService._user = _user;
      occupationService._user = _user;
      certificationPersonService._user = _user;
      logMessagesService.SetUser(_user);
      mailModelService.SetUser(contextAccessor);
    }

    // send mail
    //public async void Mail(Person person)
    //{
    //  try
    //  {
    //    //searsh model mail database
    //    var model = mailModelService.CertificationApproval(path);
    //    if (model.StatusMail == EnumStatus.Disabled)
    //      return;

    //    var url = "";
    //    var body = model.Message.Replace("{Person}", person.User.Name).Replace("{Link}", model.Link).Replace("{Manager}", person.Manager.Name).Replace("{Company}", person.Company.Name).Replace("{Occupation}", person.Occupation.Name);
    //    var message = new MailMessage
    //    {
    //      Type = EnumTypeMailMessage.Put,
    //      Name = model.Name,
    //      Url = url,
    //      Body = body
    //    };
    //    var idMessage = mailMessageService.Insert(message)._id;
    //    var sendMail = new MailLog
    //    {
    //      From = new MailLogAddress("suporte@jmsoft.com.br", "Notificação do Analisa"),
    //      To = new List<MailLogAddress>(){
    //                    new MailLogAddress(person.User.Mail, person.User.Name)
    //                },
    //      Priority = EnumPriorityMail.Low,
    //      _idPerson = person._id,
    //      NamePerson = person.User.Name,
    //      Body = body,
    //      StatusMail = EnumStatusMail.Sended,
    //      Included = DateTime.Now,
    //      Subject = model.Subject
    //    };
    //    var mailObj = mailService.Insert(sendMail);
    //    var token = SendMail(path, person, mailObj._id.ToString());
    //    var messageEnd = mailMessageService.GetAll(p => p._id == idMessage).FirstOrDefault();
    //    messageEnd.Token = token;
    //    mailMessageService.Update(messageEnd, null);
    //  }
    //  catch (Exception e)
    //  {
    //    throw new ServiceException(_user, e, this._context);
    //  }
    //}

    public string SendMail(string link, Person person, string idmail)
    {
      try
      {
        using (var client = new HttpClient())
        {
          client.BaseAddress = new Uri(link);
          var data = new
          {
            mail = person.User.Mail,
            password = person.User.Password
          };
          var json = JsonConvert.SerializeObject(data);
          var content = new StringContent(json);
          content.Headers.ContentType.MediaType = "application/json";
          client.DefaultRequestHeaders.Add("ContentType", "application/json");
          var result = client.PostAsync("manager/authentication/encrypt", content).Result;
          var resultContent = result.Content.ReadAsStringAsync().Result;
          var auth = JsonConvert.DeserializeObject<ViewPerson>(resultContent);
          client.DefaultRequestHeaders.Add("Authorization", "Bearer " + auth.Token);
          var resultMail = client.PostAsync("mail/sendmail/" + idmail, null).Result;
          return auth.Token;
        }
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string RemoveCertification(string idperson)
    {
      try
      {
        LogSave(_user._idPerson, "RemoveOnboarding:" + idperson);
        var certification = certificationService.GetAll(p => p.Person._id == idperson).FirstOrDefault();
        if (certification == null)
          return "deleted";

        certification.Status = EnumStatus.Disabled;
        certificationService.Update(certification, null);
        return "deleted";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public List<Certification> GetListExclud(ref long total, string filter, int count, int page)
    {
      try
      {
        LogSave(_user._idPerson, "ListExclud");
        int skip = (count * (page - 1));
        var detail = certificationService.GetAll(p => p.Person.User.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Person.User.Name).Skip(skip).Take(count).ToList();
        total = certificationService.GetAll(p => p.Person.User.Name.ToUpper().Contains(filter.ToUpper())).Count();

        return detail;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<BaseFields> ListPersons(string idcertification, ref long total, string filter, int count, int page)
    {
      try
      {
        int skip = (count * (page - 1));
        var details = personService.GetAll(p => p.TypeUser != EnumTypeUser.Support & p.StatusUser != EnumStatusUser.Disabled
        & p.StatusUser != EnumStatusUser.ErrorIntegration & p.TypeUser != EnumTypeUser.Administrator
        ).OrderBy(p => p.User.Name).Select(p => new BaseFields() { _id = p._id, Name = p.User.Name, Mail = p.User.Mail }).ToList();

        var detail = details.Where(p => p.Name.ToUpper().Contains(filter.ToUpper())).ToList();


        total = 999999;
        var listExclud = certificationService.GetAll(p => p._id == idcertification).FirstOrDefault().ListPersons;
        foreach (var item in listExclud)
        {
          detail.RemoveAll(p => p._id == item.IdPerson);
        }
        return detail;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public ViewCertificationProfile GetProfile(string idperson)
    {
      try
      {
        var person = personService.GetAll(p => p._id == idperson).FirstOrDefault();
        var occupation = occupationService.GetAll(p => p._id == person.Occupation._id).FirstOrDefault();

        var view = new ViewCertificationProfile();
        view.ItemSkill = new List<CertificationItem>();

        foreach (var item in occupation.Group.Company.Skills)
        {
          view.ItemSkill.Add(new CertificationItem()
          {
            _idAccount = item._idAccount,
            _id = ObjectId.GenerateNewId().ToString(),
            Status = EnumStatus.Enabled,
            Concept = item.Concept,
            IdItem = item._id,
            Name = item.Name,
            ItemCertification = item.TypeSkill == EnumTypeSkill.Hard ? EnumItemCertification.SkillCompanyHard : EnumItemCertification.SkillCompanySoft
          });
        }

        foreach (var item in occupation.Group.Skills)
        {
          view.ItemSkill.Add(new CertificationItem()
          {
            Concept = item.Concept,
            IdItem = item._id,
            Name = item.Name,
            ItemCertification = item.TypeSkill == EnumTypeSkill.Hard ? EnumItemCertification.SkillGroupHard : EnumItemCertification.SkillGroupSoft
          });
        }

        foreach (var item in occupation.Skills)
        {
          view.ItemSkill.Add(new CertificationItem()
          {
            Concept = item.Concept,
            IdItem = item._id,
            Name = item.Name,
            ItemCertification = item.TypeSkill == EnumTypeSkill.Hard ? EnumItemCertification.SkillOccupationHard : EnumItemCertification.SkillOccupationSoft
          });
        }

        view.ItemActivitie = new List<CertificationItem>();
        foreach (var item in occupation.Activities)
        {
          view.ItemActivitie.Add(new CertificationItem()
          {
            IdItem = item._id,
            Name = item.Name,
            ItemCertification = EnumItemCertification.Activitie
          });
        }

        var text = textDefaultService.GetAll(p => p.TypeText == EnumTypeText.CertificationHead).FirstOrDefault();
        if (text != null)
          view.TextDefault = text.Content.Replace("{company_name}", person.Company.Name).Replace("{employee_name}", person.User.Name)
            .Replace("{manager_name}", person.Manager.Name);

        return view;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public Certification NewCertification(CertificationItem item, string idperson)
    {
      try
      {
        var person = personService.GetAll(p => p._id == idperson).FirstOrDefault();
        item._id = ObjectId.GenerateNewId().ToString();

        var certification = new Certification()
        {
          CertificationItem = item,
          Person = person,
          Status = EnumStatus.Enabled,
          ListPersons = new List<CertificationPerson>(),
          StatusCertification = EnumStatusCertification.Wait,
          Attachments = new List<AttachmentField>()
        };

        certification = LoadMap(certification);

        return certificationService.Insert(certification);
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string AddPerson(string idcertification, BaseFields person)
    {
      try
      {
        var certification = certificationService.GetAll(p => p._id == idcertification).FirstOrDefault();

        var text = textDefaultService.GetAll(p => p.TypeText == EnumTypeText.CertificationPerson).FirstOrDefault();
        if (text != null)
          text.Content = text.Content.Replace("{company_name}", certification.Person.Company.Name).Replace("{employee_name}", certification.Person.User.Name)
            .Replace("{manager_name}", certification.Person.Manager.Name).Replace("{item_name}", certification.CertificationItem.Name);

        var textEnd = textDefaultService.GetAll(p => p.TypeText == EnumTypeText.CertificationPerson).FirstOrDefault();
        if (textEnd != null)
          textEnd.Content = textEnd.Content.Replace("{company_name}", certification.Person.Company.Name).Replace("{employee_name}", certification.Person.User.Name)
            .Replace("{manager_name}", certification.Person.Manager.Name).Replace("{item_name}", certification.CertificationItem.Name);


        var cerPerson = new CertificationPerson()
        {
          _id = ObjectId.GenerateNewId().ToString(),
          _idAccount = _user._idAccount,
          IdPerson = person._id,
          Name = person.Name,
          Mail = person.Name,
          Status = EnumStatus.Enabled,
          StatusCertificationPerson = EnumStatusCertificationPerson.Wait,
          Comments = null,
          TextDefault = text.Content,
          TextDefaultEnd = textEnd.Content
        };

        certification.ListPersons.Add(cerPerson);

        certificationService.Update(certification, null);

        return "ok";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string ApprovedCertification(string idcertification, CertificationPerson view)
    {
      try
      {
        var certification = certificationService.GetAll(p => p._id == idcertification).FirstOrDefault();

        foreach (var item in certification.ListPersons)
        {
          if (item._id == view._id)
          {
            item.StatusCertificationPerson = view.StatusCertificationPerson;
            item.Comments = view.Comments;
          }
          certificationPersonService.Update(item, null);
          certificationService.Update(certification, null);
          return "ok";
        }

        return "not found";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string RemovePerson(string idcertification, string idcertificationperson)
    {
      try
      {
        var certification = certificationService.GetAll(p => p._id == idcertification).FirstOrDefault();
        foreach (var item in certification.ListPersons)
        {
          if (item._id == idcertificationperson)
          {
            certification.ListPersons.Remove(item);
            certificationService.Update(certification, null);
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

    public string UpdateCertification(Certification certification, string idperson)
    {
      try
      {
        certificationService.Update(certification, null);
        return "update";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewCertification> ListCertificationsWaitPerson(string idperson, ref long total, string filter, int count, int page)
    {
      try
      {
        int skip = (count * (page - 1));

        var person = personService.GetAll(p => p._id == idperson).FirstOrDefault();
        List<ViewCertification> list = new List<ViewCertification>();

        //load certification guest
        foreach (var item in certificationPersonService.GetAll(p => p.StatusCertificationPerson == EnumStatusCertificationPerson.Wait).ToList())
        {
          var certification = certificationService.GetAll(
          p => p.ListPersons.Contains(item)).FirstOrDefault();
          list.Add(new ViewCertification() { _id = certification._id, Name = certification.Person.User.Name, NameItem = certification.CertificationItem.Name });
        };

        //load certification manager
        foreach (var item in certificationService.GetAll(p => p.Person.Manager._id == idperson & p.StatusCertification == EnumStatusCertification.Wait).ToList())
        {
          list.Add(new ViewCertification() { _id = item._id, Name = item.Person.User.Name, NameItem = item.CertificationItem.Name });
        };

        total = list.Count();

        return list.OrderBy(p => p.Name).Skip(skip).Take(count).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public Certification CertificationsWaitPerson(string idcertification)
    {
      try
      {
        return certificationService.GetAll(p => p._id == idcertification).FirstOrDefault();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public void SetAttachment(string idcertification, string url, string fileName, string attachmentid)
    {
      try
      {
        var certification = certificationService.GetAll(p => p._id == idcertification).FirstOrDefault();

        if (certification.Attachments == null)
        {
          certification.Attachments = new List<AttachmentField>();
        }
        certification.Attachments.Add(new AttachmentField { Url = url, Name = fileName, _idAttachment = attachmentid });
        certificationService.Update(certification, null);

      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

  }
#pragma warning restore 1998
}
