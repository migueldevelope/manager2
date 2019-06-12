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
  public class ServiceMaturity : Repository<Maturity>, IServiceMaturity
  {
    private readonly ServiceGeneric<Maturity> serviceMaturity;
    private readonly ServiceGeneric<MaturityRegister> serviceMaturityRegister;
    private readonly ServiceGeneric<Person> servicePerson;
    private readonly ServiceGeneric<SalaryScale> serviceSalaryScale;

    #region private


    public void MathMonth()
    {
      try
      {
        var list = serviceMaturity.GetAllFreeNewVersion(p => p.Status == EnumStatus.Enabled).Result.ToList();

        foreach (var item in list)
           MathMaturity(item);

      }
      catch (Exception e)
      {
        throw e;
      }
    }
    private void MathMaturity(Maturity maturity)
    {
      try
      {
        maturity.LevelMonitoring = (maturity.CountMonitoring >= 4) ? byte.Parse("5") : byte.Parse((maturity.CountMonitoring + 1).ToString());
        maturity.LevelCertification = (maturity.CountCertification >= 4) ? byte.Parse("5") : byte.Parse((maturity.CountCertification + 1).ToString());
        maturity.LevelPraise = (maturity.CountPraise >= 4) ? byte.Parse("5") : byte.Parse((maturity.CountPraise + 1).ToString());
        maturity.LevelPlan = (maturity.CountPlan <= 1) ? byte.Parse("1") : byte.Parse(maturity.CountPlan.ToString());

        maturity.Value = byte.Parse(Math.Round(decimal.Parse(((maturity.LevelMonitoring + maturity.LevelCertification + maturity.LevelPlan + maturity.LevelPraise) / 4).ToString()), 0).ToString());

         serviceMaturity.Update(maturity, null).Wait();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    #endregion

    #region Constructor
    public ServiceMaturity(DataContext context) : base(context)
    {
      try
      {
        serviceMaturity = new ServiceGeneric<Maturity>(context);
        serviceMaturityRegister = new ServiceGeneric<MaturityRegister>(context);
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
      serviceMaturity._user = _user;
      serviceMaturityRegister._user = _user;
      servicePerson._user = _user;
      serviceSalaryScale._user = _user;
    }
    public void SetUser(BaseUser user)
    {
      _user = user;
      serviceMaturity._user = user;
      serviceMaturityRegister._user = user;
      servicePerson._user = user;
      serviceSalaryScale._user = user;
    }
    #endregion

    #region Maturity

    public  string Delete(string id)
    {
      try
      {
        Maturity item = serviceMaturity.GetNewVersion(p => p._id == id).Result;
        item.Status = EnumStatus.Disabled;
         serviceMaturity.Update(item, null).Wait();
        return "Maturity deleted!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public  string New(ViewCrudMaturity view)
    {
      try
      {
        Maturity maturity =  serviceMaturity.InsertNewVersion(new Maturity()
        {
          _id = view._id,
          _idPerson = view._idPerson,
          CountMonitoring = view.CountMonitoring,
          CountPlan = view.CountPlan,
          CountPraise = view.CountPraise,
          CountCertification = view.CountCertification,
          LevelMonitoring = view.LevelMonitoring,
          LevelPlan = view.LevelPlan,
          LevelPraise = view.LevelPraise,
          LevelCertification = view.LevelCertification
        }).Result;
        return "Maturity added!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public  string Update(ViewCrudMaturity view)
    {
      try
      {
        Maturity maturity = serviceMaturity.GetNewVersion(p => p._id == view._id).Result;
        if (!maturity._idPerson.ToLower().Equals(view._idPerson.ToLower()))
          maturity._idPerson = view._idPerson;
        maturity.CountMonitoring = view.CountMonitoring;
        maturity.CountPlan = view.CountPlan;
        maturity.CountPraise = view.CountPraise;
        maturity.CountCertification = view.CountCertification;
        maturity.LevelMonitoring = view.LevelMonitoring;
        maturity.LevelPlan = view.LevelPlan;
        maturity.LevelPraise = view.LevelPraise;
        maturity.LevelCertification = view.LevelCertification;

         serviceMaturity.Update(maturity, null).Wait();


        return "Maturity altered!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public  ViewCrudMaturity Get(string id)
    {
      try
      {
        Maturity maturity =  serviceMaturity.GetNewVersion(p => p._id == id).Result;
        return new ViewCrudMaturity()
        {
          _id = maturity._id,
          _idPerson = maturity._idPerson,
          CountMonitoring = maturity.CountMonitoring,
          CountPlan = maturity.CountPlan,
          CountPraise = maturity.CountPraise,
          CountCertification = maturity.CountCertification,
          LevelMonitoring = maturity.LevelMonitoring,
          LevelPlan = maturity.LevelPlan,
          LevelPraise = maturity.LevelPraise,
          LevelCertification = maturity.LevelCertification,
          Value = maturity.Value
        };
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewCrudMaturity> List( ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        List<ViewCrudMaturity> detail = serviceMaturity.GetAllNewVersion(p => p._idPerson.ToUpper().Contains(filter.ToUpper()), count, count * (page - 1), "_idPerson").Result
          .Select(x => new ViewCrudMaturity()
          {
            _id = x._id,
            _idPerson = x._idPerson,
            CountMonitoring = x.CountMonitoring,
            CountPlan = x.CountPlan,
            CountPraise = x.CountPraise,
            CountCertification = x.CountCertification,
            LevelMonitoring = x.LevelMonitoring,
            LevelPlan = x.LevelPlan,
            LevelPraise = x.LevelPraise,
            LevelCertification = x.LevelCertification
          }).ToList();
        total = serviceMaturity.CountNewVersion(p => p._idPerson.ToUpper().Contains(filter.ToUpper())).Result;
        return detail;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    #endregion

    #region MaturityRegister
    public  string DeleteMaturityRegister(string id)
    {
      try
      {
        MaturityRegister item = serviceMaturityRegister.GetNewVersion(p => p._id == id).Result;
        item.Status = EnumStatus.Disabled;
         serviceMaturityRegister.Update(item, null).Wait();
        return "MaturityRegister deleted!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public  string NewMaturityRegister(ViewCrudMaturityRegister view)
    {
      try
      {
        var oneyearbefore = DateTime.Now.AddYears(-1);
        _user = new BaseUser() { _idAccount = view._idAccount };
        serviceMaturityRegister._user = _user;
        serviceMaturity._user = _user;

        MaturityRegister maturityregister = serviceMaturityRegister.InsertNewVersion(
          new MaturityRegister()
          {
            _id = view._id,
            _idPerson = view._idPerson,
            TypeMaturity = view.TypeMaturity,
            Date = view.Date,
            _idRegister = view._idRegister,
            Evaluation = view.Evaluation
          }).Result;

        var maturity = serviceMaturity.GetAllNewVersion(p => p._idPerson == view._idPerson).Result.FirstOrDefault();
        if (maturity == null)
           New(new ViewCrudMaturity()
          {
            _idPerson = view._idPerson
          });

        if (view.TypeMaturity == EnumTypeMaturity.Monitoring)
        {
          var monitorings = serviceMaturityRegister.CountNewVersion(p => p._idPerson == view._idPerson
          && p.TypeMaturity == EnumTypeMaturity.Monitoring && p.Date >= oneyearbefore).Result;
          maturity.CountMonitoring = monitorings;
        }
        else if (view.TypeMaturity == EnumTypeMaturity.Certification)
        {
          var certifications = serviceMaturityRegister.CountNewVersion(p => p._idPerson == view._idPerson
          && p.TypeMaturity == EnumTypeMaturity.Certification && p.Date >= oneyearbefore).Result;
          maturity.CountCertification = certifications;
        }
        else if (view.TypeMaturity == EnumTypeMaturity.Plan)
        {
          var plans = serviceMaturityRegister.GetAllNewVersion(p => p._idPerson == view._idPerson
          && p.TypeMaturity == EnumTypeMaturity.Plan && p.Date >= oneyearbefore).Result.Average(p => p.Evaluation);
          maturity.CountPlan = long.Parse(plans.ToString());
          
        }
        else if (view.TypeMaturity == EnumTypeMaturity.Praise)
        {
          var praises = serviceMaturityRegister.CountNewVersion(p => p._idPerson == view._idPerson
          && p.TypeMaturity == EnumTypeMaturity.Praise && p.Date >= oneyearbefore).Result;
          maturity.CountPraise = praises;
        }

         serviceMaturity.Update(maturity, null).Wait();

         MathMaturity(maturity);

        return "MaturityRegister added!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public  string UpdateMaturityRegister(ViewCrudMaturityRegister view)
    {
      try
      {
        MaturityRegister maturityregister = serviceMaturityRegister.GetNewVersion(p => p._id == view._id).Result;
        maturityregister._idPerson = view._idPerson;
        maturityregister.TypeMaturity = view.TypeMaturity;
        maturityregister.Date = view.Date;
        maturityregister._idRegister = view._idRegister;
         serviceMaturityRegister.Update(maturityregister, null).Wait();
        return "MaturityRegister altered!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public  ViewCrudMaturityRegister GetMaturityRegister(string id)
    {
      try
      {
        MaturityRegister maturityregister =  serviceMaturityRegister.GetNewVersion(p => p._id == id).Result;
        return new ViewCrudMaturityRegister()
        {
          _id = maturityregister._id,
          _idPerson = maturityregister._idPerson,
          TypeMaturity = maturityregister.TypeMaturity,
          Date = maturityregister.Date,
          _idRegister = maturityregister._idRegister
        };
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public List<ViewCrudMaturityRegister> ListMaturityRegister( ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        List<ViewCrudMaturityRegister> detail = serviceMaturityRegister.GetAllNewVersion(p => p._idPerson.ToUpper().Contains(filter.ToUpper()), count, count * (page - 1), "_idPerson").Result
          .Select(p => new ViewCrudMaturityRegister
          {
            _id = p._id,
            _idPerson = p._idPerson,
            TypeMaturity = p.TypeMaturity,
            Date = p.Date,
            _idRegister = p._idRegister
          }).ToList();
        total = serviceMaturityRegister.CountNewVersion(p => p._idPerson.ToUpper().Contains(filter.ToUpper())).Result;
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
