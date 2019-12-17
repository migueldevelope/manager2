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
    private readonly ServiceGeneric<Company> serviceCompany;

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
        serviceCompany = new ServiceGeneric<Company>(context);
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
      serviceCompany._user = _user;
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
      serviceCompany._user = user;
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
        var person = servicePerson.GetNewVersion(p => p._id == view._idPerson).Result;
        FluidCareers fluidcareers = serviceFluidCareers.InsertNewVersion(new FluidCareers()
        {
          _id = view._id,
          Person = person.GetViewListPersonInfo(),
          Date = DateTime.Now,
          FluidCareersView = view.FluidCareersView
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

    public List<ViewCrudSkillsCareers> GetSkills(byte type, ref long total, string filter, int count, int page)
    {
      try
      {
        int skip = (count * (page - 1));
        var myskills = new List<string>();

        var typeskill = (EnumTypeSkill)type;
        var person = servicePerson.GetNewVersion(p => p._id == _user._idPerson).Result;

        var company = serviceCompany.GetNewVersion(p => p.Status == EnumStatus.Enabled).Result?.Skills.Select(p => p._id);
        var listoccupations = serviceOccupation.GetAllNewVersion(p => p.Status == EnumStatus.Enabled).Result.Select(p => p.Skills).ToList();
        var listgroups = serviceGroup.GetAllNewVersion(p => p.Status == EnumStatus.Enabled).Result.Select(p => p.Skills).ToList();
        var list = new List<ViewListSkill>();

        if (person.Occupation == null)
          return null;

        var occupatinperson = serviceOccupation.GetNewVersion(p => p._id == person.Occupation._id).Result.Skills.Select(p => p._id);
        var groupperson = serviceGroup.GetNewVersion(p => p._id == person.Occupation._idGroup).Result.Skills.Select(p => p._id);
        var companyperson = serviceCompany.GetNewVersion(p => p._id == person.Company._id).Result.Skills.Select(p => p._id);
        myskills = myskills.Concat(occupatinperson).Concat(companyperson).
          Concat(groupperson).ToList();

        //List<string> groups = new List<string>();
        foreach (var item in listgroups)
          foreach (var skill in item)
            list.Add(skill);

        List<string> occupations = new List<string>();
        foreach (var item in listoccupations)
          foreach (var skill in item)
            list.Add(skill);

        //total = serviceSkill.CountNewVersion(p => p.Name.ToUpper().Contains(filter.ToUpper())).Result;

        //if (type == 2)
        //list = serviceSkill.GetAllNewVersion(p => p.Name.ToUpper().Contains(filter.ToUpper())).Result;
        //else
        //list = serviceSkill.GetAllNewVersion(p => p.TypeSkill == typeskill
        //&& p.Name.ToUpper().Contains(filter.ToUpper())).Result;


        /*list = list.Where(p => company.Contains(p._id)|| groups.Contains(p.Name)
        || occupations.Contains(p._id)).ToList();*/

        var result = new List<ViewCrudSkillsCareers>();
        result = list.GroupBy(p => new { p._id, p.Name, p.TypeSkill, p.Concept }).Select(p => new ViewCrudSkillsCareers()
        {
          _id = p.Key._id,
          Name = p.Key.Name,
          TypeSkill = p.Key.TypeSkill,
          Concept = p.Key.Concept,
          Count = p.Count(),
          Order = 0
        }).ToList();

        if (type == 0)
          result = result.Where(p => p.TypeSkill == EnumTypeSkill.Soft).ToList();
        else if (type == 1)
          result = result.Where(p => p.TypeSkill == EnumTypeSkill.Hard).ToList();
        else if(type == 3)
          result = result.Where(p => myskills.Contains(p._id)).ToList();

        total = result.Count();

        if (type == 2)
          return result.Where(p => p.Name.ToUpper().Contains(filter.ToUpper())).Skip(skip).Take(count).OrderByDescending(p => p.Count).ThenBy(p => p.Name).ToList();
        else
          return result.Where(p => p.Name.ToUpper().Contains(filter.ToUpper())).Skip(skip).Take(count).OrderBy(p => p.Name).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }


    public ViewFluidCareerPerson Calc(string idperson, List<ViewCrudSkillsCareers> skills)
    {
      try
      {
        var person = servicePerson.GetNewVersion(p => p._id == idperson).Result;
        if (person.Occupation == null)
          return null;

        var occupationPerson = serviceOccupation.GetNewVersion(p => p._id == person.Occupation._id).Result;
        var company = serviceCompany.GetNewVersion(p => p._id == person.Company._id).Result;
        var spheres = serviceSphere.GetAllNewVersion(p => p.Company._id == person.Company._id).Result.OrderBy(p => p.TypeSphere);
        var occupations = serviceOccupation.GetAllNewVersion(p => p.Group.Company._id == person.Company._id).Result;
        var groups = serviceGroup.GetAllNewVersion(p => p.Company._id == person.Company._id).Result;

        var totalpoints = skills.Count() * 5;
        if (totalpoints == 0)
          totalpoints = 1;


        var fluidcareers = new List<ViewFluidCareers>();
        var view = new List<ViewFluidCareersSphere>();

        foreach (var sphere in spheres)
        {
          var viewSphere = new ViewFluidCareersSphere();
          viewSphere._id = sphere._id;
          viewSphere.Name = sphere.Name;
          viewSphere.Group = new List<ViewFluidCareersGroup>();
          foreach (var group in groups.Where(p => p.Sphere._id == sphere._id).OrderBy(p => p.Line))
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
              var skillsGroup = group.Skills;
              var skillsCompany = company.Skills;
              if (skillsOccupation != null && skillsGroup != null && skills != null)
              {
                var total = 0;
                foreach (var item in skills)
                {
                  if ((skillsOccupation.Where(p => p._id == item._id).Count() > 0) || (skillsGroup.Where(p => p.Name == item.Name).Count() > 0)
                    || skillsCompany.Where(p => p._id == item._id).Count() > 0)
                  {
                    total += item.Order;
                  }
                }
                var accuracy = (total * 100) / totalpoints;
                viewOccupation.Accuracy = accuracy;
              }
              viewOccupation.Color = EnumOccupationColor.None;
              if ((viewOccupation.Accuracy >= decimal.Parse("33.33")) && (viewOccupation.Accuracy < decimal.Parse("66.66")))
                viewOccupation.Color = EnumOccupationColor.Yellow;
              else if (viewOccupation.Accuracy >= decimal.Parse("66.66"))
                viewOccupation.Color = EnumOccupationColor.Orange;

              viewOccupation.Activities = occupation.Activities;
              viewOccupation.Scopes = group.Scope;

              

              if (occupationPerson.Group.Sphere.TypeSphere >= sphere.TypeSphere)
              {
                viewGroup.Occupation.Add(viewOccupation);
                fluidcareers.Add(new ViewFluidCareers()
                {
                  Occupation = viewOccupation.Name,
                  Accuracy = viewOccupation.Accuracy,
                  Color = viewOccupation.Color,
                  Group = viewGroup.Name,
                  Sphere = viewSphere.Name,
                  Order = 0
                });
              }
                
            }
            viewGroup.Occupation = viewGroup.Occupation.ToList();
            if (viewGroup.Occupation.Count > 0)
              viewSphere.Group.Add(viewGroup);
          }
          view.Add(viewSphere);
        }
        var result = new ViewFluidCareerPerson()
        {
          FluidCareerSphere = view,
          FluidCareer = fluidcareers.OrderByDescending(p => p.Accuracy).ToList()
        };

        return result;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public ViewFluidCareersPerson GetPerson(string idperson)
    {
      try
      {
        //total = servicePerson.CountNewVersion(p => p._id == idperson && p.User.Name.ToUpper().Contains(filter.ToUpper())).Result;
        var item = servicePerson.GetNewVersion(p => p._id == idperson).Result;
        var occupations = serviceOccupation.GetAllNewVersion(p => p.Status == EnumStatus.Enabled).Result;
        var groups = serviceGroup.GetAllNewVersion(p => p.Status == EnumStatus.Enabled).Result;
        var company = serviceCompany.GetAllNewVersion(p => p.Status == EnumStatus.Enabled).Result.FirstOrDefault();

        var view = new ViewFluidCareersPerson()
        {
          _id = item._id,
          Name = item.User.Name,
          Occupation = item.Occupation?.Name,
          Group = item.Occupation?.NameGroup
        };
        view.SkillsCompany = company.Skills;

        if (view.Occupation != null)
        {
          var occupation = occupations.Where(p => p._id == item.Occupation._id).FirstOrDefault();
          var group = groups.Where(p => p._id == occupation.Group._id).FirstOrDefault();
          view.SkillsGroup = group?.Skills;
          view.SkillsOccupation = occupation.Skills;
          view.Sphere = occupation.Group.Sphere.Name;
        }
        //list.Add(view);
        //}
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
