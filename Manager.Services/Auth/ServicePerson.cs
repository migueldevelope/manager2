using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.BusinessModel;
using Manager.Core.Interfaces;
using Manager.Data;
using Manager.Services.Commons;
using Manager.Services.Specific;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using Tools;

namespace Manager.Services.Auth
{
  public class ServicePerson : Repository<Person>, IServicePerson
  {
    private ServiceGeneric<Attachments> serviceAttachment;
    private ServiceGeneric<Company> serviceCompany;
    private ServiceGeneric<Establishment> serviceEstablishment;
    private ServiceSendGrid serviceMail;
    private ServiceGeneric<Occupation> serviceOccupation;
    private ServiceGeneric<Parameter> serviceParameter;
    private ServiceGeneric<Person> servicePerson;
    private ServiceGeneric<SalaryScale> serviceSalaryScale;
    private ServiceGeneric<Schooling> serviceSchooling;
    private ServiceGeneric<User> serviceUser;

    #region Constructor
    public ServicePerson(DataContext context) : base(context)
    {
      try
      {
        serviceAttachment = new ServiceGeneric<Attachments>(context);
        serviceCompany = new ServiceGeneric<Company>(context);
        serviceEstablishment = new ServiceGeneric<Establishment>(context);
        serviceMail = new ServiceSendGrid(context);
        serviceOccupation = new ServiceGeneric<Occupation>(context);
        serviceParameter = new ServiceGeneric<Parameter>(context);
        servicePerson = new ServiceGeneric<Person>(context);
        serviceSalaryScale = new ServiceGeneric<SalaryScale>(context);
        serviceSchooling = new ServiceGeneric<Schooling>(context);
        serviceUser = new ServiceGeneric<User>(context);
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public void SetUser(IHttpContextAccessor contextAccessor)
    {
      User(contextAccessor);
      serviceAttachment._user = _user;
      serviceCompany._user = _user;
      serviceEstablishment._user = _user;
      serviceMail.SetUser(_user);
      serviceOccupation._user = _user;
      serviceParameter._user = _user;
      servicePerson._user = _user;
      serviceSalaryScale._user = _user;
      serviceSchooling._user = _user;
      serviceUser._user = _user;
      DefaultTypeRegisterPerson();
    }
    public void SetUser(BaseUser user)
    {
      serviceAttachment._user = user;
      serviceCompany._user = user;
      serviceEstablishment._user = user;
      serviceMail.SetUser(user);
      serviceOccupation._user = user;
      serviceParameter._user = user;
      servicePerson._user = user;
      serviceSalaryScale._user = user;
      serviceSchooling._user = user;
      serviceUser._user = user;
      DefaultTypeRegisterPerson();
    }
    #endregion

    #region Company
    public List<ViewListCompany> ListCompany(ref long total, string filter, int count, int page)
    {
      int skip = (count * (page - 1));
      var detail = serviceCompany.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).Skip(skip).Take(count).ToList();
      total = serviceCompany.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).Skip(skip).Take(count).Count();

      return detail.Select(p => new ViewListCompany() { _id = p._id, Name = p.Name }).ToList();
    }
    #endregion

