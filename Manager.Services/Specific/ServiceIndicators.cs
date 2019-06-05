using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.Interfaces;
using Manager.Core.Views;
using Manager.Data;
using Manager.Services.Commons;
using Manager.Views.BusinessView;
using Manager.Views.Enumns;
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
    private readonly ServiceGeneric<Monitoring> serviceMonitoring;
    private readonly ServiceGeneric<OnBoarding> serviceOnboarding;
    private readonly ServiceGeneric<Checkpoint> serviceCheckpoint;
    private readonly ServiceGeneric<Workflow> serviceWorkflow;
    private readonly ServiceGeneric<Person> servicePerson;
    private readonly ServiceGeneric<Plan> servicePlan;
    private readonly ServiceLog serviceLog;
    private readonly ServiceMailModel serviceMailModel;
    private readonly ServiceGeneric<MailLog> serviceMail;
    private readonly ServiceGeneric<Account> serviceAccount;
    private readonly ServiceGeneric<Certification> serviceCertification;
    public string path;
    private HubConnection hubConnection;

    public ServiceIndicators(DataContext context, DataContext contextLog, string pathToken)
      : base(context)
    {
      try
      {
        serviceMonitoring = new ServiceGeneric<Monitoring>(context);
        serviceOnboarding = new ServiceGeneric<OnBoarding>(context);
        servicePerson = new ServiceGeneric<Person>(context);
        servicePlan = new ServiceGeneric<Plan>(context);
        serviceCheckpoint = new ServiceGeneric<Checkpoint>(context);
        serviceLog = new ServiceLog(context, contextLog);
        serviceMailModel = new ServiceMailModel(context);
        serviceMail = new ServiceGeneric<MailLog>(contextLog);
        serviceWorkflow = new ServiceGeneric<Workflow>(context);
        serviceAccount = new ServiceGeneric<Account>(context);
        serviceCertification = new ServiceGeneric<Certification>(context);

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
      servicePerson._user = _user;
      serviceOnboarding._user = _user;
      serviceMonitoring._user = _user;
      serviceLog._user = _user;
      serviceMailModel._user = _user;
      serviceWorkflow._user = _user;
      serviceMail._user = _user;
      serviceCertification._user = _user;
      serviceCheckpoint._user = _user;
    }

    public async Task<List<ViewIndicatorsNotes>> GetNotes(string id)
    {
      try
      {
        List<ViewIndicatorsNotes> result = new List<ViewIndicatorsNotes>();
        long totalqtd = 0;
        var monitorings = serviceMonitoring.CountNewVersion(p => p.Person.Manager._id == id & p.StatusMonitoring != EnumStatusMonitoring.InProgressPerson & p.StatusMonitoring != EnumStatusMonitoring.Wait & p.StatusMonitoring != EnumStatusMonitoring.End).Result;
        var onboardings = serviceOnboarding.CountNewVersion(p => p.Person.Manager._id == id & p.StatusOnBoarding != EnumStatusOnBoarding.InProgressPerson & p.StatusOnBoarding != EnumStatusOnBoarding.WaitPerson & p.StatusOnBoarding != EnumStatusOnBoarding.End).Result;
        var workflows = serviceWorkflow.CountNewVersion(p => p.Requestor._id == id & p.StatusWorkflow == EnumWorkflow.Open).Result;

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

    public async Task<List<ViewTagsCloud>> ListTagsCloudCompany(string idmanager)
    {
      try
      {
        var list = serviceMonitoring.GetAll(p => p.Person.Manager._id == idmanager & p.StatusMonitoring == EnumStatusMonitoring.End).ToList();

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

    public async Task<List<ViewTagsCloud>> ListTagsCloud(string idmanager)
    {
      try
      {
        /*var list = serviceMonitoring.GetAll(p => p.Person.Manager._id == idmanager & p.StatusMonitoring == EnumStatusMonitoring.End).
          Select(p => p.Activities.Where(u => u.Plans.Result > 0).Select(
            x => x.Plans.Select(u => u.Skills))).ToList();*/

        var list = serviceMonitoring.GetAll(p => p.Person.Manager._id == idmanager & p.StatusMonitoring == EnumStatusMonitoring.End).ToList();

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

    public async Task<List<ViewTagsCloud>> ListTagsCloudPerson(string idperson)
    {
      try
      {

        var list = serviceMonitoring.GetAll(p => p.Person._id == idperson & p.StatusMonitoring == EnumStatusMonitoring.End).ToList();

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

    public async Task<List<ViewTagsCloud>> ListTagsCloudCompanyPerson(string idperson)
    {
      try
      {
        var list = serviceMonitoring.GetAll(p => p.Person._id == idperson & p.StatusMonitoring == EnumStatusMonitoring.End).ToList();

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

    public async Task<List<ViewIndicatorsNotes>> GetNotesPerson(string id)
    {
      try
      {
        List<ViewIndicatorsNotes> result = new List<ViewIndicatorsNotes>();
        long totalqtd = 0;
        var monitorings = serviceMonitoring.CountNewVersion(p => p.Person._id == id & p.StatusMonitoring != EnumStatusMonitoring.InProgressManager & p.StatusMonitoring != EnumStatusMonitoring.WaitManager & p.StatusMonitoring != EnumStatusMonitoring.End).Result;
        var onboardings = serviceOnboarding.CountNewVersion(p => p.Person._id == id & p.StatusOnBoarding != EnumStatusOnBoarding.InProgressManager & p.StatusOnBoarding != EnumStatusOnBoarding.WaitManager & p.StatusOnBoarding != EnumStatusOnBoarding.End).Result;
        var workflows = serviceWorkflow.CountNewVersion(p => p.Requestor._id == id & p.StatusWorkflow == EnumWorkflow.Open).Result;

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

    public async Task<bool> VerifyAccount(string id)
    {
      try
      {
        var account = serviceAccount.GetAuthentication(p => p._id == id).FirstOrDefault();
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
        servicePerson._user = _user;
        serviceOnboarding._user = _user;
        serviceMonitoring._user = _user;
        serviceLog._user = _user;
        serviceMailModel._user = _user;
        serviceMail._user = _user;
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
          foreach (var person in servicePerson.GetAuthentication(p => p.Status != EnumStatus.Disabled & p.StatusUser != EnumStatusUser.Disabled & p.StatusUser != EnumStatusUser.ErrorIntegration).ToList())
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

    public async Task<List<ViewExportStatusMonitoring>> ExportStatusMonitoring(string idperson)
    {
      try
      {

        var list = serviceMonitoring.GetAll(p => p.Person._id == idperson).ToList();
        List<ViewExportStatusMonitoring> result = new List<ViewExportStatusMonitoring>();

        foreach (var item in list)
        {
          result.Add(new ViewExportStatusMonitoring
          {
            IdMonitoring = item._id,
            NamePerson = item.Person.User.Name,
            Status = item.StatusMonitoring,
            Occupation = item.Person.Occupation.Name,
            DataEnd = item.DateEndEnd
          });
        }

        return result;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public async Task<List<ViewExportStatusOnboarding>> ExportStatusOnboarding(string idperson)
    {
      try
      {

        var list = serviceOnboarding.GetAll(p => p.Person._id == idperson).ToList();
        List<ViewExportStatusOnboarding> result = new List<ViewExportStatusOnboarding>();

        foreach (var item in list)
        {
          result.Add(new ViewExportStatusOnboarding
          {
            IdOnboarding = item._id,
            NamePerson = item.Person.User.Name,
            Status = item.StatusOnBoarding,
            Occupation = item.Person.Occupation.Name,
            DataEnd = item.DateEndEnd
          });
        }

        return result;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public async Task<List<ViewExportStatusCertification>> ExportStatusCertification()
    {
      try
      {

        var list = serviceCertification.GetAll(p => p.StatusCertification != EnumStatusCertification.Open).ToList();
        List<ViewExportStatusCertification> result = new List<ViewExportStatusCertification>();

        foreach (var item in list)
        {
          result.Add(new ViewExportStatusCertification
          {
            NameManager = item.Person.Manager.Name,
            NamePerson = item.Person.User.Name,
            NameItem = item.CertificationItem.Name,
            Status =
                item.StatusCertification == EnumStatusCertification.Open ? "Aguardando Aprovação" :
                  item.StatusCertification == EnumStatusCertification.Wait ? "Aguardando Aprovação" :
                    item.StatusCertification == EnumStatusCertification.Approved ? "Aprovado" : "Reprovado",
            Date = item.DateBegin
          });
        }

        return result;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public async Task<List<ViewExportStatusCertificationPerson>> ExportStatusCertification(string idperson)
    {
      try
      {

        var list = serviceCertification.GetAll(p => p.Person._id == idperson & p.StatusCertification != EnumStatusCertification.Open).ToList();
        List<ViewExportStatusCertificationPerson> result = new List<ViewExportStatusCertificationPerson>();

        foreach (var item in list)
        {
          result.Add(new ViewExportStatusCertificationPerson
          {
            IdCertification = item._id,
            NamePerson = item.Person.User.Name,
            NameItem = item.CertificationItem.Name,
            Status = item.StatusCertification,
            DateEnd = item.DateEnd
          });
        }

        return result;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public async Task<List<ViewExportStatusOnboardingGeral>> ExportStatusOnboarding()
    {
      try
      {

        var list = servicePerson.GetAll(p => p.StatusUser != EnumStatusUser.Disabled & p.StatusUser != EnumStatusUser.ErrorIntegration & p.TypeUser != EnumTypeUser.Administrator)
        .ToList().Select(p => new { Person = p, OnBoardings = serviceOnboarding.GetAll(x => x.Person._id == p._id).ToList() })
        .ToList();
        List<ViewExportStatusOnboardingGeral> result = new List<ViewExportStatusOnboardingGeral>();

        foreach (var rows in list)
        {
          if (rows.OnBoardings != null)
          {
            foreach (var item in rows.OnBoardings)
            {
              result.Add(new ViewExportStatusOnboardingGeral
              {
                NameManager = item.Person.Manager == null ? "Sem Gestor" : item.Person.Manager.Name,
                NamePerson = item.Person.User.Name,
                Type = item == null ? "Admissão" :
                item.Person.TypeJourney == EnumTypeJourney.OnBoardingOccupation ? "Troca de Cargo" : "Admissão",
                Occupation = item.Person.Occupation.Name,
                Status =
                item == null ? "Aguardando para iniciar" :
                  item.StatusOnBoarding == EnumStatusOnBoarding.WaitBegin ? "Aguardando para iniciar" :
                    item.StatusOnBoarding == EnumStatusOnBoarding.InProgressPerson ? "Em andamento pelo colaborador" :
                      item.StatusOnBoarding == EnumStatusOnBoarding.InProgressManager ? "Em andamento pelo gestor" :
                        item.StatusOnBoarding == EnumStatusOnBoarding.WaitPerson ? "Em andamento pelo gestor" :
                          item.StatusOnBoarding == EnumStatusOnBoarding.End ? "Finalizado" :
                            item.StatusOnBoarding == EnumStatusOnBoarding.WaitManager ? "Aguardando continuação pelo gestor" :
                              item.StatusOnBoarding == EnumStatusOnBoarding.WaitManagerRevision ? "Aguardando revisão do gestor" : "Aguardando para iniciar",
                DateBegin = item?.DateBeginPerson,
                DateEnd = item?.DateEndEnd,

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

    public async Task<List<ViewExportStatusCheckpoint>> ExportStatusCheckpoint()
    {
      try
      {

        var list = servicePerson.GetAll(p => p.StatusUser != EnumStatusUser.Disabled & p.StatusUser != EnumStatusUser.ErrorIntegration & p.TypeUser != EnumTypeUser.Administrator)
        .ToList().Select(p => new { Person = p, Checkpoint = serviceCheckpoint.GetAll(x => x.Person._id == p._id).FirstOrDefault() })
        .ToList();
        List<ViewExportStatusCheckpoint> result = new List<ViewExportStatusCheckpoint>();

        foreach (var item in list)
        {
          result.Add(new ViewExportStatusCheckpoint
          {
            NameManager = item.Person.Manager == null ? "Sem Gestor" : item.Person.Manager.Name,
            NamePerson = item.Person.User.Name,
            Status = item.Checkpoint == null ? "Aguardando para iniciar" :
              item.Checkpoint.StatusCheckpoint == EnumStatusCheckpoint.Open ? "Aguardando para iniciar" :
                item.Checkpoint.StatusCheckpoint == EnumStatusCheckpoint.Wait ? "Em Andamento" : "Finalizado",
            DateBegin = item.Checkpoint?.DateBegin,
            DateEnd = item.Checkpoint?.DateEnd,
            Occupation = item.Person.Occupation?.Name,
            Result = item.Checkpoint == null ? "Não iniciado" :
              item.Checkpoint.TypeCheckpoint == EnumCheckpoint.Approved ? "Efetivado" :
                item.Checkpoint.TypeCheckpoint == EnumCheckpoint.Disapproved ? "Não Efetivado" : "Aguardando Definição"
          });
        }

        return result;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public async Task<List<ViewExportStatusMonitoringGeral>> ExportStatusMonitoring()
    {
      try
      {

        var list = servicePerson.GetAll(p => p.StatusUser != EnumStatusUser.Disabled & p.StatusUser != EnumStatusUser.ErrorIntegration & p.TypeUser != EnumTypeUser.Administrator)
        .ToList().Select(p => new { Person = p, Monitorings = serviceMonitoring.GetAll(x => x.Person._id == p._id).ToList() })
        .ToList();
        List<ViewExportStatusMonitoringGeral> result = new List<ViewExportStatusMonitoringGeral>();

        foreach (var rows in list)
        {
          if (rows.Monitorings != null)
          {
            foreach (var item in rows.Monitorings)
            {
              result.Add(new ViewExportStatusMonitoringGeral
              {
                NameManager = item.Person.Manager == null ? "Sem Gestor" : item.Person.Manager.Name,
                NamePerson = item.Person.User.Name,
                Occupation = item.Person.Occupation.Name,
                Status =
              item == null ? "Aguardando para iniciar" :
                item.StatusMonitoring == EnumStatusMonitoring.Open ? "Aguardando para iniciar" :
                  item.StatusMonitoring == EnumStatusMonitoring.InProgressPerson ? "Em andamento pelo colaborador" :
                    item.StatusMonitoring == EnumStatusMonitoring.InProgressManager ? "Em andamento pelo gestor" :
                      item.StatusMonitoring == EnumStatusMonitoring.Wait ? "Em andamento pelo gestor" :
                        item.StatusMonitoring == EnumStatusMonitoring.End ? "Finalizado" :
                          item.StatusMonitoring == EnumStatusMonitoring.WaitManager ? "Aguardando continuação pelo gestor" :
                            item.StatusMonitoring == EnumStatusMonitoring.Disapproved ? "Aguardando revisão do gestor" : "Aguardando para iniciar",
                DateBegin = item?.DateBeginPerson,
                DateEnd = item?.DateEndEnd
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


    public async Task<List<ViewExportStatusPlan>> ExportStatusPlan()
    {
      try
      {

        var list = servicePlan.GetAll().Select(p => new ViewExportStatusPlan()
        {
          NameManager = p.Person.NameManager == null ? "Sem Gestor" : p.Person.NameManager,
          NamePerson = p.Person.User.Name,
          What = p.Name,
          Description = p.Description,
          Deadline = p.Deadline,
          Status = p.StatusPlan == EnumStatusPlan.Realized ? "Realizado" :
                        p.StatusPlan == EnumStatusPlan.NoRealized ? "Não Realizado" : "Em aberto",
          Obs = p.TextEnd,
          DateEnd = p.DateEnd
        }).ToList();


        return list;
      }
      catch (Exception e)
      {
        throw e;
      }
    }


    //public string[] ExportStatusOnboarding(ref  long total,  string filter, int count,int page)
    //{
    //  try
    //  {
    //    int skip = (count * (page - 1));

    //    var list = servicePerson.GetAll(p => p.TypeJourney == EnumTypeJourney.OnBoarding
    //    & p.Name.ToUpper().Contains(filter.ToUpper()))
    //    .ToList().Select(p => new { Person = p, OnBoarding = serviceOnboarding.GetAll(x => x.Person._id == p._id).FirstOrDefault() })
    //    .ToList().Skip(skip).Take(count).ToList();

    //    string head = "Name;NameManager;Status;";
    //    string[] rel = new string[1];
    //    rel[0] = head;

    //    foreach (var item in list)
    //    {
    //      string itemView = item.Person.User.Name + ";";
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
    //    throw e;
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


    public async Task<IEnumerable<ViewChartOnboarding>> ChartOnboarding()
    {
      try
      {

        var list = servicePerson.GetAll(p => p.StatusUser != EnumStatusUser.Disabled & p.StatusUser != EnumStatusUser.ErrorIntegration & p.TypeUser != EnumTypeUser.Administrator)
        .ToList().Select(p => new { Person = p, OnBoarding = serviceOnboarding.GetAll(x => x.Person._id == p._id).FirstOrDefault() })
        .GroupBy(p => p.OnBoarding == null ? EnumStatusOnBoarding.WaitBegin : p.OnBoarding.StatusOnBoarding).Select(x => new ViewChartOnboarding
        {
          Status = x.Key,
          Count = x.Count()
        }).ToList();

        return list;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public async Task<IEnumerable<ViewChartStatus>> ChartOnboardingRealized()
    {
      try
      {

        var list = servicePerson.GetAll(p => p.StatusUser != EnumStatusUser.Disabled & p.StatusUser != EnumStatusUser.ErrorIntegration & p.TypeUser != EnumTypeUser.Administrator)
       .ToList().Select(p => new { Person = p, OnBoarding = serviceOnboarding.GetAll(x => x.Person._id == p._id).FirstOrDefault() })
       .GroupBy(p => p.OnBoarding == null ? "Não Realizado" : (p.OnBoarding.StatusOnBoarding == EnumStatusOnBoarding.End ? "Realizado" : "Não Realizado")).Select(x => new ViewChartStatus
       {
         Status = x.Key,
         Count = x.Count()
       }).ToList();

        return list;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public async Task<IEnumerable<ViewChartMonitoring>> ChartMonitoring()
    {
      try
      {

        var list = servicePerson.GetAll(p => p.StatusUser != EnumStatusUser.Disabled & p.StatusUser != EnumStatusUser.ErrorIntegration & p.TypeUser != EnumTypeUser.Administrator)
        .ToList().Select(p => new { Person = p, Monitoring = serviceMonitoring.GetAll(x => x.Person._id == p._id).FirstOrDefault() })
        .GroupBy(p => p.Monitoring == null ? EnumStatusMonitoring.Open : p.Monitoring.StatusMonitoring).Select(x => new ViewChartMonitoring
        {
          Status = x.Key,
          Count = x.Count()
        }).ToList();

        return list;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public async Task<IEnumerable<ViewChartStatus>> ChartMonitoringRealized()
    {
      try
      {

        var list = servicePerson.GetAll(p => p.StatusUser != EnumStatusUser.Disabled & p.StatusUser != EnumStatusUser.ErrorIntegration & p.TypeUser != EnumTypeUser.Administrator)
       .ToList().Select(p => new { Person = p, Monitoring = serviceMonitoring.GetAll(x => x.Person._id == p._id).FirstOrDefault() })
       .GroupBy(p => p.Monitoring == null ? "Não Realizado" : (p.Monitoring.StatusMonitoring == EnumStatusMonitoring.End ? "Realizado" : "Não Realizado")).Select(x => new ViewChartStatus
       {
         Status = x.Key,
         Count = x.Count()
       }).ToList();

        return list;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public async Task<IEnumerable<ViewChartCheckpoint>> ChartCheckpoint()
    {
      try
      {

        var list = servicePerson.GetAll(p => p.StatusUser != EnumStatusUser.Disabled & p.StatusUser != EnumStatusUser.ErrorIntegration & p.TypeUser != EnumTypeUser.Administrator)
        .ToList().Select(p => new { Person = p, Checkpoint = serviceCheckpoint.GetAll(x => x.Person._id == p._id).FirstOrDefault() })
        .GroupBy(p => p.Checkpoint == null ? EnumStatusCheckpoint.Open : p.Checkpoint.StatusCheckpoint).Select(x => new ViewChartCheckpoint
        {
          Status = x.Key,
          Count = x.Count()
        }).ToList();

        return list;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public async Task<IEnumerable<ViewChartStatus>> ChartCheckpointRealized()
    {
      try
      {

        var list = servicePerson.GetAll(p => p.StatusUser != EnumStatusUser.Disabled & p.StatusUser != EnumStatusUser.ErrorIntegration & p.TypeUser != EnumTypeUser.Administrator)
       .ToList().Select(p => new { Person = p, Checkpoint = serviceCheckpoint.GetAll(x => x.Person._id == p._id).FirstOrDefault() })
       .GroupBy(p => p.Checkpoint == null ? "Não Realizado" : (p.Checkpoint.StatusCheckpoint == EnumStatusCheckpoint.End ? "Realizado" : "Não Realizado")).Select(x => new ViewChartStatus
       {
         Status = x.Key,
         Count = x.Count()
       }).ToList();

        return list;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public async Task<IEnumerable<ViewChartPlan>> ChartPlan()
    {
      try
      {

        var list = servicePerson.GetAll(p => p.StatusUser != EnumStatusUser.Disabled & p.StatusUser != EnumStatusUser.ErrorIntegration & p.TypeUser != EnumTypeUser.Administrator)
        .ToList().Select(p => new { Person = p, Monitoring = serviceMonitoring.GetAll(x => x.Person._id == p._id).FirstOrDefault() }).ToList();

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
                  item.Person.User.Name,
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
                  item.Person.User.Name,
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
                  item.Person.User.Name,
                  Status = plan == null ? EnumStatusPlan.Open.ToString() : plan.StatusPlan.ToString()
                });
              }
            }
          }
        }

        return result.GroupBy(p => p.Status).Select(x => new ViewChartPlan
        {
          Status = x.Key,
          Count = x.Count()
        }).ToList();

      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public async Task<IEnumerable<ViewChartStatus>> ChartPlanRealized()
    {
      try
      {

        var list = servicePerson.GetAll(p => p.StatusUser != EnumStatusUser.Disabled & p.StatusUser != EnumStatusUser.ErrorIntegration & p.TypeUser != EnumTypeUser.Administrator)
        .ToList().Select(p => new { Person = p, Monitoring = serviceMonitoring.GetAll(x => x.Person._id == p._id).FirstOrDefault() }).ToList();

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
                  item.Person.User.Name,
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
                  item.Person.User.Name,
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
                  item.Person.User.Name,
                  Status = plan.StatusPlan == EnumStatusPlan.Realized ? "Realizado" : "Não Realizado"
                });
              }
            }
          }
        }

        return result.GroupBy(p => p.Status).Select(x => new ViewChartStatus
        {
          Status = x.Key,
          Count = x.Count()
        }).ToList();

      }
      catch (Exception e)
      {
        throw e;
      }
    }

  }
#pragma warning restore 4014
}
