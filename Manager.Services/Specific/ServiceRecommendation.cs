using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.Interfaces;
using Manager.Data;
using Manager.Services.Commons;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manager.Services.Specific
{
  public class ServiceRecommendation : Repository<Recommendation>, IServiceRecommendation
  {
    private readonly ServiceGeneric<Recommendation> serviceRecommendation;
    private readonly ServiceGeneric<RecommendationPerson> serviceRecommendationPerson;
    private readonly ServiceGeneric<Person> servicePerson;
    private readonly ServiceGeneric<SalaryScale> serviceSalaryScale;

    #region Constructor
    public ServiceRecommendation(DataContext context) : base(context)
    {
      try
      {
        serviceRecommendation = new ServiceGeneric<Recommendation>(context);
        serviceRecommendationPerson = new ServiceGeneric<RecommendationPerson>(context);
        servicePerson = new ServiceGeneric<Person>(context);
        serviceSalaryScale = new ServiceGeneric<SalaryScale>(context);
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public void SetUser(IHttpContextAccessor contextAccessor)
    {
      User(contextAccessor);
      serviceRecommendation._user = _user;
      serviceRecommendationPerson._user = _user;
      servicePerson._user = _user;
      serviceSalaryScale._user = _user;
    }
    public void SetUser(BaseUser user)
    {
      _user = user;
      serviceRecommendation._user = user;
      serviceRecommendationPerson._user = user;
      servicePerson._user = user;
      serviceSalaryScale._user = user;
    }
    #endregion

    #region Recommendation
    public string Delete(string id)
    {
      try
      {
        Recommendation item = serviceRecommendation.GetNewVersion(p => p._id == id).Result;
        item.Status = EnumStatus.Disabled;
        serviceRecommendation.Update(item, null).Wait();
        return "Recommendation deleted!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public string New(ViewCrudRecommendation view)
    {
      try
      {
        Recommendation recommendation = serviceRecommendation.InsertNewVersion(new Recommendation()
        {
          _id = view._id,
          Name = view.Name,
          Image = view.Image,
          Content = view.Content,
          Skill = view.Skill          
        }).Result;
        return "Recommendation added!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }


    public void SetImage(string idBaseHelp, string url, string fileName, string attachmentid)
    {
      try
      {
        Recommendation basehelp = serviceRecommendation.GetFreeNewVersion(p => p._id == idBaseHelp).Result;
        basehelp.Image = url;
        serviceRecommendation.UpdateAccount(basehelp, null).Wait();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string Update(ViewCrudRecommendation view)
    {
      try
      {
        Recommendation recommendation = serviceRecommendation.GetNewVersion(p => p._id == view._id).Result;

        recommendation.Name = view.Name;
        recommendation.Skill = view.Skill;
        recommendation.Content = view.Image;

        serviceRecommendation.Update(recommendation, null).Wait();

        return "Recommendation altered!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public ViewCrudRecommendation Get(string id)
    {
      try
      {
        return serviceRecommendation.GetNewVersion(p => p._id == id).Result.GetViewCrud();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewListRecommendation> List(ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        List<ViewListRecommendation> detail = serviceRecommendation.GetAllNewVersion(p => p.Name.ToUpper().Contains(filter.ToUpper()), count, count * (page - 1), "Name").Result
          .Select(x => new ViewListRecommendation()
          {
            _id = x._id,
            Name = x.Name
          }).ToList();
        total = serviceRecommendation.CountNewVersion(p => p.Name.ToUpper().Contains(filter.ToUpper())).Result;
        return detail;
      }
      catch (Exception e)
      {
        throw e;
      }
    }


    #endregion

    #region RecommendationPerson
    public string RemoveRecommendationPerson(string id)
    {
      try
      {
        RecommendationPerson item = serviceRecommendationPerson.GetNewVersion(p => p._id == id).Result;
        item.Status = EnumStatus.Disabled;
        serviceRecommendationPerson.Update(item, null).Wait();
        return "RecommendationPerson deleted!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public string NewRecommendationPerson(ViewCrudRecommendationPerson view)
    {
      try
      {
        Recommendation recommendation = serviceRecommendation.GetNewVersion(p => p._id == view.Recommendation._id).Result;
        RecommendationPerson recommendationperson = serviceRecommendationPerson.InsertNewVersion(
          new RecommendationPerson()
          {
            _id = view._id,
            Recommendation = recommendation.GetViewList(),
            Content = view.Content,
            Person = view.Person,
            Comments = view.Comments
          }).Result;
        return "RecommendationPerson added!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public string UpdateRecommendationPerson(ViewCrudRecommendationPerson view)
    {
      try
      {
        RecommendationPerson recommendationperson = serviceRecommendationPerson.GetNewVersion(p => p._id == view._id).Result;
        recommendationperson.Recommendation = view.Recommendation;
        recommendationperson.Content = view.Content;
        recommendationperson.Person = view.Person;
        recommendationperson.Comments = view.Comments;
        
        recommendationperson.Recommendation = view.Recommendation;
        serviceRecommendationPerson.Update(recommendationperson, null).Wait();
        return "RecommendationPerson altered!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public ViewCrudRecommendationPerson GetRecommendationPerson(string id)
    {
      try
      {
        return serviceRecommendationPerson.GetNewVersion(p => p._id == id).Result.GetViewCrud();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewListRecommendationPerson> ListRecommendationPerson(string idrecommendation, ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        List<ViewListRecommendationPerson> detail = serviceRecommendationPerson.GetAllNewVersion(p => p.Recommendation._id == idrecommendation && p.Person.Name.ToUpper().Contains(filter.ToUpper()), count, count * (page - 1), "Person.Name").Result
          .Select(p => new ViewListRecommendationPerson
          {
            _id = p._id,
            Image = p.Recommendation.Image,
            NamePerson = p.Person.Name,
            _idPerson = p.Person._id,
            NameRecommendation = p.Recommendation.Name,
            _idRecommendation = p.Recommendation._id
          }).ToList();
        total = serviceRecommendationPerson.CountNewVersion(p => p.Recommendation._id == idrecommendation && p.Person.Name.ToUpper().Contains(filter.ToUpper())).Result;
        return detail;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public List<ViewListRecommendationPerson> ListRecommendationPerson(ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        List<ViewListRecommendationPerson> detail = serviceRecommendationPerson.GetAllNewVersion(p => p.Person.Name.ToUpper().Contains(filter.ToUpper()), count, count * (page - 1), "Person.Name").Result
          .Select(p => new ViewListRecommendationPerson
          {
            _id = p._id,
            Image = p.Recommendation.Image,
            NamePerson = p.Person.Name,
            _idPerson = p.Person._id,
            NameRecommendation = p.Recommendation.Name,
            _idRecommendation = p.Recommendation._id
          }).ToList();
        total = serviceRecommendationPerson.CountNewVersion(p => p.Person.Name.ToUpper().Contains(filter.ToUpper())).Result;
        return detail;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    #endregion

  }
}
