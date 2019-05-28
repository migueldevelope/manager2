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
  public class ServiceMeritocracy : Repository<Meritocracy>, IServiceMeritocracy
  {
    private readonly ServiceGeneric<Meritocracy> serviceMeritocracy;
    private readonly ServiceGeneric<MeritocracyScore> serviceMeritocracyScore;
    private readonly ServiceGeneric<Person> servicePerson;
    private readonly ServiceGeneric<SalaryScale> serviceSalaryScale;
    private readonly ServiceGeneric<SalaryScaleScore> serviceSalaryScaleScore;

    #region Constructor
    public ServiceMeritocracy(DataContext context) : base(context)
    {
      try
      {
        serviceMeritocracy = new ServiceGeneric<Meritocracy>(context);
        serviceMeritocracyScore = new ServiceGeneric<MeritocracyScore>(context);
        servicePerson = new ServiceGeneric<Person>(context);
        serviceSalaryScale = new ServiceGeneric<SalaryScale>(context);
        serviceSalaryScaleScore = new ServiceGeneric<SalaryScaleScore>(context);
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public void SetUser(IHttpContextAccessor contextAccessor)
    {
      User(contextAccessor);
      serviceMeritocracy._user = _user;
      serviceMeritocracyScore._user = _user;
      servicePerson._user = _user;
      serviceSalaryScale._user = _user;
      serviceSalaryScaleScore._user = _user;
    }
    public void SetUser(BaseUser user)
    {
      _user = user;
      serviceMeritocracy._user = user;
      serviceMeritocracyScore._user = user;
      servicePerson._user = user;
      serviceSalaryScale._user = user;
      serviceSalaryScaleScore._user = user;
    }
    #endregion

    #region Meritocracy
    public string Delete(string id)
    {
      try
      {
        Meritocracy item = serviceMeritocracy.GetNewVersion(p => p._id == id).Result;
        item.Status = EnumStatus.Disabled;
        serviceMeritocracy.Update(item, null);
        return "Meritocracy deleted!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public string New(ViewCrudMeritocracy view)
    {
      try
      {
        ViewListPersonMeritocracy person = null;

        Meritocracy meritocracy = serviceMeritocracy.InsertNewVersion(new Meritocracy()
        {
          _id = view._id,
          ActivitiesExcellence = 0,
          Maturity = 0,
          DateBegin = DateTime.Now,
          StatusMeritocracy = EnumStatusMeritocracy.Wait,
          Person = person,
          Status = EnumStatus.Enabled
        }).Result;
        return "Meritocracy added!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public string Update(ViewCrudMeritocracy view)
    {
      try
      {
        Meritocracy meritocracy = serviceMeritocracy.GetNewVersion(p => p._id == view._id).Result;
        
        meritocracy.ActivitiesExcellence = 0;
        meritocracy.StatusMeritocracy = view.StatusMeritocracy;
        if (meritocracy.StatusMeritocracy == EnumStatusMeritocracy.End)
          meritocracy.DateEnd = DateTime.Now;

        serviceMeritocracy.Update(meritocracy, null);

        return "Meritocracy altered!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string UpdateCompanyDate(ViewCrudMeritocracyDate view, string id)
    {
      try
      {
        Meritocracy meritocracy = serviceMeritocracy.GetNewVersion(p => p._id == id).Result;
        meritocracy.Person.CompanyDate = view.Date;
        serviceMeritocracy.Update(meritocracy, null);

        return "Meritocracy altered!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string UpdateOccupationDate(ViewCrudMeritocracyDate view, string id)
    {
      try
      {
        Meritocracy meritocracy = serviceMeritocracy.GetNewVersion(p => p._id == id).Result;
        meritocracy.Person.OccupationDate = view.Date;
        serviceMeritocracy.Update(meritocracy, null);

        return "Meritocracy altered!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string UpdateOccupationActivitiesExcellence(ViewCrudMeritocracyWeight view, string id)
    {
      try
      {
        Meritocracy meritocracy = serviceMeritocracy.GetNewVersion(p => p._id == id).Result;
        meritocracy.ActivitiesExcellence = view.Weight;
        serviceMeritocracy.Update(meritocracy, null);

        return "Meritocracy altered!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string UpdateOccupationMaturity(ViewCrudMeritocracyWeight view, string id)
    {
      try
      {
        Meritocracy meritocracy = serviceMeritocracy.GetNewVersion(p => p._id == id).Result;
        meritocracy.Maturity = view.Weight;
        serviceMeritocracy.Update(meritocracy, null);

        return "Meritocracy altered!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public ViewCrudMeritocracy Get(string id)
    {
      try
      {
        Meritocracy meritocracy = serviceMeritocracy.GetNewVersion(p => p._id == id).Result;
        return new ViewCrudMeritocracy()
        {
          _id = meritocracy._id,
          ActivitiesExcellence = meritocracy.ActivitiesExcellence,
          Maturity = meritocracy.Maturity,
          Person = meritocracy.Person
        };
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public List<ViewListMeritocracy> List(ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        List<ViewListMeritocracy> detail = serviceMeritocracy.GetAllNewVersion(p => p.Person.Name.ToUpper().Contains(filter.ToUpper()), count, count * (page - 1), "Name").Result
          .Select(x => new ViewListMeritocracy()
          {
            _id = x._id,
            Name = x.Person.Name
          }).ToList();
        total = serviceMeritocracy.CountNewVersion(p => p.Person.Name.ToUpper().Contains(filter.ToUpper())).Result;
        return detail;
      }
      catch (Exception e)
      {
        throw e;
      }
    }


    //public List<ViewListMeritocracy> ListWaitManager(string idmanager, ref long total, string filter, int count, int page)
    //{
    //  try
    //  {
    //    List<ViewListMeritocracy> list = servicePerson.GetAllNewVersion(p => p.StatusUser != EnumStatusUser.Disabled && p.StatusUser != EnumStatusUser.ErrorIntegration &&
    //                                    p.TypeUser != EnumTypeUser.Administrator &&
    //                                    p.TypeJourney == EnumTypeJourney.Meritocracy &&
    //                                    p.Manager._id == idmanager &&
    //                                    p.User.Name.ToUpper().Contains(filter.ToUpper()), count, count * (page - 1), "User.Name").Result
    //                                    .Select(p => new ViewListMeritocracy()
    //                                    {
    //                                      _id = string.Empty,
    //                                      _idPerson = p._id,
    //                                      Name = p.User.Name,
    //                                      OccupationName = p.Occupation.Name,
    //                                      StatusMeritocracy = EnumStatusMeritocracy.Open,
    //                                      TypeMeritocracy = EnumMeritocracy.None
    //                                    }).ToList();
    //    List<ViewListMeritocracy> detail = new List<ViewListMeritocracy>();
    //    if (serviceMeritocracy.Exists("Meritocracy"))
    //    {
    //      Meritocracy meritocracy;
    //      foreach (var item in list)
    //      {
    //        meritocracy = serviceMeritocracy.GetNewVersion(x => x.Person._id == item._idPerson && x.StatusMeritocracy != EnumStatusMeritocracy.End).Result;
    //        if (meritocracy != null)
    //        {
    //          item._id = meritocracy._id;
    //          item.StatusMeritocracy = meritocracy.StatusMeritocracy;
    //        }
    //        else
    //          meritocracy = serviceMeritocracy.GetNewVersion(x => x.Person._id == item._idPerson && x.StatusMeritocracy == EnumStatusMeritocracy.End).Result;


    //        if (meritocracy?.TypeMeritocracy != EnumMeritocracy.Disapproved)
    //          detail.Add(item);

    //      }
    //    }
    //    else
    //      detail = list;

    //    total = servicePerson.CountNewVersion(p => p.StatusUser != EnumStatusUser.Disabled && p.StatusUser != EnumStatusUser.ErrorIntegration &&
    //                            p.TypeUser != EnumTypeUser.Administrator &&
    //                            p.Manager._id == idmanager &&
    //                            p.User.Name.ToUpper().Contains(filter.ToUpper())).Result;
    //    return detail;
    //  }
    //  catch (Exception e)
    //  {
    //    throw e;
    //  }
    //}

    #endregion

    #region MeritocracyScore
    public string RemoveMeritocracyScore(string id)
    {
      try
      {
        MeritocracyScore item = serviceMeritocracyScore.GetNewVersion(p => p._id == id).Result;
        item.Status = EnumStatus.Disabled;
        serviceMeritocracyScore.Update(item, null);
        return "MeritocracyScore deleted!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public string NewMeritocracyScore(ViewCrudMeritocracyScore view)
    {
      try
      {
        MeritocracyScore meritocracyScore = serviceMeritocracyScore.InsertNewVersion(
          new MeritocracyScore()
          {
            _id = view._id,
            Name = view.Name,
            Weight = view.Weight,
            Enabled = view.Enabled
          }).Result;
        return "MeritocracyScore added!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public string UpdateMeritocracyScore(ViewCrudMeritocracyScore view)
    {
      try
      {
        MeritocracyScore meritocracyScore = serviceMeritocracyScore.GetNewVersion(p => p._id == view._id).Result;
        meritocracyScore.Name = view.Name;
        meritocracyScore.Enabled = view.Enabled;
        meritocracyScore.Weight = view.Weight;
        serviceMeritocracyScore.Update(meritocracyScore, null);
        return "MeritocracyScore altered!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public ViewCrudMeritocracyScore GetMeritocracyScore(string id)
    {
      try
      {
        MeritocracyScore meritocracyScore = serviceMeritocracyScore.GetNewVersion(p => p._id == id).Result;
        return new ViewCrudMeritocracyScore()
        {
          _id = meritocracyScore._id,
          Name = meritocracyScore.Name,
          Enabled = meritocracyScore.Enabled,
          Weight = meritocracyScore.Weight
        };
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public List<ViewListMeritocracyScore> ListMeritocracyScore(ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        List<ViewListMeritocracyScore> detail = serviceMeritocracyScore.GetAllNewVersion(p => p.Name.ToUpper().Contains(filter.ToUpper()), count, count * (page - 1), "Name").Result
          .Select(p => new ViewListMeritocracyScore
          {
            _id = p._id,
            Name = p.Name
          }).ToList();
        total = serviceMeritocracyScore.CountNewVersion(p => p.Name.ToUpper().Contains(filter.ToUpper())).Result;
        return detail;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    #endregion

    #region SalaryScaleScore
    public string DeleteSalaryScaleScore(string id)
    {
      try
      {
        SalaryScaleScore item = serviceSalaryScaleScore.GetNewVersion(p => p._id == id).Result;
        item.Status = EnumStatus.Disabled;
        serviceSalaryScaleScore.Update(item, null);
        return "SalaryScaleScore deleted!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public string NewSalaryScaleScore(ViewCrudSalaryScaleScore view)
    {
      try
      {
        SalaryScaleScore salaryScaleScore = serviceSalaryScaleScore.InsertNewVersion(
          new SalaryScaleScore()
          {
            _id = view._id,
            Ranking = view.Ranking,
            Step = view.Step,
            Value = view.Value
          }).Result;
        return "SalaryScaleScore added!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public string UpdateSalaryScaleScore(ViewCrudSalaryScaleScore view)
    {
      try
      {
        SalaryScaleScore salaryScaleScore = serviceSalaryScaleScore.GetNewVersion(p => p._id == view._id).Result;
        salaryScaleScore.Ranking = view.Ranking;
        salaryScaleScore.Step = view.Step;
        salaryScaleScore.Value = view.Value;
        serviceSalaryScaleScore.Update(salaryScaleScore, null);
        return "SalaryScaleScore altered!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public ViewCrudSalaryScaleScore GetSalaryScaleScore(string id)
    {
      try
      {
        SalaryScaleScore salaryScaleScore = serviceSalaryScaleScore.GetNewVersion(p => p._id == id).Result;
        return new ViewCrudSalaryScaleScore()
        {
          _id = salaryScaleScore._id,
          Ranking = salaryScaleScore.Ranking,
          Step = salaryScaleScore.Step,
          Value = salaryScaleScore.Value
        };
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public List<ViewCrudSalaryScaleScore> ListSalaryScaleScore(ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        List<ViewCrudSalaryScaleScore> detail = serviceSalaryScaleScore.GetAllFreeNewVersion(p => p.Status == EnumStatus.Enabled, count, count * (page - 1), "_id").Result
          .Select(p => new ViewCrudSalaryScaleScore
          {
            _id = p._id,
            Ranking = p.Ranking,
            Step = p.Step,
            Value = p.Value,
          }).ToList();
        total = serviceSalaryScaleScore.CountNewVersion(p => p.Status == EnumStatus.Enabled).Result;
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
