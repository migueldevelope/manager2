using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.Enumns;
using Manager.Core.Interfaces;
using Manager.Core.Views;
using Manager.Data;
using Manager.Services.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manager.Services.Specific
{
  #pragma warning disable 4014
  public class ServiceIndicators : Repository<Monitoring>, IServiceIndicators
  {
    private readonly ServiceGeneric<Monitoring> monitoringService;
    private readonly ServiceGeneric<OnBoarding> onboardingService;
    private readonly ServiceGeneric<Checkpoint> checkpointService;
    private readonly ServiceGeneric<Workflow> workflowService;
    private readonly ServiceGeneric<Person> personService;
    private readonly ServiceGeneric<Plan> planService;
    private readonly ServiceLog logService;
    private readonly ServiceMailModel mailModelService;
    private readonly ServiceGeneric<MailMessage> mailMessageService;
    private readonly ServiceGeneric<MailLog> mailService;
    private readonly ServiceGeneric<Account> accountService;
    public string path;
    private HubConnection hubConnection;

    public ServiceIndicators(DataContext context, string pathToken)
      : base(context)
    {
      try
      {
        monitoringService = new ServiceGeneric<Monitoring>(context);
        onboardingService = new ServiceGeneric<OnBoarding>(context);
        personService = new ServiceGeneric<Person>(context);
        planService = new ServiceGeneric<Plan>(context);
        checkpointService = new ServiceGeneric<Checkpoint>(context);
        logService = new ServiceLog(_context);
        mailModelService = new ServiceMailModel(context);
        mailMessageService = new ServiceGeneric<MailMessage>(context);
        mailService = new ServiceGeneric<MailLog>(context);
        workflowService = new ServiceGeneric<Workflow>(context);
        accountService = new ServiceGeneric<Account>(context);
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
      onboardingService._user = _user;
      monitoringService._user = _user;
      logService._user = _user;
      mailModelService._user = _user;
      mailMessageService._user = _user;
      workflowService._user = _user;
      mailService._user = _user;
      checkpointService._user = _user;
    }

    public List<ViewIndicatorsNotes> GetNotes(string id)
    {
      try
      {
        List<ViewIndicatorsNotes> result = new List<ViewIndicatorsNotes>();
        long totalqtd = 0;
        var monitorings = monitoringService.GetAll(p => p.Person.Manager._id == id & p.StatusMonitoring != EnumStatusMonitoring.InProgressPerson & p.StatusMonitoring != EnumStatusMonitoring.Wait & p.StatusMonitoring != EnumStatusMonitoring.End).Count();
        var onboardings = onboardingService.GetAll(p => p.Person.Manager._id == id & p.StatusOnBoarding != EnumStatusOnBoarding.InProgressPerson & p.StatusOnBoarding != EnumStatusOnBoarding.Wait & p.StatusOnBoarding != EnumStatusOnBoarding.End).Count();
        var workflows = workflowService.GetAll(p => p.Requestor._id == id & p.StatusWorkflow == EnumWorkflow.Open).Count();

        totalqtd = monitorings + onboardings + workflows;

        result.Add(new ViewIndicatorsNotes() { Name = "Monitoring", qtd = monitorings, total = totalqtd });
        result.Add(new ViewIndicatorsNotes() { Name = "Onboarding", qtd = onboardings, total = totalqtd });
        result.Add(new ViewIndicatorsNotes() { Name = "Workflow", qtd = workflows, total = totalqtd });

        return result;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewTagsCloud> ListTagsCloudCompany(string idmanager)
    {
      try
      {
        var list = monitoringService.GetAll(p => p.Person.Manager._id == idmanager & p.StatusMonitoring == EnumStatusMonitoring.End).ToList();

        List<ViewTagsCloud> listResult = new List<ViewTagsCloud>();
        foreach (var item in list)
        {
          foreach (var row in item.SkillsCompany)
          {
            if (row.Plans.Count() > 0)
              listResult.Add(new ViewTagsCloud() { text = row.Skill.Name });
          }
        }


        var result = listResult.GroupBy(x => x.text)
            .Select(x => new ViewTagsCloud()
            {
              text = x.Key,
              weight = x.Count()
            }).ToList();

        return result;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewTagsCloud> ListTagsCloud(string idmanager)
    {
      try
      {
        /*var list = monitoringService.GetAll(p => p.Person.Manager._id == idmanager & p.StatusMonitoring == EnumStatusMonitoring.End).
          Select(p => p.Activities.Where(u => u.Plans.Count() > 0).Select(
            x => x.Plans.Select(u => u.Skills))).ToList();*/

        var list = monitoringService.GetAll(p => p.Person.Manager._id == idmanager & p.StatusMonitoring == EnumStatusMonitoring.End).ToList();

        List<ViewTagsCloud> listResult = new List<ViewTagsCloud>();
        foreach (var item in list)
        {
          foreach (var row in item.Activities)
          {
            foreach (var skill in row.Plans)
            {
              foreach (var view in skill.Skills)
              {
                listResult.Add(new ViewTagsCloud() { text = view.Name });
              }
            }
          }
        }


        var result = listResult.GroupBy(x => x.text)
            .Select(x => new ViewTagsCloud()
            {
              text = x.Key,
              weight = x.Count()
            }).ToList();

        return result;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewTagsCloud> ListTagsCloudPerson(string idperson)
    {
      try
      {

        var list = monitoringService.GetAll(p => p.Person._id == idperson & p.StatusMonitoring == EnumStatusMonitoring.End).ToList();

        List<ViewTagsCloud> listResult = new List<ViewTagsCloud>();
        foreach (var item in list)
        {
          foreach (var row in item.Activities)
          {
            foreach (var skill in row.Plans)
            {
              foreach (var view in skill.Skills)
              {
                listResult.Add(new ViewTagsCloud() { text = view.Name });
              }
            }
          }
        }


        var result = listResult.GroupBy(x => x.text)
            .Select(x => new ViewTagsCloud()
            {
              text = x.Key,
              weight = x.Count()
            }).ToList();

        return result;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewTagsCloud> ListTagsCloudCompanyPerson(string idperson)
    {
      try
      {
        var list = monitoringService.GetAll(p => p.Person._id == idperson & p.StatusMonitoring == EnumStatusMonitoring.End).ToList();

        List<ViewTagsCloud> listResult = new List<ViewTagsCloud>();
        foreach (var item in list)
        {
          foreach (var row in item.SkillsCompany)
          {
            if (row.Plans.Count() > 0)
              listResult.Add(new ViewTagsCloud() { text = row.Skill.Name });
          }
        }


        var result = listResult.GroupBy(x => x.text)
            .Select(x => new ViewTagsCloud()
            {
              text = x.Key,
              weight = x.Count()
            }).ToList();

        return result;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewIndicatorsNotes> GetNotesPerson(string id)
    {
      try
      {
        List<ViewIndicatorsNotes> result = new List<ViewIndicatorsNotes>();
        long totalqtd = 0;
        var monitorings = monitoringService.GetAll(p => p.Person._id == id & p.StatusMonitoring != EnumStatusMonitoring.InProgressManager & p.StatusMonitoring != EnumStatusMonitoring.WaitManager & p.StatusMonitoring != EnumStatusMonitoring.End).Count();
        var onboardings = onboardingService.GetAll(p => p.Person._id == id & p.StatusOnBoarding != EnumStatusOnBoarding.InProgressManager & p.StatusOnBoarding != EnumStatusOnBoarding.WaitManager & p.StatusOnBoarding != EnumStatusOnBoarding.End).Count();
        var workflows = workflowService.GetAll(p => p.Requestor._id == id & p.StatusWorkflow == EnumWorkflow.Open).Count();

        totalqtd = monitorings + onboardings + workflows;
        result.Add(new ViewIndicatorsNotes() { Name = "Monitoring", qtd = monitorings, total = totalqtd });
        result.Add(new ViewIndicatorsNotes() { Name = "Onboarding", qtd = onboardings, total = totalqtd });
        result.Add(new ViewIndicatorsNotes() { Name = "Workflow", qtd = workflows, total = totalqtd });

        return result;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public bool VerifyAccount(string id)
    {
      try
      {
        var account = accountService.GetAuthentication(p => p._id == id).FirstOrDefault();
        if (account == null)
          return false;
        else
          return true;

      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public void SetUser(BaseUser baseUser)
    {
      try
      {
        _user = baseUser;
        personService._user = _user;
        onboardingService._user = _user;
        monitoringService._user = _user;
        logService._user = _user;
        mailModelService._user = _user;
        mailMessageService._user = _user;
        mailService._user = _user;
      }
      catch (Exception e)
      {
        throw e;
      }
    }


    public async Task SendMessages(string link)
    {
      try
      {
        hubConnection = new HubConnectionBuilder()
            .WithUrl(link + "messagesHub")
            .Build();

        await hubConnection.StartAsync();

        DoWork();
      }
      catch (Exception e)
      {
        throw e;
      }
    }


    private async Task DoWork()
    {
      try
      {
        while (true)
        {
          foreach (var person in personService.GetAuthentication(p => p.Status != EnumStatus.Disabled & p.StatusUser != EnumStatusUser.Disabled & p.StatusUser != EnumStatusUser.ErrorIntegration).ToList())
          {
            await hubConnection.InvokeAsync("GetNotes", person._id, person._idAccount);
            await hubConnection.InvokeAsync("GetNotesPerson", person._id, person._idAccount);
          }
          await Task.Delay(1000);
        }

      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<dynamic> ExportStatusOnboarding()
    {
      try
      {

        var list = personService.GetAll(p => p.StatusUser != EnumStatusUser.Disabled & p.StatusUser != EnumStatusUser.ErrorIntegration & p.TypeUser != EnumTypeUser.Administrator)
        .ToList().Select(p => new { Person = p, OnBoarding = onboardingService.GetAll(x => x.Person._id == p._id).FirstOrDefault() })
        .ToList();
        List<dynamic> result = new List<dynamic>();

        foreach (var item in list)
        {
          result.Add(new
          {
            item.Person.Name,
            NameManager = item.Person.Manager == null ? "Sem Gestor" : item.Person.Manager.Name,
            Status = item.OnBoarding == null ? EnumStatusOnBoarding.Open.ToString() : item.OnBoarding.StatusOnBoarding.ToString(),
            Date = item.OnBoarding?.DateEndEnd
          });
        }

        return result;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public List<dynamic> ExportStatusCheckpoint()
    {
      try
      {

        var list = personService.GetAll(p => p.StatusUser != EnumStatusUser.Disabled & p.StatusUser != EnumStatusUser.ErrorIntegration & p.TypeUser != EnumTypeUser.Administrator)
        .ToList().Select(p => new { Person = p, Checkpoint = checkpointService.GetAll(x => x.Person._id == p._id).FirstOrDefault() })
        .ToList();
        List<dynamic> result = new List<dynamic>();

        foreach (var item in list)
        {
          result.Add(new
          {
            item.Person.Name,
            NameManager = item.Person.Manager == null ? "Sem Gestor" : item.Person.Manager.Name,
            Status = item.Checkpoint == null ? EnumStatusCheckpoint.Open.ToString() : item.Checkpoint.StatusCheckpoint.ToString(),
            Date = item.Checkpoint?.DateEnd
          });
        }

        return result;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public List<dynamic> ExportStatusMonitoring()
    {
      try
      {

        var list = personService.GetAll(p => p.StatusUser != EnumStatusUser.Disabled & p.StatusUser != EnumStatusUser.ErrorIntegration & p.TypeUser != EnumTypeUser.Administrator)
        .ToList().Select(p => new { Person = p, Monitoring = monitoringService.GetAll(x => x.Person._id == p._id).FirstOrDefault() })
        .ToList();
        List<dynamic> result = new List<dynamic>();

        foreach (var item in list)
        {
          result.Add(new
          {
            item.Person.Name,
            NameManager = item.Person.Manager == null ? "Sem Gestor" : item.Person.Manager.Name,
            Status = item.Monitoring == null ? EnumStatusOnBoarding.Open.ToString() : item.Monitoring.StatusMonitoring.ToString(),
            Date = item.Monitoring?.DateEndEnd
          });
        }

        return result;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public List<dynamic> ExportStatusPlan()
    {
      try
      {

        var list = personService.GetAll(p => p.StatusUser != EnumStatusUser.Disabled & p.StatusUser != EnumStatusUser.ErrorIntegration & p.TypeUser != EnumTypeUser.Administrator)
        .ToList().Select(p => new { Person = p, Monitoring = monitoringService.GetAll(x => x.Person._id == p._id).FirstOrDefault() })
        .ToList();
        List<dynamic> result = new List<dynamic>();

        foreach (var item in list)
        {
          if (item.Monitoring != null)
          {
            foreach (var rows in item.Monitoring.Schoolings)
            {
              foreach (var plan in rows.Plans)
              {
                result.Add(new
                {
                  item.Person.Name,
                  NameManager = item.Person.Manager == null ? "Sem Gestor" : item.Person.Manager.Name,
                  plan?.Description,
                  Evalutions = plan == null ? 0 : plan.Evaluation,
                  Approved = plan?.StatusPlanApproved.ToString(),
                  Status = plan == null ? EnumStatusPlan.Open.ToString() : plan.StatusPlan.ToString(),
                  Date = plan?.DateEnd
                });
              }
            }

            foreach (var rows in item.Monitoring.SkillsCompany)
            {
              foreach (var plan in rows.Plans)
              {
                result.Add(new
                {
                  item.Person.Name,
                  NameManager = item.Person.Manager == null ? "Sem Gestor" : item.Person.Manager.Name,
                  plan?.Description,
                  Evalutions = plan == null ? 0 : plan.Evaluation,
                  Approved = plan?.StatusPlanApproved.ToString(),
                  Status = plan == null ? EnumStatusPlan.Open.ToString() : plan.StatusPlan.ToString(),
                  Date = plan?.DateEnd
                });
              }
            }

            foreach (var rows in item.Monitoring.Activities)
            {
              foreach (var plan in rows.Plans)
              {
                result.Add(new
                {
                  item.Person.Name,
                  NameManager = item.Person.Manager == null ? "Sem Gestor" : item.Person.Manager.Name,
                  plan?.Description,
                  Evalutions = plan == null ? 0 : plan.Evaluation,
                  Approved = plan?.StatusPlanApproved.ToString(),
                  Status = plan == null ? EnumStatusPlan.Open.ToString() : plan.StatusPlan.ToString(),
                  Date = plan?.DateEnd
                });
              }
            }
          }
        }

        return result;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }


    //public string[] ExportStatusOnboarding(ref long total, string filter, int count, int page)
    //{
    //  try
    //  {
    //    int skip = (count * (page - 1));

    //    var list = personService.GetAll(p => p.TypeJourney == EnumTypeJourney.OnBoarding
    //    & p.Name.ToUpper().Contains(filter.ToUpper()))
    //    .ToList().Select(p => new { Person = p, OnBoarding = onboardingService.GetAll(x => x.Person._id == p._id).FirstOrDefault() })
    //    .ToList().Skip(skip).Take(count).ToList();

    //    string head = "Name;NameManager;Status;";
    //    string[] rel = new string[1];
    //    rel[0] = head;

    //    foreach (var item in list)
    //    {
    //      string itemView = item.Person.Name + ";";
    //      if (item.Person.Manager == null)
    //        itemView += "Sem Gestor;";
    //      else
    //        itemView += item.Person.Manager.Name + ";";
    //      if (item.OnBoarding == null)
    //        itemView += EnumStatusOnBoarding.Open.ToString() + ";";
    //      else
    //        itemView += item.OnBoarding.StatusOnBoarding + ";";


    //      rel = Export(rel, itemView);
    //    }

    //    return rel;
    //  }
    //  catch (Exception e)
    //  {
    //    throw new ServiceException(_user, e, this._context);
    //  }
    //}

    public string[] Export(string[] rel, string message)
    {
      try
      {
        string[] text = rel;
        string[] lines = null;
        try
        {
          lines = new string[text.Count() + 1];
          var count = 0;
          foreach (var item in text)
          {
            lines.SetValue(item, count);
            count += 1;
          }
          lines.SetValue(message, text.Count());
        }
        catch (Exception)
        {
          lines = new string[1];
          lines.SetValue(message, 0);
        }

        return lines;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string[] FileNew(string name, string message)
    {
      try
      {
        string[] text = { message };
        string[] lines = null;
        try
        {
          text = System.IO.File.ReadAllLines(name + ".csv");
          lines = new string[text.Count() + 1];
          var count = 0;
          foreach (var item in text)
          {
            lines.SetValue(item, count);
            count += 1;
          }
          lines.SetValue(message, text.Count());
        }
        catch (Exception)
        {
          lines = new string[1];
          lines.SetValue(message, 0);
        }

        System.IO.File.WriteAllLines(name + ".csv", lines);

        return lines;
      }
      catch (Exception e)
      {
        throw e;
      }
    }


    public IEnumerable<dynamic> ChartOnboarding()
    {
      try
      {

        var list = personService.GetAll(p => p.StatusUser != EnumStatusUser.Disabled & p.StatusUser != EnumStatusUser.ErrorIntegration & p.TypeUser != EnumTypeUser.Administrator)
        .ToList().Select(p => new { Person = p, OnBoarding = onboardingService.GetAll(x => x.Person._id == p._id).FirstOrDefault() })
        .GroupBy(p => p.OnBoarding == null ? EnumStatusOnBoarding.Open : p.OnBoarding.StatusOnBoarding).Select(x => new
        {
          Status = x.Key,
          Count = x.Count()
        }).ToList();

        return list;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public IEnumerable<dynamic> ChartOnboardingRealized()
    {
      try
      {

        var list = personService.GetAll(p => p.StatusUser != EnumStatusUser.Disabled & p.StatusUser != EnumStatusUser.ErrorIntegration & p.TypeUser != EnumTypeUser.Administrator)
       .ToList().Select(p => new { Person = p, OnBoarding = onboardingService.GetAll(x => x.Person._id == p._id).FirstOrDefault() })
       .GroupBy(p => p.OnBoarding == null ? "Não Realizado" : (p.OnBoarding.StatusOnBoarding == EnumStatusOnBoarding.End ? "Realizado" : "Não Realizado")).Select(x => new
       {
         Status = x.Key,
         Count = x.Count()
       }).ToList();

        return list;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public IEnumerable<dynamic> ChartMonitoring()
    {
      try
      {

        var list = personService.GetAll(p => p.StatusUser != EnumStatusUser.Disabled & p.StatusUser != EnumStatusUser.ErrorIntegration & p.TypeUser != EnumTypeUser.Administrator)
        .ToList().Select(p => new { Person = p, Monitoring = monitoringService.GetAll(x => x.Person._id == p._id).FirstOrDefault() })
        .GroupBy(p => p.Monitoring == null ? EnumStatusMonitoring.Open : p.Monitoring.StatusMonitoring).Select(x => new
        {
          Status = x.Key,
          Count = x.Count()
        }).ToList();

        return list;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public IEnumerable<dynamic> ChartMonitoringRealized()
    {
      try
      {

        var list = personService.GetAll(p => p.StatusUser != EnumStatusUser.Disabled & p.StatusUser != EnumStatusUser.ErrorIntegration & p.TypeUser != EnumTypeUser.Administrator)
       .ToList().Select(p => new { Person = p, Monitoring = monitoringService.GetAll(x => x.Person._id == p._id).FirstOrDefault() })
       .GroupBy(p => p.Monitoring == null ? "Não Realizado" : (p.Monitoring.StatusMonitoring == EnumStatusMonitoring.End ? "Realizado" : "Não Realizado")).Select(x => new
       {
         Status = x.Key,
         Count = x.Count()
       }).ToList();

        return list;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public IEnumerable<dynamic> ChartCheckpoint()
    {
      try
      {

        var list = personService.GetAll(p => p.StatusUser != EnumStatusUser.Disabled & p.StatusUser != EnumStatusUser.ErrorIntegration & p.TypeUser != EnumTypeUser.Administrator)
        .ToList().Select(p => new { Person = p, Checkpoint = checkpointService.GetAll(x => x.Person._id == p._id).FirstOrDefault() })
        .GroupBy(p => p.Checkpoint == null ? EnumStatusCheckpoint.Open : p.Checkpoint.StatusCheckpoint).Select(x => new
        {
          Status = x.Key,
          Count = x.Count()
        }).ToList();

        return list;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public IEnumerable<dynamic> ChartCheckpointRealized()
    {
      try
      {

        var list = personService.GetAll(p => p.StatusUser != EnumStatusUser.Disabled & p.StatusUser != EnumStatusUser.ErrorIntegration & p.TypeUser != EnumTypeUser.Administrator)
       .ToList().Select(p => new { Person = p, Checkpoint = checkpointService.GetAll(x => x.Person._id == p._id).FirstOrDefault() })
       .GroupBy(p => p.Checkpoint == null ? "Não Realizado" : (p.Checkpoint.StatusCheckpoint == EnumStatusCheckpoint.End ? "Realizado" : "Não Realizado")).Select(x => new
       {
         Status = x.Key,
         Count = x.Count()
       }).ToList();

        return list;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public IEnumerable<dynamic> ChartPlan()
    {
      try
      {

        var list = personService.GetAll(p => p.StatusUser != EnumStatusUser.Disabled & p.StatusUser != EnumStatusUser.ErrorIntegration & p.TypeUser != EnumTypeUser.Administrator)
        .ToList().Select(p => new { Person = p, Monitoring = monitoringService.GetAll(x => x.Person._id == p._id).FirstOrDefault() }).ToList();

        List<dynamic> result = new List<dynamic>();

        foreach (var item in list)
        {
          if (item.Monitoring != null)
          {
            foreach (var rows in item.Monitoring.Schoolings)
            {
              foreach (var plan in rows.Plans)
              {
                result.Add(new
                {
                  item.Person.Name,
                  Status = plan == null ? EnumStatusPlan.Open.ToString() : plan.StatusPlan.ToString()
                });
              }
            }

            foreach (var rows in item.Monitoring.SkillsCompany)
            {
              foreach (var plan in rows.Plans)
              {
                result.Add(new
                {
                  item.Person.Name,
                  Status = plan == null ? EnumStatusPlan.Open.ToString() : plan.StatusPlan.ToString()
                });
              }
            }

            foreach (var rows in item.Monitoring.Activities)
            {
              foreach (var plan in rows.Plans)
              {
                result.Add(new
                {
                  item.Person.Name,
                  Status = plan == null ? EnumStatusPlan.Open.ToString() : plan.StatusPlan.ToString()
                });
              }
            }
          }
        }

        return result.GroupBy(p => p.Status).Select(x => new
        {
          Status = x.Key,
          Count = x.Count()
        }).ToList();

      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public IEnumerable<dynamic> ChartPlanRealized()
    {
      try
      {

        var list = personService.GetAll(p => p.StatusUser != EnumStatusUser.Disabled & p.StatusUser != EnumStatusUser.ErrorIntegration & p.TypeUser != EnumTypeUser.Administrator)
        .ToList().Select(p => new { Person = p, Monitoring = monitoringService.GetAll(x => x.Person._id == p._id).FirstOrDefault() }).ToList();

        List<dynamic> result = new List<dynamic>();

        foreach (var item in list)
        {
          if (item.Monitoring != null)
          {
            foreach (var rows in item.Monitoring.Schoolings)
            {
              foreach (var plan in rows.Plans)
              {
                result.Add(new
                {
                  item.Person.Name,
                  Status = plan.StatusPlan == EnumStatusPlan.Realized ? "Realizado" : "Não Realizado"
                });
              }
            }

            foreach (var rows in item.Monitoring.SkillsCompany)
            {
              foreach (var plan in rows.Plans)
              {
                result.Add(new
                {
                  item.Person.Name,
                  Status = plan.StatusPlan == EnumStatusPlan.Realized ? "Realizado" : "Não Realizado"
                });
              }
            }

            foreach (var rows in item.Monitoring.Activities)
            {
              foreach (var plan in rows.Plans)
              {
                result.Add(new
                {
                  item.Person.Name,
                  Status = plan.StatusPlan == EnumStatusPlan.Realized ? "Realizado" : "Não Realizado"
                });
              }
            }
          }
        }

        return result.GroupBy(p => p.Status).Select(x => new
        {
          Status = x.Key,
          Count = x.Count()
        }).ToList();

      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }
  }
  #pragma warning restore 4014
}
