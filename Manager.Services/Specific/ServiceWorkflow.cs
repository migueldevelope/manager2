using Manager.Core.Business;
using Manager.Core.Interfaces;
using Manager.Core.Views;
using Manager.Data;
using Manager.Services.Commons;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Collections.Generic;
using Manager.Core.Base;
using Manager.Views.BusinessView;
using Manager.Services.Auth;
using Manager.Views.Enumns;
using System.Threading.Tasks;

namespace Manager.Services.Specific
{
  public class ServiceWorkflow : Repository<Workflow>, IServiceWorkflow
  {
    private readonly ServicePerson servicePerson;
    private readonly ServiceGeneric<Workflow> serviceWorkflow;

    #region Constructor
    public ServiceWorkflow(DataContext context, DataContext contextLog, IServiceControlQueue serviceControlQueue, string pathSignalr) : base(context)
    {
      try
      {
        servicePerson = new ServicePerson(context, contextLog, null, serviceControlQueue, pathSignalr);
        serviceWorkflow = new ServiceGeneric<Workflow>(context);
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public void SetUser(IHttpContextAccessor contextAccessor)
    {
      User(contextAccessor);
      serviceWorkflow._user = _user;
      servicePerson.SetUser(_user);
    }
    public void SetUser(BaseUser user)
    {
      _user = user;
      serviceWorkflow._user = user;
      servicePerson.SetUser(user);
    }
    #endregion

    #region WorkFlow
    public List<Workflow> NewFlow(ViewFlow view)
    {
      try
      {
        if (view.Type == EnumTypeFlow.Manager)
          return Manager(view);
        return null;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public List<Workflow> Manager(ViewFlow view)
    {
      try
      {
        var person = servicePerson.GetNewVersion(p => p._id == view.IdPerson).Result;
        var manager = servicePerson.GetNewVersion(p => p._id == person.Manager._id).Result;
        var result = new List<Workflow>();
        var workflow = new Workflow
        {
          StatusWorkflow = EnumWorkflow.Open,
          Status = EnumStatus.Enabled,
          Requestor = manager.GetViewListBaseManager(),
          Sequence = 1
        };
        serviceWorkflow.InsertNewVersion(workflow).Wait();
        result.Add(workflow);
        return result;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public Workflow Approved(ViewWorkflow view)
    {
      try
      {
        var workflow = serviceWorkflow.GetAllNewVersion(p => p._id == view._idWorkflow).Result.FirstOrDefault();
        workflow.StatusWorkflow = EnumWorkflow.Approved;
        workflow.Commetns = view.Comments;
        workflow.Date = DateTime.Now;
        serviceWorkflow.Update(workflow, null).Wait();
        return workflow;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public Workflow Disapproved(ViewWorkflow view)
    {
      try
      {
        var workflow = serviceWorkflow.GetAllNewVersion(p => p._id == view._idWorkflow).Result.FirstOrDefault();
        workflow.StatusWorkflow = EnumWorkflow.Disapproved;
        workflow.Commetns = view.Comments;
        workflow.Date = DateTime.Now;
        serviceWorkflow.Update(workflow, null).Wait();
        return workflow;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    #endregion

  }
}
