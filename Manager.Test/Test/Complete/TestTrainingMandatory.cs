using Manager.Core.Business;
using Manager.Core.Views;
using Manager.Services.Commons;
using Manager.Services.Specific;
using Manager.Test.Commons;
using Manager.Views.Enumns;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Manager.Test.Test.Complete
{
  public class TestTrainingMandatory : TestCommons<OnBoarding>
  {
    private ServiceMandatoryTraining serviceMandatoryTraining;
    private ServiceEvent serviceEvent;
    private readonly ServiceGeneric<Person> servicePerson;

    public TestTrainingMandatory()
    {
      base.Init();
      serviceMandatoryTraining = new ServiceMandatoryTraining(base.context);
      serviceEvent = new ServiceEvent(context, context, "http://10.0.0.15/");
      serviceMandatoryTraining.SetUser(baseUser);
      serviceEvent.SetUser(base.baseUser);

      servicePerson = new ServiceGeneric<Person>(base.context)
      {
        _user = base.baseUser
      };
      serviceEvent._user = base.baseUser;
    }

    [Fact]
    public void TestTrainigPlanComplete()
    {
      try
      {
        long total = 0;
        var person = servicePerson.GetAll(p => p.User.Name.Contains("Analisa")).FirstOrDefault();

        
        //var course = serviceEvent.ListCourse(1, 1, "").Result.FirstOrDefault();
        //var listperson = serviceMandatoryTraining.ListPerson(course._id, person.Company._id, 10, 1, "");

        //var view = new ViewAddPersonMandatory()
        //{
        //  Person = person,
        //  BeginDate = DateTime.Now,
        //  Course = course,
        //  TypeMandatoryTraining = EnumTypeMandatoryTraining.Mandatory
        //};

        //serviceMandatoryTraining.AddPerson(view);
        //var list = serviceMandatoryTraining.List(1, 1, "");

        //var listPlan = serviceMandatoryTraining.ListTrainingPlan(person.Company._id, 1, 1, "");
        

      }
      catch (Exception e)
      {
        throw e;
      }
    }

  }
}
