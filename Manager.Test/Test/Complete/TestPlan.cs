using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.Interfaces;
using Manager.Core.Views;
using Manager.Services.Commons;
using Manager.Services.Specific;
using Manager.Test.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Manager.Test.Test.Complete
{
  public class TestPlan : TestCommons<Account>
  {
    private readonly IServicePlan servicePlan;
    private readonly ServiceGeneric<Person> servicePerson;

    public TestPlan()
    {
      try
      {
        base.Init();
        servicePlan = new ServicePlan(context, context, "");
        servicePerson = new ServiceGeneric<Person>(base.context)
        {
          _user = base.baseUser
        };
        servicePlan.SetUser(base.contextAccessor);
        //servicePlan._user = base.baseUser;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    //[Fact]
    //public void ManagerTest()
    //{
    //  try
    //  {
    //    var person = servicePerson.GetAll(p => p.User.Name.Contains("gestor")).FirstOrDefault();
    //    //long total = 0;
    //    //var plans = servicePlan.ListPlans(ref total, person._id, "", 100, 1, 1, 1, 1, 1, 1, 1);
    //    var idmonitoring = "5b912f42840add76dccd4801";
    //    var idplan = "5b912f5a840add76dccd4808";
    //    var plan = servicePlan.GetPlan(idmonitoring, idplan);

    //    plan.NewAction = EnumNewAction.Yes;

    //    var list = new List<ViewPlanNewUp>();
    //    list.Add(new ViewPlanNewUp()
    //    {
    //      _id = plan._id,
    //      _idAccount = plan._idAccount,
    //      Name = plan.Name,
    //      Description = plan.Description,
    //      Deadline = plan.Deadline,
    //      Skills = plan.Skills,
    //      UserInclude = plan.UserInclude,
    //      DateInclude = plan.DateInclude,
    //      TypePlan = plan.TypePlan,
    //      SourcePlan = plan.SourcePlan,
    //      TypeAction = plan.TypeAction,
    //      StatusPlan = plan.StatusPlan,
    //      TextEnd = plan.TextEnd,
    //      DateEnd = plan.DateEnd,
    //      Evaluation = plan.Evaluation,
    //      Result = "",
    //      StatusPlanApproved = EnumStatusPlanApproved.Approved,
    //      Status = plan.Status,
    //      TypeViewPlan = EnumTypeViewPlan.Update,
    //      NewAction = plan.NewAction
    //    });

    //    list.Add(new ViewPlanNewUp()
    //    {
    //      _id = plan.PlanNew._id,
    //      _idAccount = plan.PlanNew._idAccount,
    //      Name = plan.PlanNew.Name,
    //      Description = plan.PlanNew.Description,
    //      Deadline = plan.PlanNew.Deadline,
    //      Skills = plan.PlanNew.Skills,
    //      UserInclude = plan.PlanNew.UserInclude,
    //      DateInclude = plan.PlanNew.DateInclude,
    //      TypePlan = plan.PlanNew.TypePlan,
    //      SourcePlan = plan.PlanNew.SourcePlan,
    //      TypeAction = plan.PlanNew.TypeAction,
    //      StatusPlan = plan.PlanNew.StatusPlan,
    //      TextEnd = plan.PlanNew.TextEnd,
    //      DateEnd = plan.PlanNew.DateEnd,
    //      Evaluation = plan.PlanNew.Evaluation,
    //      Result = "",
    //      StatusPlanApproved = plan.PlanNew.StatusPlanApproved,
    //      Status = plan.PlanNew.Status,
    //      TypeViewPlan = EnumTypeViewPlan.New,
    //      NewAction = plan.NewAction
    //    });

    //    servicePlan.NewUpdatePlan(idmonitoring, list);

    //  }
    //  catch (Exception e)
    //  {
    //    throw e;
    //  }
    //}

  }

}
