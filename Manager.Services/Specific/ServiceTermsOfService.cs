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
  public class ServiceTermsOfService : Repository<TermsOfService>, IServiceTermsOfService
  {
    private readonly ServiceGeneric<TermsOfService> serviceTermsOfService;
    private readonly ServiceGeneric<Establishment> serviceEstablishment;
    private readonly ServiceGeneric<Person> servicePerson;
    private readonly ServiceGeneric<SalaryScale> serviceSalaryScale;

    #region Constructor
    public ServiceTermsOfService(DataContext context) : base(context)
    {
      try
      {
        serviceTermsOfService = new ServiceGeneric<TermsOfService>(context);
        serviceEstablishment = new ServiceGeneric<Establishment>(context);
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
      serviceTermsOfService._user = _user;
      serviceEstablishment._user = _user;
      servicePerson._user = _user;
      serviceSalaryScale._user = _user;
    }
    public void SetUser(BaseUser user)
    {
      _user = user;
      serviceTermsOfService._user = user;
      serviceEstablishment._user = user;
      servicePerson._user = user;
      serviceSalaryScale._user = user;
    }
    #endregion

    #region TermsOfService
    public string Delete(string id)
    {
      try
      {
        TermsOfService item = serviceTermsOfService.GetFreeNewVersion(p => p._id == id).Result;
        item.Status = EnumStatus.Disabled;
        serviceTermsOfService.UpdateAccount(item, null);
        return "TermsOfService deleted!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string New(ViewCrudTermsOfService view)
    {
      try
      {
        TermsOfService termsofservice = serviceTermsOfService.InsertFreeNewVersion(new TermsOfService()
        {
          _id = view._id,
          Text = view.Text,
          Date = view.Date
        }).Result;
        return "TermsOfService added!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public string Update(ViewCrudTermsOfService view)
    {
      try
      {
        TermsOfService termsofservice = serviceTermsOfService.GetFreeNewVersion(p => p._id == view._id).Result;

        termsofservice.Text = view.Text;
        termsofservice.Date = view.Date;
        serviceTermsOfService.UpdateAccount(termsofservice, null);

        return "TermsOfService altered!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public ViewCrudTermsOfService Get(string id)
    {
      try
      {
        TermsOfService termsofservice = serviceTermsOfService.GetFreeNewVersion(p => p._id == id).Result;
        return new ViewCrudTermsOfService()
        {
          _id = termsofservice._id,
          Text = termsofservice.Text,
          Date = termsofservice.Date
        };
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public ViewListTermsOfService GetTerm()
    {
      try
      {
        var date = serviceTermsOfService.GetAllFreeNewVersion(p => p.Status == EnumStatus.Enabled).Result.Max(p => p.Date);
        TermsOfService termsofservice = serviceTermsOfService.GetAllFreeNewVersion(p => p.Date == date).Result.FirstOrDefault();
        return new ViewListTermsOfService()
        {
          Text = termsofservice.Text,
          Date = termsofservice.Date,
          _id = termsofservice._id
        };

      }
      catch (Exception e)
      {
        return null;
      }
    }
    public List<ViewListTermsOfService> List(ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        List<ViewListTermsOfService> detail = serviceTermsOfService.GetAllFreeNewVersion(p => p.Text.ToUpper().Contains(filter.ToUpper()), count, count * (page - 1), "Text").Result
          .Select(x => new ViewListTermsOfService()
          {
            _id = x._id,
            Text = x.Text,
            Date = x.Date
          }).ToList();
        total = serviceTermsOfService.CountFreeNewVersion(p => p.Text.ToUpper().Contains(filter.ToUpper())).Result;
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
