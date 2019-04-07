using Manager.Core.Base;
using Manager.Core.Business;
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
using System.Threading.Tasks;
using Tools;

namespace Manager.Services.Auth
{
  public class ServiceUser : Repository<User>, IServiceUser
  {
    private ServiceGeneric<Attachments> serviceAttachment;
    private ServiceGeneric<Checkpoint> serviceCheckpoint;
    private ServiceGeneric<Company> serviceCompany;
    private ServiceGeneric<Log> serviceLog;
    private ServiceSendGrid serviceMail;
    private ServiceGeneric<Monitoring> serviceMonitoring;
    private ServiceGeneric<Occupation> serviceOccupation;
    private ServiceGeneric<OnBoarding> serviceOnboarding;
    private ServiceGeneric<Person> servicePerson;
    private ServiceGeneric<Plan> servicePlan;
    private ServiceGeneric<User> serviceUser;

    #region Constructor
    public ServiceUser(DataContext context) : base(context)
    {
      try
      {
        serviceAttachment = new ServiceGeneric<Attachments>(context);
        serviceCompany = new ServiceGeneric<Company>(context);
        serviceCheckpoint = new ServiceGeneric<Checkpoint>(context);
        serviceLog = new ServiceGeneric<Log>(context);
        serviceMail = new ServiceSendGrid(context);
        serviceMonitoring = new ServiceGeneric<Monitoring>(context);
        serviceOccupation = new ServiceGeneric<Occupation>(context);
        serviceOnboarding = new ServiceGeneric<OnBoarding>(context);
        servicePerson = new ServiceGeneric<Person>(context);
        servicePlan = new ServiceGeneric<Plan>(context);
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
      serviceCheckpoint._user = _user;
      serviceCompany._user = _user;
      serviceLog._user = _user;
      serviceMail._user = _user;
      serviceMonitoring._user = _user;
      serviceOccupation._user = _user;
      serviceOnboarding._user = _user;
      servicePerson._user = _user;
      servicePlan._user = _user;
      serviceUser._user = _user;
    }
    public void SetUser(BaseUser user)
    {
      serviceAttachment._user = user;
      serviceCheckpoint._user = user;
      serviceCompany._user = user;
      serviceLog._user = user;
      serviceMail._user = user;
      serviceMonitoring._user = user;
      serviceOccupation._user = user;
      serviceOnboarding._user = user;
      servicePerson._user = user;
      servicePlan._user = user;
      serviceUser._user = user;
    }
    #endregion

    #region User
    public List<ViewListUser> List(ref long total, int count, int page, string filter, EnumTypeUser type)
    {
      try
      {
        switch (type)
        {
          case EnumTypeUser.Support:
          case EnumTypeUser.Administrator:
            total = serviceUser.CountNewVersion(p => p.Name.Contains(filter)).Result;
            return serviceUser.GetAllNewVersion(p => p.Name.Contains(filter)).Result
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
            total = serviceUser.CountNewVersion(p => p.Name.Contains(filter)).Result;
            return serviceUser.GetAllNewVersion(p => p.Name.Contains(filter)).Result
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
    public ViewCrudUser Get(string iduser)
    {
      try
      {
        User user = serviceUser.GetNewVersion(p => p._id == iduser).Result;
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
          Schooling = user.Schooling == null ? null : new ViewListSchooling { _id = user.Schooling._id, Name = user.Schooling.Name, Order = user.Schooling.Order },
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
    public ViewCrudUser New(ViewCrudUser view)
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
          Schooling = view.Schooling == null ? null : new Schooling() { _id = view.Schooling._id, Name = view.Name, Order = view.Schooling.Order },
          Sex = view.Sex,
          ChangePassword = EnumChangePassword.AccessFirst
        };

        if (user.Mail.IndexOf("@maristas.org.br") != -1 || user.Mail.IndexOf("@pucrs.br") != -1)
          user.ChangePassword = EnumChangePassword.No;

        user = serviceUser.InsertNewVersion(user).Result;
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
          Schooling = (user.Schooling == null) ? null : new ViewListSchooling { _id = user.Schooling._id, Name = user.Schooling.Name, Order = user.Schooling.Order },
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
    public ViewCrudUser Update(ViewCrudUser view)
    {
      try
      {
        User user = serviceUser.GetNewVersion(p => p._id == view._id).Result;
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
        user.Schooling = user.Schooling == null ? null : new Schooling() { _id = view.Schooling._id, Name = view.Name, Order = view.Schooling.Order };
        user.Sex = view.Sex;
        serviceUser.Update(user, null);
        foreach(var item in servicePerson.GetAll(p => p.User._id == view._id).ToList())
        {
          item.User = user;
          servicePerson.Update(item, null);
        }
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
          Schooling = user.Schooling == null ? null : new ViewListSchooling { _id = user.Schooling._id, Name = user.Schooling.Name, Order = user.Schooling.Order },
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
        User user = serviceUser.GetNewVersion(p => p._id == idUser).Result;
        string oldPass = EncryptServices.GetMD5Hash(resetPass.OldPassword);
        if (user.ChangePassword == EnumChangePassword.AlterPass && user.Password != oldPass)
          return "error_old_password";
        string newPass = EncryptServices.GetMD5Hash(resetPass.NewPassword);
        user.Password = newPass;
        user.ChangePassword = EnumChangePassword.No;
        serviceUser.Update(user, null);
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
        User user = serviceUser.GetFreeNewVersion(p => p.ForeignForgotPassword == foreign).Result;
        if (user == null)
          return "error_valid";
        serviceUser._user = new BaseUser()
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
        serviceUser.Update(user, null);
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
        User user = serviceUser.GetFreeNewVersion(p => p.Mail == mail).Result;
        if (user == null)
          return "error_email";
        string guid = string.Concat(Guid.NewGuid().ToString(), user._id);
        string message = "";
        if (forgotPassword.Message == string.Empty)
          message = string.Format("Hello {0}<br>To reset your password click the link below<br>http://{1}/evaluation_f/forgot", user.Name, forgotPassword.Link);
        else
          message = forgotPassword.Message.Replace("{User}", user.Name).Replace("{Link}", forgotPassword.Link + "/" + guid);

        serviceUser._user = new BaseUser()
        {
          _idAccount = user._idAccount,
          NamePerson = user.Name,
          Mail = user.Mail,
          _idPerson = user._id
        };
        serviceMail._user = serviceUser._user;

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
        sendMail = serviceMail.InsertNewVersion(sendMail).Result;
        user.ChangePassword = EnumChangePassword.ForgotPassword;
        user.ForeignForgotPassword = guid;
        serviceUser.Update(user, null);
        await serviceMail.Send(sendMail, pathSendGrid);
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
        serviceUser.Insert(user);
        return user;
      }
      catch (Exception e)
      {
        throw e;
      }
    }


    public List<ViewInfoPerson> ListPerson(string iduser, ref long total, string filter, int count, int page)
    {
      int skip = (count * (page - 1));
      var detail = servicePerson.GetAll(p => p.User._id == iduser & p.User.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.User.Name).Skip(skip).Take(count).ToList();
      total = servicePerson.GetAll(p => p.User.Name.ToUpper().Contains(filter.ToUpper())).Count();

      return detail.Select(p => new ViewInfoPerson
      {
        _id = p._id,
        TypeJourney = p.TypeJourney,
        Occupation = p.Occupation?.Name,
        Name = p.User.Name,
        Manager = p.Manager?.Name,
        Company = new ViewListCompany() { _id = p.Company._id, Name = p.Company.Name },
        Establishment = (p.Establishment == null) ? null : new ViewListEstablishment() { _id = p.Establishment._id, Name = p.Establishment.Name },
        Registration = p.Registration,
        User = new ViewListUser()
        {
          Document = p.User.Document,
          Mail = p.User.Mail,
          Name = p.User.Name,
          Phone = p.User.Phone,
          _id = p.User._id
        }
      }).ToList();
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

        return serviceUser.Insert(model);
      }
      catch (Exception e)
      {
        throw e;
      }
    }



    public User UpdateUserView(User user)
    {
      try
      {
        var pass = serviceUser.GetAll(p => p._id == user._id).SingleOrDefault().Password;
        serviceUser.Update(user, null);
        return user;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public User UpdateUser(User user)
    {
      try
      {
        var pass = serviceUser.GetAll(p => p._id == user._id).SingleOrDefault().Password;
        if (user.Password != EncryptServices.GetMD5Hash(pass))
          user.Password = EncryptServices.GetMD5Hash(user.Password);
        serviceUser.Update(user, null);
        return user;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public void SetPhoto(string idUser, string url)
    {
      try
      {
        var user = serviceUser.GetAll(p => p._id == idUser).SingleOrDefault();
        user.PhotoUrl = url;
        serviceUser.Update(user, null);
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<User> GetUsers(string idcompany, string filter)
    {
      try
      {
        return serviceUser.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }


    public string GetPhoto(string idUser)
    {
      try
      {
        return this.serviceUser.GetAll(p => p._id == idUser).FirstOrDefault().PhotoUrl;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public User GetAuthentication(string mail, string password)
    {
      try
      {
        return serviceUser.GetAuthentication(p => p.Status == EnumStatus.Enabled & p.Mail == mail && p.Password == password).SingleOrDefault();
      }
      catch (Exception e)
      {
        throw e;
      }
    }


    public User GetUser(string id)
    {
      try
      {
        return serviceUser.GetAll(p => p._id == id).FirstOrDefault();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<User> ListUser(Expression<Func<User, bool>> filter)
    {
      try
      {
        return serviceUser.GetAll(filter).ToList();
      }
      catch (Exception e)
      {
        throw e;
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
          detail = serviceUser.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Name).Skip(skip).Take(count).ToList();
          total = serviceUser.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).Count();
        }

        else if (typeUser == EnumTypeUser.Administrator)
        {
          detail = serviceUser.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Name).Skip(skip).Take(count).ToList();
          total = serviceUser.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).Count();
        }

        else
        {
          detail = serviceUser.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Name).Skip(skip).Take(count).ToList();
          total = serviceUser.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).Count();
        }

        return detail;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public User GetUserCrudOld(string iduser)
    {
      try
      {
        return serviceUser.GetAll(p => p._id == iduser).FirstOrDefault();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<Occupation> ListOccupation(ref long total, string filter, int count, int page)
    {
      int skip = (count * (page - 1));
      var detail = serviceOccupation.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Name).Skip(skip).Take(count).ToList();
      total = serviceOccupation.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).Count();

      return detail;
    }

    public List<Person> ListPersonOld(string iduser, ref long total, string filter, int count, int page)
    {
      int skip = (count * (page - 1));
      var detail = servicePerson.GetAll(p => p.User._id == iduser & p.User.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.User.Name).Skip(skip).Take(count).ToList();
      total = servicePerson.GetAll(p => p.User.Name.ToUpper().Contains(filter.ToUpper())).Count();

      return detail;
    }

    public List<User> ListManager(ref long total, string filter, int count, int page)
    {
      int skip = (count * (page - 1));
      var detail = serviceUser.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Name).ToList();
      //var detail = userService.GetAll(p => p.StatusUser != EnumStatusUser.Disabled & p.StatusUser != EnumStatusUser.ErrorIntegration & p.TypeUser != EnumTypeUser.Administrator & p.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Name).Skip(skip).Take(count).ToList();
      //total = userService.GetAll(p => p.StatusUser != EnumStatusUser.Disabled & p.StatusUser != EnumStatusUser.ErrorIntegration & p.TypeUser != EnumTypeUser.Administrator & p.Name.ToUpper().Contains(filter.ToUpper())).Count();

      return detail;
    }

    public List<Company> ListCompany(ref long total, string filter, int count, int page)
    {
      int skip = (count * (page - 1));
      var detail = serviceCompany.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).Skip(skip).Take(count).ToList();
      total = serviceCompany.GetAll(p => p.Name.ToUpper().Contains(filter.ToUpper())).Skip(skip).Take(count).Count();

      return detail;
    }

    public List<User> ListAll()
    {
      try
      {
        return serviceUser.GetAuthentication(p => p.Status != EnumStatus.Disabled).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    #endregion




  }
}
