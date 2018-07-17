using Manager.Core.Business;
using Manager.Core.Enumns;
using Manager.Core.Interfaces;
using Manager.Core.Views;
using Manager.Data;
using Manager.Services.Commons;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using System;
using System.Linq;
using System.Collections.Generic;
using Manager.Core.Base;

namespace Manager.Services.Specific
{
  public class ServiceWorkflow : Repository<Workflow>, IServiceWorkflow
  {
    private readonly IServicePerson personService;
    private readonly ServiceGeneric<Workflow> workflowService;

    public BaseUser user { get => _user; set => user = _user; }

    public ServiceWorkflow(DataContext context, IServicePerson _personService)
      : base(context)
    {
      try
      {
        workflowService = new ServiceGeneric<Workflow>(context);
        personService = _personService;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

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
        throw new ServiceException(_user, e, this._context);
      }
    }

    public List<Workflow> Manager(ViewFlow view)
    {
      try
      {
        var person = personService.GetPerson(view.IdPerson);
        var manager = personService.GetPerson(person.Manager._id.ToString());
        var result = new List<Workflow>();
        var workflow = new Workflow
        {
          StatusWorkflow = EnumWorkflow.Open,
          Status = EnumStatus.Enabled,
          Requestor = manager,
          Sequence = 1
        };
        workflowService.Insert(workflow);
        result.Add(workflow);
        return result;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public Workflow Approved(ViewWorkflow view)
    {
      try
      {
        var workflow = workflowService.GetAll(p => p._id == view.IdWorkflow).FirstOrDefault();
        workflow.StatusWorkflow = EnumWorkflow.Approved;
        workflow.Commetns = view.Comments;
        workflow.Date = DateTime.Now;
        workflowService.Update(workflow, null);
        return workflow;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public Workflow Disapproved(ViewWorkflow view)
    {
      try
      {
        var workflow = workflowService.GetAll(p => p._id == view.IdWorkflow).FirstOrDefault();
        workflow.StatusWorkflow = EnumWorkflow.Disapproved;
        workflow.Commetns = view.Comments;
        workflow.Date = DateTime.Now;
        workflowService.Update(workflow, null);
        return workflow;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public void SetUser(IHttpContextAccessor contextAccessor)
    {
      User(contextAccessor);
      workflowService._user = _user;
    }

  }
}
