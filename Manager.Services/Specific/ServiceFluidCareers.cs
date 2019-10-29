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
  public class ServiceFluidCareers : Repository<FluidCareers>, IServiceFluidCareers
  {
    private readonly ServiceGeneric<FluidCareers> serviceFluidCareers;
    private readonly ServiceGeneric<Occupation> serviceOccupation;
    private readonly ServiceGeneric<Person> servicePerson;
    private readonly ServiceGeneric<Sphere> serviceSphere;
    private readonly ServiceGeneric<Group> serviceGroup;
    private readonly ServiceGeneric<Skill> serviceSkill;

    #region Constructor
    public ServiceFluidCareers(DataContext context) : base(context)
    {
      try
      {
        serviceFluidCareers = new ServiceGeneric<FluidCareers>(context);
        serviceOccupation = new ServiceGeneric<Occupation>(context);
        servicePerson = new ServiceGeneric<Person>(context);
        serviceSphere = new ServiceGeneric<Sphere>(context);
        serviceGroup = new ServiceGeneric<Group>(context);
        serviceSkill = new ServiceGeneric<Skill>(context);
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public void SetUser(IHttpContextAccessor contextAccessor)
    {
      User(contextAccessor);
      serviceFluidCareers._user = _user;
      serviceOccupation._user = _user;
      servicePerson._user = _user;
      serviceGroup._user = _user;
      serviceSkill._user = _user;
      serviceSphere._user = _user;
    }
    public void SetUser(BaseUser user)
    {
      _user = user;
      serviceFluidCareers._user = user;
      serviceOccupation._user = user;
      servicePerson._user = user;
      serviceGroup._user = user;
      serviceSkill._user = user;
      serviceSphere._user = user;
    }
    #endregion

    #region FluidCareers
    public string Delete(string id)
    {
      try
      {
        FluidCareers item = serviceFluidCareers.GetNewVersion(p => p._id == id).Result;
        item.Status = EnumStatus.Disabled;
        serviceFluidCareers.Update(item, null).Wait();
        return "FluidCareers deleted!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public string New(ViewCrudFluidCareers view)
    {
      try
      {
        FluidCareers fluidcareers = serviceFluidCareers.InsertNewVersion(new FluidCareers()
        {
          _id = view._id
        }).Result;
        return "FluidCareers added!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public string Update(ViewCrudFluidCareers view)
    {
      try
      {
        FluidCareers fluidcareers = serviceFluidCareers.GetNewVersion(p => p._id == view._id).Result;

        serviceFluidCareers.Update(fluidcareers, null).Wait();

        return "FluidCareers altered!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public ViewCrudFluidCareers Get(string id)
    {
      try
      {
        return null;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public List<ViewListFluidCareers> List(ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        List<ViewListFluidCareers> detail = serviceFluidCareers.GetAllNewVersion(p => p.Person.Name.ToUpper().Contains(filter.ToUpper()), count, count * (page - 1), "Person.Name").Result
          .Select(x => new ViewListFluidCareers()
          {
            _id = x._id
          }).ToList();
        total = serviceFluidCareers.CountNewVersion(p => p.Person.Name.ToUpper().Contains(filter.ToUpper())).Result;
        return detail;
      }
      catch (Exception e)
      {
        throw e;
      }
    }


    public ViewFluidCareers Calc(string idperson, List<ViewCrudSkillsCareers> skills)
    {
      try
      {
        var person = servicePerson.GetNewVersion(p => p._id == idperson).Result;
        var spheres = serviceSphere.GetAllNewVersion(p => p.Company._id == person.Company._id).Result;
        var occupations = serviceOccupation.GetAllNewVersion(p => p.Group.Company._id == person.Company._id).Result;
        var groups = serviceGroup.GetAllNewVersion(p => p.Company._id == person.Company._id).Result;

        var view = new ViewFluidCareers();
        view.Sphere = new List<ViewFluidCareersSphere>();

        foreach (var sphere in spheres)
        {
          var viewSphere = new ViewFluidCareersSphere();
          viewSphere._id = sphere._id;
          viewSphere.Name = sphere.Name;
          viewSphere.Group = new List<ViewFluidCareersGroup>();
          foreach (var group in groups.Where(p => p.Sphere._id == sphere._id))
          {
            var viewGroup = new ViewFluidCareersGroup();
            viewGroup._id = group._id;
            viewGroup.Name = group.Name;
            viewGroup.Occupation = new List<ViewCrudOccupationCareers>();
            foreach (var occupation in occupations.Where(p => p.Group._id == group._id))
            {
              var viewOccupation = new ViewCrudOccupationCareers();
              viewOccupation._id = occupation._id;
              viewOccupation.Name = occupation.Name;
              var skillsOccupation = occupation.Skills;
              var skillsGroup = group.Skills.ToList();
              if (skillsOccupation != null && skillsGroup != null && skills != null)
              {
                var total = 0;
                foreach (var item in skills)
                {
                  if ((skillsOccupation.Where(p => p._id == item._id).Count() > 0) || (skillsGroup.Where(p => p._id == item._id).Count() > 0))
                  {
                    total += item.Order;
                  }
                }
                var accuracy = (total * 100) / 55;
                viewOccupation.Accuracy = accuracy;
              }
              viewOccupation.Color = EnumOccupationColor.None;
              if ((viewOccupation.Accuracy >= decimal.Parse("33.33")) && (viewOccupation.Accuracy < decimal.Parse("66.66")))
                viewOccupation.Color = EnumOccupationColor.Yellow;
              else if (viewOccupation.Accuracy >= decimal.Parse("66.66"))
                viewOccupation.Color = EnumOccupationColor.Orange;

              viewGroup.Occupation.Add(viewOccupation);
            }
            if (viewGroup.Occupation.Count > 0)
              viewSphere.Group.Add(viewGroup);
          }
          view.Sphere.Add(viewSphere);
        }

        return view;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    #endregion

    #region private

    #endregion


  }
}
