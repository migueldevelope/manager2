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
  public class ServiceNewsletter : Repository<Newsletter>, IServiceNewsletter
  {
    private readonly ServiceGeneric<Newsletter> serviceNewsletter;
    private readonly ServiceGeneric<NewsletterRead> serviceNewsletterRead;
    private readonly ServiceGeneric<Person> servicePerson;

    #region Constructor
    public ServiceNewsletter(DataContext context) : base(context)
    {
      try
      {
        serviceNewsletter = new ServiceGeneric<Newsletter>(context);
        serviceNewsletterRead = new ServiceGeneric<NewsletterRead>(context);
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
      serviceNewsletter._user = _user;
      servicePerson._user = _user;
    }
    public void SetUser(BaseUser user)
    {
      _user = user;
      serviceNewsletter._user = user;
      servicePerson._user = user;
    }
    #endregion

    #region Newsletter
    public string Delete(string id)
    {
      try
      {
        Newsletter item = serviceNewsletter.GetNewVersion(p => p._id == id).Result;
        item.Status = EnumStatus.Disabled;
        serviceNewsletter.Update(item, null).Wait();
        return "Newsletter deleted!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string New(ViewCrudNewsletter view)
    {
      try
      {
        Newsletter newsletter = serviceNewsletter.InsertNewVersion(new Newsletter()
        {
          _id = view._id,
          Title = view.Title,
          Description = view.Description,
          Enabled = view.Enabled,
          Infra = view.Infra,
          Employee = view.Employee,
          Manager = view.Manager,
          Included = DateTime.Now
        }).Result;


        return "Newsletter added!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public string Update(ViewCrudNewsletter view)
    {
      try
      {
        var model = serviceNewsletter.GetNewVersion(p => p._id == view._id).Result;

        model.Title = view.Title;
        model.Description = view.Description;
        model.Enabled = view.Enabled;
        model.Infra = view.Infra;
        model.Employee = view.Employee;
        model.Manager = view.Manager;

        serviceNewsletter.Update(model, null).Wait();


        return "Newsletter altered!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public ViewCrudNewsletter Get(string id)
    {
      try
      {
        return serviceNewsletter.GetNewVersion(p => p._id == id).Result.GetViewCrud();
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public List<ViewListNewsletter> List(ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        List<ViewListNewsletter> detail = serviceNewsletter.GetAllNewVersion(p => p.Title.ToUpper().Contains(filter.ToUpper()), count, count * (page - 1), "Title").Result
          .Select(x => x.GetViewList()).ToList();
        total = serviceNewsletter.CountNewVersion(p => p.Title.ToUpper().Contains(filter.ToUpper())).Result;
        return detail;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewListNewsletter> ListNewsletter(EnumPortal portal, ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        var detail = serviceNewsletter.GetAllFreeNewVersion(p => p.Status == EnumStatus.Enabled && p.Enabled == true && p.Title.ToUpper().Contains(filter.ToUpper()), count, count * (page - 1), "Title").Result;
        if (portal == EnumPortal.Infra)
          detail = detail.Where(p => p.Infra == true).ToList();
        if (portal == EnumPortal.Employee)
          detail = detail.Where(p => p.Employee == true).ToList();
        if (portal == EnumPortal.Manager)
          detail = detail.Where(p => p.Infra == true).ToList();

        total = serviceNewsletter.CountNewVersion(p => p.Title.ToUpper().Contains(filter.ToUpper())).Result;
        return detail.Select(x => x.GetViewList()).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    #endregion

    #region NewsletterRead
    public string DeleteNewsletterRead(string id)
    {
      try
      {
        var item = serviceNewsletterRead.GetNewVersion(p => p._id == id).Result;
        item.Status = EnumStatus.Disabled;
        serviceNewsletterRead.Update(item, null).Wait();
        return "Newsletter deleted!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string NewNewsletterRead(ViewCrudNewsletterRead view)
    {
      try
      {
        var newsletter = serviceNewsletterRead.InsertNewVersion(new NewsletterRead()
        {
          _id = view._id,
          _idNewsletter = view._idNewsletter,
          _idUser = view._idUser,
          ReadDate = view.ReadDate,
          DontShow = view.DontShow
        }).Result;


        return "Newsletter added!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public string UpdateNewsletterRead(ViewCrudNewsletterRead view)
    {
      try
      {
        var model = serviceNewsletterRead.GetNewVersion(p => p._id == view._id).Result;

        model._idNewsletter = view._idNewsletter;
        model._idUser = view._idUser;
        model.ReadDate = view.ReadDate;
        model.DontShow = view.DontShow;

        serviceNewsletterRead.Update(model, null).Wait();


        return "Newsletter altered!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public ViewCrudNewsletterRead GetNewsletterRead(string id)
    {
      try
      {
        return serviceNewsletterRead.GetNewVersion(p => p._id == id).Result.GetViewCrud();
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public List<ViewListNewsletterRead> ListNewsletterRead(ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        var detail = serviceNewsletterRead.GetAllNewVersion(p => p._idNewsletter.ToUpper().Contains(filter.ToUpper()), count, count * (page - 1), "Title").Result
          .Select(x => x.GetViewList()).ToList();
        total = serviceNewsletterRead.CountNewVersion(p => p._idNewsletter.ToUpper().Contains(filter.ToUpper())).Result;
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
