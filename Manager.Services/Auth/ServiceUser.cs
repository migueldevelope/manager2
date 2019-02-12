using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.Enumns;
using Manager.Core.Interfaces;
using Manager.Core.Views;
using Manager.Data;
using Manager.Services.Commons;
using Manager.Services.Specific;
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
  public class ServiceUser : Repository<User>, IServiceUser
  {
    private ServiceGeneric<User> userService;
    private ServiceGeneric<Person> personService;
    private ServiceGeneric<Attachments> attachmentService;
    private ServiceGeneric<Company> companyService;
    private ServiceGeneric<Occupation> occupationService;
    private ServiceSendGrid mailService;

    public BaseUser user { get => _user; set => user = _user; }

    public void SetUser(IHttpContextAccessor contextAccessor)
    {
      try
      {
        User(contextAccessor);
        userService._user = _user;
        attachmentService._user = _user;
        mailService._user = _user;
        companyService._user = _user;
        occupationService._user = _user;
        personService._user = _user;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public ServiceUser(DataContext context)
      : base(context)
    {
      try
      {
        userService = new ServiceGeneric<User>(context);
        attachmentService = new ServiceGeneric<Attachments>(context);
        mailService = new ServiceSendGrid(context);
        companyService = new ServiceGeneric<Company>(context);
        occupationService = new ServiceGeneric<Occupation>(context);
        personService = new ServiceGeneric<Person>(context);
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
        userService._user = _user;
        attachmentService._user = _user;
        mailService._user = _user;
        companyService._user = _user;
        occupationService._user = _user;
        personService._user = _user;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string ScriptPerson(string idaccount)
    {
      try
      {
        var persons = personService.GetAuthentication(p => p.Status == EnumStatus.Enabled & p._idAccount == idaccount & p.User == null).ToList();
        foreach (var item in persons)
        {
          var user = new User()
          {
            _id = ObjectId.GenerateNewId().ToString(),
            _idAccount = item._idAccount,
            Status = EnumStatus.Enabled,
            ChangePassword = item.ChangePassword,
            Coins = item.Coins,
            DateAdm = item.DateAdm,
            DateBirth = item.DateBirth,
            Document = item.Document,
            DocumentCTPF = item.DocumentCTPF,
            DocumentID = item.DocumentID,
            ForeignForgotPassword = item.ForeignForgotPassword,
            HolidayReturn = item.HolidayReturn,
            Mail = item.Mail,
            MotiveAside = item.MotiveAside,
            Name = item.Name,
            Password = item.Password,
            Phone = item.Phone,
            PhoneFixed = item.PhoneFixed,
            PhotoUrl = item.PhotoUrl,
            Schooling = item.Schooling,
            Sex = item.Sex
          };

          item.User = userService.InsertAccount(user);
          personService.UpdateAccount(item, null);
          UpdateManager(user, item._id);
        }
        return "ok";

      }
      catch (Exception e)
      {
        throw e;
      }
    }

    private async Task UpdateManager(User user, string idperson)
    {
      var managers = personService.GetAuthentication(p => p.Status == EnumStatus.Enabled & p._idAccount == user._idAccount).ToList();
      foreach(var item in managers)
      {
        try
        {
          if (item.Manager._id == idperson)
          {
            item.Manager.User = user;
            personService.UpdateAccount(item, null);
          }
          else if (item.Manager.Manager._id == idperson)
          {
            item.Manager.Manager.User = user;
            personService.UpdateAccount(item, null);
          }
          else if (item.Manager.Manager.Manager._id == idperson)
          {
            item.Manager.Manager.Manager.User = user;
            personService.UpdateAccount(item, null);
          }
          else if (item.Manager.Manager._id == idperson)
          {
            item.Manager.User = user;
            personService.UpdateAccount(item, null);
          }
          else if (item.Manager.Manager.Manager.Manager._id == idperson)
          {
            item.Manager.Manager.Manager.Manager.User = user;
            personService.UpdateAccount(item, null);
          }
          else if (item.Manager.Manager._id == idperson)
          {
            item.Manager.User = user;
            personService.UpdateAccount(item, null);
          }
          else if (item.Manager.Manager.Manager.Manager.Manager._id == idperson)
          {
            item.Manager.Manager.Manager.Manager.Manager.Manager.User = user;
            personService.UpdateAccount(item, null);
          }
          else if (item.Manager.Manager.Manager.Manager.Manager.Manager.Manager._id == idperson)
          {
            item.Manager.Manager.Manager.Manager.Manager.Manager.User = user;
            personService.UpdateAccount(item, null);
          }
          else if (item.Manager.Manager.Manager.Manager.Manager.Manager.Manager.Manager._id == idperson)
          {
            item.Manager.Manager.Manager.Manager.Manager.Manager.Manager.User = user;
            personService.UpdateAccount(item, null);
          }


        }
        catch
        {

        }
        
      }
    }

    public User NewUser(User user)
    {
      try
      {
        user.Password = EncryptServices.GetMD5Hash(user.Password);
        user.Status = EnumStatus.Enabled;
        userService.Insert(user);
        return user;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public User NewUserView(User user)
    {

      try
      {
        var authMaristas = false;
        var authPUC = false;
        try
        {
          authMaristas = user.Mail.Substring(user.Mail.IndexOf("@"), user.Mail.Length - user.Mail.IndexOf("@")) == "@maristas.org.br" ? true : false;
          authPUC = user.Mail.Substring(user.Mail.IndexOf("@"), user.Mail.Length - user.Mail.IndexOf("@")) == "@pucrs.br" ? true : false;
        }
        catch (Exception)
        {

        }




        User model = new User()
        {
          Name = user.Name,

          ChangePassword = EnumChangePassword.AccessFirst,
          DateAdm = user.DateAdm,
          DateBirth = user.DateBirth,
          Document = user.Document,
          Phone = user.Phone,
          Mail = user.Mail,
          Status = EnumStatus.Enabled,
          Schooling = user.Schooling,
          PhoneFixed = user.PhoneFixed,
          DocumentID = user.DocumentID,
          DocumentCTPF = user.DocumentCTPF,
          Sex = user.Sex,
          HolidayReturn = user.HolidayReturn,
          MotiveAside = user.MotiveAside
        };

        if (user.Password == string.Empty)
          model.Password = EncryptServices.GetMD5Hash(user.Document);
        else
          model.Password = EncryptServices.GetMD5Hash(user.Password);



        if ((authMaristas) || (authPUC))
          model.ChangePassword = EnumChangePassword.No;

        return userService.Insert(model);
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }



    public User UpdateUserView(User user)
    {
      try
      {
        var pass = userService.GetAll(p => p._id == user._id).SingleOrDefault().Password;
        userService.Update(user, null);
        return user;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public User UpdateUser(User user)
    {
      try
      {
        var pass = userService.GetAll(p => p._id == user._id).SingleOrDefault().Password;
        if (user.Password != EncryptServices.GetMD5Hash(pass))
          user.Password = EncryptServices.GetMD5Hash(user.Password);
        userService.Update(user, null);
        return user;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public void SetPhoto(string idUser, string url)
    {
      try
      {
        var user = userService.GetAll(p => p._id == idUser).SingleOrDefault();
        user.PhotoUrl = url;
        userService.Update(user, null);
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public List<User> GetUsers(string idcompany, string filter)
    {
      try
      {
        return userService.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).ToList();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }


    public string GetPhoto(string idUser)
    {
      try
      {
        return this.userService.GetAll(p => p._id == idUser).FirstOrDefault().PhotoUrl;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public User GetAuthentication(string mail, string password)
    {
      try
      {
        return userService.GetAuthentication(p => p.Status == EnumStatus.Enabled & p.Mail == mail && p.Password == password).SingleOrDefault();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string AlterPassword(ViewAlterPass resetPass, string idUser)
    {
      try
      {
        var user = userService.GetAll(p => p._id == idUser).FirstOrDefault();
        var oldPass = EncryptServices.GetMD5Hash(resetPass.OldPassword);
        if (user.ChangePassword == EnumChangePassword.AlterPass)
        {
          if (user.Password != oldPass)
            return "error_old_password";
        }
        var newPass = EncryptServices.GetMD5Hash(resetPass.NewPassword);
        user.Password = newPass;
        user.ChangePassword = EnumChangePassword.No;
        userService.Update(user, null);
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
        var user = userService.GetAuthentication(p => p.ForeignForgotPassword == foreign).FirstOrDefault();
        var userBase = new BaseUser()
        {
          _idAccount = user._idAccount,
          NamePerson = user.Name,
          Mail = user.Mail,
          _idPerson = user._id
        };

        Init(context, userBase);
        if (user == null)
          return "error_valid";
        var newPass = EncryptServices.GetMD5Hash(resetPass.NewPassword);
        user.Password = newPass;
        user.ChangePassword = EnumChangePassword.No;
        user.ForeignForgotPassword = "";
        userService.Update(user, null);
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
        var user = userService.GetAuthentication(p => p.Mail == mail).FirstOrDefault();

        var userBase = new BaseUser()
        {
          _idAccount = user._idAccount,
          NamePerson = user.Name,
          Mail = user.Mail,
          _idPerson = user._id
        };

        Init(context, userBase);
        var guid = Guid.NewGuid().ToString() + user._id.ToString();
        var message = "";
        if (forgotPassword.Message == string.Empty)
        {
          message = "Hello " + user.Name;
          message += "<br> To reset your password click the link below";
          message += "<br> " + "http://" + forgotPassword.Link + "/evaluation_f/forgot";
        }
        else
        {
          message = forgotPassword.Message.Replace("{User}", user.Name);
          message = message.Replace("{Link}", forgotPassword.Link + "/" + guid);
        }
        var sendMail = new MailLog
        {
          From = new MailLogAddress("suporte@jmsoft.com.br", "Notificação do Analisa"),
          To = new List<MailLogAddress>(){
                    new MailLogAddress(user.Mail, user.Name)
                },
          Priority = EnumPriorityMail.Low,
          _idPerson = user._id,
          NamePerson = user.Name,
          Body = message,
          StatusMail = EnumStatusMail.Sended,
          Included = DateTime.Now,
          Subject = forgotPassword.Subject
        };
        var mailObj = mailService.Insert(sendMail);
        user.ChangePassword = EnumChangePassword.ForgotPassword;
        user.ForeignForgotPassword = guid;

        await mailService.Send(mailObj, pathSendGrid);

        userService.Update(user, null);
        return "ok";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public User GetUser(string id)
    {
      try
      {
        return userService.GetAll(p => p._id == id).FirstOrDefault();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public List<User> ListUser(Expression<Func<User, bool>> filter)
    {
      try
      {
        return userService.GetAll(filter).ToList();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public List<User> GetUsersCrud(EnumTypeUser typeUser, ref long total, string filter, int count, int page)
    {
      try
      {
        int skip = (count * (page - 1));
        List<User> detail = null;
        if (typeUser == EnumTypeUser.Support)
        {
          detail = userService.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Name).Skip(skip).Take(count).ToList();
          total = userService.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).Count();
        }

        else if (typeUser == EnumTypeUser.Administrator)
        {
          detail = userService.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Name).Skip(skip).Take(count).ToList();
          total = userService.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).Count();
        }

        else
        {
          detail = userService.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Name).Skip(skip).Take(count).ToList();
          total = userService.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).Count();
        }

        return detail;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public User GetUserCrud(string iduser)
    {
      try
      {
        return userService.GetAll(p => p._id == iduser).FirstOrDefault();
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

    public List<User> ListManager(ref long total, string filter, int count, int page)
    {
      int skip = (count * (page - 1));
      var detail = userService.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Name).ToList();
      //var detail = userService.GetAll(p => p.StatusUser != EnumStatusUser.Disabled & p.StatusUser != EnumStatusUser.ErrorIntegration & p.TypeUser != EnumTypeUser.Administrator & p.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Name).Skip(skip).Take(count).ToList();
      //total = userService.GetAll(p => p.StatusUser != EnumStatusUser.Disabled & p.StatusUser != EnumStatusUser.ErrorIntegration & p.TypeUser != EnumTypeUser.Administrator & p.Name.ToUpper().Contains(filter.ToUpper())).Count();

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
        userService._user = _user;
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

    public List<User> ListAll()
    {
      try
      {
        return userService.GetAuthentication(p => p.Status != EnumStatus.Disabled).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }
  }
}
