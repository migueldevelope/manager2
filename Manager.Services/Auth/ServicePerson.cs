using Manager.Core.Base;
using Manager.Core.Business;
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
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Tools;

namespace Manager.Services.Auth
{
  public class ServicePerson : Repository<Person>, IServicePerson
  {
    private ServiceGeneric<User> userService;
    private ServiceGeneric<Person> personService;
    private ServiceGeneric<Attachments> attachmentService;
    private ServiceGeneric<Schooling> schoolingService;
    private ServiceGeneric<Establishment> establishmentService;
    private ServiceGeneric<Company> companyService;
    private ServiceGeneric<Occupation> occupationService;
    private ServiceSendGrid mailService;
    private ServiceGeneric<Parameter> parameterService;
    private ServiceGeneric<SalaryScale> salaryScaleService;


    #region Constructor
    public ServicePerson(DataContext context) : base(context)
    {
      try
      {
        userService = new ServiceGeneric<User>(context);
        personService = new ServiceGeneric<Person>(context);
        attachmentService = new ServiceGeneric<Attachments>(context);
        mailService = new ServiceSendGrid(context);
        companyService = new ServiceGeneric<Company>(context);
        occupationService = new ServiceGeneric<Occupation>(context);
        parameterService = new ServiceGeneric<Parameter>(context);
        schoolingService = new ServiceGeneric<Schooling>(context);
        establishmentService = new ServiceGeneric<Establishment>(context);
        salaryScaleService = new ServiceGeneric<SalaryScale>(context);
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public void SetUser(IHttpContextAccessor contextAccessor)
    {
      try
      {
        User(contextAccessor);
        personService._user = _user;
        attachmentService._user = _user;
        mailService._user = _user;
        companyService._user = _user;
        occupationService._user = _user;
        userService._user = _user;
        parameterService._user = _user;
        schoolingService._user = _user;
        establishmentService._user = _user;
        salaryScaleService._user = _user;
        DefaultTypeRegisterPerson();
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    private void Init(DataContext context, BaseUser user)
    {
      try
      {
        _user = user;
        personService._user = _user;
        attachmentService._user = _user;
        mailService._user = _user;
        companyService._user = _user;
        occupationService._user = _user;
        userService._user = _user;
        parameterService._user = _user;
        schoolingService._user = _user;
        establishmentService._user = _user;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public BaseUser user { get => _user; set => user = _user; }
    #endregion

    #region Person
    public List<ViewListPersonCrud> GetPersons(ref long total, int count, int page, string filter, EnumTypeUser type)
    {
      try
      {
        switch (type)
        {
          case EnumTypeUser.Support:
          case EnumTypeUser.Administrator:
            total = personService.CountNewVersion(p => p.User.Name.Contains(filter)).Result;
            return personService.GetAllNewVersion(p => p.User.Name.Contains(filter), count, count * (page - 1), "User.Name").Result
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
          case EnumTypeUser.HR:
          case EnumTypeUser.ManagerHR:
            total = personService.CountNewVersion(p => p.User.Name.Contains(filter) && p.TypeUser != EnumTypeUser.Administrator && p.TypeUser != EnumTypeUser.Support).Result;
            return personService.GetAllNewVersion(p => p.User.Name.Contains(filter) && p.TypeUser != EnumTypeUser.Administrator && p.TypeUser != EnumTypeUser.Support, count, count * (page - 1), "User.Name").Result
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
        Person person = personService.GetNewVersion(p => p._id == id).Result;
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
          Schooling = view.User.Schooling == null ? null : schoolingService.GetNewVersion(p => p._id == view.User._id).Result,
          Sex = view.User.Sex,
          ChangePassword = EnumChangePassword.AccessFirst
        };

        if (user.Mail.IndexOf("@maristas.org.br") != -1 || user.Mail.IndexOf("@pucrs.br") != -1)
          user.ChangePassword = EnumChangePassword.No;

        user = userService.InsertNewVersion(user).Result;

        Person person = new Person()
        {
          Company = companyService.GetNewVersion(p => p._id == view.Company._id).Result,
          DateLastOccupation = view.DateLastOccupation,
          DateLastReadjust = view.DateLastReadjust,
          DateResignation = view.DateResignation,
          Establishment = view.Establishment == null ? null : establishmentService.GetNewVersion(p => p._id == view.Establishment._id).Result,
          HolidayReturn = view.HolidayReturn,
          Manager = null,
          MotiveAside = view.MotiveAside,
          Occupation = view.Occupation == null ? null : occupationService.GetNewVersion(p => p._id == view.Occupation._id).Result,
          Registration = view.Registration,
          Salary = view.Salary,
          DocumentManager = null,
          StatusUser = view.StatusUser,
          TypeJourney = view.TypeJourney,
          TypeUser = view.TypeUser,
          User = user
        };

        foreach (var item in view.SalaryScales)
          person.SalaryScales.Add(new SalaryScalePerson() { _idSalaryScale = item._idSalaryScale, NameSalaryScale = item.NameSalaryScale });

        /// TODO: Manager
        person = personService.InsertNewVersion(person).Result;
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
        User user = userService.GetNewVersion(p => p._id == view.User._id).Result;
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
        user.Schooling = view.User.Schooling == null ? null : schoolingService.GetNewVersion(p => p._id == view.User._id).Result;
        user.Sex = view.User.Sex;
        user = userService.UpdateNewVersion(user).Result;

        Person person = personService.GetNewVersion(p => p._id == view._id).Result;
        person.Company = companyService.GetNewVersion(p => p._id == view.Company._id).Result;
        person.DateLastOccupation = view.DateLastOccupation;
        person.DateLastReadjust = view.DateLastReadjust;
        person.DateResignation = view.DateResignation;
        person.Establishment = view.Establishment == null ? null : establishmentService.GetNewVersion(p => p._id == view.Establishment._id).Result;
        person.HolidayReturn = view.HolidayReturn;
        person.Manager = null;
        person.MotiveAside = view.MotiveAside;
        person.Occupation = view.Occupation == null ? null : occupationService.GetNewVersion(p => p._id == view.Occupation._id).Result;
        person.Registration = view.Registration;
        person.Salary = view.Salary;
        person.DocumentManager = null;
        person.StatusUser = view.StatusUser;
        person.TypeJourney = view.TypeJourney;
        person.TypeUser = view.TypeUser;
        person.User = user;
        foreach (var item in view.SalaryScales)
          person.SalaryScales.Add(new SalaryScalePerson() { _idSalaryScale = item._idSalaryScale, NameSalaryScale = item.NameSalaryScale });

        /// TODO: Manager
        person = personService.UpdateNewVersion(person).Result;
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

    public List<SalaryScalePerson> ListSalaryScale(string idoccupation)
    {
      try
      {
        return occupationService.GetAll(p => p._id == idoccupation).FirstOrDefault().SalaryScales
          .Select(p => new SalaryScalePerson
          {
            _idSalaryScale = p._idSalaryScale,
            NameSalaryScale = p.NameSalaryScale
          }).OrderBy(p => p.NameSalaryScale).ToList();
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
        return personService.GetAll(p => p.StatusUser != EnumStatusUser.Disabled & p.StatusUser != EnumStatusUser.ErrorIntegration & p.TypeUser != EnumTypeUser.Administrator & p._id == idperson).ToList().Select(
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
        personService.Insert(person);
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
          authMaristas = person.User.Mail.Substring(user.Mail.IndexOf("@"), user.Mail.Length - user.Mail.IndexOf("@")) == "@maristas.org.br" ? true : false;
          authPUC = person.User.Mail.Substring(user.Mail.IndexOf("@"), user.Mail.Length - user.Mail.IndexOf("@")) == "@pucrs.br" ? true : false;
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
          User = person.User
        };
        foreach (var item in person.SalaryScales)
          model.SalaryScales.Add(new SalaryScalePerson() { _idSalaryScale = item._idSalaryScale, NameSalaryScale = item.NameSalaryScale });



        if (person.Manager != null)
        {
          var manager = personService.GetAll(p => p._id == model.Manager._id).FirstOrDefault();
          if (manager != null)
          {
            if (manager.User != null)
              model.DocumentManager = manager.User.Document;
          }
        }



        return personService.Insert(model);
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
        Person model = personService.GetAll(p => p._id == id).FirstOrDefault();


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
          var manager = personService.GetAll(p => p._id == model.Manager._id).FirstOrDefault();
          if (manager != null)
          {
            if (manager.User != null)
              model.DocumentManager = manager.User.Document;
          }
        }

        personService.Update(model, null);
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
        var pass = personService.GetAll(p => p._id == person._id).SingleOrDefault().User.Password;
        personService.Update(person, null);
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
        personService.Update(person, null);
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
        var person = personService.GetAll(p => p._id == idPerson).SingleOrDefault();
        personService.Update(person, null);
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
        return personService.GetAll(p => p.StatusUser != EnumStatusUser.Disabled & p.StatusUser != EnumStatusUser.ErrorIntegration & p.TypeUser != EnumTypeUser.Administrator & p._id == idPerson).ToList().Select(
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
        return personService.GetAll(p => p.StatusUser != EnumStatusUser.Disabled & p.StatusUser != EnumStatusUser.ErrorIntegration & p.TypeUser != EnumTypeUser.Administrator & p.User.Name.ToUpper().Contains(filter.ToUpper())).ToList()
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

    public List<Person> GetPersons(string idcompany, string filter)
    {
      try
      {
        return personService.GetAll(p => p.Company._id == idcompany & p.StatusUser != EnumStatusUser.Disabled & p.StatusUser != EnumStatusUser.ErrorIntegration & p.TypeUser != EnumTypeUser.Administrator & p.User.Name.ToUpper().Contains(filter.ToUpper())).ToList();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public List<ViewPersonTeam> GetPersonTeam(ref long total, string idPerson, string filter, int count, int page)
    {
      try
      {
        int skip = (count * (page - 1));
        var detail = personService.GetAll(p => p.StatusUser != EnumStatusUser.Disabled & p.StatusUser != EnumStatusUser.ErrorIntegration & p.TypeUser != EnumTypeUser.Administrator & p.Manager._id == idPerson & p._id != idPerson & p.User.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.User.Name).Skip(skip).Take(count).ToList();
        total = personService.GetAll(p => p.StatusUser != EnumStatusUser.Disabled & p.StatusUser != EnumStatusUser.ErrorIntegration & p.TypeUser != EnumTypeUser.Administrator & p.Manager._id == idPerson & p._id != idPerson & p.User.Name.ToUpper().Contains(filter.ToUpper())).Count();

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
        return this.personService.GetAll(p => p.StatusUser != EnumStatusUser.Disabled & p.StatusUser != EnumStatusUser.ErrorIntegration & p.StatusUser != EnumStatusUser.Disabled & p.StatusUser != EnumStatusUser.ErrorIntegration & p._id == idPerson).FirstOrDefault().User.PhotoUrl;
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
        return personService.GetAll(p => p._id == id).FirstOrDefault();
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
        return personService.GetAll(filter).ToList();
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
          detail = personService.GetAll(p => p.User.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.User.Name).Skip(skip).Take(count).ToList();
          total = personService.GetAll(p => p.User.Name.ToUpper().Contains(filter.ToUpper())).Count();
        }

        else if (typeUser == EnumTypeUser.Administrator)
        {
          detail = personService.GetAll(p => p.TypeUser != EnumTypeUser.Support & p.User.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.User.Name).Skip(skip).Take(count).ToList();
          total = personService.GetAll(p => p.TypeUser != EnumTypeUser.Support & p.User.Name.ToUpper().Contains(filter.ToUpper())).Count();
        }

        else
        {
          detail = personService.GetAll(p => p.TypeUser != EnumTypeUser.Support & p.TypeUser != EnumTypeUser.Administrator & p.User.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.User.Name).Skip(skip).Take(count).ToList();
          total = personService.GetAll(p => p.TypeUser != EnumTypeUser.Support & p.TypeUser != EnumTypeUser.Administrator & p.User.Name.ToUpper().Contains(filter.ToUpper())).Count();
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
        return personService.GetAll(p => p._id == idperson).FirstOrDefault();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public List<Occupation> ListOccupation(ref long total, string filter, int count, int page)
    {
      int skip = (count * (page - 1));
      var detail = occupationService.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Name).Skip(skip).Take(count).ToList();
      total = occupationService.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).Count();

      return detail;
    }

    public List<Person> ListManager(ref long total, string filter, int count, int page)
    {
      int skip = (count * (page - 1));
      var detail = personService.GetAll(p => p.TypeUser != EnumTypeUser.Employee & p.TypeUser != EnumTypeUser.HR & p.StatusUser != EnumStatusUser.Disabled & p.StatusUser != EnumStatusUser.ErrorIntegration & p.TypeUser != EnumTypeUser.Administrator & p.User.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.User.Name).ToList();
      //var detail = personService.GetAll(p => p.StatusUser != EnumStatusUser.Disabled & p.StatusUser != EnumStatusUser.ErrorIntegration & p.TypeUser != EnumTypeUser.Administrator & p.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Name).Skip(skip).Take(count).ToList();
      //total = personService.GetAll(p => p.StatusUser != EnumStatusUser.Disabled & p.StatusUser != EnumStatusUser.ErrorIntegration & p.TypeUser != EnumTypeUser.Administrator & p.Name.ToUpper().Contains(filter.ToUpper())).Count();

      return detail;
    }

    public List<Company> ListCompany(ref long total, string filter, int count, int page)
    {
      int skip = (count * (page - 1));
      var detail = companyService.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).Skip(skip).Take(count).ToList();
      total = companyService.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).Skip(skip).Take(count).Count();

      return detail;
    }

    public void SetUser(BaseUser baseUser)
    {
      try
      {
        _user = baseUser;
        personService._user = _user;
        attachmentService._user = _user;
        mailService._user = _user;
        companyService._user = _user;
        occupationService._user = _user;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<Person> ListAll()
    {
      try
      {
        return personService.GetAuthentication(p => p.Status != EnumStatus.Disabled & p.StatusUser != EnumStatusUser.Disabled & p.StatusUser != EnumStatusUser.ErrorIntegration).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string AddPersonUser(ViewPersonUser view)
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
        view.Person.User = userService.Insert(view.User);

        personService.Insert(view.Person);

        return "ok";
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string UpdatePersonUser(ViewPersonUser view)
    {
      try
      {
        var pass = userService.GetAll(p => p._id == view.User._id).SingleOrDefault().Password;
        if (view.User.Password != EncryptServices.GetMD5Hash(pass))
          view.User.Password = EncryptServices.GetMD5Hash(view.User.Password);

        view.Person.User = view.User;
        personService.Update(view.Person, null);
        userService.Update(view.User, null);
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
        var parameter = parameterService.GetAll(p => p.Name == "typeregisterperson").FirstOrDefault();
        if (parameter == null)
          parameterService.Insert(new Parameter()
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
