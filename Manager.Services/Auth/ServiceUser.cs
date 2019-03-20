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
  public class ServiceUser : Repository<User>, IServiceUser
  {
    private ServiceGeneric<User> userService;
    private ServiceGeneric<Person> personService;
    private ServiceGeneric<Attachments> attachmentService;
    private ServiceGeneric<Company> companyService;
    private ServiceGeneric<Occupation> occupationService;
    private ServiceGeneric<OnBoarding> onboardingService;
    private ServiceGeneric<Monitoring> monitoringService;
    private ServiceGeneric<Checkpoint> checkpointService;
    private ServiceGeneric<Plan> planService;
    private ServiceGeneric<Log> logService;
    private ServiceSendGrid mailService;

    #region Constructor
    public ServiceUser(DataContext context) : base(context)
    {
      try
      {
        userService = new ServiceGeneric<User>(context);
        attachmentService = new ServiceGeneric<Attachments>(context);
        mailService = new ServiceSendGrid(context);
        companyService = new ServiceGeneric<Company>(context);
        occupationService = new ServiceGeneric<Occupation>(context);
        personService = new ServiceGeneric<Person>(context);
        onboardingService = new ServiceGeneric<OnBoarding>(context);
        monitoringService = new ServiceGeneric<Monitoring>(context);
        checkpointService = new ServiceGeneric<Checkpoint>(context);
        planService = new ServiceGeneric<Plan>(context);
        logService = new ServiceGeneric<Log>(context);
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
        userService._user = _user;
        attachmentService._user = _user;
        mailService._user = _user;
        companyService._user = _user;
        occupationService._user = _user;
        personService._user = _user;
        onboardingService._user = _user;
        monitoringService._user = _user;
        checkpointService._user = _user;
        planService._user = _user;
        logService._user = _user;
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
        userService._user = _user;
        attachmentService._user = _user;
        mailService._user = _user;
        companyService._user = _user;
        occupationService._user = _user;
        personService._user = _user;
        logService._user = _user;
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    #endregion

    #region User
    public List<ViewListUser> GetUsers(ref long total, int count, int page, string filter, EnumTypeUser type)
    {
      try
      {
        switch (type)
        {
          case EnumTypeUser.Support:
          case EnumTypeUser.Administrator:
            total = userService.CountNewVersion(p => p.Name.Contains(filter)).Result;
            return userService.GetAllNewVersion(p => p.Name.Contains(filter)).Result
            .Select(x => new ViewListUser()
            {
              _id = x._id,
              Document = x.Document,
              Mail = x.Mail,
              Name = x.Name,
              Phone = x.Phone
            }).ToList();
          default:
            // TODO: colocar o flag de usuário administrativo
            total = userService.CountNewVersion(p => p.Name.Contains(filter)).Result;
            return userService.GetAllNewVersion(p => p.Name.Contains(filter)).Result
            .Select(x => new ViewListUser()
            {
              _id = x._id,
              Document = x.Document,
              Mail = x.Mail,
              Name = x.Name,
              Phone = x.Phone
            }).ToList();
        }
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public ViewCrudUser GetUserCrud(string iduser)
    {
      try
      {
        User user =  userService.GetNewVersion(p => p._id == iduser).Result;
        return new ViewCrudUser()
        {
          _id = user._id,
          Document = user.Document,
          Mail = user.Mail,
          Name = user.Name,
          Phone = user.Phone,
          DateAdm = user.DateAdm,
          DateBirth = user.DateBirth,
          DocumentCTPF = user.DocumentCTPF,
          DocumentID = user.DocumentCTPF,
          PhoneFixed = user.PhoneFixed,
          Schooling = new ViewListSchooling { _id = user.Schooling._id, Name = user.Schooling.Name, Order = user.Schooling.Order},
          Sex = user.Sex,
          PhotoUrl = user.PhotoUrl,
          Password = string.Empty
        };
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public ViewCrudUser NewUser(ViewCrudUser view)
    {
      try
      {
        User user = new User()
        {
          DateAdm = view.DateAdm,
          DateBirth = view.DateBirth,
          Document = view.Document,
          DocumentCTPF = view.DocumentCTPF,
          DocumentID = view.DocumentID,
          Mail = view.Mail,
          Name = view.Name,
          Password = EncryptServices.GetMD5Hash(view.Password),
          ForeignForgotPassword = string.Empty,
          Coins = 0,
          Phone = view.Phone,
          PhoneFixed = view.PhoneFixed,
          PhotoUrl = view.PhotoUrl,
          Schooling = new Schooling() { _id = view.Schooling._id, Name = view.Name, Order = view.Schooling.Order },
          Sex = view.Sex,
          ChangePassword = EnumChangePassword.AccessFirst
        };

        if (user.Mail.IndexOf("@maristas.org.br") != -1 || user.Mail.IndexOf("@pucrs.br") != -1)
          user.ChangePassword = EnumChangePassword.No;

        user = userService.InsertNewVersion(user).Result;
        return new ViewCrudUser()
        {
          _id = user._id,
          Document = user.Document,
          Mail = user.Mail,
          Name = user.Name,
          Phone = user.Phone,
          DateAdm = user.DateAdm,
          DateBirth = user.DateBirth,
          DocumentCTPF = user.DocumentCTPF,
          DocumentID = user.DocumentCTPF,
          PhoneFixed = user.PhoneFixed,
          Schooling = new ViewListSchooling { _id = user.Schooling._id, Name = user.Schooling.Name, Order = user.Schooling.Order },
          Sex = user.Sex,
          PhotoUrl = user.PhotoUrl,
          Password = string.Empty
        };
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public ViewCrudUser UpdateUser(ViewCrudUser view)
    {
      try
      {
        User user = userService.GetNewVersion(p => p._id == view._id).Result;
        user.DateAdm = view.DateAdm;
        user.DateBirth = view.DateBirth;
        user.Document = view.Document;
        user.DocumentCTPF = view.DocumentCTPF;
        user.DocumentID = view.DocumentID;
        user.Mail = view.Mail;
        user.Name = view.Name;
        user.Phone = view.Phone;
        user.PhoneFixed = view.PhoneFixed;
        user.PhotoUrl = view.PhotoUrl;
        user.Schooling = new Schooling() { _id = view.Schooling._id, Name = view.Name, Order = view.Schooling.Order };
        user.Sex = view.Sex;
        user = userService.UpdateNewVersion(user).Result;
        return new ViewCrudUser()
        {
          _id = user._id,
          Document = user.Document,
          Mail = user.Mail,
          Name = user.Name,
          Phone = user.Phone,
          DateAdm = user.DateAdm,
          DateBirth = user.DateBirth,
          DocumentCTPF = user.DocumentCTPF,
          DocumentID = user.DocumentCTPF,
          PhoneFixed = user.PhoneFixed,
          Schooling = new ViewListSchooling { _id = user.Schooling._id, Name = user.Schooling.Name, Order = user.Schooling.Order },
          Sex = user.Sex,
          PhotoUrl = user.PhotoUrl,
          Password = string.Empty
        };
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    #endregion

    #region Password
    public string AlterPassword(ViewAlterPass resetPass, string idUser)
    {
      try
      {
        User user = userService.GetNewVersion(p => p._id == idUser).Result;
        string oldPass = EncryptServices.GetMD5Hash(resetPass.OldPassword);
        if (user.ChangePassword == EnumChangePassword.AlterPass && user.Password != oldPass)
          return "error_old_password";
        string newPass = EncryptServices.GetMD5Hash(resetPass.NewPassword);
        user.Password = newPass;
        user.ChangePassword = EnumChangePassword.No;
        user = userService.UpdateNewVersion(user).Result;
        return "Password changed!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public string AlterPasswordForgot(ViewAlterPass resetPass, string foreign)
    {
      try
      {
        User user = userService.GetFreeNewVersion(p => p.ForeignForgotPassword == foreign).Result;
        if (user == null)
          return "error_valid";
        userService._user = new BaseUser()
        {
          _idAccount = user._idAccount,
          NamePerson = user.Name,
          Mail = user.Mail,
          _idPerson = user._id
        };
        string newPass = EncryptServices.GetMD5Hash(resetPass.NewPassword);
        user.Password = newPass;
        user.ChangePassword = EnumChangePassword.No;
        user.ForeignForgotPassword = string.Empty;
        user = userService.UpdateNewVersion(user).Result;
        return "Password changed!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public async Task<string> ForgotPassword(string mail, ViewForgotPassword forgotPassword, string pathSendGrid)
    {
      try
      {
        User user = userService.GetFreeNewVersion(p => p.Mail == mail).Result;
        if (user == null)
          return "error_email";
        string guid = string.Concat(Guid.NewGuid().ToString(), user._id);
        string message = "";
        if (forgotPassword.Message == string.Empty)
          message = string.Format("Hello {0}<br>To reset your password click the link below<br>http://{1}/evaluation_f/forgot", user.Name, forgotPassword.Link);
        else
          message = forgotPassword.Message.Replace("{User}", user.Name).Replace("{Link}", forgotPassword.Link + "/" + guid);

        userService._user = new BaseUser()
        {
          _idAccount = user._idAccount,
          NamePerson = user.Name,
          Mail = user.Mail,
          _idPerson = user._id
        };
        mailService._user = userService._user;

        MailLog sendMail = new MailLog
        {
          From = new MailLogAddress("suporte@jmsoft.com.br", "Notificação do Analisa"),
          To = new List<MailLogAddress>()
            { new MailLogAddress(user.Mail, user.Name) },
          Priority = EnumPriorityMail.Low,
          _idPerson = user._id,
          NamePerson = user.Name,
          Body = message,
          StatusMail = EnumStatusMail.Sended,
          Included = DateTime.Now,
          Subject = forgotPassword.Subject
        };
        sendMail = mailService.InsertNewVersion(sendMail).Result;
        user.ChangePassword = EnumChangePassword.ForgotPassword;
        user.ForeignForgotPassword = guid;
        user = userService.UpdateNewVersion(user).Result;
        await mailService.Send(sendMail, pathSendGrid);
        return "Email sent successfully!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    #endregion

    #region Old
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
          Sex = user.Sex
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

    public List<User> GetUsersCrudOld(EnumTypeUser typeUser, ref long total, string filter, int count, int page)
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

    public User GetUserCrudOld(string iduser)
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

    public List<Person> ListPerson(string iduser, ref long total, string filter, int count, int page)
    {
      int skip = (count * (page - 1));
      var detail = personService.GetAll(p => p.User._id == iduser & p.User.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.User.Name).Skip(skip).Take(count).ToList();
      total = personService.GetAll(p => p.User.Name.ToUpper().Contains(filter.ToUpper())).Count();

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
    #endregion




  }
}
