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
    public ServiceUser(DataContext context, DataContext contextLog) : base(context)
    {
      try
      {
        serviceAttachment = new ServiceGeneric<Attachments>(context);
        serviceCompany = new ServiceGeneric<Company>(context);
        serviceCheckpoint = new ServiceGeneric<Checkpoint>(context);
        serviceLog = new ServiceGeneric<Log>(contextLog);
        serviceMail = new ServiceSendGrid(contextLog);
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
            total = serviceUser.CountNewVersion(p => p.UserAdmin == false && p.Name.Contains(filter)).Result;
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
          Nickname = user.Nickname,
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
          Nickname = view.Nickname,
          Password = EncryptServices.GetMD5Hash(view.Password),
          ForeignForgotPassword = string.Empty,
          Coins = 0,
          Phone = view.Phone,
          PhoneFixed = view.PhoneFixed,
          PhotoUrl = view.PhotoUrl,
          Schooling = view.Schooling == null ? null : new Schooling() { _id = view.Schooling._id, Name = view.Name, Order = view.Schooling.Order },
          Sex = view.Sex,
          ChangePassword = EnumChangePassword.AccessFirst,
          UserAdmin = false
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
          Nickname = user.Nickname,
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
        user.Nickname = view.Nickname;
        user.Phone = view.Phone;
        user.PhoneFixed = view.PhoneFixed;
        user.PhotoUrl = view.PhotoUrl;
        user.Schooling = user.Schooling == null ? null : new Schooling() { _id = view.Schooling._id, Name = view.Name, Order = view.Schooling.Order };
        user.Sex = view.Sex;
        serviceUser.Update(user, null);
        foreach (var item in servicePerson.GetAll(p => p.User._id == view._id).ToList())
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
          Nickname = user.Nickname,
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
    public string GetPhoto(string idUser)
    {
      try
      {
        return serviceUser.GetAll(p => p._id == idUser).FirstOrDefault().PhotoUrl;
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
        var person = serviceUser.GetAll(p => p._id == idUser).SingleOrDefault();
        person.PhotoUrl = url;
        serviceUser.Update(person, null);
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
          message = forgotPassword.Message.Replace("{Person}", user.Name).Replace("{Link}", forgotPassword.Link + "/" + guid);

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

    #region Person
    public List<ViewListPersonInfo> ListPerson(string iduser, ref long total, string filter, int count, int page)
    {
      List<Person> detail = servicePerson.GetAllNewVersion(p => p.User._id == iduser & p.User.Name.ToUpper().Contains(filter.ToUpper()), count, count * (page - 1), "User.Name").Result;
      total = servicePerson.CountNewVersion(p => p.User._id == iduser & p.User.Name.ToUpper().Contains(filter.ToUpper())).Result;
      return detail.Select(p => new ViewListPersonInfo
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
    #endregion

  }
}
