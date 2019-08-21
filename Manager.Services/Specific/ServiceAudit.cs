using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.Interfaces;
using Manager.Data;
using Manager.Services.Commons;
using Manager.Views.Audit;
using Manager.Views.Enumns;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manager.Services.Specific
{
  public class ServiceAudit : IServiceAudit
  {

    private readonly ServiceGeneric<Person> servicePerson;
    private readonly ServiceGeneric<User> serviceUser;
    private readonly ServiceGeneric<Occupation> serviceOccupation;

    #region Constructor
    public ServiceAudit(DataContext context)
    {
      try
      {
        servicePerson = new ServiceGeneric<Person>(context);
        serviceUser = new ServiceGeneric<User>(context);
        serviceOccupation = new ServiceGeneric<Occupation>(context);
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public void SetUser(IHttpContextAccessor contextAccessor)
    {
      servicePerson.User(contextAccessor);
      serviceUser._user = servicePerson._user;
      serviceOccupation._user = servicePerson._user;
    }
    public void SetUser(BaseUser user)
    {
      //_user = user;
      servicePerson._user = user;
      serviceUser._user = user;
      serviceOccupation._user = user;
    }
    #endregion

    #region Generic List
    public List<ViewAuditUser> ListUser()
    {
      try
      {
        Person person = servicePerson.GetFreeNewVersion(p => p.User._id == serviceUser._user._idUser).Result;
        switch (person.TypeUser)
        {
          case EnumTypeUser.Support:
          case EnumTypeUser.Administrator:
          case EnumTypeUser.Anonymous:
            return serviceUser.GetAllFreeNewVersion(p => p._idAccount == serviceUser._user._idAccount).Result
              .Select(p => new ViewAuditUser()
              {
                IdUser = p._id,
                Document = p.Document,
                Mail = p.Mail,
                NameUser = p.Name,
                DisabledUser = p.Status.ToString()
              }).ToList();
          default:
            throw new Exception("Not available!");
        }
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewAuditPerson> ListPerson()
    {
      try
      {
        Person person = servicePerson.GetFreeNewVersion(p => p.User._id == serviceUser._user._idUser).Result;

        switch (person.TypeUser)
        {
          case EnumTypeUser.Support:
          case EnumTypeUser.Administrator:
          case EnumTypeUser.Anonymous:
            return servicePerson.GetAllFreeNewVersion(p => p._idAccount == serviceUser._user._idAccount).Result
              .Select(p => new ViewAuditPerson()
              {
                IdUser = p.User._id,
                Document = p.User.Document,
                Mail = p.User.Mail,
                NameUser = p.User.Name,
                DisabledUser = p.Status.ToString(),
                IdPerson = p._id,
                DisabledPerson = p.Status.ToString(),
                IdEstablishment = p.Establishment?._id,
                NameEstablishment = p.Establishment?.Name,
                IdCompany = p.Company._id,
                NameCompany = p.Company.Name,
                Registration = p.Registration,
                StatusUser = p.StatusUser.ToString(),
                IdManager = p.Manager?._id,
                NameManager = p.Manager?.Name,
                IdOccupation = p.Occupation?._id,
                NameOccupation = p.Occupation?.Name,
                TypeJorney = p.TypeJourney
              }).ToList();
          default:
            throw new Exception("Not available!");
        }
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewAuditOccupationSkills> ListOccupationSkills()
    {
      try
      {
        var occupations = serviceOccupation.GetAllFreeNewVersion().Result.ToList();

        List<ViewAuditOccupationSkills> result = new List<ViewAuditOccupationSkills>();
        foreach (var occupation in occupations)
        {
          if (occupation.Skills != null)
            foreach (var item in occupation.Skills)
            {
              if (item != null)
                result.Add(new ViewAuditOccupationSkills()
                {
                  OccupationName = occupation.Name,
                  SkillsName = item.Name
                });
            }

        }


        return result;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    #endregion
  }
}
