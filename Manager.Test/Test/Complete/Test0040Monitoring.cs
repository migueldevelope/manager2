//using Manager.Core.Business;
//using Manager.Core.BusinessModel;
//using Manager.Services.Commons;
//using Manager.Services.Specific;
//using Manager.Test.Commons;
//using Manager.Views.Enumns;
//using MongoDB.Bson;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using Xunit;

//namespace Manager.Test.Test.Complete
//{
//    public class Test0040Monitoring : TestCommons<Monitoring>
//  {
//    //private ServiceMonitoring serviceMonitoring;
//    private readonly ServiceGeneric<Person> servicePerson;

//    public Test0040Monitoring()
//    {
//      base.Init();
//      //serviceMonitoring = new ServiceMonitoring(context, context, "", null);
//      //serviceMonitoring.SetUser(base.baseUser);

//      servicePerson = new ServiceGeneric<Person>(base.context);
//      servicePerson._user = base.baseUser;
//    }

//    private void Test()
//    {
//      //var i = serviceMonitoring.Exists("");
//    }
//    //[Fact]
//    //public void TestMonitoringComplete()
//    //{
//    //  try
//    //  {
//    //    long total = 0;
//    //    var person = servicePerson.GetAllNewVersion(p => p.User.Name.Contains("Ariel")).FirstOrDefault();

//    //    var list = serviceMonitoring.ListMonitoringsWait(person.Manager._id, "Ariel", 10, 1).FirstOrDefault();
//    //    var newOn = serviceMonitoring.NewMonitoring(list, person.Manager._id);

//    //    foreach (var item in newOn.SkillsCompany)
//    //    {
//    //      item.CommentsManager = "teste 1";
//    //    }
//    //    newOn.StatusMonitoring = EnumStatusMonitoring.Wait;
//    //    serviceMonitoring.UpdateMonitoring(newOn, person.Manager._id);

//    //    var listskills = serviceMonitoring.GetSkills(person._id);


//    //  }
//    //  catch (Exception e)
//    //  {
//    //    throw e;
//    //  }
//    //}


//  }
//}
