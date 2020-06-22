using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.Interfaces;
using Manager.Data;
using Manager.Services.Commons;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manager.Services.Specific
{
  public class ServiceElearningFluid : Repository<ElearningFluid>, IServiceElearningFluid
  {
    private readonly ServiceGeneric<ElearningFluid> serviceElearningFluid;
    private readonly ServiceGeneric<ElearningFluidQuestions> serviceElearningFluidQuestions;
    private readonly ServiceGeneric<Person> servicePerson;


    #region Constructor
    public ServiceElearningFluid(DataContext context) : base(context)
    {
      try
      {
        serviceElearningFluid = new ServiceGeneric<ElearningFluid>(context);
        serviceElearningFluidQuestions = new ServiceGeneric<ElearningFluidQuestions>(context);
        servicePerson = new ServiceGeneric<Person>(context);
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public void SetUser(IHttpContextAccessor contextAccessor)
    {
      User(contextAccessor);
      serviceElearningFluid._user = _user;
      serviceElearningFluidQuestions._user = _user;
      servicePerson._user = _user;
    }
    public void SetUser(BaseUser user)
    {
      _user = user;
      serviceElearningFluid._user = user;
      serviceElearningFluidQuestions._user = user;
      servicePerson._user = user;
    }
    #endregion

    #region ElearningFluid
    public string Delete(string id)
    {
      try
      {
        ElearningFluid item = serviceElearningFluid.GetNewVersion(p => p._id == id).Result;
        item.Status = EnumStatus.Disabled;
        serviceElearningFluid.Update(item, null).Wait();
        return "ElearningFluid deleted!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public ViewCrudElearningFluid New(ViewCrudElearningFluid view)
    {
      try
      {
        var model = serviceElearningFluid.GetNewVersion(p => p._idUser == _user._idUser).Result;

        if (model == null)
          model = serviceElearningFluid.InsertNewVersion(Load()).Result;

        return model.GetViewCrud();
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public ViewCrudElearningFluid Update(ViewCrudElearningFluid view)
    {
      try
      {
        var model = serviceElearningFluid.GetNewVersion(p => p._id == view._id).Result;

        model.Person = view.Person;
        model.DateBegin = view.DateBegin;
        model.DateEnd = view.DateEnd;
        model.Score = view.Score;
        model.Questions = view.Questions;
        model.StatusElearningFluid = view.StatusElearningFluid;

        serviceElearningFluid.Update(model, null).Wait();

        return model.GetViewCrud();
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public ViewCrudElearningFluid Get(string id)
    {
      try
      {
        return serviceElearningFluid.GetNewVersion(p => p._id == id).Result.GetViewCrud();
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public List<ViewListElearningFluid> List(ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        List<ViewListElearningFluid> detail = serviceElearningFluid.GetAllNewVersion(p => p.Person.Name.ToUpper().Contains(filter.ToUpper()), count, count * (page - 1), "Person.Name").Result
          .Select(x => new ViewListElearningFluid()
          {
            _id = x._id,
          }).ToList();
        total = serviceElearningFluid.CountNewVersion(p => p.Person.Name.ToUpper().Contains(filter.ToUpper())).Result;
        return detail;
      }
      catch (Exception e)
      {
        throw e;
      }
    }


    #endregion

    #region ElearningFluidQuestions
    public string DeleteElearningFluidQuestions(string id)
    {
      try
      {
        ElearningFluidQuestions item = serviceElearningFluidQuestions.GetNewVersion(p => p._id == id).Result;
        item.Status = EnumStatus.Disabled;
        serviceElearningFluidQuestions.Update(item, null).Wait();
        return "ElearningFluidQuestions deleted!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public ViewCrudElearningFluidQuestions NewElearningFluidQuestions(ViewCrudElearningFluidQuestions view)
    {
      try
      {

        var model = serviceElearningFluidQuestions.InsertNewVersion(
          new ElearningFluidQuestions()
          {
            _id = view._id,
            Theme = view.Theme,
            Question = view.Question,
            ChoiceA = view.ChoiceA,
            ChoiceB = view.ChoiceB,
            ChoiceC = view.ChoiceC,
            Correct = view.Correct
          }).Result;

        return model.GetViewCrud();
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public ViewCrudElearningFluidQuestions UpdateElearningFluidQuestions(ViewCrudElearningFluidQuestions view)
    {
      try
      {
        var model = serviceElearningFluidQuestions.GetNewVersion(p => p._id == view._id).Result;
        model.Theme = view.Theme;
        model.Question = view.Question;
        model.ChoiceA = view.ChoiceA;
        model.ChoiceB = view.ChoiceB;
        model.ChoiceC = view.ChoiceC;
        model.Correct = view.Correct;

        serviceElearningFluidQuestions.Update(model, null).Wait();
        return model.GetViewCrud();
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public ViewCrudElearningFluidQuestions GetElearningFluidQuestions(string id)
    {
      try
      {
        return serviceElearningFluidQuestions.GetNewVersion(p => p._id == id).Result.GetViewCrud();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewListElearningFluidQuestions> ListElearningFluidQuestions(ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        List<ViewListElearningFluidQuestions> detail = serviceElearningFluidQuestions.GetAllNewVersion(p => p.Question.ToUpper().Contains(filter.ToUpper()), count, count * (page - 1), "Question").Result
          .Select(p => new ViewListElearningFluidQuestions
          {
            _id = p._id,
            Question = p.Question
          }).ToList();
        total = serviceElearningFluidQuestions.CountNewVersion(p => p.Question.ToUpper().Contains(filter.ToUpper())).Result;
        return detail;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    #endregion

    #region private 

    private ElearningFluid Load()
    {
      try
      {
        var person = servicePerson.GetNewVersion(p => p.User._id == _user._idUser).Result;
        var questions = serviceElearningFluidQuestions.GetAllNewVersion(p => p.Status == EnumStatus.Enabled).Result;

        var view = new ElearningFluid()
        {
          _idUser = _user._idUser,
          Person = person.GetViewListBase(),
          DateBegin = DateTime.Now,
          Score = 0,
          StatusElearningFluid = EnumStatusElearningFluid.Open,
          Questions = new List<ViewCrudElearningFluidAnswer>()
        };

        foreach (var item in questions)
        {
          var question = new ViewCrudElearningFluidAnswer()
          {
            Theme = item.Theme,
            Question = item.Question,
            ChoiceA = item.ChoiceA,
            ChoiceB = item.ChoiceB,
            ChoiceC = item.ChoiceC,
            Correct = item.Correct,
            _id = ObjectId.GenerateNewId().ToString()
          };

          view.Questions.Add(question);
        }

        return view;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    #endregion

  }
}