    #region Manager
    public List<ViewListPerson> ListManager(ref long total, string filter, int count, int page)
    {
      int skip = (count * (page - 1));
      var detail = servicePerson.GetAll(p => p.TypeUser != EnumTypeUser.Employee & p.TypeUser != EnumTypeUser.HR & p.StatusUser != EnumStatusUser.Disabled & p.StatusUser != EnumStatusUser.ErrorIntegration & p.TypeUser != EnumTypeUser.Administrator & p.User.Name.ToUpper().Contains(filter.ToUpper()))
        .OrderBy(p => p.User.Name)
         .Select(item => new ViewListPerson()
         {
           _id = item._id,
           Company = new ViewListCompany() { _id = item.Company._id, Name = item.Company.Name },
           Establishment = new ViewListEstablishment() { _id = item.Establishment._id, Name = item.Establishment.Name },
           Registration = item.Registration,
           User = new ViewListUser() { _id = item.User._id, Name = item.User.Name, Document = item.User.Document, Mail = item.User.Mail, Phone = item.User.Phone }
         }).ToList();
      //var detail = personService.GetAll(p => p.StatusUser != EnumStatusUser.Disabled & p.StatusUser != EnumStatusUser.ErrorIntegration & p.TypeUser != EnumTypeUser.Administrator & p.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Name).Skip(skip).Take(count).ToList();
      //total = personService.GetAll(p => p.StatusUser != EnumStatusUser.Disabled & p.StatusUser != EnumStatusUser.ErrorIntegration & p.TypeUser != EnumTypeUser.Administrator & p.Name.ToUpper().Contains(filter.ToUpper())).Count();

      return detail;
    }
    #endregion

    #region Occupation
    public List<ViewListOccupation> ListOccupation(ref long total, string filter, int count, int page)
    {
      int skip = (count * (page - 1));
      var detail = serviceOccupation.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Name)
        .Skip(skip).Take(count).ToList()
        ;
      total = serviceOccupation.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).Count();

