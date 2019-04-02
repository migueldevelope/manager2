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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

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
    private readonly ServiceGeneric<MailMessage> serviceMailMessage;
    private readonly ServiceMailModel serviceMailModel;
    private readonly ServiceGeneric<Parameter> serviceParameter;
    private readonly ServiceGeneric<Person> servicePerson;
    private readonly ServiceGeneric<Questions> serviceQuestions;
    private readonly ServiceGeneric<TextDefault> serviceTextDefault;
    public string path;

    #region Contructor
    public ServiceCheckpoint(DataContext context, string pathToken) : base(context)
    {
      try
      {
        serviceAuthentication = new ServiceAuthentication(context);
        serviceCheckpoint = new ServiceGeneric<Checkpoint>(context);
        serviceCompany = new ServiceGeneric<Company>(context);
        serviceLog = new ServiceLog(_context);
        serviceLogMessages = new ServiceLogMessages(context);
        serviceMail = new ServiceGeneric<MailLog>(context);
        serviceMailMessage = new ServiceGeneric<MailMessage>(context);
        serviceMailModel = new ServiceMailModel(context);
        serviceParameter = new ServiceGeneric<Parameter>(context);
        servicePerson = new ServiceGeneric<Person>(context);
        serviceQuestions = new ServiceGeneric<Questions>(context);
        serviceTextDefault = new ServiceGeneric<TextDefault>(context);
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
      serviceMailMessage._user = _user;
      serviceMailModel.SetUser(_user);
      serviceParameter._user = _user;
      servicePerson._user = _user;
      serviceQuestions._user = _user;
      serviceTextDefault._user = _user;
    }
    public void SetUser(BaseUser user)
    {
      serviceCheckpoint._user = user;
      serviceCompany._user = user;
      serviceLog.SetUser(user);
      serviceLogMessages.SetUser(user);
      serviceMail._user = user;
      serviceMailMessage._user = user;
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
        List<ViewListCheckpoint> list = servicePerson.GetAllNewVersion(p => p.StatusUser != EnumStatusUser.Disabled && p.StatusUser != EnumStatusUser.ErrorIntegration &&
                                        p.TypeUser != EnumTypeUser.Administrator &&
                                        p.TypeJourney == EnumTypeJourney.Checkpoint &&
                                        p.Manager._id == idmanager &&
                                        p.User.Name.ToUpper().Contains(filter.ToUpper()), count, count * (page - 1), "User.Name").Result
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
            }
            detail.Add(item);
          }
        }
        else
          detail = list;

        total = servicePerson.CountNewVersion(p => p.StatusUser != EnumStatusUser.Disabled && p.StatusUser != EnumStatusUser.ErrorIntegration &&
                                p.TypeUser != EnumTypeUser.Administrator &&
                                p.TypeJourney == EnumTypeJourney.Checkpoint &&
                                p.Manager._id == idmanager &&
                                p.User.Name.ToUpper().Contains(filter.ToUpper())).Result;
        return detail;
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
        List<ViewListCheckpoint> list = servicePerson.GetAllNewVersion(p => p.StatusUser != EnumStatusUser.Disabled && p.StatusUser != EnumStatusUser.ErrorIntegration &&
                                        p.TypeUser != EnumTypeUser.Administrator &&
                                        p.TypeJourney == EnumTypeJourney.Checkpoint &&
                                        p._id == idperson).Result
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
        LogSave(_user._idPerson, "List ended");
        int skip = (count * (page - 1));
        var detail = serviceCheckpoint.GetAllNewVersion(p => p.Person.User.Name.ToUpper().Contains(filter.ToUpper()), count, skip, "Person.User.Name").Result
          .Select(s => new ViewListCheckpoint()
          {
            _id = s._id,
            _idPerson = s.Person._id,
            Name = s.Person.User.Name,
            StatusCheckpoint = s.StatusCheckpoint,
            TypeCheckpoint = s.TypeCheckpoint,
            OccupationName = s.Occupation?.Name
          }).ToList();

        total = serviceCheckpoint.GetAll(p => p.Person.User.Name.ToUpper().Contains(filter.ToUpper())).Count();

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
        Person person = servicePerson.GetNewVersion(p => p.StatusUser != EnumStatusUser.Disabled && p.StatusUser != EnumStatusUser.ErrorIntegration &&
                                        p.TypeUser != EnumTypeUser.Administrator &&
                                        p.TypeJourney == EnumTypeJourney.Checkpoint &&
                                        p._id == idperson).Result;
        if (person == null)
          throw new Exception("Person not available!");

        Checkpoint checkpoint = null;
        if (serviceCheckpoint.Exists("Checkpoint"))
          checkpoint = serviceCheckpoint.GetNewVersion(x => x.Person._id == idperson && x.StatusCheckpoint != EnumStatusCheckpoint.End).Result;

        if (checkpoint == null)
        {
          checkpoint = new Checkpoint
          {
            Person = person,
            StatusCheckpoint = EnumStatusCheckpoint.Open,
            DateBegin = DateTime.Now,
            DataAccess = person.User.DateAdm == null ? DateTime.Now : DateTime.Parse(person.User.DateAdm.ToString()).AddDays(Deadline()),
            TypeCheckpoint = EnumCheckpoint.None,
            Occupation = new ViewListOccupation()
            {
              _id = person.Occupation._id,
              Name = person.Occupation.Name,
              Line = person.Occupation.Line,
              Company = new ViewListCompany() { _id = person.Occupation.Group.Company._id, Name = person.Occupation.Group.Company.Name },
              Group = new ViewListGroup()
              {
                _id = person.Occupation.Group._id,
                Name = person.Occupation.Group.Name,
                Line = person.Occupation.Group.Line,
                Axis = new ViewListAxis()
                {
                  _id = person.Occupation.Group.Axis._id,
                  Name = person.Occupation.Group.Axis.Name,
                  TypeAxis = person.Occupation.Group.Axis.TypeAxis
                },
                Sphere = new ViewListSphere()
                {
                  _id = person.Occupation.Group.Sphere._id,
                  Name = person.Occupation.Group.Sphere.Name,
                  TypeSphere = person.Occupation.Group.Sphere.TypeSphere
                }
              },
              Process = person.Occupation.Process.Select(p => new ViewListProcessLevelTwo()
              {
                _id = p._id,
                Name = p.Name,
                Order = p.Order,
                ProcessLevelOne = new ViewListProcessLevelOne()
                {
                  _id = p.ProcessLevelOne._id,
                  Name = p.ProcessLevelOne.Name,
                  Order = p.ProcessLevelOne.Order,
                  Area = new ViewListArea()
                  {
                    _id = p.ProcessLevelOne.Area._id,
                    Name = p.ProcessLevelOne.Area.Name
                  }
                }
              }).ToList()
            }
          };
          checkpoint = LoadMap(checkpoint);
          checkpoint = serviceCheckpoint.InsertNewVersion(checkpoint).Result;
          LogSave(idperson, string.Format("Start new process | {0}",checkpoint._id));
        }
        return new ViewListCheckpoint()
        {
          _id = checkpoint._id,
          Name = checkpoint.Person.User.Name,
          OccupationName = checkpoint.Person.Occupation.Name,
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
        if (checkpoint == null)
          throw new Exception("Checkpoint not available!");

        if (checkpoint.StatusCheckpoint == EnumStatusCheckpoint.Open)
        {
          checkpoint.StatusCheckpoint = EnumStatusCheckpoint.Wait;
          serviceCheckpoint.Update(checkpoint, null);
        }

        return new ViewCrudCheckpoint()
        {
          _id = checkpoint._id,
          DataAccess = checkpoint.DataAccess,
          DateBegin = checkpoint.DateBegin,
          DateEnd = checkpoint.DateEnd,
          StatusCheckpoint = checkpoint.StatusCheckpoint,
          TypeCheckpoint = checkpoint.TypeCheckpoint,
          Person = new ViewListPerson()
          {
            _id = checkpoint.Person._id,
            Company = new ViewListCompany() { _id = checkpoint.Person.Company._id, Name = checkpoint.Person.Company.Name },
            Establishment = checkpoint.Person.Establishment == null ? null : new ViewListEstablishment() { _id = checkpoint.Person.Establishment._id, Name = checkpoint.Person.Establishment.Name },
            Registration = checkpoint.Person.Registration,
            User = new ViewListUser()
            {
              Document = checkpoint.Person.User.Document,
              Mail = checkpoint.Person.User.Mail,
              Name = checkpoint.Person.User.Name,
              Phone = checkpoint.Person.User.Phone,
              _id = checkpoint.Person.User._id
            }
          },
          Occupation = new ViewListOccupation()
          {
            _id = checkpoint.Person.Occupation._id,
            Name = checkpoint.Person.Occupation.Name,
            Line = checkpoint.Person.Occupation.Line,
            Company = new ViewListCompany() { _id = checkpoint.Person.Occupation.Group.Company._id, Name = checkpoint.Person.Occupation.Group.Company.Name },
            Group = new ViewListGroup()
            {
              _id = checkpoint.Person.Occupation.Group._id,
              Name = checkpoint.Person.Occupation.Group.Name,
              Line = checkpoint.Person.Occupation.Group.Line,
              Axis = new ViewListAxis()
              {
                _id = checkpoint.Person.Occupation.Group.Axis._id,
                Name = checkpoint.Person.Occupation.Group.Axis.Name,
                TypeAxis = checkpoint.Person.Occupation.Group.Axis.TypeAxis
              },
              Sphere = new ViewListSphere()
              {
                _id = checkpoint.Person.Occupation.Group.Sphere._id,
                Name = checkpoint.Person.Occupation.Group.Sphere.Name,
                TypeSphere = checkpoint.Person.Occupation.Group.Sphere.TypeSphere
              }
            },
            Process = checkpoint.Person.Occupation.Process.Select(p => new ViewListProcessLevelTwo()
            {
              _id = p._id,
              Name = p.Name,
              Order = p.Order,
              ProcessLevelOne = new ViewListProcessLevelOne()
              {
                _id = p.ProcessLevelOne._id,
                Name = p.ProcessLevelOne.Name,
                Order = p.ProcessLevelOne.Order,
                Area = new ViewListArea()
                {
                  _id = p.ProcessLevelOne.Area._id,
                  Name = p.ProcessLevelOne.Area.Name
                }
              }              
            }).ToList()
          },
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
              TypeQuestion = x.Question.TypeQuestion,
              Company = new ViewListCompany() { _id = x.Question.Company._id, Name = x.Question.Company.Name }
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
                TypeRotine = i.Question.TypeRotine,
                Company = new ViewListCompany() { _id = i.Question.Company._id, Name = i.Question.Company.Name }
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

        LogSave(_user._idPerson, string.Format("Delete | {0}", idcheckpoint));
        checkpoint.Status = EnumStatus.Disabled;
        serviceCheckpoint.Update(checkpoint, null);
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
        if (checkpoint == null)
          throw new Exception("Checkpoint not available!");

        if (view.StatusCheckpoint == EnumStatusCheckpoint.End)
        {
          checkpoint.DateEnd = DateTime.Now;
          checkpoint.StatusCheckpoint = view.StatusCheckpoint;
          checkpoint.TypeCheckpoint = view.TypeCheckpoint;
          Person person = servicePerson.GetNewVersion(p => p._id == view.Person._id).Result;
          if (view.TypeCheckpoint == EnumCheckpoint.Approved)
          {
            person.TypeJourney = EnumTypeJourney.Monitoring;
            servicePerson.Update(person, null);

            MailRhApproved(person, "Aprovado");
            MailPerson(person, "Aprovado");

            serviceLogMessages.NewLogMessage("Checkpoint", string.Format(" Colaborador {0} aprovado no Checkpoint", person.User.Name), person);
            LogSave(view.Person._id, string.Format("Approved | {0}.", view._id));
          }
          else
          {
            MailRhDisapproved(person, "Reprovado");
            LogSave(view.Person._id, string.Format("Disapproved | {0}.", view._id));
          }
        }
        checkpoint.Comments = view.Comments;
        checkpoint.Questions = view.Questions?.Select(p => new CheckpointQuestions()
        {
          _id = p._id,
          _idAccount = _user._idAccount,
          Status = EnumStatus.Enabled,
          Mark = p.Mark,
          Question = FindQuestion(p.Question._id, p.Question.Company.Name, view.Person.User.Name),
          Itens = p.Itens?.Select(x => new CheckpointQuestions()
          {
            _id = x._id,
            Status = EnumStatus.Enabled,
            Question = new Questions()
            {
              _id = x.Question._id,
              _idAccount = _user._idAccount,
              Status = EnumStatus.Enabled,
              Company = serviceCompany.GetNewVersion(c => c._id == x.Question.Company._id).Result,
              Content = x.Question.Content,
              Name = x.Question.Name,
              Order = x.Question.Order,
              Template = null,
              TypeQuestion = x.Question.TypeQuestion,
              TypeRotine = x.Question.TypeRotine
            },
            Itens = null,
            Mark = x.Mark,
            _idAccount = _user._idAccount
          }).ToList()
        }).ToList();
        serviceCheckpoint.Update(checkpoint, null);
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
          throw new Exception("Checkpoint not available!");

        LogSave(idperson, string.Format("Person ended | {0}",checkpoint._id));

        return new ViewCrudCheckpoint()
        {
          _id = checkpoint._id,
          DataAccess = checkpoint.DataAccess,
          DateBegin = checkpoint.DateBegin,
          DateEnd = checkpoint.DateEnd,
          StatusCheckpoint = checkpoint.StatusCheckpoint,
          TypeCheckpoint = checkpoint.TypeCheckpoint,
          Person = new ViewListPerson()
          {
            _id = checkpoint.Person._id,
            Company = new ViewListCompany() { _id = checkpoint.Person.Company._id, Name = checkpoint.Person.Company.Name },
            Establishment = checkpoint.Person.Establishment == null ? null : new ViewListEstablishment() { _id = checkpoint.Person.Establishment._id, Name = checkpoint.Person.Establishment.Name },
            Registration = checkpoint.Person.Registration,
            User = new ViewListUser()
            {
              Document = checkpoint.Person.User.Document,
              Mail = checkpoint.Person.User.Mail,
              Name = checkpoint.Person.User.Name,
              Phone = checkpoint.Person.User.Phone,
              _id = checkpoint.Person.User._id
            }
          },
          Occupation = new ViewListOccupation()
          {
            _id = checkpoint.Person.Occupation._id,
            Name = checkpoint.Person.Occupation.Name,
            Line = checkpoint.Person.Occupation.Line,
            Company = new ViewListCompany() { _id = checkpoint.Person.Occupation.Group.Company._id, Name = checkpoint.Person.Occupation.Group.Company.Name },
            Group = new ViewListGroup()
            {
              _id = checkpoint.Person.Occupation.Group._id,
              Name = checkpoint.Person.Occupation.Group.Name,
              Line = checkpoint.Person.Occupation.Group.Line,
              Axis = new ViewListAxis()
              {
                _id = checkpoint.Person.Occupation.Group.Axis._id,
                Name = checkpoint.Person.Occupation.Group.Axis.Name,
                TypeAxis = checkpoint.Person.Occupation.Group.Axis.TypeAxis
              },
              Sphere = new ViewListSphere()
              {
                _id = checkpoint.Person.Occupation.Group.Sphere._id,
                Name = checkpoint.Person.Occupation.Group.Sphere.Name,
                TypeSphere = checkpoint.Person.Occupation.Group.Sphere.TypeSphere
              }
            },
            Process = checkpoint.Person.Occupation.Process.Select(p => new ViewListProcessLevelTwo()
            {
              _id = p._id,
              Name = p.Name,
              Order = p.Order,
              ProcessLevelOne = new ViewListProcessLevelOne()
              {
                _id = p.ProcessLevelOne._id,
                Name = p.ProcessLevelOne.Name,
                Order = p.ProcessLevelOne.Order,
                Area = new ViewListArea()
                {
                  _id = p.ProcessLevelOne.Area._id,
                  Name = p.ProcessLevelOne.Area.Name
                }
              }
            }).ToList()
          },
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
              TypeQuestion = x.Question.TypeQuestion,
              Company = new ViewListCompany() { _id = x.Question.Company._id, Name = x.Question.Company.Name }
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
                TypeRotine = i.Question.TypeRotine,
                Company = new ViewListCompany() { _id = i.Question.Company._id, Name = i.Question.Company.Name }
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
    private Checkpoint LoadMap(Checkpoint checkpoint)
    {
      try
      {
        checkpoint.Questions = new List<CheckpointQuestions>();
        var itens = new List<CheckpointQuestions>();
        foreach (var item in checkpoint.Person.Company.Skills)
        {
          itens.Add(new CheckpointQuestions()
          {
            Question = new Questions()
            {
              Name = item.Name,
              Content = item.Concept,
              Order = 0,
              Company = checkpoint.Person.Company,
              Status = EnumStatus.Enabled,
              TypeQuestion = EnumTypeQuestion.Skill,
              _id = ObjectId.GenerateNewId().ToString(),
              _idAccount = _user._idAccount
            }
            ,
            _idAccount = item._idAccount,
            _id = ObjectId.GenerateNewId().ToString()
          });
        }
        foreach (var item in serviceQuestions.GetAll(p => p.TypeQuestion == EnumTypeQuestion.Skill & p.TypeRotine == EnumTypeRotine.Checkpoint).ToList())
        {
          checkpoint.Questions.Add(new CheckpointQuestions()
          {
            Question =
             new Questions()
             {
               _id = item._id,
               _idAccount = item._idAccount,
               Company = item.Company,
               Content = item.Content.Replace("{company_name}", checkpoint.Person.Company.Name).Replace("{employee_name}", checkpoint.Person.User.Name),
               Name = item.Name,
               Order = item.Order,
               Status = item.Status,
               Template = item.Template,
               TypeQuestion = item.TypeQuestion,
               TypeRotine = item.TypeRotine
             },
            _idAccount = item._idAccount,
            _id = ObjectId.GenerateNewId().ToString(),
            Itens = itens
          });
        }
        foreach (var item in serviceQuestions.GetAll(p => p.TypeQuestion == EnumTypeQuestion.Default & p.TypeRotine == EnumTypeRotine.Checkpoint).ToList())
        {
          checkpoint.Questions.Add(new CheckpointQuestions()
          {
            Question =
            new Questions()
            {
              _id = item._id,
              _idAccount = item._idAccount,
              Company = item.Company,
              Content = item.Content.Replace("{company_name}", checkpoint.Person.Company.Name).Replace("{employee_name}", checkpoint.Person.User.Name),
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
        var text = serviceTextDefault.GetAll(p => p.TypeText == EnumTypeText.Checkpoint).FirstOrDefault();
        if (text != null)
          checkpoint.TextDefault = text.Content.Replace("{company_name}", checkpoint.Person.Company.Name).Replace("{employee_name}", checkpoint.Person.User.Name).Replace("{manager_name}", checkpoint.Person.Manager.Name);
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
        var parameter = serviceParameter.GetAll(p => p.Name == "DeadlineAdm").FirstOrDefault();
        if (parameter == null)
        {
          return int.Parse(serviceParameter.Insert(new Parameter()
          {
            Name = "DeadlineAdm",
            Status = EnumStatus.Enabled,
            Content = "90"
          }).Content);
        }
        else
          return int.Parse(parameter.Content);
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    private async void MailRhApproved(Person person, string result)
    {
      try
      {
        var mailDefault = MailDefault();
        //searsh model mail database
        var model = serviceMailModel.CheckpointResultApproved(path);
        if (model.StatusMail == EnumStatus.Disabled)
          return;

        var url = "";
        var body = model.Message.Replace("{Person}", person.User.Name).Replace("{Link}", model.Link).Replace("{Manager}", person.Manager.Name).Replace("{Company}", person.Company.Name).Replace("{Occupation}", person.Occupation.Name);
        var message = new MailMessage
        {
          Type = EnumTypeMailMessage.Put,
          Name = model.Name,
          Url = url,
          Body = body
        };

        List<MailLogAddress> listMail = new List<MailLogAddress>();
        foreach (var item in mailDefault.Split(";"))
        {
          listMail.Add(new MailLogAddress(item, item));
        }
        var idMessage = serviceMailMessage.Insert(message)._id;
        var sendMail = new MailLog
        {
          From = new MailLogAddress("suporte@jmsoft.com.br", "Notificação do Analisa"),
          To = listMail,
          Priority = EnumPriorityMail.Low,
          _idPerson = person._id,
          NamePerson = mailDefault,
          Body = body,
          StatusMail = EnumStatusMail.Sended,
          Included = DateTime.Now,
          Subject = model.Subject
        };
        var mailObj = serviceMail.Insert(sendMail);
        var token = SendMail(path, person, mailObj._id.ToString());
        var messageEnd = serviceMailMessage.GetAll(p => p._id == idMessage).FirstOrDefault();
        messageEnd.Token = token;
        serviceMailMessage.Update(messageEnd, null);
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    private async void MailRhDisapproved(Person person, string result)
    {
      try
      {
        var mailDefault = MailDefault();
        //searsh model mail database
        var model = serviceMailModel.CheckpointResultDisapproved(path);
        if (model.StatusMail == EnumStatus.Disabled)
          return;

        var url = "";
        var body = model.Message.Replace("{Person}", person.User.Name).Replace("{Link}", model.Link).Replace("{Manager}", person.Manager.Name).Replace("{Company}", person.Company.Name).Replace("{Occupation}", person.Occupation.Name);
        var message = new MailMessage
        {
          Type = EnumTypeMailMessage.Put,
          Name = model.Name,
          Url = url,
          Body = body
        };

        List<MailLogAddress> listMail = new List<MailLogAddress>();
        foreach (var item in mailDefault.Split(";"))
        {
          listMail.Add(new MailLogAddress(item, item));
        }
        var idMessage = serviceMailMessage.Insert(message)._id;
        var sendMail = new MailLog
        {
          From = new MailLogAddress("suporte@jmsoft.com.br", "Notificação do Analisa"),
          To = listMail,
          Priority = EnumPriorityMail.Low,
          _idPerson = person._id,
          NamePerson = mailDefault,
          Body = body,
          StatusMail = EnumStatusMail.Sended,
          Included = DateTime.Now,
          Subject = model.Subject
        };
        var mailObj = serviceMail.Insert(sendMail);
        var token = SendMail(path, person, mailObj._id.ToString());
        var messageEnd = serviceMailMessage.GetAll(p => p._id == idMessage).FirstOrDefault();
        messageEnd.Token = token;
        serviceMailMessage.Update(messageEnd, null);
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    private async void MailPerson(Person person, string result)
    {
      try
      {
        //searsh model mail database
        var model = serviceMailModel.CheckpointResultPerson(path);
        if (model.StatusMail == EnumStatus.Disabled)
          return;

        var url = "";
        var body = model.Message.Replace("{Person}", person.User.Name).Replace("{Link}", model.Link).Replace("{Manager}", person.Manager.Name).Replace("{Company}", person.Company.Name).Replace("{Occupation}", person.Occupation.Name);
        var message = new MailMessage
        {
          Type = EnumTypeMailMessage.Put,
          Name = model.Name,
          Url = url,
          Body = body
        };

        List<MailLogAddress> listMail = new List<MailLogAddress>
        {
          new MailLogAddress(person.User.Mail, person.User.Name)
        };

        var idMessage = serviceMailMessage.Insert(message)._id;
        var sendMail = new MailLog
        {
          From = new MailLogAddress("suporte@jmsoft.com.br", "Notificação do Analisa"),
          To = listMail,
          Priority = EnumPriorityMail.Low,
          _idPerson = person._id,
          NamePerson = person.User.Name,
          Body = body,
          StatusMail = EnumStatusMail.Sended,
          Included = DateTime.Now,
          Subject = model.Subject
        };
        var mailObj = serviceMail.Insert(sendMail);
        var token = SendMail(path, person, mailObj._id.ToString());
        var messageEnd = serviceMailMessage.GetAll(p => p._id == idMessage).FirstOrDefault();
        messageEnd.Token = token;
        serviceMailMessage.Update(messageEnd, null);
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
        result.Content = result.Content.Replace("{company_name}", nameCompany).Replace("{employee_name}", namePerson);
        return result;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    #endregion

    #region Old

    public List<Checkpoint> ListCheckpointsEndOld(string idmanager, ref long total, string filter, int count, int page)
    {
      try
      {
        LogSave(idmanager, "ListEnd");
        int skip = (count * (page - 1));
        var detail = serviceCheckpoint.GetAll(p => p.Person.Manager._id == idmanager & p.StatusCheckpoint == EnumStatusCheckpoint.End & p.Person.User.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Person.User.Name).Skip(skip).Take(count).ToList();
        total = serviceCheckpoint.GetAll(p => p.Person.Manager._id == idmanager & p.StatusCheckpoint == EnumStatusCheckpoint.End & p.Person.User.Name.ToUpper().Contains(filter.ToUpper())).Count();

        return detail;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public Checkpoint PersonCheckpointEndOld(string idperson)
    {
      try
      {
        LogSave(idperson, "PersonEnd");
        return serviceCheckpoint.GetAll(p => p.Person._id == idperson & p.StatusCheckpoint == EnumStatusCheckpoint.End).FirstOrDefault();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<Checkpoint> ListCheckpointsWaitOld(string idmanager, ref long total, string filter, int count, int page)
    {
      try
      {
        LogSave(idmanager, "List");
        int skip = (count * (page - 1));
        var list = servicePerson.GetAll(p => p.StatusUser != EnumStatusUser.Disabled & p.StatusUser != EnumStatusUser.ErrorIntegration & p.TypeUser != EnumTypeUser.Administrator & p.TypeJourney == EnumTypeJourney.Checkpoint & p.Manager._id == idmanager
        & p.User.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.User.Name)
        .ToList().Select(p => new { Person = p, Checkpoint = serviceCheckpoint.GetAll(x => x.Person._id == p._id).FirstOrDefault() })
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
        throw e;
      }
    }

    public Checkpoint ListCheckpointsWaitPersonOld(string idperson)
    {
      try
      {
        LogSave(idperson, "List");
        var list = servicePerson.GetAll(p => p.StatusUser != EnumStatusUser.Disabled & p.StatusUser != EnumStatusUser.ErrorIntegration & p.TypeUser != EnumTypeUser.Administrator & p.TypeJourney == EnumTypeJourney.Checkpoint & p._id == idperson)
        .OrderBy(p => p.User.Name)
        .ToList().Select(p => new { Person = p, Checkpoint = serviceCheckpoint.GetAll(x => x.Person._id == p._id).FirstOrDefault() })
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
            detail.Add(item.Checkpoint);
        }


        return detail.FirstOrDefault();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public Checkpoint GetCheckpointsOld(string id)
    {
      try
      {
        return serviceCheckpoint.GetAll(p => p._id == id)
          .ToList().Select(p => new Checkpoint()
          {
            _id = p._id,
            _idAccount = p._idAccount,
            Status = p.Status,
            Person = p.Person,
            DateBegin = p.DateBegin,
            DateEnd = p.DateEnd,
            Comments = p.Comments,
            Questions = p.Questions.OrderBy(x => x.Question.Order).ToList(),
            StatusCheckpoint = p.StatusCheckpoint,
            TypeCheckpoint = p.TypeCheckpoint,
            TextDefault = p.TextDefault,
            DataAccess = p.DataAccess
          })
          .FirstOrDefault();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public Checkpoint NewCheckpointOld(Checkpoint checkpoint, string idperson)
    {
      try
      {
        LogSave(checkpoint.Person._id, "Checkpoint Process");
        checkpoint.StatusCheckpoint = EnumStatusCheckpoint.Wait;
        checkpoint.DateBegin = DateTime.Now;
        checkpoint = LoadMap(checkpoint);
        if (checkpoint.Person.User.DateAdm != null)
          checkpoint.DataAccess = DateTime.Parse(checkpoint.Person.User.DateAdm.ToString()).AddDays(Deadline());
        else
          checkpoint.DataAccess = DateTime.Now;

        if (checkpoint._id != null)
          return checkpoint;
        else
          return serviceCheckpoint.Insert(checkpoint);
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string UpdateCheckpointOld(Checkpoint checkpoint, string idperson)
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
            servicePerson.Update(checkpoint.Person, null);
            MailRhApproved(checkpoint.Person, "Aprovado");
            MailPerson(checkpoint.Person, "Aprovado");

            serviceLogMessages.NewLogMessage("Checkpoint", " Colaborador " + checkpoint.Person.User.Name + " aprovado no Checkpoint", checkpoint.Person);
          }
          else
          {
            MailRhDisapproved(checkpoint.Person, "Reprovado");

          }
        }


        serviceCheckpoint.Update(checkpoint, null);
        return "update";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public void LogSave(string idperson, string local)
    {
      try
      {
        var user = servicePerson.GetAll(p => p._id == idperson).FirstOrDefault();
        var log = new ViewLog()
        {
          Description = "Checkpoint",
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

    private string MailDefault()
    {
      try
      {
        var par = serviceParameter.GetAll(p => p.Name == "mailcheckpoint").FirstOrDefault();
        if (par == null)
          return serviceParameter.Insert(new Parameter()
          {
            Name = "mailcheckpoint",
            Content = "suporte@jmsoft.com.br",
            Status = EnumStatus.Enabled
          }).Content;
        else
          return par.Content;
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
        throw e;
      }
    }

    public string RemoveCheckpointOld(string idperson)
    {
      try
      {
        LogSave(_user._idPerson, "RemoveOnboarding:" + idperson);
        var checkpoint = serviceCheckpoint.GetAll(p => p.Person._id == idperson).FirstOrDefault();
        if (checkpoint == null)
          return "deleted";

        checkpoint.Status = EnumStatus.Disabled;
        serviceCheckpoint.Update(checkpoint, null);
        return "deleted";
      }
      catch (Exception e)
      {
        throw e;
      }
    }


    public List<Checkpoint> GetListExcludOld(ref long total, string filter, int count, int page)
    {
      try
      {
        LogSave(_user._idPerson, "ListExclud");
        int skip = (count * (page - 1));
        var detail = serviceCheckpoint.GetAll(p => p.Person.User.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Person.User.Name).Skip(skip).Take(count).ToList();
        total = serviceCheckpoint.GetAll(p => p.Person.User.Name.ToUpper().Contains(filter.ToUpper())).Count();

        return detail;
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
