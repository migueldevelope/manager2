﻿using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.BusinessModel;
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
    private ServiceLog serviceLog;
    private ServiceSendGrid serviceMail;
    private ServiceGeneric<Monitoring> serviceMonitoring;
    private ServiceGeneric<Occupation> serviceOccupation;
    private ServiceGeneric<OnBoarding> serviceOnboarding;
    private ServiceGeneric<Person> servicePerson;
    private ServiceGeneric<Plan> servicePlan;
    private ServiceGeneric<User> serviceUser;
    private readonly ServiceTermsOfService serviceTermsOfService;

    #region Constructor
    public ServiceUser(DataContext context, DataContext contextLog) : base(context)
    {
      try
      {
        serviceAttachment = new ServiceGeneric<Attachments>(context);
        serviceCompany = new ServiceGeneric<Company>(context);
        serviceCheckpoint = new ServiceGeneric<Checkpoint>(context);
        serviceLog = new ServiceLog(context, contextLog);
        serviceMail = new ServiceSendGrid(contextLog);
        serviceMonitoring = new ServiceGeneric<Monitoring>(context);
        serviceOccupation = new ServiceGeneric<Occupation>(context);
        serviceOnboarding = new ServiceGeneric<OnBoarding>(context);
        servicePerson = new ServiceGeneric<Person>(context);
        servicePlan = new ServiceGeneric<Plan>(context);
        serviceUser = new ServiceGeneric<User>(context);
        serviceTermsOfService = new ServiceTermsOfService(context);
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
      serviceLog.SetUser(_user);
    }
    public void SetUser(BaseUser user)
    {
      _user = user;
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
      serviceLog.SetUser(_user);
    }
    #endregion

    #region User

    public string AlterPassHR(string iduser)
    {
      try
      {
        var user = serviceUser.GetNewVersion(p => p._id == iduser).Result;
        user.ChangePassword = EnumChangePassword.AccessFirst;
        //user.Password = EncryptServices.GetMD5Hash("1234");
        user.Password = EncryptServices.GetMD5Hash(user.Document);
        Task.Run(() => LogSave(_user._idPerson, "alterpassrh :" + user._id));
        var i = serviceUser.Update(user, null);

        return "alterpass";
      }
      catch (Exception e)
      {
        throw e;
      }
    }


    public string Delete(string iduser)
    {
      try
      {
        var user = serviceUser.GetNewVersion(p => p._id == iduser).Result;
        user.Status = EnumStatus.Disabled;
        var i = serviceUser.Update(user, null);

        foreach (var person in servicePerson.GetAllNewVersion(p => p.User._id == iduser).Result)
        {
          person.Status = EnumStatus.Disabled;
          var x = servicePerson.Update(person, null);
        }

        return "deleted";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public void CheckTermOfService(string iduser)
    {
      try
      {
        var user = serviceUser.GetFreeNewVersion(p => p._id == iduser).Result;
        if (user.UserTermOfServices == null)
          user.UserTermOfServices = new List<UserTermOfService>();

        serviceTermsOfService.SetUser(_user);
        serviceTermsOfService._user = _user;
        var term = serviceTermsOfService.GetTerm();
        user.UserTermOfServices.Add(new UserTermOfService() { _idTermOfService = term._id, Date = term.Date });

        serviceUser.Update(user, null).Wait();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewCrudUser> List(int count, int page, string filter, EnumTypeUser type)
    {
      try
      {
        switch (type)
        {
          case EnumTypeUser.Support:
          case EnumTypeUser.Administrator:
            var total = serviceUser.CountNewVersion(p => p.Name.Contains(filter)).Result;
            return serviceUser.GetAllNewVersion(p => p.Name.Contains(filter)).Result
            .Select(x => x.GetViewList()).ToList();
          default:
            total = serviceUser.CountNewVersion(p => p.UserAdmin == false && p.Name.Contains(filter)).Result;
            return serviceUser.GetAllNewVersion(p => p.Name.Contains(filter)).Result
            .Select(x => x.GetViewList()).ToList();
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
        return serviceUser.GetNewVersion(p => p._id == iduser).Result.GetViewCrud();
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
        //var exists = serviceUser.CountFreeNewVersion(p => p.Mail == view.Mail).Result;
        //if (exists > 0)
        //  throw new Exception("existsmailornickname");

        var exists = serviceUser.CountFreeNewVersion(p => p.Document == view.Document).Result;
        if (exists > 0)
          throw new Exception("existsdocument");

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
          Schooling = view.Schooling,
          Sex = view.Sex,
          ChangePassword = EnumChangePassword.AccessFirst,
          UserAdmin = false,
          ShowSalary = view.ShowSalary
        };

        if (user.Mail != null)
        {
          if (user.Mail.IndexOf("@maristas.org.br") != -1 || user.Mail.IndexOf("@pucrs.br") != -1)
            user.ChangePassword = EnumChangePassword.No;
        }
        user = serviceUser.InsertNewVersion(user).Result;
        return user.GetViewCrud();
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
        //var exists = serviceUser.CountFreeNewVersion(p => p._id != view._id && (p.Mail == view.Mail)).Result;
        //if (exists > 0)
        //  throw new Exception("existsmailornickname");

        var exists = serviceUser.CountFreeNewVersion(p => p._id != view._id && (p.Document == view.Document)).Result;
        if (exists > 0)
          throw new Exception("existsdocument");

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
        user.Schooling = view.Schooling;
        user.Sex = view.Sex;
        user.ShowSalary = view.ShowSalary;
        serviceUser.Update(user, null).Wait();
        foreach (var item in servicePerson.GetAllNewVersion(p => p.User._id == view._id).Result.ToList())
        {
          item.User = user.GetViewCrud();
          servicePerson.Update(item, null).Wait();
        }
        return user.GetViewCrud();
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
        var photo = serviceUser.GetAllNewVersion(p => p._id == idUser).Result.FirstOrDefault().PhotoUrl;
        return photo;
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
        var user = serviceUser.GetNewVersion(p => p._id == idUser).Result;
        user.PhotoUrl = url;
        serviceUser.Update(user, null).Wait();
        foreach (var item in servicePerson.GetAllNewVersion(p => p.User._id == user._id).Result)
        {
          item.User.PhotoUrl = url;
          var i = servicePerson.Update(item, null);
        }
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
        serviceUser.Update(user, null).Wait();
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
          _idUser = user._id
        };
        string newPass = EncryptServices.GetMD5Hash(resetPass.NewPassword);
        user.Password = newPass;
        user.ChangePassword = EnumChangePassword.No;
        user.ForeignForgotPassword = string.Empty;
        serviceUser.Update(user, null).Wait();
        return "Password changed!";
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public string ForgotPassword(string mail, ViewForgotPassword forgotPassword, string pathSendGrid)
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
          _idUser = user._id
        };
        serviceMail._user = serviceUser._user;

        MailLog sendMail = new MailLog
        {
          From = new MailLogAddress("suporte@fluidstate.com.br", "Suporte ao Cliente | Fluid"),
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
        serviceUser.Update(user, null).Wait();
        serviceMail.Send(sendMail, pathSendGrid);
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
        _idManager = p.Manager?._id,
        Registration = p.Registration
      }).ToList();
    }
    #endregion

    #region private

    private void LogSave(string idperson, string local)
    {
      try
      {
        var user = servicePerson.GetAllNewVersion(p => p._id == idperson).Result.FirstOrDefault();
        var log = new ViewLog()
        {
          Description = "User ",
          Local = local,
          _idPerson = user._id
        };
        serviceLog.NewLog(log);
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    #endregion

  }
}
