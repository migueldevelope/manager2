//using Manager.Core.Business;
//using Manager.Core.BusinessModel;
//using Manager.Core.Interfaces;
//using Manager.Services.Commons;
//using Manager.Services.Specific;
//using Manager.Test.Commons;
//using Manager.Views.BusinessCrud;
//using Manager.Views.Enumns;
//using MongoDB.Bson;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using Xunit;

//namespace Manager.Test.Test.Complete
//{
//  public class Test0020Onboarding : TestCommons<OnBoarding>
//  {
//    private ServiceOnBoarding serviceOnBoarding;
//    private readonly ServiceGeneric<Person> servicePerson;

//    public Test0020Onboarding()
//    {
//      base.Init();
      

//      serviceOnBoarding = new ServiceOnBoarding(context, context, "http://10.0.0.14/", base.serviceControlQueue);
//      serviceOnBoarding.SetUser(base.baseUser);

//      servicePerson = new ServiceGeneric<Person>(base.context);
//      servicePerson._user = base.baseUser;
//    }

//    [Fact]
//    public void TestOnBoardingComplete()
//    {
//      try
//      {
//        long total = 0;
//        var person = servicePerson.GetAllNewVersion(p => p.User.Name.Contains("Ariel")).Result.FirstOrDefault();

//        var list = serviceOnBoarding.ListOnBoardingsWait(person.Manager._id, ref total, "Ariel", 10, 1).FirstOrDefault();
        
//        var newOnboarding = serviceOnBoarding.New(list._id);
//        var getOnboarding = serviceOnBoarding.Get(newOnboarding._id);

//        foreach (var item in getOnboarding.SkillsCompany)
//        {
//          item.CommentsManager = "teste company 1";
//        }
//        foreach (var item in getOnboarding.SkillsGroup)
//        {
//          item.CommentsManager = "teste group 1";
//        }
//        foreach (var item in getOnboarding.SkillsOccupation)
//        {
//          item.CommentsManager = "teste occupation 1";
//        }
//        getOnboarding.CommentsManager = "test comment manager";
//        getOnboarding.StatusOnBoarding = EnumStatusOnBoarding.End;

//        var result = serviceOnBoarding.Update(getOnboarding);


//      }
//      catch (Exception e)
//      {
//        throw e;
//      }
//    }

//  }
//}
