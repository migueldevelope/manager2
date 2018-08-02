using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.Enumns;
using Manager.Core.Interfaces;
using Manager.Core.Views;
using Manager.Data;
using Manager.Services.Commons;
using Manager.Services.Specific;
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
    private ServiceGeneric<Person> personService;
    private ServiceGeneric<Attachment> attachmentService;
    private ServiceGeneric<Company> companyService;
    private ServiceGeneric<Occupation> occupationService;
    private ServiceSendGrid mailService;

    public BaseUser user { get => _user; set => user = _user; }

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

      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public ServicePerson(DataContext context)
      : base(context)
    {
      try
      {
        personService = new ServiceGeneric<Person>(context);
        attachmentService = new ServiceGeneric<Attachment>(context);
        mailService = new ServiceSendGrid(context);
        companyService = new ServiceGeneric<Company>(context);
        occupationService = new ServiceGeneric<Occupation>(context);
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
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
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public ViewPersonHead Head(string idperson)
    {
      try
      {
        return personService.GetAll(p => p.StatusUser != EnumStatusUser.Disabled & p._id == idperson).ToList().Select(
        person => new ViewPersonHead
        {
          IdPerson = idperson,
          NamePerson = person.Name,
          Occupation = person.Occupation,
          ActionFocus = ""
        }).FirstOrDefault();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public Person NewPerson(Person person)
    {
      try
      {
        person.Password = EncryptServices.GetMD5Hash(person.Password);
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

    public string NewPersonView(Person person)
    {
      try
      {
        Person model = new Person()
        {
          Name = person.Name,
          ChangePassword = EnumChangePassword.No,
          TypeUser = person.TypeUser,
          DateAdm = person.DateAdm,
          DateBirth = person.DateBirth,
          Document = person.Document,
          Manager = person.Manager,
          Occupation = person.Occupation,
          Phone = person.Phone,
          Mail = person.Mail,
          Registration = person.Registration,
          Company = person.Company,
          Password = EncryptServices.GetMD5Hash(person.Document),
          StatusUser = person.StatusUser,
          Status = EnumStatus.Enabled
        };

        if (person.Manager != null)
          model.DocumentManager = person.Manager.Document;

        personService.Insert(model);
        return "ok";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }


    public string UpdatePerson(string id, ViewPersonsCrud person)
    {
      try
      {
        Person model = personService.GetAll(p => p._id == id).FirstOrDefault();

        model.Name = person.Name;
        model.TypeUser = person.TypeUser;
        model.DateAdm = person.DateAdm;
        model.DateBirth = person.DateBirth;
        model.Document = person.Document;
        model.Manager = person.Manager;
        model.Occupation = person.Occupation;
        model.Phone = person.Phone;
        model.Mail = person.Mail;
        model.Registration = person.Registration;
        model.Company = person.Company;
        model.StatusUser = person.StatusUser;

        if (person.Manager != null)
          model.DocumentManager = person.Manager.Document;

        personService.Update(model, null);
        return "ok";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public Person UpdatePerson(Person person)
    {
      try
      {
        var pass = personService.GetAll(p => p._id == person._id).SingleOrDefault().Password;
        if (person.Password != EncryptServices.GetMD5Hash(pass))
          person.Password = EncryptServices.GetMD5Hash(person.Password);
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
        person.PhotoUrl = url;
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
        return personService.GetAll(p => p.StatusUser != EnumStatusUser.Disabled & p._id == idPerson).ToList().Select(
        detail => new ViewPersonDetail()
        {
          Birth = detail.DateBirth,
          Mail = detail.Mail,
          Name = detail.Name,
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
        return personService.GetAll(p => p.StatusUser != EnumStatusUser.Disabled & p.Name.ToUpper().Contains(filter.ToUpper())).ToList()
          .Select(item => new ViewPersonList()
          {
            IdPerson = item._id,
            NamePerson = item.Name
          }).ToList();
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
        var detail = personService.GetAll(p => p.StatusUser != EnumStatusUser.Disabled & p.Manager._id == idPerson & p._id != idPerson & p.Name.ToUpper().Contains(filter.ToUpper())).ToList();
        total = detail.Count();

        return detail.Skip(skip).Take(count).ToList()
          .Select(item => new ViewPersonTeam()
          {
            Name = item.Name,
            IdPerson = item._id,
            Occupation = item.Occupation,
            DataAdm = item.DateAdm
          }).ToList();
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
        return this.personService.GetAll(p => p.StatusUser != EnumStatusUser.Disabled & p.StatusUser != EnumStatusUser.Disabled & p._id == idPerson).FirstOrDefault().PhotoUrl;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public Person GetAuthentication(string mail, string password)
    {
      try
      {
        return personService.GetAuthentication(p => p.StatusUser != EnumStatusUser.Disabled & p.Mail == mail && p.Password == password).SingleOrDefault();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string AlterPassword(ViewAlterPass resetPass, string idPerson)
    {
      try
      {
        var person = personService.GetAll(p => p._id == idPerson).FirstOrDefault();
        var oldPass = EncryptServices.GetMD5Hash(resetPass.OldPassword);
        if (person.ChangePassword == EnumChangePassword.AlterPass)
        {
          if (person.Password != oldPass)
            return "error_old_password";
        }
        var newPass = EncryptServices.GetMD5Hash(resetPass.NewPassword);
        person.Password = newPass;
        person.ChangePassword = EnumChangePassword.No;
        personService.Update(person, null);
        return "ok";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string AlterPasswordForgot(ViewAlterPass resetPass, string foreign)
    {
      try
      {
        DataContext context = _context;
        var person = personService.GetAuthentication(p => p.ForeignForgotPassword == foreign).FirstOrDefault();
        var user = new BaseUser()
        {
          _idAccount = person._idAccount,
          NamePerson = person.Name,
          Mail = person.Mail,
          _idPerson = person._id
        };

        Init(context, user);
        if (person == null)
          return "error_valid";
        var newPass = EncryptServices.GetMD5Hash(resetPass.NewPassword);
        person.Password = newPass;
        person.ChangePassword = EnumChangePassword.No;
        person.ForeignForgotPassword = "";
        personService.Update(person, null);
        return "ok";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public async Task<string> ForgotPassword(string mail, ViewForgotPassword forgotPassword, string pathSendGrid)
    {
      try
      {
        DataContext context = base._context;
        var person = personService.GetAuthentication(p => p.Mail == mail).FirstOrDefault();

        var user = new BaseUser()
        {
          _idAccount = person._idAccount,
          NamePerson = person.Name,
          Mail = person.Mail,
          _idPerson = person._id
        };

        Init(context, user);
        var guid = Guid.NewGuid().ToString() + person._id.ToString();
        var message = "";
        if (forgotPassword.Message == string.Empty)
        {
          message = "Hello " + person.Name;
          message += "<br> To reset your password click the link below";
          message += "<br> " + "http://" + forgotPassword.Link + "/evaluation_f/forgot";
        }
        else
        {
          message = forgotPassword.Message.Replace("{Person}", person.Name);
          message = message.Replace("{Link}", forgotPassword.Link + "/" + guid);
        }
        var sendMail = new MailLog
        {
          From = new MailLogAddress("suporte@jmsoft.com.br", "Suporte"),
          To = new List<MailLogAddress>(){
                    new MailLogAddress(person.Mail, person.Name)
                },
          Priority = EnumPriorityMail.Low,
          _idPerson = person._id,
          NamePerson = person.Name,
          Body = message,
          StatusMail = EnumStatusMail.Sended,
          Included = DateTime.Now,
          Subject = forgotPassword.Subject
        };
        var mailObj = mailService.Insert(sendMail);
        person.ChangePassword = EnumChangePassword.ForgotPassword;
        person.ForeignForgotPassword = guid;

        await mailService.Send(mailObj, pathSendGrid);

        personService.Update(person, null);
        return "ok";
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

    public List<Person> GetPersonsCrud(ref long total, string filter, int count, int page)
    {
      try
      {
        int skip = (count * (page - 1));
        var detail = personService.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).ToList();
        total = detail.Count();

        return detail.Skip(skip).Take(count).ToList();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public Person GetPersonCrud(string idperson)
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
      var detail = occupationService.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).ToList();
      total = detail.Count();

      return detail.Skip(skip).Take(count).ToList();
    }

    public List<Person> ListManager(ref long total, string filter, int count, int page)
    {
      int skip = (count * (page - 1));
      var detail = personService.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).ToList();
      total = detail.Count();

      return detail.Skip(skip).Take(count).ToList();
    }

    public List<Company> ListCompany(ref long total, string filter, int count, int page)
    {
      int skip = (count * (page - 1));
      var detail = companyService.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).ToList();
      total = detail.Count();

      return detail.Skip(skip).Take(count).ToList();
    }
  }
}
