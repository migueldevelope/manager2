using Manager.Core.Business;
using Manager.Core.Enumns;
using Manager.Core.Interfaces;
using Manager.Core.Views;
using Manager.Data;
using Manager.Services.Commons;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Manager.Services.Specific
{
  public class ServiceCheckpoint : Repository<Checkpoint>, IServiceCheckpoint
  {
    private readonly ServiceGeneric<Checkpoint> checkpointService;
    private readonly ServiceGeneric<Person> personService;
    private readonly ServiceLog logService;
    private readonly ServiceMailModel mailModelService;
    private readonly ServiceGeneric<Questions> questionsService;
    private readonly ServiceGeneric<MailMessage> mailMessageService;
    private readonly ServiceGeneric<MailLog> mailService;
    public string path;

    public ServiceCheckpoint(DataContext context, string pathToken)
      : base(context)
    {
      try
      {
        checkpointService = new ServiceGeneric<Checkpoint>(context);
        personService = new ServiceGeneric<Person>(context);
        logService = new ServiceLog(_context);
        mailModelService = new ServiceMailModel(context);
        questionsService = new ServiceGeneric<Questions>(context);
        mailMessageService = new ServiceGeneric<MailMessage>(context);
        mailService = new ServiceGeneric<MailLog>(context);
        path = pathToken;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public List<Checkpoint> ListCheckpointsEnd(string idmanager, ref long total, string filter, int count, int page)
    {
      try
      {
        LogSave(idmanager, "ListEnd");
        //var manager = personService.GetAll(p => p._id == idmanager).FirstOrDefault();
        int skip = (count * (page - 1));
        var detail = checkpointService.GetAll(p => p.Person.Manager._id == idmanager & p.StatusCheckpoint == EnumStatusCheckpoint.End & p.Person.Name.ToUpper().Contains(filter.ToUpper())).ToList();
        total = detail.Count();

        return detail.Skip(skip).Take(count).ToList();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    private void newOnZero()
    {
      try
      {
        var on = checkpointService.GetAuthentication(p => p.Status == EnumStatus.Disabled).Count();
        if (on == 0)
        {
          var person = personService.GetAll().FirstOrDefault();
          var zero = checkpointService.Insert(new Checkpoint() { Person = person, Status = EnumStatus.Disabled, StatusCheckpoint = EnumStatusCheckpoint.End });
        }
      }
      catch (Exception)
      {
        throw;
      }
    }

    public List<Checkpoint> ListCheckpointsWait(string idmanager, ref long total, string filter, int count, int page)
    {
      try
      {
        LogSave(idmanager, "List");
        newOnZero();
        int skip = (count * (page - 1));
        var list = personService.GetAll(p => p.TypeJourney == EnumTypeJourney.Checkpoint & p.Manager._id == idmanager
        & p.Name.ToUpper().Contains(filter.ToUpper()))
        .ToList().Select(p => new { Person = p, Checkpoint = checkpointService.GetAll(x => x.Person._id == p._id).FirstOrDefault() })
        .ToList();

        var detail = new List<Checkpoint>();
        foreach (var item in list)
        {
          if (item.Checkpoint == null)
            detail.Add(new Checkpoint
            {
              Person = item.Person,
              _id = null,
              StatusCheckpoint = EnumStatusCheckpoint.Open
            });
          else
            if (item.Checkpoint.StatusCheckpoint != EnumStatusCheckpoint.End)
            detail.Add(item.Checkpoint);
        }

        total = detail.Count();
        return detail.Skip(skip).Take(count).ToList();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }



    public Checkpoint GetCheckpoints(string id)
    {
      try
      {
        return checkpointService.GetAll(p => p._id == id)
          .ToList().Select(p => new Checkpoint()
          {
            _id = p._id,
            _idAccount = p._idAccount,
            Status = p.Status,
            Person = p.Person,
            DateBegin = p.DateBegin,
            DateEnd = p.DateEnd,
            Comments = p.Comments,
            Skill = p.Skill.OrderBy(x => x.Skill.Name).ToList(),
            Questions = p.Questions.OrderBy(x => x.Quesntion.Name).ToList(),
            StatusCheckpoint = p.StatusCheckpoint,
            TypeCheckpoint = p.TypeCheckpoint
          })
          .FirstOrDefault();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    private Checkpoint loadMap(Checkpoint checkpoint)
    {
      try
      {
        checkpoint.Skill = new List<CheckpointSkill>();
        foreach (var item in checkpoint.Person.Company.Skills)
        {
          checkpoint.Skill.Add(new CheckpointSkill() { Skill = item, _idAccount = item._idAccount, _id = ObjectId.GenerateNewId().ToString() });
        }

        checkpoint.Questions = new List<CheckpointQuestions>();
        foreach (var item in questionsService.GetAll().ToList())
        {
          checkpoint.Questions.Add(new CheckpointQuestions() { Quesntion = item, _idAccount = item._idAccount, _id = ObjectId.GenerateNewId().ToString() });
        }

        return checkpoint;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public Checkpoint NewCheckpoint(Checkpoint checkpoint, string idperson)
    {
      try
      {
        LogSave(checkpoint.Person._id, "Checkpoint Process");
        checkpoint.StatusCheckpoint = EnumStatusCheckpoint.InProgressManager;
        checkpoint.DateBegin = DateTime.Now;

        return checkpoint;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string UpdateCheckpoint(Checkpoint checkpoint, string idperson)
    {
      try
      {
        LogSave(checkpoint.Person._id, "Checkpoint update");
        if (checkpoint.StatusCheckpoint == EnumStatusCheckpoint.End)
        {
          checkpoint.DateEnd = DateTime.Now;
          if (checkpoint.TypeCheckpoint == EnumCheckpoint.Approved)
          {
            checkpoint.Person.TypeJourney = EnumTypeJourney.Monitoring;
            personService.Update(checkpoint.Person, null);
          }
        }

        checkpointService.Update(checkpoint, null);
        return "update";
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
          Description = "Access Checkpoint ",
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
      checkpointService._user = _user;
      logService._user = _user;
      mailModelService._user = _user;
      mailMessageService._user = _user;
      mailService._user = _user;
    }

    // send mail
    public void Mail(Person person)
    {
      try
      {
        //searsh model mail database
        var model = mailModelService.CheckpointApproval(path);
        var url = "";
        var body = model.Message.Replace("{Person}", person.Name).Replace("{Link}", model.Link);
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
          From = new MailLogAddress("suporte@jmsoft.com.br", "Suporte"),
          To = new List<MailLogAddress>(){
                        new MailLogAddress(person.Mail, person.Name)
                    },
          Priority = EnumPriorityMail.Low,
          _idPerson = person._id,
          NamePerson = person.Name,
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

    public string SendMail(string link, Person person, string idmail)
    {
      try
      {
        using (var client = new HttpClient())
        {
          client.BaseAddress = new Uri(link);
          var data = new
          {
            mail = person.Mail,
            password = person.Password
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

    public string RemoveCheckpoint(string idperson)
    {
      try
      {
        LogSave(_user._idPerson, "RemoveOnboarding:" + idperson);
        var checkpoint = checkpointService.GetAll(p => p.Person._id == idperson).FirstOrDefault();
        checkpoint.Status = EnumStatus.Disabled;
        checkpointService.Update(checkpoint, null);
        return "deleted";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public List<Checkpoint> GetListExclud(ref long total, string filter, int count, int page)
    {
      try
      {
        LogSave(_user._idPerson, "ListExclud");
        int skip = (count * (page - 1));
        var detail = checkpointService.GetAll(p => p.Person.Name.ToUpper().Contains(filter.ToUpper())).ToList();
        total = detail.Count();

        return detail.Skip(skip).Take(count).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }
  }
}
