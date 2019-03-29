using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.BusinessModel;
using Manager.Core.Enumns;
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

namespace Manager.Services.Specific
{
#pragma warning disable 1998
  public class ServiceCertification : Repository<Certification>, IServiceCertification
  {
    private readonly ServiceAuthentication serviceAuthentication;
    private readonly ServiceGeneric<Certification> serviceCertification;
    private readonly ServiceGeneric<Monitoring> monitoringService;
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


    #region Constructor
    public ServiceCertification(DataContext context, string pathToken)
      : base(context)
    {
      try
      {
        serviceCertification = new ServiceGeneric<Certification>(context);
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
        monitoringService = new ServiceGeneric<Monitoring>(context);
        serviceAuthentication = new ServiceAuthentication(context);
        path = pathToken;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }
    public void SetUser(IHttpContextAccessor contextAccessor)
    {
      User(contextAccessor);
      personService._user = _user;
      serviceCertification._user = _user;
      logService._user = _user;
      mailModelService._user = _user;
      mailMessageService._user = _user;
      mailService._user = _user;
      questionsService._user = _user;
      textDefaultService._user = _user;
      parameterService._user = _user;
      occupationService._user = _user;
      monitoringService._user = _user;
      certificationPersonService._user = _user;
      logMessagesService._user = _user;
      mailModelService.SetUser(contextAccessor);
    }

    #endregion

    #region private

    private class ViewCertificationComparer : IEqualityComparer<ViewCertification>
    {
      public bool Equals(ViewCertification x, ViewCertification y)
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

      public int GetHashCode(ViewCertification obj)
      {
        return obj._id.GetHashCode();
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

    private async void LogSave(string iduser, string local)
    {
      try
      {
        var user = personService.GetAll(p => p._id == iduser).FirstOrDefault();
        var log = new ViewLog()
        {
          Description = "Access Certification ",
          Local = local,
          _idPerson = user._id
        };
        logService.NewLog(log);
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    //send mail
    private async void Mail(Person person, BaseFields guest)
    {
      try
      {
        //searsh model mail database
        var model = mailModelService.Certification(path);
        if (model.StatusMail == EnumStatus.Disabled)
          return;

        var url = "";
        var body = model.Message.Replace("{Person}", person.User.Name).Replace("{Link}", model.Link).Replace("{Manager}", person.Manager.Name).Replace("{Company}", person.Company.Name).Replace("{Occupation}", person.Occupation.Name).Replace("{Guest}", guest.Name);
        var message = new MailMessage
        {
          Type = EnumTypeMailMessage.Put,
          Name = model.Name,
          Url = url,
          Body = body
        };
        var idMessage = mailMessageService.Insert(message)._id;
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
        var mailObj = mailService.Insert(sendMail);
        var token = SendMail(path, person, mailObj._id.ToString());
        var messageEnd = mailMessageService.GetAll(p => p._id == idMessage).FirstOrDefault();
        messageEnd.Token = token;
        mailMessageService.Update(messageEnd, null);
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    private string SendMail(string link, Person person, string idmail)
    {
      try
      {
        ViewPerson view = serviceAuthentication.AuthenticationMail(person);
        using (var client = new HttpClient())
        {
          client.BaseAddress = new Uri(link);
          //var data = new
          //{
          //  mail = person.User.Mail,
          //  password = person.User.Password
          //};
          //var json = JsonConvert.SerializeObject(data);
          //var content = new StringContent(json);
          //content.Headers.ContentType.MediaType = "application/json";
          //client.DefaultRequestHeaders.Add("ContentType", "application/json");
          //var result = client.PostAsync("manager/authentication/encrypt", content).Result;
          //var resultContent = result.Content.ReadAsStringAsync().Result;
          //var auth = JsonConvert.DeserializeObject<ViewPerson>(resultContent);
          client.DefaultRequestHeaders.Add("Authorization", "Bearer " + view.Token);
          var resultMail = client.PostAsync("mail/sendmail/" + idmail, null).Result;
          return view.Token;
        }
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    #endregion

    #region certification
    public void SetAttachment(string idcertification, string url, string fileName, string attachmentid)
    {
      try
      {
        var certification = serviceCertification.GetAll(p => p._id == idcertification).FirstOrDefault();

        if (certification.Attachments == null)
        {
          certification.Attachments = new List<AttachmentField>();
        }
        certification.Attachments.Add(new AttachmentField { Url = url, Name = fileName, _idAttachment = attachmentid });
        serviceCertification.Update(certification, null);

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
        var certification = serviceCertification.GetAll(p => p.Person._id == idperson).FirstOrDefault();
        if (certification == null)
          return "deleted";

        certification.Status = EnumStatus.Disabled;
        serviceCertification.Update(certification, null);
        return "deleted";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string RemovePerson(string idcertification, string idcertificationperson)
    {
      try
      {
        var certification = serviceCertification.GetAll(p => p._id == idcertification).FirstOrDefault();
        foreach (var item in certification.ListPersons)
        {
          if (item._id == idcertificationperson)
          {
            certification.ListPersons.Remove(item);
            serviceCertification.Update(certification, null);
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

    public string UpdateStatusCertification(ViewCertificationStatus viewcertification, string idperson)
    {
      try
      {
        var certification = serviceCertification.GetAll(p => p._id == viewcertification._idCertification).FirstOrDefault();

        foreach (var item in certification.ListPersons)
        {
          if (item.IdPerson == idperson)
          {
            item.StatusCertificationPerson = viewcertification.StatusCertificationPerson;
            item.Comments = viewcertification.Comments;
            item.DateApprovation = DateTime.Now;
            certificationPersonService.Update(item, null);

            serviceCertification.Update(certification, null);



            var open = certification.ListPersons.Where(p => p.StatusCertificationPerson == EnumStatusCertificationPerson.Wait).Count();
            if (open == 0)
            {
              var disapproved = certification.ListPersons.Where(p => p.StatusCertificationPerson == EnumStatusCertificationPerson.Disapproved).Count();
              if (disapproved > 0)
                certification.StatusCertification = EnumStatusCertification.Disaproved;
              else
                certification.StatusCertification = EnumStatusCertification.Approved;

              certification.DateEnd = DateTime.Now;

              serviceCertification.Update(certification, null);
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

    public List<ViewCertification> ListCertificationsWaitPerson(string idperson, ref long total, string filter, int count, int page)
    {
      try
      {
        int skip = (count * (page - 1));

        var person = personService.GetAll(p => p._id == idperson).FirstOrDefault();
        List<ViewCertification> list = new List<ViewCertification>();

        //load certification guest
        foreach (var item in certificationPersonService.GetAll(p => p.StatusCertificationPerson == EnumStatusCertificationPerson.Wait & p.IdPerson == idperson).ToList())
        {
          var certification = serviceCertification.GetAll(
          p => p.ListPersons.Contains(item)).FirstOrDefault();
          if (certification != null)
          {
            list.Add(new ViewCertification() { _id = certification._id, Name = certification.Person.User.Name, NameItem = certification.CertificationItem.Name, Manager = false });
          }

        };

        //load certification manager
        foreach (var item in serviceCertification.GetAll(p => p.Person.Manager._id == idperson & p.StatusCertification == EnumStatusCertification.Wait).ToList())
        {
          list.Add(new ViewCertification() { _id = item._id, Name = item.Person.User.Name, NameItem = item.CertificationItem.Name, Manager = true });
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

    public List<ViewCertificationItem> ListCertificationPerson(string idperson, ref long total, string filter, int count, int page)
    {
      try
      {
        int skip = (count * (page - 1));

        var result = serviceCertification.GetAll(p => p.Person._id == idperson & p.StatusCertification == EnumStatusCertification.Approved
        & p.CertificationItem.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.CertificationItem.Name).Skip(skip).Take(count)
        .Select(p => new ViewCertificationItem
        {
          NameItem = p.CertificationItem.Name,
          ItemCertificationView =
          (p.CertificationItem.ItemCertification == EnumItemCertification.SkillCompanyHard) ? EnumItemCertificationView.Company :
            (p.CertificationItem.ItemCertification == EnumItemCertification.SkillCompanySoft) ? EnumItemCertificationView.Company :
              (p.CertificationItem.ItemCertification == EnumItemCertification.SkillGroupSoft) ? EnumItemCertificationView.Soft :
                (p.CertificationItem.ItemCertification == EnumItemCertification.SkillOccupationSoft) ? EnumItemCertificationView.Soft :
                EnumItemCertificationView.Hard
        }).ToList();

        total = result.Count();

        return result;
      }
      catch (Exception e)
      {
        throw e;
      }
    }


    public List<ViewListCertification> GetListExclud(ref long total, string filter, int count, int page)
    {
      try
      {
        LogSave(_user._idPerson, "ListExclud");
        int skip = (count * (page - 1));
        var detail = serviceCertification.GetAll(p => p.Person.User.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Person.User.Name).Skip(skip).Take(count).ToList();
        total = serviceCertification.GetAll(p => p.Person.User.Name.ToUpper().Contains(filter.ToUpper())).Count();

        return detail.Select(p => new ViewListCertification()
        {
          _id = p._id,
          Name = p.Person.User.Name,
          idPerson = p.Person._id,
          StatusCertification = p.StatusCertification
        }).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewListBasePerson> ListPersons(string idcertification, ref long total, string filter, int count, int page)
    {
      try
      {
        int skip = (count * (page - 1));
        var certificaiton = serviceCertification.GetAll(p => p._id == idcertification).FirstOrDefault();

        var details = personService.GetAll(p => p.TypeUser != EnumTypeUser.Support & p.StatusUser != EnumStatusUser.Disabled
        & p.StatusUser != EnumStatusUser.ErrorIntegration & p.TypeUser != EnumTypeUser.Administrator
        ).OrderBy(p => p.User.Name).Select(p => new ViewListBasePerson() { _id = p._id, Name = p.User.Name, Mail = p.User.Mail }).ToList();

        var detail = details.Where(p => p.Name.ToUpper().Contains(filter.ToUpper())).ToList();


        total = 999999;
        var listExclud = serviceCertification.GetAll(p => p._id == idcertification).FirstOrDefault().ListPersons;
        foreach (var item in listExclud)
        {
          detail.RemoveAll(p => p._id == item.IdPerson);
        }
        detail.RemoveAll(p => p._id == certificaiton.Person._id);

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
        var remove = serviceCertification.GetAll(p => p.Person._id == idperson
        & p.StatusCertification != EnumStatusCertification.Disaproved
        & p.StatusCertification != EnumStatusCertification.Open).ToList();


        var view = new ViewCertificationProfile
        {
          ItemSkill = new List<CertificationItem>()
        };

        foreach (var item in occupation.Group.Company.Skills)
        {
          if (remove.Where(p => p.CertificationItem.IdItem == item._id).Count() == 0)
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
        }

        foreach (var item in occupation.Group.Skills)
        {
          if (remove.Where(p => p.CertificationItem.IdItem == item._id).Count() == 0)
          {
            view.ItemSkill.Add(new CertificationItem()
            {
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
            view.ItemSkill.Add(new CertificationItem()
            {
              Concept = item.Concept,
              IdItem = item._id,
              Name = item.Name,
              ItemCertification = item.TypeSkill == EnumTypeSkill.Hard ? EnumItemCertification.SkillOccupationHard : EnumItemCertification.SkillOccupationSoft
            });
          }

        }

        view.ItemActivitie = new List<CertificationItem>();
        foreach (var item in occupation.Activities)
        {
          if (remove.Where(p => p.CertificationItem.IdItem == item._id).Count() == 0)
          {
            view.ItemActivitie.Add(new CertificationItem()
            {
              IdItem = item._id,
              Name = item.Name,
              ItemCertification = EnumItemCertification.Activitie
            });
          }
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

    public ViewCrudCertification NewCertification(CertificationItem item, string idperson)
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
          StatusCertification = EnumStatusCertification.Open,
          Attachments = new List<AttachmentField>()
        };

        certification = LoadMap(certification);

        var view = serviceCertification.Insert(certification);

        return new ViewCrudCertification()
        {
          _id = view._id,
          Name = view.Person.User.Name,
          _idPerson = view.Person._id,
          TextDefault = view.TextDefault,
          StatusCertification = view.StatusCertification,
          CertificationItem = new ViewListCertificationItem()
          {
            _id = view.CertificationItem._id,
            Name = view.CertificationItem.Name,
            ItemCertification = view.CertificationItem.ItemCertification,
            _idItem = view.CertificationItem.IdItem
          },
          Attachments = view.Attachments.Select(p => new ViewCrudAttachmentField() { _idAttachment = p._idAttachment, Name = p.Name, Url = p.Url }).ToList(),
          ListPersons = view.ListPersons.Select(p => new ViewCrudCertificationPerson()
          {
            _id = p._id,
            Name = p.Name,
            Comments = p.Comments,
            StatusCertificationPerson = p.StatusCertificationPerson,
            TextDefault = p.TextDefault,
            TextDefaultEnd = p.TextDefaultEnd
          }).ToList(),
          Questions = view.Questions.Select(p => new ViewListCertificationQuestions()
          {
            Answer = p.Answer,
            Question = new ViewListQuestions()
            {
              _id = p.Question._id,
              Name = p.Question.Name
            }
          }).ToList()
        };
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string AddPerson(string idcertification, ViewListBasePerson person)
    {
      try
      {
        var certification = serviceCertification.GetAll(p => p._id == idcertification).FirstOrDefault();

        var text = textDefaultService.GetAll(p => p.TypeText == EnumTypeText.CertificationPerson).FirstOrDefault();
        if (text != null)
          text.Content = text.Content.Replace("{company_name}", certification.Person.Company.Name).Replace("{employee_name}", certification.Person.User.Name)
            .Replace("{manager_name}", certification.Person.Manager.Name).Replace("{item_name}", certification.CertificationItem.Name);

        var textEnd = textDefaultService.GetAll(p => p.TypeText == EnumTypeText.CertificationPersonEnd).FirstOrDefault();
        if (textEnd != null)
          textEnd.Content = textEnd.Content.Replace("{company_name}", certification.Person.Company.Name).Replace("{employee_name}", certification.Person.User.Name)
            .Replace("{manager_name}", certification.Person.Manager.Name).Replace("{item_name}", certification.CertificationItem.Name);


        var cerPerson = new CertificationPerson()
        {
          IdPerson = person._id,
          Name = person.Name,
          Mail = person.Mail,
          Status = EnumStatus.Enabled,
          StatusCertificationPerson = EnumStatusCertificationPerson.Wait,
          Comments = null,
          TextDefault = text.Content,
          TextDefaultEnd = textEnd.Content
        };

        cerPerson = certificationPersonService.Insert(cerPerson);

        certification.ListPersons.Add(cerPerson);



        serviceCertification.Update(certification, null);

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
        var certification = serviceCertification.GetAll(p => p._id == idcertification).FirstOrDefault();

        foreach (var item in certification.ListPersons)
        {
          if (item._id == view._id)
          {
            item.StatusCertificationPerson = view.StatusCertificationPerson;
            item.Comments = view.Comments;
          }
          certificationPersonService.Update(item, null);
          serviceCertification.Update(certification, null);
          return "ok";
        }

        return "not found";
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
        var certification = serviceCertification.GetAll(p => p._id == view._id).FirstOrDefault();
        certification.StatusCertification = view.StatusCertification;
        certification.TextDefault = view.TextDefault;
        foreach (var question in certification.Questions)
        {
          var item = view.Questions.Where(p => p.Question._id == question.Question._id).FirstOrDefault();
          question.Answer = item.Answer;
        };

        if (certification.StatusCertification == EnumStatusCertification.Wait)
        {
          var monitoring = monitoringService.GetAll(p => p._id == idmonitoring & p.StatusMonitoring == EnumStatusMonitoring.Show).FirstOrDefault();
          if (monitoring != null)
          {
            monitoringService.Delete(idmonitoring, true);
          }
        }

        if (certification.StatusCertification == EnumStatusCertification.Wait)
        {
          certification.DateBegin = DateTime.Now;
          foreach (var item in certification.ListPersons)
          {
            try
            {
              Mail(certification.Person, new BaseFields() { Name = item.Name, Mail = item.Mail });
            }
            catch (Exception)
            {

            }

          }
        }

        serviceCertification.Update(certification, null);
        return "update";
      }
      catch (Exception e)
      {
        throw e;
      }
    }


    public ViewListCertification CertificationsWaitPerson(string idcertification)
    {
      try
      {
        return serviceCertification.GetAll(p => p._id == idcertification).Select(p => new ViewListCertification()
        {
          _id = p._id,
          Name = p.Person.User.Name,
          idPerson = p.Person._id,
          StatusCertification = p.StatusCertification
        }).FirstOrDefault();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    #endregion

    #region old

    public List<Certification> GetListExcludOld(ref long total, string filter, int count, int page)
    {
      try
      {
        LogSave(_user._idPerson, "ListExclud");
        int skip = (count * (page - 1));
        var detail = serviceCertification.GetAll(p => p.Person.User.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Person.User.Name).Skip(skip).Take(count).ToList();
        total = serviceCertification.GetAll(p => p.Person.User.Name.ToUpper().Contains(filter.ToUpper())).Count();

        return detail;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<BaseFields> ListPersonsOld(string idcertification, ref long total, string filter, int count, int page)
    {
      try
      {
        int skip = (count * (page - 1));
        var certificaiton = serviceCertification.GetAll(p => p._id == idcertification).FirstOrDefault();

        var details = personService.GetAll(p => p.TypeUser != EnumTypeUser.Support & p.StatusUser != EnumStatusUser.Disabled
        & p.StatusUser != EnumStatusUser.ErrorIntegration & p.TypeUser != EnumTypeUser.Administrator
        ).OrderBy(p => p.User.Name).Select(p => new BaseFields() { _id = p._id, Name = p.User.Name, Mail = p.User.Mail }).ToList();

        var detail = details.Where(p => p.Name.ToUpper().Contains(filter.ToUpper())).ToList();


        total = 999999;
        var listExclud = serviceCertification.GetAll(p => p._id == idcertification).FirstOrDefault().ListPersons;
        foreach (var item in listExclud)
        {
          detail.RemoveAll(p => p._id == item.IdPerson);
        }
        detail.RemoveAll(p => p._id == certificaiton.Person._id);

        return detail;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public ViewCertificationProfile GetProfileOld(string idperson)
    {
      try
      {
        var person = personService.GetAll(p => p._id == idperson).FirstOrDefault();
        var occupation = occupationService.GetAll(p => p._id == person.Occupation._id).FirstOrDefault();
        var remove = serviceCertification.GetAll(p => p.Person._id == idperson
        & p.StatusCertification != EnumStatusCertification.Disaproved
        & p.StatusCertification != EnumStatusCertification.Open).ToList();


        var view = new ViewCertificationProfile
        {
          ItemSkill = new List<CertificationItem>()
        };

        foreach (var item in occupation.Group.Company.Skills)
        {
          if (remove.Where(p => p.CertificationItem.IdItem == item._id).Count() == 0)
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
        }

        foreach (var item in occupation.Group.Skills)
        {
          if (remove.Where(p => p.CertificationItem.IdItem == item._id).Count() == 0)
          {
            view.ItemSkill.Add(new CertificationItem()
            {
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
            view.ItemSkill.Add(new CertificationItem()
            {
              Concept = item.Concept,
              IdItem = item._id,
              Name = item.Name,
              ItemCertification = item.TypeSkill == EnumTypeSkill.Hard ? EnumItemCertification.SkillOccupationHard : EnumItemCertification.SkillOccupationSoft
            });
          }

        }

        view.ItemActivitie = new List<CertificationItem>();
        foreach (var item in occupation.Activities)
        {
          if (remove.Where(p => p.CertificationItem.IdItem == item._id).Count() == 0)
          {
            view.ItemActivitie.Add(new CertificationItem()
            {
              IdItem = item._id,
              Name = item.Name,
              ItemCertification = EnumItemCertification.Activitie
            });
          }
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

    public Certification NewCertificationOld(CertificationItem item, string idperson)
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
          StatusCertification = EnumStatusCertification.Open,
          Attachments = new List<AttachmentField>()
        };

        certification = LoadMap(certification);

        return serviceCertification.Insert(certification);
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string AddPersonOld(string idcertification, BaseFields person)
    {
      try
      {
        var certification = serviceCertification.GetAll(p => p._id == idcertification).FirstOrDefault();

        var text = textDefaultService.GetAll(p => p.TypeText == EnumTypeText.CertificationPerson).FirstOrDefault();
        if (text != null)
          text.Content = text.Content.Replace("{company_name}", certification.Person.Company.Name).Replace("{employee_name}", certification.Person.User.Name)
            .Replace("{manager_name}", certification.Person.Manager.Name).Replace("{item_name}", certification.CertificationItem.Name);

        var textEnd = textDefaultService.GetAll(p => p.TypeText == EnumTypeText.CertificationPersonEnd).FirstOrDefault();
        if (textEnd != null)
          textEnd.Content = textEnd.Content.Replace("{company_name}", certification.Person.Company.Name).Replace("{employee_name}", certification.Person.User.Name)
            .Replace("{manager_name}", certification.Person.Manager.Name).Replace("{item_name}", certification.CertificationItem.Name);


        var cerPerson = new CertificationPerson()
        {
          IdPerson = person._id,
          Name = person.Name,
          Mail = person.Mail,
          Status = EnumStatus.Enabled,
          StatusCertificationPerson = EnumStatusCertificationPerson.Wait,
          Comments = null,
          TextDefault = text.Content,
          TextDefaultEnd = textEnd.Content
        };

        cerPerson = certificationPersonService.Insert(cerPerson);

        certification.ListPersons.Add(cerPerson);



        serviceCertification.Update(certification, null);

        return "ok";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string ApprovedCertificationOld(string idcertification, CertificationPerson view)
    {
      try
      {
        var certification = serviceCertification.GetAll(p => p._id == idcertification).FirstOrDefault();

        foreach (var item in certification.ListPersons)
        {
          if (item._id == view._id)
          {
            item.StatusCertificationPerson = view.StatusCertificationPerson;
            item.Comments = view.Comments;
          }
          certificationPersonService.Update(item, null);
          serviceCertification.Update(certification, null);
          return "ok";
        }

        return "not found";
      }
      catch (Exception e)
      {
        throw e;
      }
    }


    public string UpdateCertificationOld(Certification certification, string idperson, string idmonitoring)
    {
      try
      {
        if (certification.StatusCertification == EnumStatusCertification.Wait)
        {
          var monitoring = monitoringService.GetAll(p => p._id == idmonitoring & p.StatusMonitoring == EnumStatusMonitoring.Show).FirstOrDefault();
          if (monitoring != null)
          {
            monitoringService.Delete(idmonitoring, true);
          }
        }

        if (certification.StatusCertification == EnumStatusCertification.Wait)
        {
          certification.DateBegin = DateTime.Now;
          foreach (var item in certification.ListPersons)
          {
            try
            {
              Mail(certification.Person, new BaseFields() { Name = item.Name, Mail = item.Mail });
            }
            catch (Exception)
            {

            }

          }
        }

        serviceCertification.Update(certification, null);
        return "update";
      }
      catch (Exception e)
      {
        throw e;
      }
    }


    public Certification CertificationsWaitPersonOld(string idcertification)
    {
      try
      {
        return serviceCertification.GetAll(p => p._id == idcertification).FirstOrDefault();
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