      return detail.Select(p => new ViewListOccupation()
      {
        _id = p._id,
        Name = p.Name,
        Company = new ViewListCompany() { _id = p.Group.Company._id, Name = p.Group.Company.Name },
        Group = new ViewListGroup()
        {
          _id = p.Group._id,
          Name = p.Group.Name,
          Axis = new ViewListAxis() { _id = p.Group.Axis._id, Name = p.Group.Axis.Name, TypeAxis = p.Group.Axis.TypeAxis },
          Line = p.Group.Line,
          Sphere = new ViewListSphere() { _id = p.Group.Sphere._id, TypeSphere = p.Group.Sphere.TypeSphere, Name = p.Group.Sphere.Name },
        }
      }).ToList();

    }
    #endregion

    #region Person
    public string AddPersonUser(ViewCrudPersonUser view)
    {
      try
      {
        var authMaristas = false;
        var authPUC = false;
        try
        {
          authMaristas = view.User.Mail.Substring(view.User.Mail.IndexOf("@"), view.User.Mail.Length - view.User.Mail.IndexOf("@")) == "@maristas.org.br" ? true : false;
          authPUC = view.User.Mail.Substring(view.User.Mail.IndexOf("@"), view.User.Mail.Length - view.User.Mail.IndexOf("@")) == "@pucrs.br" ? true : false;
        }
        catch (Exception)
        {

        }
        var user = new User()
        {
          Name = view.User.Name,
          Nickname = view.User.Nickname,
          Document = view.User.Document,
          Mail = view.User.Mail,
          Phone = view.User.Phone,
          Password = view.User.Password,
          DateBirth = view.User.DateBirth,
          DateAdm = view.User.DateAdm,
          Schooling = (view.User.Schooling == null) ? null : serviceSchooling.GetAll(p => p._id == view.User.Schooling._id).FirstOrDefault(),
          PhotoUrl = view.User.PhotoUrl,
          PhoneFixed = view.User.PhoneFixed,
          DocumentID = view.User.DocumentID,
          DocumentCTPF = view.User.DocumentCTPF,
          Sex = view.User.Sex,
          UserAdmin = false
        };

        BaseFields manager = null;
        if (view.Person.Manager != null)
        {
          manager = servicePerson.GetAll(p => p._id == view.Person.Manager._id).
         Select(p => new BaseFields()
         {
           _id = p._id,
           Name = p.User.Name,
           Mail = p.User.Mail
         }).FirstOrDefault();
        }

        SalaryScalePerson salaryScale = null;
        if (view.Person.SalaryScales != null)
          salaryScale = serviceSalaryScale.GetAll(p => p._id == view.Person.SalaryScales._idSalaryScale)
            .Select(p => new SalaryScalePerson() { _idSalaryScale = p._id, NameSalaryScale = p.Name })
            .FirstOrDefault();

        var person = new Person()
        {
          StatusUser = view.Person.StatusUser,
          Company = serviceCompany.GetAll(p => p._id == view.Person.Company._id).FirstOrDefault(),
          Occupation = (view.Person.Occupation == null) ? null : serviceOccupation.GetAll(p => p._id == view.Person.Occupation._id).FirstOrDefault(),
          Manager = manager,
          DateLastOccupation = view.Person.DateLastOccupation,
          Salary = view.Person.Salary,
          DateLastReadjust = view.Person.DateLastReadjust,
          DateResignation = view.Person.DateResignation,
          TypeJourney = view.Person.TypeJourney,
          Establishment = (view.Person.Establishment == null) ? null : serviceEstablishment.GetAll(p => p._id == view.Person.Establishment._id).FirstOrDefault(),
          HolidayReturn = view.Person.HolidayReturn,
          MotiveAside = view.Person.MotiveAside,
          TypeUser = view.Person.TypeUser,
          Registration = view.Person.Registration,
          SalaryScales = salaryScale
        };

        user.Password = EncryptServices.GetMD5Hash(view.User.Password);
        user.ChangePassword = Manager.Views.Enumns.EnumChangePassword.AccessFirst;
        user.Status = EnumStatus.Enabled;
        person.Status = EnumStatus.Enabled;

        if ((authMaristas) || (authPUC))
          user.ChangePassword = Manager.Views.Enumns.EnumChangePassword.No;

        //person.User = user;
        person.User = serviceUser.Insert(user);

        servicePerson.Insert(person);

        return "ok";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string UpdatePersonUser(ViewCrudPersonUser view)
    {
      try
      {
        User user = serviceUser.GetAll(p => p._id == view.User._id).SingleOrDefault();
        user.Name = view.User.Name;
        user.Nickname = view.User.Nickname;
        user.Document = view.User.Document;
        user.Mail = view.User.Mail;
        user.Phone = view.User.Phone;
        user.DateBirth = view.User.DateBirth;
        user.DateAdm = view.User.DateAdm;
        user.Schooling = view.User.Schooling == null ? null : serviceSchooling.GetAll(p => p._id == view.User.Schooling._id).FirstOrDefault();
        user.PhotoUrl = view.User.PhotoUrl;
        user.PhoneFixed = view.User.PhoneFixed;
        user.DocumentID = view.User.DocumentID;
        user.DocumentCTPF = view.User.DocumentCTPF;
        user.Sex = view.User.Sex;

        BaseFields manager = null;
        if (view.Person.Manager != null)
        {
          manager = servicePerson.GetAll(p => p._id == view.Person.Manager._id).
           Select(p => new BaseFields()
           {
             _id = p._id,
             Name = p.User.Name,
             Mail = p.User.Mail
           }).FirstOrDefault();
        }
        SalaryScalePerson salaryScale = null;
        if (view.Person.SalaryScales != null)
          salaryScale = serviceSalaryScale.GetAll(p => p._id == view.Person.SalaryScales._idSalaryScale)
            .Select(p => new SalaryScalePerson() { _idSalaryScale = p._id, NameSalaryScale = p.Name })
            .FirstOrDefault();

        Person person = servicePerson.GetAll(p => p._id == view.Person._id).FirstOrDefault();
        person.StatusUser = view.Person.StatusUser;
        person.Company = serviceCompany.GetAll(p => p._id == view.Person.Company._id).FirstOrDefault();
        person.Occupation = view.Person.Occupation == null ? null : serviceOccupation.GetAll(p => p._id == view.Person.Occupation._id).FirstOrDefault();
        person.Manager = manager;
        person.DateLastOccupation = view.Person.DateLastOccupation;
        person.Salary = view.Person.Salary;
        person.DateLastReadjust = view.Person.DateLastReadjust;
        person.DateResignation = view.Person.DateResignation;
        person.TypeJourney = view.Person.TypeJourney;
        person.Establishment = (view.Person.Establishment == null) ? null : serviceEstablishment.GetAll(p => p._id == view.Person.Establishment._id).FirstOrDefault();
        person.HolidayReturn = view.Person.HolidayReturn;
        person.MotiveAside = view.Person.MotiveAside;
        person.TypeUser = view.Person.TypeUser;
        person.Registration = view.Person.Registration;
        person.SalaryScales = salaryScale;
        person.User = user;
        servicePerson.Update(person, null);
        serviceUser.Update(user, null);
        return "Person altered!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }


    public List<ViewListSalaryScalePerson> ListSalaryScale(string idoccupation)
    {
      try
      {
        var occupation = serviceOccupation.GetAll(p => p._id == idoccupation).FirstOrDefault();
        if (occupation.SalaryScales != null)
          return occupation.SalaryScales
            .Select(p => new ViewListSalaryScalePerson
            {
              _idSalaryScale = p._idSalaryScale,
              NameSalaryScale = p.NameSalaryScale
            }).OrderBy(p => p.NameSalaryScale).ToList();
        else
          return null;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewListPersonTeam> ListTeam(ref long total, string idPerson, string filter, int count, int page)
    {
      try
      {
        int skip = (count * (page - 1));
        var detail = servicePerson.GetAll(p => p.StatusUser != EnumStatusUser.Disabled & p.StatusUser != EnumStatusUser.ErrorIntegration & p.TypeUser != EnumTypeUser.Administrator & p.Manager._id == idPerson & p._id != idPerson & p.User.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.User.Name).Skip(skip).Take(count).ToList();
        total = servicePerson.GetAll(p => p.StatusUser != EnumStatusUser.Disabled & p.StatusUser != EnumStatusUser.ErrorIntegration & p.TypeUser != EnumTypeUser.Administrator & p.Manager._id == idPerson & p._id != idPerson & p.User.Name.ToUpper().Contains(filter.ToUpper())).Count();

        return detail
          .Select(item => new ViewListPersonTeam()
          {
            Name = item.User.Name,
            _idPerson = item._id,
            Occupation = item.Occupation?.Name,
            DataAdm = item.User.DateAdm
          }).OrderBy(p => p.Name).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewListPersonCrud> List(ref long total, int count, int page, string filter, EnumTypeUser type)
    {
      try
      {
        switch (type)
        {
          case EnumTypeUser.Support:
          case EnumTypeUser.Administrator:
            total = servicePerson.CountNewVersion(p => p.User.Name.ToUpper().Contains(filter.ToUpper())).Result;
            return servicePerson.GetAllNewVersion(p => p.User.Name.ToUpper().Contains(filter.ToUpper()), count, count * (page - 1), "User.Name").Result
            .Select(x => new ViewListPersonCrud()
            {
              _id = x._id,
              Registration = x.Registration,
              User = new ViewListUser() { _id = x.User._id, Name = x.User.Name, Document = x.User.Document, Mail = x.User.Mail, Phone = x.User.Phone },
              Company = new ViewListCompany() { _id = x.Company._id, Name = x.Company.Name },
              Establishment = x.Establishment == null ? null : new ViewListEstablishment() { _id = x.Establishment._id, Name = x.Establishment.Name },
              StatusUser = x.StatusUser,
              TypeJourney = x.TypeJourney,
              TypeUser = x.TypeUser,
              Occupation = x.Occupation?.Name
            }).OrderBy(p => p.User.Name).ToList();
          case EnumTypeUser.HR:
          case EnumTypeUser.ManagerHR:
            total = servicePerson.CountNewVersion(p => p.User.Name.ToUpper().Contains(filter.ToUpper()) && p.TypeUser != EnumTypeUser.Administrator && p.TypeUser != EnumTypeUser.Support).Result;
            return servicePerson.GetAllNewVersion(p => p.User.Name.ToUpper().Contains(filter.ToUpper()) && p.TypeUser != EnumTypeUser.Administrator && p.TypeUser != EnumTypeUser.Support, count, count * (page - 1), "User.Name").Result
            .Select(x => new ViewListPersonCrud()
            {
              _id = x._id,
              Registration = x.Registration,
              User = new ViewListUser() { _id = x.User._id, Name = x.User.Name, Document = x.User.Document, Mail = x.User.Mail, Phone = x.User.Phone },
              Company = new ViewListCompany() { _id = x.Company._id, Name = x.Company.Name },
              Establishment = x.Establishment == null ? null : new ViewListEstablishment() { _id = x.Establishment._id, Name = x.Establishment.Name },
              StatusUser = x.StatusUser,
              TypeJourney = x.TypeJourney,
              TypeUser = x.TypeUser,
              Occupation = x.Occupation?.Name
            }).ToList();
          default:
            total = 0;
            return new List<ViewListPersonCrud>();
        }
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public ViewCrudPerson Get(string id)
    {
      try
      {
        Person person = servicePerson.GetNewVersion(p => p._id == id).Result;
        return new ViewCrudPerson()
        {
          _id = person._id,
          Company = new ViewListCompany() { _id = person.Company._id, Name = person.Company.Name },
          DateLastOccupation = person.DateLastOccupation,
          DateLastReadjust = person.DateLastReadjust,
          DateResignation = person.DateResignation,
          Establishment = person.Establishment == null ? null : new ViewListEstablishment() { _id = person.Establishment._id, Name = person.Establishment.Name },
          HolidayReturn = person.HolidayReturn,
          Manager = person.Manager == null ? null : new ViewBaseFields()
          {
            _id = person.Manager._id,
            Name = person.Manager.Name,
            Mail = person.Manager.Mail
          },
          MotiveAside = person.MotiveAside,
          Occupation = person.Occupation == null ? null : new ViewListOccupation()
          {
            _id = person.Occupation._id,
            Name = person.Occupation.Name,
            Line = person.Occupation.Line,
            Company = new ViewListCompany() { _id = person.Occupation.Group.Company._id, Name = person.Occupation.Group.Company.Name },
            Group = new ViewListGroup()
            {
              _id = person.Occupation.Group._id,
              Name = person.Occupation.Group.Name,
              Line = person.Occupation.Group.Line,
              Axis = new ViewListAxis()
              {
                _id = person.Occupation.Group.Axis._id,
                Name = person.Occupation.Group.Axis.Name,
                TypeAxis = person.Occupation.Group.Axis.TypeAxis
              },
              Sphere = new ViewListSphere()
              {
                _id = person.Occupation.Group.Sphere._id,
                Name = person.Occupation.Group.Sphere.Name,
                TypeSphere = person.Occupation.Group.Sphere.TypeSphere
              }
            },
            Process = person.Occupation.Process.Select(p => new ViewListProcessLevelTwo()
            {
              _id = p._id,
              Name = p.Name,
              Order = p.Order,
              ProcessLevelOne = new ViewListProcessLevelOne()
              {
                _id = p.ProcessLevelOne._id,
                Name = p.ProcessLevelOne.Name,
                Order = p.ProcessLevelOne.Order,
                Area = new ViewListArea()
                {
                  _id = p.ProcessLevelOne.Area._id,
                  Name = p.ProcessLevelOne.Area.Name
                }
              }
            }).ToList()
          },
          Registration = person.Registration,
          Salary = person.Salary,
          StatusUser = person.StatusUser,
          TypeJourney = person.TypeJourney,
          TypeUser = person.TypeUser,
          User = new ViewCrudUser()
          {
            Name = person.User.Name,
            Nickname = person.User.Nickname,
            DateAdm = person.User.DateAdm,
            DateBirth = person.User.DateBirth,
            Document = person.User.Document,
            DocumentCTPF = person.User.DocumentCTPF,
            DocumentID = person.User.DocumentID,
            Mail = person.User.Mail,
            Password = string.Empty,
            Phone = person.User.Phone,
            PhoneFixed = person.User.PhoneFixed,
            PhotoUrl = person.User.PhotoUrl,
            Schooling = person.User.Schooling == null ? null : new ViewListSchooling() { _id = person.User.Schooling._id, Name = person.User.Schooling.Name, Order = person.User.Schooling.Order },
            Sex = person.User.Sex,
            _id = person.User._id
          }
        };
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public ViewCrudPerson New(ViewCrudPerson view)
    {
      try
      {
        User user = new User()
        {
          DateAdm = view.User.DateAdm,
          DateBirth = view.User.DateBirth,
          Document = view.User.Document,
          DocumentCTPF = view.User.DocumentCTPF,
          DocumentID = view.User.DocumentID,
          Mail = view.User.Mail,
          Name = view.User.Name,
          Nickname = view.User.Nickname,
          Password = EncryptServices.GetMD5Hash(view.User.Password),
          ForeignForgotPassword = string.Empty,
          Coins = 0,
          Phone = view.User.Phone,
          PhoneFixed = view.User.PhoneFixed,
          PhotoUrl = view.User.PhotoUrl,
          Schooling = view.User.Schooling == null ? null : serviceSchooling.GetNewVersion(p => p._id == view.User._id).Result,
          Sex = view.User.Sex,
          ChangePassword = EnumChangePassword.AccessFirst,
          UserAdmin = false
        };

        if (user.Mail.IndexOf("@maristas.org.br") != -1 || user.Mail.IndexOf("@pucrs.br") != -1)
          user.ChangePassword = EnumChangePassword.No;

        if (view.User._id == null)
          user = serviceUser.InsertNewVersion(user).Result;
        else
          user = serviceUser.GetNewVersion(p => p._id == view.User._id).Result;

        BaseFields manager = null;
        if (view.Manager != null)
        {
          manager = servicePerson.GetAll(p => p._id == view.Manager._id).
           Select(p => new BaseFields()
           {
             _id = p._id,
             Name = p.User.Name,
             Mail = p.User.Mail
           }).FirstOrDefault();
        }
        SalaryScalePerson salaryScale = null;
        if (view.SalaryScales != null)
          salaryScale = serviceSalaryScale.GetAll(p => p._id == view.SalaryScales._idSalaryScale)
            .Select(p => new SalaryScalePerson() { _idSalaryScale = p._id, NameSalaryScale = p.Name })
            .FirstOrDefault();

        Person person = new Person()
        {
          Company = serviceCompany.GetNewVersion(p => p._id == view.Company._id).Result,
          DateLastOccupation = view.DateLastOccupation,
          DateLastReadjust = view.DateLastReadjust,
          DateResignation = view.DateResignation,
          Establishment = view.Establishment == null ? null : serviceEstablishment.GetNewVersion(p => p._id == view.Establishment._id).Result,
          HolidayReturn = view.HolidayReturn,
          Manager = manager,
          MotiveAside = view.MotiveAside,
          Occupation = view.Occupation == null ? null : serviceOccupation.GetNewVersion(p => p._id == view.Occupation._id).Result,
          Registration = view.Registration,
          Salary = view.Salary,
          DocumentManager = null,
          StatusUser = view.StatusUser,
          TypeJourney = view.TypeJourney,
          TypeUser = view.TypeUser,
          User = user,
          SalaryScales = salaryScale
        };

        person = servicePerson.InsertNewVersion(person).Result;
        return new ViewCrudPerson()
        {
          _id = person._id,
          Company = new ViewListCompany() { _id = person.Company._id, Name = person.Company.Name },
          DateLastOccupation = person.DateLastOccupation,
          DateLastReadjust = person.DateLastReadjust,
          DateResignation = person.DateResignation,
          Establishment = person.Establishment == null ? null : new ViewListEstablishment() { _id = person.Establishment._id, Name = person.Establishment.Name },
          HolidayReturn = person.HolidayReturn,
          Manager = person.Manager == null ? null : new ViewBaseFields()
          {
            _id = person.Manager._id,
            Name = person.Manager.Name,
            Mail = person.Manager.Mail
          },
          MotiveAside = person.MotiveAside,
          Occupation = person.Occupation == null ? null : new ViewListOccupation()
          {
            _id = person.Occupation._id,
            Name = person.Occupation.Name,
            Line = person.Occupation.Line,
            Company = new ViewListCompany() { _id = person.Occupation.Group.Company._id, Name = person.Occupation.Group.Company.Name },
            Group = new ViewListGroup()
            {
              _id = person.Occupation.Group._id,
              Name = person.Occupation.Group.Name,
              Line = person.Occupation.Group.Line,
              Axis = new ViewListAxis()
              {
                _id = person.Occupation.Group.Axis._id,
                Name = person.Occupation.Group.Axis.Name,
                TypeAxis = person.Occupation.Group.Axis.TypeAxis
              },
              Sphere = new ViewListSphere()
              {
                _id = person.Occupation.Group.Sphere._id,
                Name = person.Occupation.Group.Sphere.Name,
                TypeSphere = person.Occupation.Group.Sphere.TypeSphere
              }
            },
            Process = person.Occupation.Process.Select(p => new ViewListProcessLevelTwo()
            {
              _id = p._id,
              Name = p.Name,
              Order = p.Order,
              ProcessLevelOne = new ViewListProcessLevelOne()
              {
                _id = p.ProcessLevelOne._id,
                Name = p.ProcessLevelOne.Name,
                Order = p.ProcessLevelOne.Order,
                Area = new ViewListArea()
                {
                  _id = p.ProcessLevelOne.Area._id,
                  Name = p.ProcessLevelOne.Area.Name
                }
              }
            }).ToList()
          },
          Registration = person.Registration,
          Salary = person.Salary,
          StatusUser = person.StatusUser,
          TypeJourney = person.TypeJourney,
          TypeUser = person.TypeUser,
          User = new ViewCrudUser()
          {
            Name = person.User.Name,
            Nickname = person.User.Nickname,
            DateAdm = person.User.DateAdm,
            DateBirth = person.User.DateBirth,
            Document = person.User.Document,
            DocumentCTPF = person.User.DocumentCTPF,
            DocumentID = person.User.DocumentID,
            Mail = person.User.Mail,
            Password = string.Empty,
            Phone = person.User.Phone,
            PhoneFixed = person.User.PhoneFixed,
            PhotoUrl = person.User.PhotoUrl,
            Schooling = person.User.Schooling == null ? null : new ViewListSchooling() { _id = person.User.Schooling._id, Name = person.User.Schooling.Name, Order = person.User.Schooling.Order },
            Sex = person.User.Sex,
            _id = person.User._id
          }
        };
        // TODO: Manager
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public string Update(ViewCrudPerson view)
    {
      try
      {
        User user = serviceUser.GetNewVersion(p => p._id == view.User._id).Result;
        user.DateAdm = view.User.DateAdm;
        user.DateBirth = view.User.DateBirth;
        user.Document = view.User.Document;
        user.DocumentCTPF = view.User.DocumentCTPF;
        user.DocumentID = view.User.DocumentID;
        user.Mail = view.User.Mail;
        user.Name = view.User.Name;
        user.Nickname = view.User.Nickname;
        user.Phone = view.User.Phone;
        user.PhoneFixed = view.User.PhoneFixed;
        user.PhotoUrl = view.User.PhotoUrl;
        user.Schooling = view.User.Schooling == null ? null : serviceSchooling.GetNewVersion(p => p._id == view.User.Schooling._id).Result;
        user.Sex = view.User.Sex;
        serviceUser.Update(user, null);

        BaseFields manager = null;
        if (view.Manager != null)
        {
          manager = servicePerson.GetAll(p => p._id == view.Manager._id).
           Select(p => new BaseFields()
           {
             _id = p._id,
             Name = p.User.Name,
             Mail = p.User.Mail
           }).FirstOrDefault();
        }
        SalaryScalePerson salaryScale = null;
        if (view.SalaryScales != null)
          salaryScale = serviceSalaryScale.GetAll(p => p._id == view.SalaryScales._idSalaryScale)
            .Select(p => new SalaryScalePerson() { _idSalaryScale = p._id, NameSalaryScale = p.Name })
            .FirstOrDefault();

        Person person = servicePerson.GetNewVersion(p => p._id == view._id).Result;
        person.Company = serviceCompany.GetNewVersion(p => p._id == view.Company._id).Result;
        person.DateLastOccupation = view.DateLastOccupation;
        person.DateLastReadjust = view.DateLastReadjust;
        person.DateResignation = view.DateResignation;
        person.Establishment = view.Establishment == null ? null : serviceEstablishment.GetNewVersion(p => p._id == view.Establishment._id).Result;
        person.HolidayReturn = view.HolidayReturn;
        person.Manager = manager;
        person.MotiveAside = view.MotiveAside;
        person.Occupation = view.Occupation == null ? null : serviceOccupation.GetNewVersion(p => p._id == view.Occupation._id).Result;
        person.Registration = view.Registration;
        person.Salary = view.Salary;
        person.DocumentManager = null;
        person.StatusUser = view.StatusUser;
        person.TypeJourney = view.TypeJourney;
        person.TypeUser = view.TypeUser;
        person.User = user;
        person.SalaryScales = salaryScale;
        servicePerson.Update(person, null);
        return "Person altered!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<SalaryScalePerson> ListSalaryScaleOld(string idoccupation)
    {
      try
      {
        var occupation = serviceOccupation.GetAll(p => p._id == idoccupation).FirstOrDefault();
        if (occupation.SalaryScales != null)
          return occupation.SalaryScales
            .Select(p => new SalaryScalePerson
            {
              _idSalaryScale = p._idSalaryScale,
              NameSalaryScale = p.NameSalaryScale
            }).OrderBy(p => p.NameSalaryScale).ToList();
        else
          return null;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public List<ViewListPerson> GetPersons(string idcompany, string filter)
    {
      try
      {
        return servicePerson.GetAll(p => p.Company._id == idcompany & p.StatusUser
        != EnumStatusUser.Disabled & p.StatusUser != EnumStatusUser.ErrorIntegration
        & p.TypeUser != EnumTypeUser.Administrator & p.User.Name.ToUpper().Contains(filter.ToUpper()))
         .Select(item => new ViewListPerson()
         {
           _id = item._id,
           Company = new ViewListCompany() { _id = item.Company._id, Name = item.Company.Name },
           Establishment = new ViewListEstablishment() { _id = item.Establishment._id, Name = item.Establishment.Name },
           Registration = item.Registration,
           User = new ViewListUser() { _id = item.User._id, Name = item.User.Name, Document = item.User.Document, Mail = item.User.Mail, Phone = item.User.Phone }
         }).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    #endregion

    #region Private
    private void DefaultTypeRegisterPerson()
    {
      try
      {
        var parameter = serviceParameter.GetAll(p => p.Key == "typeregisterperson").FirstOrDefault();
        if (parameter == null)
          serviceParameter.Insert(new Parameter()
          {
            Name = "Tipo do cadastro da pessoa",
            Key = "typeregisterperson",
            Content = "0",
            Help = "Informe 0 para cadastro normal e 1 para cadastro de multicontratos.",
            Status = EnumStatus.Enabled
          });
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    #endregion

  }
}
