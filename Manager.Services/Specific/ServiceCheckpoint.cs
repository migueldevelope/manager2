﻿using Manager.Core.Base;
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
    public string path;

    #region Contructor
    public ServiceCheckpoint(DataContext context, DataContext contextLog, string pathToken) : base(context)
    {
      try
      {
        serviceAuthentication = new ServiceAuthentication(context, contextLog);
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
            else
              checkpoint = serviceCheckpoint.GetNewVersion(x => x.Person._id == item._idPerson && x.StatusCheckpoint == EnumStatusCheckpoint.End).Result;


            if (checkpoint?.TypeCheckpoint != EnumCheckpoint.Disapproved)
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
        
        int skip = (count * (page - 1));
        var detail = serviceCheckpoint.GetAllNewVersion(p => p.Person.User.Name.ToUpper().Contains(filter.ToUpper()), count, skip, "Person.User.Name").Result
          .Select(p => new ViewListCheckpoint()
          {
            _id = p._id,
            _idPerson = p.Person._id,
            Name = p.Person.User.Name,
            StatusCheckpoint = p.StatusCheckpoint,
            TypeCheckpoint = p.TypeCheckpoint,
            OccupationName = p.Occupation?.Name
          }).ToList();

        total = serviceCheckpoint.CountNewVersion(p => p.Person.User.Name.ToUpper().Contains(filter.ToUpper())).Result;

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
          Task.Run(() => LogSave(_user._idPerson, string.Format("Start new process | {0}", checkpoint._id)));
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
          return null;
        //throw new Exception("Checkpoint not available!");

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
          Comments = checkpoint.Comments,
          Person = new ViewListPersonInfo()
          {
            _id = checkpoint.Person._id,
            TypeJourney = checkpoint.Person.TypeJourney,
            Occupation = checkpoint.Person.Occupation.Name,
            Name = checkpoint.Person.User.Name,
            Manager = checkpoint.Person.Manager.Name,
            Company = new ViewListCompany() { _id = checkpoint.Person.Company._id, Name = checkpoint.Person.Company.Name },
            Establishment = (checkpoint.Person.Establishment == null) ? null : new ViewListEstablishment() { _id = checkpoint.Person.Establishment._id, Name = checkpoint.Person.Establishment.Name },
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

        Task.Run(() => LogSave(_user._idPerson, string.Format("Delete | {0}", idcheckpoint)));
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
        checkpoint.Comments = view.Comments;

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
        }
        checkpoint.Comments = view.Comments;
        checkpoint.Questions = view.Questions?.Select(p => new CheckpointQuestions()
        {
          _id = p._id,
          _idAccount = _user._idAccount,
          Status = EnumStatus.Enabled,
          Mark = p.Mark,
          Question = p.Question == null ? null : FindQuestion(p.Question._id, p.Question.Company.Name, view.Person.User.Name),
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
          return null;
        //throw new Exception("Checkpoint not available!");

        Task.Run(() => LogSave(_user._idPerson, string.Format("Person ended | {0}", checkpoint._id)));

        return new ViewCrudCheckpoint()
        {
          _id = checkpoint._id,
          DataAccess = checkpoint.DataAccess,
          DateBegin = checkpoint.DateBegin,
          DateEnd = checkpoint.DateEnd,
          StatusCheckpoint = checkpoint.StatusCheckpoint,
          TypeCheckpoint = checkpoint.TypeCheckpoint,
          Person = new ViewListPersonInfo()
          {
            _id = checkpoint.Person._id,
            TypeJourney = checkpoint.Person.TypeJourney,
            Occupation = checkpoint.Person.Occupation.Name,
            Name = checkpoint.Person.User.Name,
            Manager = checkpoint.Person.Manager?.Name,
            Company = new ViewListCompany() { _id = checkpoint.Person.Company._id, Name = checkpoint.Person.Company.Name },
            Establishment = (checkpoint.Person.Establishment == null) ? null : new ViewListEstablishment() { _id = checkpoint.Person.Establishment._id, Name = checkpoint.Person.Establishment.Name },
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
        var parameter = serviceParameter.GetAll(p => p.Key == "DeadlineAdm").FirstOrDefault();
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
    private async Task MailRhApproved(Person person, string result)
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
        var mailObj = serviceMail.InsertNewVersion(sendMail).Result;
        var token = SendMail(path, person, mailObj._id.ToString());
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    private async Task MailRhDisapproved(Person person, string result)
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
        var mailObj = serviceMail.InsertNewVersion(sendMail).Result;
        var token = SendMail(path, person, mailObj._id.ToString());
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    private async Task MailPerson(Person person, string result)
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
    private async Task LogSave(string idperson, string local)
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
        Parameter parameter = serviceParameter.GetAll(p => p.Key == "mailcheckpoint").FirstOrDefault();
        if (parameter == null)
          return serviceParameter.InsertNewVersion(new Parameter()
          {
            Name = "E-mail do RH para enviar aviso de Decisão de Efetivação | Checkpoint",
            Key = "mailcheckpoint",
            Help = "Informe um e-mail, ou vários e-mails separados por ponto-e-virgula, para enviar os avisos.",
            Content = "suporte@jmsoft.com.br",
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

  }
#pragma warning restore 1998
}
