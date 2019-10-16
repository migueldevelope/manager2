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
#pragma warning disable 1998
  public class ServiceCheckpoint : Repository<Checkpoint>, IServiceCheckpoint
  {
    private readonly ServiceAuthentication serviceAuthentication;
    private readonly ServiceGeneric<Checkpoint> serviceCheckpoint;
    private readonly ServiceGeneric<Company> serviceCompany;
    private readonly ServiceLog serviceLog;
    private readonly ServiceLogMessages serviceLogMessages;
    private readonly ServiceGeneric<MailLog> serviceMail;
    private readonly ServiceMailModel serviceMailModel;
    private readonly ServiceGeneric<Parameter> serviceParameter;
    private readonly ServiceGeneric<Person> servicePerson;
    private readonly ServiceGeneric<Questions> serviceQuestions;
    private readonly ServiceGeneric<TextDefault> serviceTextDefault;
    private readonly IServiceControlQueue serviceControlQueue;
    public string path;

    #region Contructor
    public ServiceCheckpoint(DataContext context, DataContext contextLog, string pathToken, IServiceControlQueue _serviceControlQueue) : base(context)
    {
      try
      {
        serviceAuthentication = new ServiceAuthentication(context, contextLog, _serviceControlQueue, pathToken);
        serviceCheckpoint = new ServiceGeneric<Checkpoint>(context);
        serviceCompany = new ServiceGeneric<Company>(context);
        serviceLog = new ServiceLog(context, contextLog);
        serviceLogMessages = new ServiceLogMessages(context);
        serviceMail = new ServiceGeneric<MailLog>(context);
        serviceMailModel = new ServiceMailModel(contextLog);
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
      serviceCheckpoint._user = _user;
      serviceCompany._user = _user;
      serviceLog.SetUser(_user);
      serviceLogMessages.SetUser(_user);
      serviceMail._user = _user;
      serviceMailModel.SetUser(_user);
      serviceParameter._user = _user;
      servicePerson._user = _user;
      serviceQuestions._user = _user;
      serviceTextDefault._user = _user;
    }
    public void SetUser(BaseUser user)
    {
      _user = user;
      serviceCheckpoint._user = user;
      serviceCompany._user = user;
      serviceLog.SetUser(user);
      serviceLogMessages.SetUser(user);
      serviceMail._user = user;
      serviceMailModel.SetUser(user);
      serviceParameter._user = user;
      servicePerson._user = user;
      serviceQuestions._user = user;
      serviceTextDefault._user = user;
    }
    #endregion

    #region Checkpoint

    public List<ViewListCheckpoint> ListWaitManager(string idmanager, ref long total, string filter, int count, int page)
    {
      try
      {
        List<ViewListCheckpoint> list = servicePerson.GetAllNewVersion(p => p.Manager._id == idmanager && p.Occupation != null &&
                                        p.StatusUser != EnumStatusUser.Disabled &&
                                        p.TypeJourney == EnumTypeJourney.Checkpoint &&
                                        p.User.Name.ToUpper().Contains(filter.ToUpper()), count, count * (page - 1), "User.Name").Result
                                        .Select(p => new ViewListCheckpoint()
                                        {
                                          _id = string.Empty,
                                          _idPerson = p._id,
                                          Name = p.User.Name,
                                          OccupationName = p.Occupation.Name,
                                          StatusCheckpoint = EnumStatusCheckpoint.Open,
                                          TypeCheckpoint = EnumCheckpoint.None,
                                          Photo = p.User?.PhotoUrl
                                        }).ToList();
        List<ViewListCheckpoint> detail = new List<ViewListCheckpoint>();
        if (serviceCheckpoint.Exists("Checkpoint"))
        {
          Checkpoint checkpoint;
          foreach (var item in list)
          {
            checkpoint = serviceCheckpoint.GetNewVersion(x => x.Person._id == item._idPerson && x.StatusCheckpoint != EnumStatusCheckpoint.End).Result;
            if (checkpoint != null)
            {
              item._id = checkpoint._id;
              item.StatusCheckpoint = checkpoint.StatusCheckpoint;
              item.TypeCheckpoint = checkpoint.TypeCheckpoint;
              item.OccupationName = checkpoint.Occupation?.Name;
            }
            else
              checkpoint = serviceCheckpoint.GetNewVersion(x => x.Person._id == item._idPerson && x.StatusCheckpoint == EnumStatusCheckpoint.End).Result;

            if (checkpoint?.TypeCheckpoint != EnumCheckpoint.Disapproved)
              detail.Add(item);

          }
        }
        else
          detail = list;

        total = servicePerson.CountNewVersion(p => p.Manager._id == idmanager && p.Occupation != null &&
                                p.TypeJourney == EnumTypeJourney.Checkpoint &&
                                p.User.Name.ToUpper().Contains(filter.ToUpper())).Result;

        return detail;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewListCheckpoint> ListWaitManager_V2(List<ViewListIdIndicators> persons, ref long total, string filter, int count, int page)
    {
      try
      {

        int skip = (count * (page - 1));

        List<ViewListCheckpoint> detail = new List<ViewListCheckpoint>();

        persons = persons.Where(p => p.TypeJourney == EnumTypeJourney.Checkpoint).ToList();

        foreach (var item in persons)
        {
          var checkpoint = serviceCheckpoint.GetNewVersion(x => x.Person._id == item._id && x.StatusCheckpoint != EnumStatusCheckpoint.End).Result;
          var view = new ViewListCheckpoint()
          {
            Name = item.Name,
            OccupationName = item.OccupationName,
            StatusCheckpoint = EnumStatusCheckpoint.Open,
            TypeCheckpoint = EnumCheckpoint.None
          };
          if (checkpoint != null)
          {
            view._id = checkpoint._id;
            view.StatusCheckpoint = checkpoint.StatusCheckpoint;
            view.TypeCheckpoint = checkpoint.TypeCheckpoint;
            view.OccupationName = checkpoint.Occupation?.Name;
          }
          detail.Add(view);
        }

        total = detail.Count();

        return detail.OrderBy(p => p.Name).Skip(skip).Take(count).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public ViewListCheckpoint ListWaitPerson(string idperson)
    {
      try
      {
        List<ViewListCheckpoint> list = servicePerson.GetAllNewVersion(p => p._id == idperson && p.Occupation != null && p.TypeJourney == EnumTypeJourney.Checkpoint).Result
          .Select(p => new ViewListCheckpoint()
          {
            _id = string.Empty,
            _idPerson = p._id,
            Name = p.User.Name,
            OccupationName = p.Occupation.Name,
            StatusCheckpoint = EnumStatusCheckpoint.Open,
            TypeCheckpoint = EnumCheckpoint.None
          }).ToList();
        List<ViewListCheckpoint> detail = new List<ViewListCheckpoint>();
        if (serviceCheckpoint.Exists("Checkpoint"))
        {
          Checkpoint checkpoint;
          foreach (var item in list)
          {
            checkpoint = serviceCheckpoint.GetNewVersion(x => x.Person._id == item._idPerson && x.StatusCheckpoint != EnumStatusCheckpoint.End).Result;
            if (checkpoint != null)
            {
              item._id = checkpoint._id;
              item.StatusCheckpoint = checkpoint.StatusCheckpoint;
              item.TypeCheckpoint = checkpoint.TypeCheckpoint;
              item.OccupationName = checkpoint.Occupation?.Name;
              detail.Add(item);
            }
            else
              detail.Add(item);
          }
        }
        else
          detail = list;
        return detail.FirstOrDefault();
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public List<ViewListCheckpoint> ListEnded(ref long total, string filter, int count, int page)
    {
      try
      {

        int skip = (count * (page - 1));
        var detail = serviceCheckpoint.GetAllNewVersion(p => p.Person.Name.ToUpper().Contains(filter.ToUpper()), count, skip, "Person.User.Name").Result
          .Select(p => new ViewListCheckpoint()
          {
            _id = p._id,
            _idPerson = p.Person._id,
            Name = p.Person.Name,
            StatusCheckpoint = p.StatusCheckpoint,
            TypeCheckpoint = p.TypeCheckpoint,
            OccupationName = p.Occupation?.Name
          }).ToList();

        total = serviceCheckpoint.CountNewVersion(p => p.Person.Name.ToUpper().Contains(filter.ToUpper())).Result;

        return detail;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public ViewListCheckpoint NewCheckpoint(string idperson)
    {
      try
      {
        Person person = servicePerson.GetNewVersion(p => p._id == idperson && p.TypeJourney == EnumTypeJourney.Checkpoint).Result;
        if (person == null)
          throw new Exception("Person not available!");

        Checkpoint checkpoint = null;
        if (serviceCheckpoint.Exists("Checkpoint"))
          checkpoint = serviceCheckpoint.GetNewVersion(x => x.Person._id == idperson && x.StatusCheckpoint != EnumStatusCheckpoint.End).Result;

        if (checkpoint == null)
        {
          checkpoint = new Checkpoint
          {
            Person = person.GetViewListPersonInfo(),
            StatusCheckpoint = EnumStatusCheckpoint.Open,
            DateBegin = DateTime.Now,
            DataAccess = person.User.DateAdm == null ? DateTime.Now : DateTime.Parse(person.User.DateAdm.ToString()).AddDays(Deadline()),
            TypeCheckpoint = EnumCheckpoint.None,
            Occupation = person.Occupation
          };
          checkpoint = LoadMap(checkpoint);
          checkpoint = serviceCheckpoint.InsertNewVersion(checkpoint).Result;
          Task.Run(() => LogSave(_user._idPerson, string.Format("Start new process | {0}", checkpoint._id)));
        }
        return new ViewListCheckpoint()
        {
          _id = checkpoint._id,
          Name = checkpoint.Person.Name,
          OccupationName = checkpoint.Person.Occupation,
          StatusCheckpoint = checkpoint.StatusCheckpoint,
          TypeCheckpoint = checkpoint.TypeCheckpoint,
          _idPerson = checkpoint.Person._id
        };
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public ViewCrudCheckpoint GetCheckpoint(string id)
    {
      try
      {
        Checkpoint checkpoint = serviceCheckpoint.GetFreeNewVersion(p => p._id == id).Result;
        Person person = servicePerson.GetFreeNewVersion(p => p._id == checkpoint.Person._id).Result;

        if (checkpoint == null)
          return null;
        //throw new Exception("Checkpoint not available!");

        if (checkpoint.StatusCheckpoint == EnumStatusCheckpoint.Open)
        {
          checkpoint.StatusCheckpoint = EnumStatusCheckpoint.Wait;
          serviceCheckpoint.Update(checkpoint, null).Wait();
        }

        return new ViewCrudCheckpoint()
        {
          _id = checkpoint._id,
          DataAccess = checkpoint.DataAccess,
          DateBegin = checkpoint.DateBegin,
          DateEnd = checkpoint.DateEnd,
          StatusCheckpoint = checkpoint.StatusCheckpoint,
          TypeCheckpoint = checkpoint.TypeCheckpoint,
          Comments = checkpoint.Comments,
          Person = checkpoint.Person,
          Occupation = person.Occupation,
          TextDefault = checkpoint.TextDefault,
          Questions = checkpoint.Questions?.OrderBy(o => o.Question.Order).Select(x => new ViewCrudCheckpointQuestion()
          {
            _id = x._id,
            Mark = x.Mark,
            Question = new ViewCrudQuestions()
            {
              _id = x.Question._id,
              Name = x.Question.Name,
              Content = x.Question.Content,
              Order = x.Question.Order,
              TypeRotine = x.Question.TypeRotine,
              TypeQuestion = x.Question.TypeQuestion
            },
            Itens = x.Itens?.OrderBy(o => o.Question.Name).Select(i => new ViewCrudCheckpointQuestion()
            {
              _id = i._id,
              Question = new ViewCrudQuestions()
              {
                _id = i.Question._id,
                Content = i.Question.Content,
                Name = i.Question.Name,
                Order = i.Question.Order,
                TypeQuestion = i.Question.TypeQuestion,
                TypeRotine = i.Question.TypeRotine
              },
              Mark = i.Mark,
              Itens = null
            }).ToList()
          }).ToList()
        };
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public string DeleteCheckpoint(string idcheckpoint)
    {
      try
      {
        var checkpoint = serviceCheckpoint.GetNewVersion(p => p.Person._id == idcheckpoint).Result;
        if (checkpoint == null)
          return "Checkpoint not available!";

        Task.Run(() => LogSave(_user._idPerson, string.Format("Delete | {0}", idcheckpoint)));
        checkpoint.Status = EnumStatus.Disabled;
        serviceCheckpoint.Update(checkpoint, null).Wait();
        return "Checkpoint deleted!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public string UpdateCheckpoint(ViewCrudCheckpoint view)
    {
      try
      {
        Checkpoint checkpoint = serviceCheckpoint.GetNewVersion(p => p._id == view._id && p.StatusCheckpoint != EnumStatusCheckpoint.End).Result;
        Person person = servicePerson.GetNewVersion(p => p._id == checkpoint.Person._id).Result;

        checkpoint.Comments = view.Comments;

        if (checkpoint == null)
          throw new Exception("Checkpoint not available!");

        if (view.StatusCheckpoint == EnumStatusCheckpoint.End)
        {
          checkpoint.DateEnd = DateTime.Now;
          checkpoint.StatusCheckpoint = view.StatusCheckpoint;
          checkpoint.TypeCheckpoint = view.TypeCheckpoint;
          if (view.TypeCheckpoint == EnumCheckpoint.Approved)
          {
            person.TypeJourney = EnumTypeJourney.Monitoring;
            servicePerson.Update(person, null).Wait();

            Task.Run(() => MailRhApproved(person, "Aprovado"));
            Task.Run(() => MailPerson(person, "Aprovado"));

            serviceLogMessages.NewLogMessage("Checkpoint", string.Format(" Colaborador {0} aprovado no Checkpoint", person.User.Name), person);
            Task.Run(() => LogSave(_user._idPerson, string.Format("Approved | {0}.", view._id)));
          }
          else
          {
            Task.Run(() => MailRhDisapproved(person, "Reprovado"));
            Task.Run(() => LogSave(_user._idPerson, string.Format("Disapproved | {0}.", view._id)));
          }
          Task.Run(() => SendQueue(checkpoint._id, person._id));

        }
        checkpoint.Comments = view.Comments;
        checkpoint.Questions = view.Questions?.Select(p => new CheckpointQuestions()
        {
          _id = p._id,
          Mark = p.Mark,
          Question = p.Question,
          Itens = p.Itens?.Select(x => new CheckpointQuestions()
          {
            _id = x._id,
            Question = new ViewCrudQuestions()
            {
              _id = x.Question._id,
              Content = x.Question.Content,
              Name = x.Question.Name,
              Order = x.Question.Order,
              TypeQuestion = x.Question.TypeQuestion,
              TypeRotine = x.Question.TypeRotine
            },
            Itens = null,
            Mark = x.Mark
          }).ToList()
        }).ToList();
        serviceCheckpoint.Update(checkpoint, null).Wait();
        return "Checkpoint altered!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public ViewCrudCheckpoint PersonCheckpointEnd(string idperson)
    {
      try
      {
        Checkpoint checkpoint = serviceCheckpoint.GetFreeNewVersion(p => p.Person._id == idperson && p.StatusCheckpoint == EnumStatusCheckpoint.End).Result;
        if (checkpoint == null)
          return null;

        Person person = servicePerson.GetFreeNewVersion(p => p._id == checkpoint.Person._id).Result;
        Task.Run(() => LogSave(_user._idPerson, string.Format("Person ended | {0}", checkpoint._id)));

        return new ViewCrudCheckpoint()
        {
          _id = checkpoint._id,
          DataAccess = checkpoint.DataAccess,
          DateBegin = checkpoint.DateBegin,
          DateEnd = checkpoint.DateEnd,
          StatusCheckpoint = checkpoint.StatusCheckpoint,
          TypeCheckpoint = checkpoint.TypeCheckpoint,
          Person = checkpoint.Person,
          Occupation = person.Occupation,
          TextDefault = checkpoint.TextDefault,
          Questions = checkpoint.Questions?.OrderBy(o => o.Question.Order).Select(x => new ViewCrudCheckpointQuestion()
          {
            _id = x._id,
            Mark = x.Mark,
            Question = new ViewCrudQuestions()
            {
              _id = x.Question._id,
              Name = x.Question.Name,
              Content = x.Question.Content,
              Order = x.Question.Order,
              TypeRotine = x.Question.TypeRotine,
              TypeQuestion = x.Question.TypeQuestion
            },
            Itens = x.Itens?.OrderBy(o => o.Question.Name).Select(i => new ViewCrudCheckpointQuestion()
            {
              _id = i._id,
              Question = new ViewCrudQuestions()
              {
                _id = i.Question._id,
                Content = i.Question.Content,
                Name = i.Question.Name,
                Order = i.Question.Order,
                TypeQuestion = i.Question.TypeQuestion,
                TypeRotine = i.Question.TypeRotine
              },
              Mark = i.Mark,
              Itens = null
            }).ToList()
          }).ToList()
        };
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    #endregion

    #region Private


    private void SendQueue(string id, string idperson)
    {
      try
      {
        var data = new ViewCrudMaturityRegister
        {
          _idPerson = idperson,
          TypeMaturity = EnumTypeMaturity.Checkpoint,
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

    private Checkpoint LoadMap(Checkpoint checkpoint)
    {
      try
      {
        var person = servicePerson.GetNewVersion(p => p._id == checkpoint.Person._id).Result;
        var company = serviceCompany.GetNewVersion(p => p._id == person.Company._id).Result;

        checkpoint.Questions = new List<CheckpointQuestions>();
        var itens = new List<CheckpointQuestions>();
        foreach (var item in company.Skills)
        {
          itens.Add(new CheckpointQuestions()
          {
            Question = new ViewCrudQuestions()
            {
              Name = item.Name,
              Content = item.Concept,
              Order = 0,
              TypeQuestion = EnumTypeQuestion.Skill,
              _id = ObjectId.GenerateNewId().ToString(),
              TypeRotine = EnumTypeRotine.Checkpoint
            },
            _id = ObjectId.GenerateNewId().ToString()
          });
        }
        foreach (var item in serviceQuestions.GetAllNewVersion(p => p.TypeQuestion == EnumTypeQuestion.Skill & p.TypeRotine == EnumTypeRotine.Checkpoint).Result)
        {
          checkpoint.Questions.Add(new CheckpointQuestions()
          {
            Question =
             new ViewCrudQuestions()
             {
               _id = item._id,
               Content = item.Content.Replace("{company_name}", company.Name).Replace("{employee_name}", checkpoint.Person.Name),
               Name = item.Name,
               Order = item.Order,
               TypeQuestion = item.TypeQuestion,
               TypeRotine = item.TypeRotine
             },
            _id = ObjectId.GenerateNewId().ToString(),
            Itens = itens
          });
        }
        foreach (var item in serviceQuestions.GetAllNewVersion(p => p.TypeQuestion == EnumTypeQuestion.Default & p.TypeRotine == EnumTypeRotine.Checkpoint).Result)
        {
          checkpoint.Questions.Add(new CheckpointQuestions()
          {
            Question =
            new ViewCrudQuestions()
            {
              _id = item._id,
              Content = item.Content.Replace("{company_name}", company.Name).Replace("{employee_name}", checkpoint.Person.Name),
              Name = item.Name,
              Order = item.Order,
              TypeQuestion = item.TypeQuestion,
              TypeRotine = item.TypeRotine
            },
            _id = ObjectId.GenerateNewId().ToString()
          });
        }
        var text = serviceTextDefault.GetNewVersion(p => p.TypeText == EnumTypeText.Checkpoint).Result;
        if (text != null)
          checkpoint.TextDefault = text.Content.Replace("{company_name}", company.Name).Replace("{employee_name}", checkpoint.Person.Name).Replace("{manager_name}", checkpoint.Person.Manager);

        return checkpoint;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    private int Deadline()
    {
      try
      {
        var parameter = serviceParameter.GetAllNewVersion(p => p.Key == "DeadlineAdm").Result.FirstOrDefault();
        if (parameter == null)
        {
          return int.Parse(serviceParameter.InsertNewVersion(new Parameter()
          {
            Name = "Total de dias do contrato de experiência",
            Key = "DeadlineAdm",
            Help = "Quantidade de dias para o final do contrato de experiência, a contar da data de admissão.",
            Status = EnumStatus.Enabled,
            Content = "90"
          }).Result.Content);
        }
        else
          return int.Parse(parameter.Content);
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    private void MailRhApproved(Person person, string result)
    {
      try
      {
        var mailDefault = MailDefault();
        //searsh model mail database
        var model = serviceMailModel.CheckpointResultApproved(path);
        if (model.StatusMail == EnumStatus.Disabled)
          return;

        var body = model.Message.Replace("{Person}", person.User.Name).Replace("{Link}", model.Link).Replace("{Manager}", person.Manager.Name).Replace("{Company}", person.Company.Name).Replace("{Occupation}", person.Occupation.Name);

        List<MailLogAddress> listMail = new List<MailLogAddress>();
        foreach (var item in mailDefault.Split(";"))
        {
          listMail.Add(new MailLogAddress(item, item));
        }
        var sendMail = new MailLog
        {
          From = new MailLogAddress("suporte@analisa.solutions", "Suporte ao Cliente | Analisa fluid careers"),
          To = listMail,
          Priority = EnumPriorityMail.Low,
          _idPerson = person._id,
          NamePerson = mailDefault,
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
    private void MailRhDisapproved(Person person, string result)
    {
      try
      {
        var mailDefault = MailDefault();
        //searsh model mail database
        var model = serviceMailModel.CheckpointResultDisapproved(path);
        if (model.StatusMail == EnumStatus.Disabled)
          return;

        var body = model.Message.Replace("{Person}", person.User.Name).Replace("{Link}", model.Link).Replace("{Manager}", person.Manager.Name).Replace("{Company}", person.Company.Name).Replace("{Occupation}", person.Occupation.Name);

        List<MailLogAddress> listMail = new List<MailLogAddress>();
        foreach (var item in mailDefault.Split(";"))
        {
          listMail.Add(new MailLogAddress(item, item));
        }
        var sendMail = new MailLog
        {
          From = new MailLogAddress("suporte@analisa.solutions", "Suporte ao Cliente | Analisa fluid careers"),
          To = listMail,
          Priority = EnumPriorityMail.Low,
          _idPerson = person._id,
          NamePerson = mailDefault,
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
    private void MailPerson(Person person, string result)
    {
      try
      {
        //searsh model mail database
        var model = serviceMailModel.CheckpointResultPerson(path);
        if (model.StatusMail == EnumStatus.Disabled)
          return;

        var body = model.Message.Replace("{Person}", person.User.Name).Replace("{Link}", model.Link).Replace("{Manager}", person.Manager.Name).Replace("{Company}", person.Company.Name).Replace("{Occupation}", person.Occupation.Name);
        List<MailLogAddress> listMail = new List<MailLogAddress>
        {
          new MailLogAddress(person.User.Mail, person.User.Name)
        };
        var sendMail = new MailLog
        {
          From = new MailLogAddress("suporte@analisa.solutions", "Suporte ao Cliente | Analisa fluid careers"),
          To = listMail,
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
    private Questions FindQuestion(string id, string nameCompany, string namePerson)
    {
      try
      {
        Questions result = serviceQuestions.GetNewVersion(x => x._id == id).Result;
        if (result == null)
          return null;

        result.Content = result.Content.Replace("{company_name}", nameCompany).Replace("{employee_name}", namePerson);
        return result;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    private void LogSave(string idperson, string local)
    {
      try
      {
        ViewLog log = new ViewLog()
        {
          Description = "Checkpoint",
          Local = local,
          _idPerson = idperson
        };
        serviceLog.NewLog(log);
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    private string MailDefault()
    {
      try
      {
        Parameter parameter = serviceParameter.GetAllNewVersion(p => p.Key == "mailcheckpoint").Result.FirstOrDefault();
        if (parameter == null)
          return serviceParameter.InsertNewVersion(new Parameter()
          {
            Name = "E-mail do RH para enviar aviso de Decisão de Efetivação | Checkpoint",
            Key = "mailcheckpoint",
            Help = "Informe um e-mail, ou vários e-mails separados por ponto-e-virgula, para enviar os avisos.",
            Content = "suporte@analisa.solutions",
            Status = EnumStatus.Enabled
          }).Result.Content;
        else
          return parameter.Content;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public string SendMail(string link, Person person, string idmail)
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

    public List<ViewExportStatusCheckpoint> ExportStatusCheckpoint(List<ViewListIdIndicators> persons)
    {
      try
      {
        List<ViewExportStatusCheckpoint> result = new List<ViewExportStatusCheckpoint>();
        foreach (var item in persons)
        {
          var checkpoint = serviceCheckpoint.GetNewVersion(p => p.Person._id == item._id).Result;
          if (checkpoint != null)
            result.Add(new ViewExportStatusCheckpoint
            {
              NameManager = checkpoint?.Person?.Manager,
              NamePerson = checkpoint?.Person?.Name,
              Status = checkpoint == null ? "Aguardando para iniciar" :
             checkpoint.StatusCheckpoint == EnumStatusCheckpoint.Open ? "Aguardando para iniciar" :
                checkpoint.StatusCheckpoint == EnumStatusCheckpoint.Wait ? "Em Andamento" : "Finalizado",
              DateBegin = checkpoint?.DateBegin,
              DateEnd = checkpoint?.DateEnd,
              Occupation = checkpoint?.Person?.Occupation,
              Result = checkpoint == null ? "Não iniciado" :
              checkpoint.TypeCheckpoint == EnumCheckpoint.Approved ? "Efetivado" :
                checkpoint.TypeCheckpoint == EnumCheckpoint.Disapproved ? "Não Efetivado" : "Aguardando Definição"
            });
          else
          {
            if (item.TypeJourney == EnumTypeJourney.Checkpoint)
            {
              var person = servicePerson.GetNewVersion(p => p._id == item._id).Result;
              result.Add(new ViewExportStatusCheckpoint
              {
                NameManager = person.Manager == null ? "Sem Gestor" : person.Manager.Name,
                NamePerson = person.User.Name,
                Status = "Aguardando para iniciar",
                Occupation = person.Occupation?.Name,
                Result = "Não iniciado"
              });
            }
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

  }
#pragma warning restore 1998
}
