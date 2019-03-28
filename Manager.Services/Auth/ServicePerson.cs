using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.BusinessModel;
using Manager.Core.Enumns;
using Manager.Core.Interfaces;
using Manager.Core.Views;
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
using System.Linq.Expressions;
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
          Sex = view.User.Sex
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

    public List<ViewListPersonTeam> GetPersonTeam(ref long total, string idPerson, string filter, int count, int page)
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
            Occupation = (item.Occupation == null) ? null : item.Occupation.Name,
            DataAdm = item.User.DateAdm
          }).OrderBy(p => p.Name).ToList();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public List<ViewListPersonCrud> GetPersons(ref long total, int count, int page, string filter, EnumTypeUser type)
    {
      try
      {
        switch (type)
        {
          case EnumTypeUser.Support:
          case EnumTypeUser.Administrator:
            total = servicePerson.CountNewVersion(p => p.User.Name.Contains(filter)).Result;
            return servicePerson.GetAllNewVersion(p => p.User.Name.Contains(filter), count, count * (page - 1), "User.Name").Result
            .Select(x => new ViewListPersonCrud()
            {
              _id = x._id,
              Registration = x.Registration,
              User = new ViewListUser() { _id = x.User._id, Name = x.User.Name, Document = x.User.Document, Mail = x.User.Mail, Phone = x.User.Phone },
              Company = new ViewListCompany() { _id = x.Company._id, Name = x.Company.Name },
              Establishment = x.Establishment == null ? null : new ViewListEstablishment() { _id = x.Establishment._id, Name = x.Establishment.Name },
              StatusUser = x.StatusUser,
              TypeJourney = x.TypeJourney,
              TypeUser = x.TypeUser
            }).OrderBy(p => p.User.Name).ToList();
          case EnumTypeUser.HR:
          case EnumTypeUser.ManagerHR:
            total = servicePerson.CountNewVersion(p => p.User.Name.Contains(filter) && p.TypeUser != EnumTypeUser.Administrator && p.TypeUser != EnumTypeUser.Support).Result;
            return servicePerson.GetAllNewVersion(p => p.User.Name.Contains(filter) && p.TypeUser != EnumTypeUser.Administrator && p.TypeUser != EnumTypeUser.Support, count, count * (page - 1), "User.Name").Result
            .Select(x => new ViewListPersonCrud()
            {
              _id = x._id,
              Registration = x.Registration,
              User = new ViewListUser() { _id = x.User._id, Name = x.User.Name, Document = x.User.Document, Mail = x.User.Mail, Phone = x.User.Phone },
              Company = new ViewListCompany() { _id = x.Company._id, Name = x.Company.Name },
              Establishment = x.Establishment == null ? null : new ViewListEstablishment() { _id = x.Establishment._id, Name = x.Establishment.Name },
              StatusUser = x.StatusUser,
              TypeJourney = x.TypeJourney,
              TypeUser = x.TypeUser
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
    public ViewCrudPerson GetPersonCrud(string id)
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
          Manager = null,
          MotiveAside = person.MotiveAside,
          Occupation = null,
          Registration = person.Registration,
          Salary = person.Salary,
          StatusUser = person.StatusUser,
          TypeJourney = person.TypeJourney,
          TypeUser = person.TypeUser,
          User = new ViewCrudUser()
          {
            Name = person.User.Name,
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
        // TODO: Manager, Occupation
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public ViewCrudPerson NewPerson(ViewCrudPerson view)
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
          Password = EncryptServices.GetMD5Hash(view.User.Password),
          ForeignForgotPassword = string.Empty,
          Coins = 0,
          Phone = view.User.Phone,
          PhoneFixed = view.User.PhoneFixed,
          PhotoUrl = view.User.PhotoUrl,
          Schooling = view.User.Schooling == null ? null : serviceSchooling.GetNewVersion(p => p._id == view.User._id).Result,
          Sex = view.User.Sex,
          ChangePassword = EnumChangePassword.AccessFirst
        };

        if (user.Mail.IndexOf("@maristas.org.br") != -1 || user.Mail.IndexOf("@pucrs.br") != -1)
          user.ChangePassword = EnumChangePassword.No;

        user = serviceUser.InsertNewVersion(user).Result;

        Person person = new Person()
        {
          Company = serviceCompany.GetNewVersion(p => p._id == view.Company._id).Result,
          DateLastOccupation = view.DateLastOccupation,
          DateLastReadjust = view.DateLastReadjust,
          DateResignation = view.DateResignation,
          Establishment = view.Establishment == null ? null : serviceEstablishment.GetNewVersion(p => p._id == view.Establishment._id).Result,
          HolidayReturn = view.HolidayReturn,
          Manager = null,
          MotiveAside = view.MotiveAside,
          Occupation = view.Occupation == null ? null : serviceOccupation.GetNewVersion(p => p._id == view.Occupation._id).Result,
          Registration = view.Registration,
          Salary = view.Salary,
          DocumentManager = null,
          StatusUser = view.StatusUser,
          TypeJourney = view.TypeJourney,
          TypeUser = view.TypeUser,
          User = user,
          SalaryScales = new SalaryScalePerson() { _idSalaryScale = view.SalaryScales._idSalaryScale, NameSalaryScale = view.SalaryScales.NameSalaryScale }
        };

        /*foreach (var item in view.SalaryScales)
          person.SalaryScales.Add(new SalaryScalePerson() { _idSalaryScale = item._idSalaryScale, NameSalaryScale = item.NameSalaryScale });
          */
        /// TODO: Manager
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
          Manager = null,
          MotiveAside = person.MotiveAside,
          Occupation = null,
          Registration = person.Registration,
          Salary = person.Salary,
          StatusUser = person.StatusUser,
          TypeJourney = person.TypeJourney,
          TypeUser = person.TypeUser,
          User = new ViewCrudUser()
          {
            Name = person.User.Name,
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
        // TODO: Manager, Occupation
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public ViewCrudPerson UpdatePerson(ViewCrudPerson view)
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
        user.Phone = view.User.Phone;
        user.PhoneFixed = view.User.PhoneFixed;
        user.PhotoUrl = view.User.PhotoUrl;
        user.Schooling = view.User.Schooling == null ? null : serviceSchooling.GetNewVersion(p => p._id == view.User._id).Result;
        user.Sex = view.User.Sex;
        user = serviceUser.UpdateNewVersion(user).Result;

        Person person = servicePerson.GetNewVersion(p => p._id == view._id).Result;
        person.Company = serviceCompany.GetNewVersion(p => p._id == view.Company._id).Result;
        person.DateLastOccupation = view.DateLastOccupation;
        person.DateLastReadjust = view.DateLastReadjust;
        person.DateResignation = view.DateResignation;
        person.Establishment = view.Establishment == null ? null : serviceEstablishment.GetNewVersion(p => p._id == view.Establishment._id).Result;
        person.HolidayReturn = view.HolidayReturn;
        person.Manager = null;
        person.MotiveAside = view.MotiveAside;
        person.Occupation = view.Occupation == null ? null : serviceOccupation.GetNewVersion(p => p._id == view.Occupation._id).Result;
        person.Registration = view.Registration;
        person.Salary = view.Salary;
        person.DocumentManager = null;
        person.StatusUser = view.StatusUser;
        person.TypeJourney = view.TypeJourney;
        person.TypeUser = view.TypeUser;
        person.User = user;
        person.SalaryScales = new SalaryScalePerson() { _idSalaryScale = view.SalaryScales._idSalaryScale, NameSalaryScale = view.SalaryScales.NameSalaryScale };

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
        person.Manager = manager;

        person = servicePerson.UpdateNewVersion(person).Result;
        return new ViewCrudPerson()
        {
          _id = person._id,
          Company = new ViewListCompany() { _id = person.Company._id, Name = person.Company.Name },
          DateLastOccupation = person.DateLastOccupation,
          DateLastReadjust = person.DateLastReadjust,
          DateResignation = person.DateResignation,
          Establishment = person.Establishment == null ? null : new ViewListEstablishment() { _id = person.Establishment._id, Name = person.Establishment.Name },
          HolidayReturn = person.HolidayReturn,
          Manager = null,
          MotiveAside = person.MotiveAside,
          Occupation = null,
          Registration = person.Registration,
          Salary = person.Salary,
          StatusUser = person.StatusUser,
          TypeJourney = person.TypeJourney,
          TypeUser = person.TypeUser,
          User = new ViewCrudUser()
          {
            Name = person.User.Name,
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
        // TODO: Manager, Occupation
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
    #endregion

    #region Person Old
    public ViewPersonHead Head(string idperson)
    {
      try
      {
        return servicePerson.GetAll(p => p.StatusUser != EnumStatusUser.Disabled && p.TypeUser != EnumTypeUser.Administrator && p._id == idperson).ToList().Select(
        person => new ViewPersonHead
        {
          IdPerson = idperson,
          NamePerson = person.User.Name,
          Occupation = person.Occupation,
          ActionFocus = ""
        }).FirstOrDefault();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public Person NewPersonOld(Person person)
    {
      try
      {
        person.StatusUser = EnumStatusUser.Enabled;
        person.Status = EnumStatus.Enabled;
        servicePerson.Insert(person);
        return person;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public Person NewPersonView(Person person)
    {

      try
      {
        var authMaristas = false;
        var authPUC = false;
        try
        {
          authMaristas = person.User.Mail.Substring(_user.Mail.IndexOf("@"), _user.Mail.Length - _user.Mail.IndexOf("@")) == "@maristas.org.br" ? true : false;
          authPUC = person.User.Mail.Substring(_user.Mail.IndexOf("@"), _user.Mail.Length - _user.Mail.IndexOf("@")) == "@pucrs.br" ? true : false;
        }
        catch (Exception)
        {

        }




        Person model = new Person()
        {
          Manager = person.Manager,
          Occupation = person.Occupation,
          Company = person.Company,
          StatusUser = person.StatusUser,
          TypeJourney = person.TypeJourney,
          Status = EnumStatus.Enabled,
          Establishment = person.Establishment,
          HolidayReturn = person.HolidayReturn,
          MotiveAside = person.MotiveAside,
          DateLastOccupation = person.DateLastOccupation,
          Salary = person.Salary,
          DateLastReadjust = person.DateLastReadjust,
          DateResignation = person.DateResignation,
          Registration = person.Registration,
          TypeUser = person.TypeUser,
          User = person.User,
          SalaryScales = person.SalaryScales == null ? null : new SalaryScalePerson() { _idSalaryScale = person.SalaryScales._idSalaryScale, NameSalaryScale = person.SalaryScales.NameSalaryScale }
        };
        /*foreach (var item in person.SalaryScales)
          model.SalaryScales.Add(new SalaryScalePerson() { _idSalaryScale = item._idSalaryScale, NameSalaryScale = item.NameSalaryScale });
          */


        if (person.Manager != null)
        {
          var manager = servicePerson.GetAll(p => p._id == model.Manager._id).FirstOrDefault();
          if (manager != null)
          {
            if (manager.User != null)
              model.DocumentManager = manager.User.Document;
          }
        }



        return servicePerson.Insert(model);
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }


    public string UpdatePersonOld(string id, ViewPersonsCrud person)
    {
      try
      {
        Person model = servicePerson.GetAll(p => p._id == id).FirstOrDefault();


        model.Manager = person.Manager;
        model.Occupation = person.Occupation;
        model.Company = person.Company;
        model.StatusUser = person.StatusUser;
        model.Establishment = person.Establishment;
        model.HolidayReturn = person.HolidayReturn;
        model.MotiveAside = person.MotiveAside;
        model.DateLastOccupation = person.DateLastOccupation;
        model.Salary = person.Salary;
        model.DateLastReadjust = person.DateLastReadjust;
        model.DateResignation = person.DateResignation;
        model.Registration = person.Registration;
        model.TypeUser = person.TypeUser;
        model.User = person.User;

        if (person.Manager != null)
        {
          var manager = servicePerson.GetAll(p => p._id == model.Manager._id).FirstOrDefault();
          if (manager != null)
          {
            if (manager.User != null)
              model.DocumentManager = manager.User.Document;
          }
        }

        servicePerson.Update(model, null);
        return "ok";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public Person UpdatePersonView(Person person)
    {
      try
      {
        var pass = servicePerson.GetAll(p => p._id == person._id).SingleOrDefault().User.Password;
        servicePerson.Update(person, null);
        return person;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public Person UpdatePersonOld(Person person)
    {
      try
      {
        servicePerson.Update(person, null);
        return person;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public void SetPhoto(string idPerson, string url)
    {
      try
      {
        var person = servicePerson.GetAll(p => p._id == idPerson).SingleOrDefault();
        servicePerson.Update(person, null);
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }


    public ViewPersonDetail GetPersonDetail(string idPerson)
    {
      try
      {
        return servicePerson.GetAll(p => p.StatusUser != EnumStatusUser.Disabled && p.TypeUser != EnumTypeUser.Administrator && p._id == idPerson).ToList().Select(
        detail => new ViewPersonDetail()
        {
          Birth = detail.User.DateBirth,
          Mail = detail.User.Mail,
          Name = detail.User.Name,
          Occuaption = detail.Occupation,
          IdPerson = idPerson
        }).FirstOrDefault();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public List<ViewPersonList> GetPersons(string filter)
    {
      try
      {
        return servicePerson.GetAll(p => p.StatusUser != EnumStatusUser.Disabled & p.StatusUser != EnumStatusUser.ErrorIntegration & p.TypeUser != EnumTypeUser.Administrator & p.User.Name.ToUpper().Contains(filter.ToUpper())).ToList()
          .Select(item => new ViewPersonList()
          {
            IdPerson = item._id,
            NamePerson = item.User.Name
          }).ToList();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
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
        throw new ServiceException(_user, e, this._context);
      }
    }

    public List<ViewPersonTeam> GetPersonTeamOld(ref long total, string idPerson, string filter, int count, int page)
    {
      try
      {
        int skip = (count * (page - 1));
        var detail = servicePerson.GetAll(p => p.StatusUser != EnumStatusUser.Disabled & p.StatusUser != EnumStatusUser.ErrorIntegration & p.TypeUser != EnumTypeUser.Administrator & p.Manager._id == idPerson & p._id != idPerson & p.User.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.User.Name).Skip(skip).Take(count).ToList();
        total = servicePerson.GetAll(p => p.StatusUser != EnumStatusUser.Disabled & p.StatusUser != EnumStatusUser.ErrorIntegration & p.TypeUser != EnumTypeUser.Administrator & p.Manager._id == idPerson & p._id != idPerson & p.User.Name.ToUpper().Contains(filter.ToUpper())).Count();

        return detail
          .Select(item => new ViewPersonTeam()
          {
            Name = item.User.Name,
            IdPerson = item._id,
            Occupation = item.Occupation,
            DataAdm = item.User.DateAdm
          }).OrderBy(p => p.Name).ToList();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string GetPhoto(string idPerson)
    {
      try
      {
        return this.servicePerson.GetAll(p => p.StatusUser != EnumStatusUser.Disabled & p.StatusUser != EnumStatusUser.ErrorIntegration & p.StatusUser != EnumStatusUser.Disabled & p.StatusUser != EnumStatusUser.ErrorIntegration & p._id == idPerson).FirstOrDefault().User.PhotoUrl;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }


    public Person GetPerson(string id)
    {
      try
      {
        return servicePerson.GetAll(p => p._id == id).FirstOrDefault();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public List<Person> ListPerson(Expression<Func<Person, bool>> filter)
    {
      try
      {
        return servicePerson.GetAll(filter).ToList();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public List<Person> GetPersonsCrud(EnumTypeUser typeUser, ref long total, string filter, int count, int page)
    {
      try
      {
        int skip = (count * (page - 1));
        List<Person> detail = null;
        if (typeUser == EnumTypeUser.Support)
        {
          detail = servicePerson.GetAll(p => p.User.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.User.Name).Skip(skip).Take(count).ToList();
          total = servicePerson.GetAll(p => p.User.Name.ToUpper().Contains(filter.ToUpper())).Count();
        }

        else if (typeUser == EnumTypeUser.Administrator)
        {
          detail = servicePerson.GetAll(p => p.TypeUser != EnumTypeUser.Support & p.User.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.User.Name).Skip(skip).Take(count).ToList();
          total = servicePerson.GetAll(p => p.TypeUser != EnumTypeUser.Support & p.User.Name.ToUpper().Contains(filter.ToUpper())).Count();
        }

        else
        {
          detail = servicePerson.GetAll(p => p.TypeUser != EnumTypeUser.Support & p.TypeUser != EnumTypeUser.Administrator & p.User.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.User.Name).Skip(skip).Take(count).ToList();
          total = servicePerson.GetAll(p => p.TypeUser != EnumTypeUser.Support & p.TypeUser != EnumTypeUser.Administrator & p.User.Name.ToUpper().Contains(filter.ToUpper())).Count();
        }

        return detail;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public Person GetPersonCrudOld(string idperson)
    {
      try
      {
        return servicePerson.GetAll(p => p._id == idperson).FirstOrDefault();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

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

    public List<ViewListCompany> ListCompany(ref long total, string filter, int count, int page)
    {
      int skip = (count * (page - 1));
      var detail = serviceCompany.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).Skip(skip).Take(count).ToList();
      total = serviceCompany.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).Skip(skip).Take(count).Count();

      return detail.Select(p => new ViewListCompany() { _id = p._id, Name = p.Name }).ToList();
    }

    public List<Person> ListAll()
    {
      try
      {
        return servicePerson.GetAuthentication(p => p.Status != EnumStatus.Disabled & p.StatusUser != EnumStatusUser.Disabled & p.StatusUser != EnumStatusUser.ErrorIntegration).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string AddPersonUserOld(ViewPersonUser view)
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

        view.User.Password = EncryptServices.GetMD5Hash(view.User.Password);
        view.User.ChangePassword = Manager.Views.Enumns.EnumChangePassword.AccessFirst;
        view.User.Status = EnumStatus.Enabled;
        view.Person.Status = EnumStatus.Enabled;

        if ((authMaristas) || (authPUC))
          view.User.ChangePassword = Manager.Views.Enumns.EnumChangePassword.No;

        view.Person.User = view.User;
        view.Person.User = serviceUser.Insert(view.User);

        servicePerson.Insert(view.Person);

        return "ok";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string UpdatePersonUserOld(ViewPersonUser view)
    {
      try
      {
        var pass = serviceUser.GetAll(p => p._id == view.User._id).SingleOrDefault().Password;
        if (view.User.Password != EncryptServices.GetMD5Hash(pass))
          view.User.Password = EncryptServices.GetMD5Hash(view.User.Password);

        view.Person.User = view.User;
        servicePerson.Update(view.Person, null);
        serviceUser.Update(view.User, null);
        return "update";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    private void DefaultTypeRegisterPerson()
    {
      try
      {
        var parameter = serviceParameter.GetAll(p => p.Name == "typeregisterperson").FirstOrDefault();
        if (parameter == null)
          serviceParameter.Insert(new Parameter()
          {
            Name = "typeregisterperson",
            Content = "0",
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
