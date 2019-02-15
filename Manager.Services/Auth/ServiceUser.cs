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
    private ServiceGeneric<PersonOld> personOldService;
    private ServiceGeneric<Attachments> attachmentService;
    private ServiceGeneric<Company> companyService;
    private ServiceGeneric<Occupation> occupationService;
    private ServiceGeneric<OnBoarding> onboardingService;
    private ServiceGeneric<Monitoring> monitoringService;
    private ServiceGeneric<MonitoringOld> monitoringOldService;
    private ServiceGeneric<Checkpoint> checkpointService;
    private ServiceGeneric<Plan> planService;
    private ServiceGeneric<Log> logService;
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
        personOldService._user = _user;
        onboardingService._user = _user;
        monitoringService._user = _user;
        monitoringOldService._user = _user;
        checkpointService._user = _user;
        planService._user = _user;
        logService._user = _user;
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
        personOldService = new ServiceGeneric<PersonOld>(context);
        onboardingService = new ServiceGeneric<OnBoarding>(context);
        monitoringService = new ServiceGeneric<Monitoring>(context);
        monitoringOldService = new ServiceGeneric<MonitoringOld>(context);
        checkpointService = new ServiceGeneric<Checkpoint>(context);
        planService = new ServiceGeneric<Plan>(context);
        logService = new ServiceGeneric<Log>(context);
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
        logService._user = _user;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string ScriptPerson()
    {
      try
      {
        //var persons = personService.GetAuthentication(p => p.Status == EnumStatus.Enabled & p._idAccount == idaccount & p.User == null).ToList();
        //var persons = personService.GetAuthentication(p => p.Status == EnumStatus.Enabled & p._idAccount == idaccount).ToList();
        //var persons = personService.GetAuthentication(p => p.Status == EnumStatus.Enabled).ToList();
        var persons =
          (from per in personOldService.GetAuthentication(p => p.Status == EnumStatus.Enabled)
           select new ViewPersonOld
           {
             Status = per.Status,
             _idAccount = per._idAccount,
             _id = per._id,
             StatusUser = per.StatusUser,
             Manager = per.Manager._id,
             Company = per.Company,
             Occupation = per.Occupation,
             DocumentManager = per.DocumentManager,
             DateLastOccupation = per.DateLastOccupation,
             Salary = per.Salary,
             DateLastReadjust = per.DateLastReadjust,
             DateResignation = per.DateResignation,
             TypeJourney = per.TypeJourney,
             Establishment = per.Establishment,
             HolidayReturn = per.HolidayReturn,
             MotiveAside = per.MotiveAside,
             Name = per.Name,
             TypeUser = per.TypeUser,
             Registration = per.Registration,
             DateBirth = per.DateBirth,
             DateAdm = per.DateAdm,
             Schooling = per.Schooling,
             PhotoUrl = per.PhotoUrl,
             Coins = per.Coins,
             ChangePassword = per.ChangePassword,
             ForeignForgotPassword = per.ForeignForgotPassword,
             PhoneFixed = per.PhoneFixed,
             DocumentID = per.DocumentID,
             DocumentCTPF = per.DocumentCTPF,
             Sex = per.Sex,
             Document = per.Document,
             Mail = per.Mail,
             Phone = per.Phone,
             Password = per.Password
           }
          ).ToList();



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
            Mail = item.Mail,
            Name = item.Name,
            Password = item.Password,
            Phone = item.Phone,
            PhoneFixed = item.PhoneFixed,
            PhotoUrl = item.PhotoUrl,
            Schooling = item.Schooling,
            Sex = item.Sex
          };

          userService.InsertAccount(user);

          var person = new Person()
          {
            Company = item.Company,
            DateLastOccupation = item.DateLastOccupation,
            DateLastReadjust = item.DateLastReadjust,
            DateResignation = item.DateResignation,
            DocumentManager = item.DocumentManager,
            Establishment = item.Establishment,
            HolidayReturn = item.HolidayReturn,
            MotiveAside = item.MotiveAside,
            Occupation = item.Occupation,
            Registration = item.Registration.ToString(),
            Salary = item.Salary,
            Status = item.Status,
            StatusUser = item.StatusUser,
            TypeJourney = item.TypeJourney,
            TypeUser = item.TypeUser,
            User = user,
            _id = item._id,
            _idAccount = item._idAccount
          };
          var manager = (from man in personOldService.GetAuthentication(p => p._id == item.Manager)
                         select new
                         {
                           Name = man.Name,
                           Mail = man.Mail,
                           _id = man._id
                         }).FirstOrDefault();

          if (manager != null)
            person.Manager = new BaseFields() { Name = manager.Name, Mail = manager.Mail };

          personService.InsertAccountId(person);
        }

        var list = personService.GetAuthentication(p => p.Status == EnumStatus.Enabled).ToList();
        foreach (var item in list)
        {
          if (item.Manager != null)
          {
            var idmanager = personService.GetAuthentication(p => p.User.Mail == item.Manager.Mail).FirstOrDefault()._id;
            item.Manager._id = idmanager;
            personService.UpdateAccount(item, null);
          }

        }
        return "ok";

      }
      catch (Exception e)
      {
        throw e;
      }
    }

    //private async Task UpdateManager(User user, string idperson)
    //{
    //  var managers = personService.GetAuthentication(p => p.Status == EnumStatus.Enabled & p._idAccount == user._idAccount).ToList();
    //  foreach (var item in managers)
    //  {
    //    try
    //    {
    //      if (item.Manager._id == idperson)
    //      {
    //        item.Manager.User = user;
    //        personService.UpdateAccount(item, null);
    //      }
    //      else if (item.Manager.Manager._id == idperson)
    //      {
    //        item.Manager.Manager.User = user;
    //        personService.UpdateAccount(item, null);
    //      }
    //      else if (item.Manager.Manager.Manager._id == idperson)
    //      {
    //        item.Manager.Manager.Manager.User = user;
    //        personService.UpdateAccount(item, null);
    //      }
    //      else if (item.Manager.Manager._id == idperson)
    //      {
    //        item.Manager.User = user;
    //        personService.UpdateAccount(item, null);
    //      }
    //      else if (item.Manager.Manager.Manager.Manager._id == idperson)
    //      {
    //        item.Manager.Manager.Manager.Manager.User = user;
    //        personService.UpdateAccount(item, null);
    //      }
    //      else if (item.Manager.Manager._id == idperson)
    //      {
    //        item.Manager.User = user;
    //        personService.UpdateAccount(item, null);
    //      }
    //      else if (item.Manager.Manager.Manager.Manager.Manager._id == idperson)
    //      {
    //        item.Manager.Manager.Manager.Manager.Manager.Manager.User = user;
    //        personService.UpdateAccount(item, null);
    //      }
    //      else if (item.Manager.Manager.Manager.Manager.Manager.Manager.Manager._id == idperson)
    //      {
    //        item.Manager.Manager.Manager.Manager.Manager.Manager.User = user;
    //        personService.UpdateAccount(item, null);
    //      }
    //      else if (item.Manager.Manager.Manager.Manager.Manager.Manager.Manager.Manager._id == idperson)
    //      {
    //        item.Manager.Manager.Manager.Manager.Manager.Manager.Manager.User = user;
    //        personService.UpdateAccount(item, null);
    //      }


    //    }
    //    catch
    //    {

    //    }

    //  }
    //}

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

    public string ScriptOnBoarding()
    {
      try
      {
        //var valid = onboardingService.GetAuthentication(p => p.Status == EnumStatus.Enabled || p.Status == EnumStatus.Disabled).ToList();

        var onboardings =
          (from onb in onboardingService.GetAuthentication(p => p.Status == EnumStatus.Enabled || p.Status == EnumStatus.Disabled)
           select new
           {
             _id = onb._id,
             _idAccount = onb._idAccount,
             Status = onb.Status,
             Person = onb.Person._id,
             DateBeginPerson = onb.DateBeginPerson,
             DateBeginManager = onb.DateBeginManager,
             DateBeginEnd = onb.DateBeginEnd,
             DateEndPerson = onb.DateEndPerson,
             DateEndManager = onb.DateEndManager,
             DateEndEnd = onb.DateEndEnd,
             CommentsPerson = onb.CommentsPerson,
             CommentsManager = onb.CommentsManager,
             CommentsEnd = onb.CommentsEnd,
             SkillsCompany = onb.SkillsCompany,
             SkillsGroup = onb.SkillsGroup,
             SkillsOccupation = onb.SkillsOccupation,
             Scopes = onb.Scopes,
             Schoolings = onb.Schoolings,
             Activities = onb.Activities,
             StatusOnBoarding = onb.StatusOnBoarding
           })
          .ToList();
        foreach (var item in onboardings)
        {
          var personOld = (from old in personOldService.GetAuthentication(p => p._id == item.Person)
                           select new
                           {
                             Mail = old.Mail
                           }
                           ).FirstOrDefault();
          Person person = null;
          if (personOld != null)
          {
            person = personService.GetAuthentication(p => p.User.Mail == personOld.Mail).FirstOrDefault();

            var onboarding = new OnBoarding()
            {
              _id = item._id,
              Person = person,
              DateBeginPerson = item.DateBeginPerson,
              DateBeginManager = item.DateBeginManager,
              DateBeginEnd = item.DateBeginEnd,
              DateEndPerson = item.DateEndPerson,
              DateEndManager = item.DateEndManager,
              DateEndEnd = item.DateEndEnd,
              CommentsPerson = item.CommentsPerson,
              CommentsManager = item.CommentsManager,
              CommentsEnd = item.CommentsEnd,
              SkillsCompany = item.SkillsCompany,
              SkillsGroup = item.SkillsGroup,
              SkillsOccupation = item.SkillsOccupation,
              Scopes = item.Scopes,
              Schoolings = item.Schoolings,
              Activities = item.Activities,
              StatusOnBoarding = item.StatusOnBoarding,
              Status = item.Status,
              _idAccount = item._idAccount
            };
            onboardingService.UpdateAccount(onboarding, null);
          }



        }

        return "ok";

      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string ScriptCheckpoint()
    {
      try
      {
        //var valid = checkpointService.GetAuthentication(p => p.Status == EnumStatus.Enabled || p.Status == EnumStatus.Disabled).ToList();

        var checkpoints =
          (from onb in checkpointService.GetAuthentication(p => p.Status == EnumStatus.Enabled || p.Status == EnumStatus.Disabled)
           select new
           {
             _id = onb._id,
             _idAccount = onb._idAccount,
             Status = onb.Status,
             Person = onb.Person._id,
             DateBegin = onb.DateBegin,
             DateEnd = onb.DateEnd,
             Comments = onb.Comments,
             TextDefault = onb.TextDefault,
             Questions = onb.Questions,
             StatusCheckpoint = onb.StatusCheckpoint,
             DataAccess = onb.DataAccess,
             TypeCheckpoint = onb.TypeCheckpoint
           })
          .ToList();
        foreach (var item in checkpoints)
        {
          var personOld = (from old in personOldService.GetAuthentication(p => p._id == item.Person)
                           select new
                           {
                             Mail = old.Mail
                           }
                           ).FirstOrDefault();
          Person person = null;
          if (personOld != null)
          {
            person = personService.GetAuthentication(p => p.User.Mail == personOld.Mail).FirstOrDefault();

            var checkpoint = new Checkpoint()
            {
              _id = item._id,
              Person = person,
              DateBegin = item.DateBegin,
              DateEnd = item.DateEnd,
              Comments = item.Comments,
              TextDefault = item.TextDefault,
              Questions = item.Questions,
              StatusCheckpoint = item.StatusCheckpoint,
              DataAccess = item.DataAccess,
              TypeCheckpoint = item.TypeCheckpoint,
              Status = item.Status,
              _idAccount = item._idAccount
            };
            checkpointService.UpdateAccount(checkpoint, null);
          }



        }

        return "ok";

      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string ScriptMonitoring()
    {
      try
      {
        //var valid = monitoringService.GetAuthentication(p => p.Status == EnumStatus.Enabled || p.Status == EnumStatus.Disabled).ToList();

        //var monitorings =
        //  (from onb in monitoringService.GetAuthentication(p => p.Status == EnumStatus.Enabled || p.Status == EnumStatus.Disabled)
        //   select new
        //   {
        //     _id = onb._id,
        //     _idAccount = onb._idAccount,
        //     Status = onb.Status,
        //     Person = onb.Person._id,
        //     DateBeginPerson = onb.DateBeginPerson,
        //     DateBeginManager = onb.DateBeginManager,
        //     DateBeginEnd = onb.DateBeginEnd,
        //     DateEndPerson = onb.DateEndPerson,
        //     DateEndManager = onb.DateEndManager,
        //     DateEndEnd = onb.DateEndEnd,
        //     CommentsPerson = onb.CommentsPerson,
        //     CommentsManager = onb.CommentsManager,
        //     CommentsEnd = onb.CommentsEnd,
        //     SkillsCompany = onb.SkillsCompany.Select(x => new MonitoringSkills()
        //     {
        //       Skill = x.Skill,
        //       CommentsPerson = x.CommentsPerson,
        //       CommentsManager = x.CommentsManager,
        //       Praise = x.Praise,
        //       Comments = x.Comments,
        //       StatusViewManager = x.StatusViewManager,
        //       StatusViewPerson = x.StatusViewPerson,
        //       Plans = null
        //     }),
        //     Schoolings = onb.Schoolings.Select(x => new MonitoringSchooling()
        //     {
        //       Schooling = x.Schooling,
        //       CommentsPerson = x.CommentsPerson,
        //       CommentsManager = x.CommentsManager,
        //       Praise = x.Praise,
        //       Comments = x.Comments,
        //       StatusViewManager = x.StatusViewManager,
        //       StatusViewPerson = x.StatusViewPerson,
        //       Plans = null
        //     }),
        //     Activities = onb.Activities.Select(x => new MonitoringActivities()
        //     {
        //       Activities = x.Activities,
        //       CommentsPerson = x.CommentsPerson,
        //       CommentsManager = x.CommentsManager,
        //       Praise = x.Praise,
        //       Comments = x.Comments,
        //       StatusViewManager = x.StatusViewManager,
        //       StatusViewPerson = x.StatusViewPerson,
        //       Plans = null
        //     }),
        //     StatusMonitoring = onb.StatusMonitoring
        //   })
        //  .ToList();

        var monitorings = monitoringOldService.GetAuthentication(p => p.Status == EnumStatus.Enabled || p.Status == EnumStatus.Disabled).ToList();

        foreach (var item in monitorings)
        {
          var personOld = (from old in personOldService.GetAuthentication(p => p._id == item.Person._id)
                           select new
                           {
                             Mail = old.Mail
                           }
                          ).FirstOrDefault();
          Person person = null;
          if (personOld != null)
          {
            person = personService.GetAuthentication(p => p.User.Mail == personOld.Mail).FirstOrDefault();

            var monitoring = new Monitoring()
            {
              _id = item._id,
              Person = person,
              DateBeginPerson = item.DateBeginPerson,
              DateBeginManager = item.DateBeginManager,
              DateBeginEnd = item.DateBeginEnd,
              DateEndPerson = item.DateEndPerson,
              DateEndManager = item.DateEndManager,
              DateEndEnd = item.DateEndEnd,
              CommentsPerson = item.CommentsPerson,
              CommentsManager = item.CommentsManager,
              CommentsEnd = item.CommentsEnd,
              StatusMonitoring = item.StatusMonitoring,
              Status = item.Status,
              _idAccount = item._idAccount
            };

            if (item.SkillsCompany != null)
            {
              foreach (var row in item.SkillsCompany.ToList())
              {
                var view = new MonitoringSkills();
                monitoring.SkillsCompany = new List<MonitoringSkills>();
                view.Skill = row.Skill;
                view.Praise = row.Praise;
                view.Status = row.Status;
                view._id = row._id;
                view._idAccount = row._idAccount;
                view.Comments = row.Comments;
                view.CommentsManager = row.CommentsManager;
                view.CommentsPerson = row.CommentsPerson;
                view.StatusViewManager = row.StatusViewManager;
                view.StatusViewPerson = row.StatusViewPerson;
                foreach (var plan in row.Plans)
                {
                  var planNew = new Plan();
                  view.Plans = new List<Plan>();

                  planNew.Name = plan.Name;
                  planNew.Description = plan.Description;
                  planNew.Deadline = plan.Deadline;
                  planNew.Skills = plan.Skills;
                  planNew.UserInclude = person;
                  planNew.DateInclude = plan.DateInclude;
                  planNew.TypePlan = plan.TypePlan;
                  planNew.SourcePlan = plan.SourcePlan;
                  planNew.TypeAction = plan.TypeAction;
                  planNew.StatusPlan = plan.StatusPlan;
                  planNew.TextEnd = plan.TextEnd;
                  planNew.TextEndManager = plan.TextEndManager;
                  planNew.DateEnd = plan.DateEnd;
                  planNew.Evaluation = plan.Evaluation;
                  planNew.Result = plan.Result;
                  planNew.StatusPlanApproved = plan.StatusPlanApproved;
                  planNew.Attachments = plan.Attachments;
                  planNew.NewAction = plan.NewAction;
                  planNew.StructPlans = plan.StructPlans;
                  view.Plans.Add(ScriptPlan(planNew));
                }

                monitoring.SkillsCompany.Add(view);

              };
            }

            if (item.Schoolings != null)
            {
              foreach (var row in item.Schoolings.ToList())
              {
                var view = new MonitoringSchooling();
                monitoring.Schoolings = new List<MonitoringSchooling>();
                view.Schooling = row.Schooling;
                view.Praise = row.Praise;
                view.Status = row.Status;
                view._id = row._id;
                view._idAccount = row._idAccount;
                view.Comments = row.Comments;
                view.CommentsManager = row.CommentsManager;
                view.CommentsPerson = row.CommentsPerson;
                view.StatusViewManager = row.StatusViewManager;
                view.StatusViewPerson = row.StatusViewPerson;
                foreach (var plan in row.Plans)
                {
                  var planNew = new Plan();
                  view.Plans = new List<Plan>();

                  planNew.Name = plan.Name;
                  planNew.Description = plan.Description;
                  planNew.Deadline = plan.Deadline;
                  planNew.Skills = plan.Skills;
                  planNew.UserInclude = person;
                  planNew.DateInclude = plan.DateInclude;
                  planNew.TypePlan = plan.TypePlan;
                  planNew.SourcePlan = plan.SourcePlan;
                  planNew.TypeAction = plan.TypeAction;
                  planNew.StatusPlan = plan.StatusPlan;
                  planNew.TextEnd = plan.TextEnd;
                  planNew.TextEndManager = plan.TextEndManager;
                  planNew.DateEnd = plan.DateEnd;
                  planNew.Evaluation = plan.Evaluation;
                  planNew.Result = plan.Result;
                  planNew.StatusPlanApproved = plan.StatusPlanApproved;
                  planNew.Attachments = plan.Attachments;
                  planNew.NewAction = plan.NewAction;
                  planNew.StructPlans = plan.StructPlans;
                  view.Plans.Add(ScriptPlan(planNew));
                }

                monitoring.Schoolings.Add(view);

              };
            }

            if (item.Activities != null)
            {
              foreach (var row in item.Activities.ToList())
              {
                var view = new MonitoringActivities();
                monitoring.Activities = new List<MonitoringActivities>();
                view.Activities = row.Activities;
                view.Praise = row.Praise;
                view.Status = row.Status;
                view._id = row._id;
                view._idAccount = row._idAccount;
                view.Comments = row.Comments;
                view.CommentsManager = row.CommentsManager;
                view.CommentsPerson = row.CommentsPerson;
                view.StatusViewManager = row.StatusViewManager;
                view.StatusViewPerson = row.StatusViewPerson;
                foreach (var plan in row.Plans)
                {
                  var planNew = new Plan();
                  view.Plans = new List<Plan>();

                  planNew.Name = plan.Name;
                  planNew.Description = plan.Description;
                  planNew.Deadline = plan.Deadline;
                  planNew.Skills = plan.Skills;
                  planNew.UserInclude = person;
                  planNew.DateInclude = plan.DateInclude;
                  planNew.TypePlan = plan.TypePlan;
                  planNew.SourcePlan = plan.SourcePlan;
                  planNew.TypeAction = plan.TypeAction;
                  planNew.StatusPlan = plan.StatusPlan;
                  planNew.TextEnd = plan.TextEnd;
                  planNew.TextEndManager = plan.TextEndManager;
                  planNew.DateEnd = plan.DateEnd;
                  planNew.Evaluation = plan.Evaluation;
                  planNew.Result = plan.Result;
                  planNew.StatusPlanApproved = plan.StatusPlanApproved;
                  planNew.Attachments = plan.Attachments;
                  planNew.NewAction = plan.NewAction;
                  planNew.StructPlans = plan.StructPlans;
                  view.Plans.Add(ScriptPlan(planNew));
                }

                monitoring.Activities.Add(view);

              };
            }

            monitoringService.InsertAccountId(monitoring);
          }



        }

        return "ok";

      }
      catch (Exception e)
      {
        throw e;
      }
    }

    private Plan ScriptPlan(Plan plan)
    {
      try
      {
        return planService.Insert(plan);
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public string ScriptLog()
    {
      throw new NotImplementedException();
    }
  }
}
