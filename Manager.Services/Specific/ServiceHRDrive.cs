using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.BusinessModel;
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
  public class ServiceHRDrive : Repository<HRDrive>, IServiceHRDrive
  {
    private readonly ServiceGeneric<HRDrive> serviceHRDrive;
    private readonly ServiceGeneric<Person> servicePerson;
    private readonly ServiceGeneric<Attachments> serviceAttachment;

    #region Constructor
    public ServiceHRDrive(DataContext context) : base(context)
    {
      try
      {
        serviceHRDrive = new ServiceGeneric<HRDrive>(context);
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
      serviceHRDrive._user = _user;
      servicePerson._user = _user;
    }
    public void SetUser(BaseUser user)
    {
      _user = user;
      serviceHRDrive._user = user;
      servicePerson._user = user;
    }
    #endregion

    #region HRDrive
    public string Delete(string id)
    {
      try
      {
        HRDrive item = serviceHRDrive.GetNewVersion(p => p._id == id).Result;
        item.Status = EnumStatus.Disabled;
        serviceHRDrive.Update(item, null).Wait();
        return "HRDrive deleted!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public string Add(AttachmentDrive att)
    {
      try
      {
        var person = servicePerson.GetNewVersion(p => p._id == _user._idPerson).Result;
        var view = new ViewCrudHRDrive()
        {
          _idPerson = person._id,
          NamePerson = person.User?.Name,
          Attachments  = att.GetViewCrud()
        };

        var hrdrive = serviceHRDrive.GetNewVersion(p => p._idPerson == view._idPerson).Result;
        var exists = true;

        if (hrdrive == null)
        {
          exists = false;
          hrdrive = new HRDrive()
          {
            _id = view._id,
            _idPerson = view._idPerson,
            NamePerson = view.NamePerson,
            Attachments = new List<AttachmentDrive>()
          };
        }

        hrdrive.Attachments.Add(new AttachmentDrive()
        {
          Date = DateTime.Now,
          Name = view.Attachments.Name,
          Url = view.Attachments.Url,
          _idAttachment = view.Attachments._idAttachment
        });


        if (exists)
          serviceHRDrive.Update(hrdrive, null).Wait();
        else
          serviceHRDrive.InsertNewVersion(hrdrive).Wait();

        return "HRDrive added!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public ViewCrudHRDrive Get(string id)
    {
      try
      {
        var hrdrive = serviceHRDrive.GetNewVersion(p => p._idPerson == _user._idPerson).Result;
        var attachment = hrdrive.Attachments.Where(p => p._idAttachment == id).FirstOrDefault();

        var view = new ViewCrudHRDrive()
        {
          _idPerson = hrdrive._idPerson,
          NamePerson = hrdrive.NamePerson,
          Attachments = attachment.GetViewCrud()
        };

        return view;

      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewListHRDrive> List(ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        List<ViewListHRDrive> detail = serviceHRDrive.GetAllNewVersion(p => p.NamePerson.ToUpper().Contains(filter.ToUpper()), count, count * (page - 1), "NamePerson").Result
          .Select(x => new ViewListHRDrive()
          {
            _id = x._id,
            NamePerson = x.NamePerson,
            Attachments = x.Attachments?.Select(p => p.GetViewCrud()).ToList(),
            _idPerson = x._idPerson
          }).ToList();
        total = serviceHRDrive.CountNewVersion(p => p.NamePerson.ToUpper().Contains(filter.ToUpper())).Result;
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
