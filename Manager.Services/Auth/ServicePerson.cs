using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.BusinessModel;
using Manager.Core.Interfaces;
using Manager.Data;
using Manager.Services.Commons;
using Manager.Services.Specific;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.BusinessView;
using Manager.Views.Enumns;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    private ServiceGeneric<PersonHistory> servicePersonHistory;
    private ServiceGeneric<SalaryScale> serviceSalaryScale;
    private ServiceGeneric<Schooling> serviceSchooling;
    private ServiceGeneric<OnBoarding> serviceOnboarding;
    private ServiceGeneric<Monitoring> serviceMonitoring;
    private ServiceGeneric<Checkpoint> serviceCheckpoint;
    private ServiceGeneric<OffBoarding> serviceOffBoarding;
    private readonly ServiceGeneric<FeelingDay> serviceFeelingDay;
    private ServiceGeneric<User> serviceUser;
    private readonly IQueueClient queueClient;
    private HubConnection hubConnection;
    private readonly string pathSignalr;
    IServiceControlQueue seviceControlQueue;

    #region Constructor
    public ServicePerson(DataContext context, DataContext contextLog, IServiceControlQueue _seviceControlQueue, string _pathSignalr) : base(context)
    {
      try
      {
        queueClient = new QueueClient(_seviceControlQueue.ServiceBusConnectionString(), "structmanager");
        serviceAttachment = new ServiceGeneric<Attachments>(context);
        serviceCompany = new ServiceGeneric<Company>(context);
        serviceEstablishment = new ServiceGeneric<Establishment>(context);
        serviceMail = new ServiceSendGrid(contextLog);
        serviceOccupation = new ServiceGeneric<Occupation>(context);
        serviceFeelingDay = new ServiceGeneric<FeelingDay>(context);
        serviceParameter = new ServiceGeneric<Parameter>(context);
        serviceOffBoarding = new ServiceGeneric<OffBoarding>(context);
        servicePerson = new ServiceGeneric<Person>(context);
        servicePersonHistory = new ServiceGeneric<PersonHistory>(context);
        serviceSalaryScale = new ServiceGeneric<SalaryScale>(context);
        serviceSchooling = new ServiceGeneric<Schooling>(context);
        serviceUser = new ServiceGeneric<User>(context);
        serviceOnboarding = new ServiceGeneric<OnBoarding>(context);
        serviceMonitoring = new ServiceGeneric<Monitoring>(context);
        serviceCheckpoint = new ServiceGeneric<Checkpoint>(context);
        seviceControlQueue = _seviceControlQueue;
        pathSignalr = _pathSignalr;
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
      serviceFeelingDay._user = _user;
      serviceOccupation._user = _user;
      serviceParameter._user = _user;
      servicePerson._user = _user;
      servicePersonHistory._user = _user;
      serviceSalaryScale._user = _user;
      serviceSchooling._user = _user;
      serviceUser._user = _user;
      serviceOnboarding._user = _user;
      serviceMonitoring._user = _user;
      serviceCheckpoint._user = _user;
      serviceOffBoarding._user = _user;
      DefaultTypeRegisterPerson();
    }
    public void SetUser(BaseUser user)
    {
      _user = user;
      serviceAttachment._user = user;
      serviceCompany._user = user;
      serviceEstablishment._user = user;
      serviceMail.SetUser(user);
      serviceOccupation._user = user;
      serviceParameter._user = user;
      servicePerson._user = user;
      servicePersonHistory._user = user;
      serviceSalaryScale._user = user;
      serviceSchooling._user = user;
      serviceFeelingDay._user = user;
      serviceUser._user = user;
      serviceOnboarding._user = user;
      serviceMonitoring._user = user;
      serviceCheckpoint._user = user;
      serviceOffBoarding._user = user;
      DefaultTypeRegisterPerson();
    }
    #endregion

    #region Company
    public List<ViewListCompany> ListCompany(ref long total, string filter, int count, int page)
    {
      int skip = (count * (page - 1));
      var detail = serviceCompany.GetAllNewVersion(p => p.Name.ToUpper().Contains(filter.ToUpper())).Result.Skip(skip).Take(count).ToList();
      total = serviceCompany.CountNewVersion(p => p.Name.ToUpper().Contains(filter.ToUpper())).Result;

      return detail.Select(p => new ViewListCompany() { _id = p._id, Name = p.Name }).ToList();
    }
    #endregion

    #region Manager
    public List<ViewBaseFields> ListManager(ref long total, string filter, int count, int page)
    {
      try
      {

        total = servicePerson.CountNewVersion(p => p.StatusUser != EnumStatusUser.Disabled & p.TypeUser != EnumTypeUser.Administrator & p.User.Name.ToUpper().Contains(filter.ToUpper())).Result;
        int skip = (count * (page - 1));
        return servicePerson.GetAllNewVersion(p => p.TypeUser != EnumTypeUser.Employee & p.TypeUser != EnumTypeUser.HR & p.StatusUser != EnumStatusUser.Disabled & p.TypeUser != EnumTypeUser.Administrator & p.User.Name.ToUpper().Contains(filter.ToUpper()))
          .Result.OrderBy(p => p.User.Name)
           .Select(item => item.GetViewBaseFields()).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    #endregion

    #region Occupation
    public List<ViewListOccupationResume> ListOccupation(ref long total, string filter, int count, int page)
    {
      total = serviceOccupation.CountNewVersion(p => p.Name.ToUpper().Contains(filter.ToUpper())).Result;
      int skip = count * (page - 1);
      return serviceOccupation.GetAllNewVersion(p => p.Name.ToUpper().Contains(filter.ToUpper())).Result.OrderBy(p => p.Name).ThenBy(o => o.Description)
        .Skip(skip).Take(count).Select(p => p.GetViewListResume()).ToList();

    }

    public List<_ViewListBase> ListOccupationManager(string idmanager, ref long total, string filter, int count, int page)
    {
      var occupations = servicePerson.GetAllNewVersion(p => p.Manager._id == idmanager).Result.Select
        (p => new _ViewListBase()
        {
          _id = p.Occupation?._id,
          Name = p.Occupation?.Name
        }).ToList();

      //distinct
      occupations = occupations.GroupBy(i => i._id).Select(g => new _ViewListBase
      {
        _id = g.Key,
        Name = g.Max(row => row.Name)
      }).ToList();

      total = occupations.Where(p => p.Name.ToUpper().Contains(filter.ToUpper())).Count();

      int skip = count * (page - 1);
      return occupations.Where(p => p.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Name)
        .Skip(skip).Take(count).ToList();

    }

    public List<ViewListOccupationProcess> ListOccupationProcess(ref long total, string filter, int count, int page)
    {
      var occupations = serviceOccupation.GetAllNewVersion(p => p.Status == EnumStatus.Enabled).Result.Select
        (p => new ViewListOccupationProcess()
        {
          _id = p._id,
          Name = p.Name,
          Process = p.Process == null ? null : p.Process.Select(x => new ViewListProcessLevelTwo()
          {
            _id = x._id,
            Name = x.Name,
            ProcessLevelOne = x.ProcessLevelOne
          }).ToList()
        }).ToList();


      total = occupations.Where(p => p.Name.ToUpper().Contains(filter.ToUpper())).Count();

      int skip = count * (page - 1);
      return occupations.Where(p => p.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Name)
        .Skip(skip).Take(count).ToList();

    }

    #endregion

    #region Person

    public List<ViewUserNickName> GetPersonNickName(string nickname)
    {
      try
      {
        var users = serviceUser.GetAllNewVersion(p => p.Nickname == nickname).Result;
        var list = new List<ViewUserNickName>();
        foreach (var user in users)
        {
          var persons = servicePerson.GetAllNewVersion(p => p.User._id == user._id).Result;

          var viewUser = new ViewUserNickName()
          {
            DateAdm = user.DateAdm,
            DateBirth = user.DateBirth,
            Document = user.Document,
            Mail = user.Mail,
            Name = user.Name,
            Nickname = user.Nickname,
            Phone = user.Phone,
            Schooling = user.Schooling,
            Sex = Enum.GetName(typeof(EnumSex), user.Sex),
            _id = user._id,
            Contracts = new List<ViewPersonNickName>()
          };

          foreach (var person in persons)
          {
            var viewPerson = new ViewPersonNickName()
            {
              Company = person.Company,
              DateResignation = person.DateResignation,
              Establishment = person.Establishment,
              Manager = person.Manager == null ? null : new ViewBaseFields()
              {
                _id = person.Manager._id,
                Mail = person.Manager.Mail,
                Name = person.Manager.Name
              },
              Occupation = person.Occupation,
              Registration = person.Registration,
              StatusUser = Enum.GetName(typeof(EnumStatusUser), person.StatusUser),
              TypeJourney = Enum.GetName(typeof(EnumTypeJourney), person.TypeJourney),
              TypeUser = Enum.GetName(typeof(EnumTypeUser), person.TypeUser),
              _id = person._id
            };
            viewUser.Contracts.Add(viewPerson);
          }

          list.Add(viewUser);
        }

        return list;
      }
      catch (Exception e)
      {
        throw e;
      }
    }


    public List<_ViewListBase> GetPersons()
    {
      try
      {
        return servicePerson.GetAllNewVersion(p => p.TypeJourney != EnumTypeJourney.OutOfJourney).Result
          .Select(p => new _ViewListBase() { _id = p._id, Name = p.User?.Name }).OrderBy(p => p.Name).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewListIdIndicators> GetFilterPersons(List<_ViewList> idmanagers)
    {
      try
      {
        var managers = idmanagers.Select(p => p._id).ToList();
        if (managers.Count > 0)
          return servicePerson.GetAllNewVersion(p => p.StatusUser != EnumStatusUser.Disabled
          && p.TypeUser > EnumTypeUser.Administrator
          && p.TypeJourney != EnumTypeJourney.OutOfJourney
          && managers.Contains(p.Manager._id))
          .Result.Select(p => new ViewListIdIndicators()
          {
            _id = p._id,
            TypeJourney = p.TypeJourney,
            Name = p.User?.Name,
            OccupationName = p.Occupation?.Name,
            DateAdm = p.User?.DateAdm,
            Manager = p.Manager?.Name,
            DateLastOccupation = p.DateLastOccupation
          }).ToList();
        else
          return servicePerson.GetAllNewVersion(p => p.StatusUser != EnumStatusUser.Disabled
        && p.TypeUser > EnumTypeUser.Administrator
        && p.TypeJourney != EnumTypeJourney.OutOfJourney)
        .Result.Select(p => new ViewListIdIndicators()
        {
          _id = p._id,
          TypeJourney = p.TypeJourney,
          Name = p.User?.Name,
          OccupationName = p.Occupation?.Name,
          DateAdm = p.User?.DateAdm,
          Manager = p.Manager?.Name,
          DateLastOccupation = p.DateLastOccupation
        }).ToList();

        //if (idmanagers.Count() > 0)
        //{
        //  foreach (var person in persons)
        //  {
        //    foreach (var manager in idmanagers)
        //    {
        //      if (manager._id == person.Manager?._id)
        //        person.Status = EnumStatus.Disabled;
        //    }
        //  }

        //  persons = persons.Where(p => p.Status == EnumStatus.Disabled).ToList();
        //}

        //return persons.Select(p => new ViewListIdIndicators()
        //{
        //  _id = p._id,
        //  TypeJourney = p.TypeJourney,
        //  Name = p.User?.Name,
        //  OccupationName = p.Occupation?.Name,
        //  DateAdm = p.User?.DateAdm,
        //  Manager = p.Manager?.Name,
        //  DateLastOccupation = p.DateLastOccupation
        //}).ToList();
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
        bool modifyManager = false;
        var exists = serviceUser.CountFreeNewVersion(p => p._id != view.User._id && (p.Document == view.User.Document)).Result;
        if (exists > 0)
          throw new Exception("existsdocument");

        Person person = servicePerson.GetNewVersion(p => p._id == view.Person._id).Result;

        #region Variáveis para salvar dados antigos
        ViewListEstablishment saveEstablishment = person.Establishment;
        EnumStatusUser saveStatusUser = person.StatusUser;
        ViewListOccupationResume saveOccupation = person.Occupation;
        decimal saveSalary = person.Salary;
        BaseFields saveManager = person.Manager;
        EnumTypeJourney saveTypeJourney = person.TypeJourney;
        #endregion

        User user = serviceUser.GetNewVersion(p => p._id == view.User._id).Result;

        user.Name = view.User.Name;
        user.Nickname = view.User.Nickname;
        user.Document = view.User.Document;
        user.Mail = view.User.Mail;
        user.Phone = view.User.Phone;
        user.DateBirth = view.User.DateBirth;
        user.DateAdm = view.User.DateAdm;
        user.Schooling = view.User.Schooling;
        user.PhotoUrl = view.User.PhotoUrl;
        user.PhoneFixed = view.User.PhoneFixed;
        user.DocumentID = view.User.DocumentID;
        user.DocumentCTPF = view.User.DocumentCTPF;
        user.Sex = view.User.Sex;

        BaseFields manager = null;
        if (view.Person.Manager != null)
          modifyManager = true;
        
        SalaryScalePerson salaryScale = null;
        if (view.Person.SalaryScales != null)
          salaryScale = serviceSalaryScale.GetAllNewVersion(p => p._id == view.Person.SalaryScales._idSalaryScale).Result
            .Select(p => new SalaryScalePerson() { _idSalaryScale = p._id, NameSalaryScale = p.Name })
            .FirstOrDefault();

        person.StatusUser = view.Person.StatusUser;
        person.Company = view.Person.Company;
        person.Occupation = view.Person.Occupation;
        person.Manager = manager;
        person.DateLastOccupation = view.Person.DateLastOccupation;
        person.Salary = view.Person.Salary;
        person.DateLastReadjust = view.Person.DateLastReadjust;
        person.DateResignation = view.Person.DateResignation;
        person.TypeJourney = view.Person.TypeJourney;
        person.Establishment = view.Person.Establishment;
        person.HolidayReturn = view.Person.HolidayReturn;
        person.MotiveAside = view.Person.MotiveAside;
        person.TypeUser = view.Person.TypeUser;
        person.Registration = view.Person.Registration;
        person.SalaryScales = salaryScale;
        person.Workload = view.Person.Workload;
        person.User = user.GetViewCrud();

        #region Ajustes na manutenção da pessoa
        if (person.StatusUser == EnumStatusUser.Disabled)
        {
          person.TypeJourney = EnumTypeJourney.OutOfJourney;
          var persons = servicePerson.CountNewVersion(p => p.User._id == person.User._id &&
          p.StatusUser != EnumStatusUser.Disabled).Result;
          if (persons == 0)
          {
            user.Status = EnumStatus.Disabled;
            var i = serviceUser.Update(user, null);
          }
        }
        if (person.TypeUser == EnumTypeUser.Administrator || person.TypeUser == EnumTypeUser.Support || person.TypeUser == EnumTypeUser.Anonymous)
        {
          person.TypeJourney = EnumTypeJourney.OutOfJourney;
        }
        if (person.Occupation == null)
        {
          person.TypeJourney = EnumTypeJourney.OutOfJourney;
        }
        if (saveOccupation?._id != view.Person.Occupation?._id && (view.Person.DateLastOccupation == null || view.Person.DateLastOccupation < DateTime.UtcNow.AddDays(-15)))
        {
          person.DateLastOccupation = DateTime.UtcNow;
        }
        if (saveSalary != view.Person.Salary && (view.Person.DateLastReadjust == null || view.Person.DateLastReadjust < DateTime.UtcNow.AddDays(-15)))
        {
          person.DateLastReadjust = DateTime.UtcNow;
        }
        #endregion

        var xi = servicePerson.Update(person, null);
        var ix = serviceUser.Update(user, null);

        if (modifyManager)
          manager = UpdateManager(person, view.Person.Manager._id, person.Manager?._id);

        #region Registrar os históricos da pessoa nova
        PersonHistory personHistory = null;
        if (saveEstablishment?._id != person.Establishment?._id)
        {
          personHistory = new PersonHistory()
          {
            Person = person.GetViewList(),
            TypeHistory = person.StatusUser == EnumStatusUser.Disabled ? EnumTypeHistory.Demission : EnumTypeHistory.Change,
            TypeChange = EnumTypeHistoryChange.Establishment,
            Register = DateTime.UtcNow,
            OldKey = saveEstablishment?._id,
            OldValue = saveEstablishment?.Name,
            NewKey = person.Establishment?._id,
            NewValue = person.Establishment?.Name
          };
          personHistory = servicePersonHistory.InsertNewVersion(personHistory).Result;
        }
        if (saveStatusUser != person.StatusUser)
        {
          personHistory = new PersonHistory()
          {
            Person = person.GetViewList(),
            TypeHistory = person.StatusUser == EnumStatusUser.Disabled ? EnumTypeHistory.Demission : EnumTypeHistory.Change,
            TypeChange = EnumTypeHistoryChange.StatusUser,
            Register = DateTime.UtcNow,
            OldKey = ((int)saveStatusUser).ToString(),
            OldValue = saveStatusUser.ToString(),
            NewKey = ((int)person.StatusUser).ToString(),
            NewValue = person.StatusUser.ToString()
          };
          personHistory = servicePersonHistory.InsertNewVersion(personHistory).Result;
        }
        if (saveOccupation?._id != person.Occupation?._id)
        {
          personHistory = new PersonHistory()
          {
            Person = person.GetViewList(),
            TypeHistory = person.StatusUser == EnumStatusUser.Disabled ? EnumTypeHistory.Demission : EnumTypeHistory.Change,
            TypeChange = EnumTypeHistoryChange.Occupation,
            Register = DateTime.UtcNow,
            OldKey = saveOccupation?._id,
            OldValue = saveOccupation?.Name,
            NewKey = person.Occupation?._id,
            NewValue = person.Occupation?.Name
          };
          personHistory = servicePersonHistory.InsertNewVersion(personHistory).Result;
        }
        if (saveSalary != person.Salary)
        {
          personHistory = new PersonHistory()
          {
            Person = person.GetViewList(),
            TypeHistory = person.StatusUser == EnumStatusUser.Disabled ? EnumTypeHistory.Demission : EnumTypeHistory.Change,
            TypeChange = EnumTypeHistoryChange.Salary,
            Register = DateTime.UtcNow,
            OldKey = null,
            OldValue = saveSalary.ToString(),
            NewKey = null,
            NewValue = person.Salary.ToString()
          };
          personHistory = servicePersonHistory.InsertNewVersion(personHistory).Result;
        }
        if (saveManager?._id != person.Manager?._id)
        {
          personHistory = new PersonHistory()
          {
            Person = person.GetViewList(),
            TypeHistory = person.StatusUser == EnumStatusUser.Disabled ? EnumTypeHistory.Demission : EnumTypeHistory.Change,
            TypeChange = EnumTypeHistoryChange.Manager,
            Register = DateTime.UtcNow,
            OldKey = saveManager?._id,
            OldValue = saveManager?.Name,
            NewKey = person.Manager?._id,
            NewValue = person.Manager?.Name
          };
          personHistory = servicePersonHistory.InsertNewVersion(personHistory).Result;
        }
        if (saveTypeJourney != person.TypeJourney)
        {
          personHistory = new PersonHistory()
          {
            Person = person.GetViewList(),
            TypeHistory = person.StatusUser == EnumStatusUser.Disabled ? EnumTypeHistory.Demission : EnumTypeHistory.Change,
            TypeChange = EnumTypeHistoryChange.Jorney,
            Register = DateTime.UtcNow,
            OldKey = ((int)saveTypeJourney).ToString(),
            OldValue = saveTypeJourney.ToString(),
            NewKey = ((int)person.TypeJourney).ToString(),
            NewValue = person.TypeJourney.ToString()
          };
          personHistory = servicePersonHistory.InsertNewVersion(personHistory).Result;
        }
        #endregion

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
        Occupation occupation = serviceOccupation.GetAllNewVersion(p => p._id == idoccupation).Result.FirstOrDefault();
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
        var datenow = DateTime.Parse(DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + " 00:00");
        var feelings = serviceFeelingDay.GetAllNewVersion(p => p.Date == datenow).Result;

        total = servicePerson.CountNewVersion(p => p.StatusUser != EnumStatusUser.Disabled && p.Manager._id == idPerson && p.TypeJourney != EnumTypeJourney.OutOfJourney && p._id != idPerson && p.User.Name.ToUpper().Contains(filter.ToUpper())).Result;
        int skip = count * (page - 1);
        return servicePerson.GetAllNewVersion(p => p.StatusUser != EnumStatusUser.Disabled && p.Manager._id == idPerson && p.TypeJourney != EnumTypeJourney.OutOfJourney && p._id != idPerson && p.User.Name.ToUpper().Contains(filter.ToUpper()), count, skip, "User.Name").Result
          .Select(item => new ViewListPersonTeam()
          {
            Name = item.User.Name,
            _idPerson = item._id,
            Occupation = item.Occupation?.Name,
            DataAdm = item.User.DateAdm,
            Photo = item.User.PhotoUrl,
            Feeling = feelings.Where(p => p._idUser == item.User._id).FirstOrDefault()?.Feeling
          }).OrderBy(p => p.Name).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewListPersonTeam> ListTeam_V2(ref long total, List<ViewListIdIndicators> persons, string filter, int count, int page)
    {
      try
      {

        int skip = count * (page - 1);
        total = persons.Count();

        return persons.Where(p => p.Name.ToUpper().Contains(filter.ToUpper()))
          .Select(item => new ViewListPersonTeam()
          {
            Name = item.Name,
            _idPerson = item._id,
            Occupation = item.OccupationName,
            DataAdm = item.DateAdm
          }).OrderBy(p => p.Name).Skip(skip).Take(count).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }


    public List<ViewListPerson> ListPersonsCompany(ref long total, string idcompany, string filter, int count, int page)
    {
      try
      {
        total = servicePerson.CountNewVersion(p => p.TypeJourney != EnumTypeJourney.OutOfJourney && p.Company._id == idcompany & p.User.Name.ToUpper().Contains(filter.ToUpper())).Result;
        int skip = count * (page - 1);
        return servicePerson.GetAllNewVersion(p => p.TypeJourney != EnumTypeJourney.OutOfJourney && p.Company._id == idcompany & p.User.Name.ToUpper().Contains(filter.ToUpper())).Result.OrderBy(p => p.User.Name).Skip(skip).Take(count)
          .Select(p => new ViewListPerson()
          {
            _id = p._id,
            Company = p.Company,
            Establishment = p.Establishment,
            Registration = p.Registration,
            User = p.User
          }).OrderBy(p => p.User.Name).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<ViewListPersonCrud> List(ref long total, int count, int page, string filter, EnumTypeUser type, EnumStatusUserFilter status)
    {
      try
      {
        var offboardings = serviceOffBoarding.GetAllNewVersion(p => p.Status == EnumStatus.Enabled).Result;

        switch (type)
        {
          case EnumTypeUser.Support:
          case EnumTypeUser.Administrator:
            if (status == EnumStatusUserFilter.Enabled)
            {
              total = servicePerson.CountNewVersion(p => p.StatusUser != EnumStatusUser.Disabled & p.User.Name.ToUpper().Contains(filter.ToUpper())).Result;
              return servicePerson.GetAllNewVersion(p => p.StatusUser != EnumStatusUser.Disabled & p.User.Name.ToUpper().Contains(filter.ToUpper()), count, count * (page - 1), "User.Name").Result
              .Select(x => new ViewListPersonCrud()
              {
                _id = x._id,
                Registration = x.Registration,
                User = x.User,
                Company = x.Company,
                Establishment = x.Establishment,
                StatusUser = x.StatusUser,
                TypeJourney = x.TypeJourney,
                TypeUser = x.TypeUser,
                Occupation = x.Occupation?.Name,
                Manager = x.Manager?.Name,
                StatusFormOffBoardingStep1 = offboardings.Where(p => p.Person._id == x._id).FirstOrDefault() == null ? EnumStatusFormOffBoarding.Open : offboardings.Where(p => p.Person._id == x._id).FirstOrDefault().Step1.StatusFormOffBoarding,
                StatusFormOffBoardingStep2 = offboardings.Where(p => p.Person._id == x._id).FirstOrDefault() == null ? EnumStatusFormOffBoarding.Open : offboardings.Where(p => p.Person._id == x._id).FirstOrDefault().Step2.StatusFormOffBoarding,
              }).OrderBy(p => p.User.Name).ToList();
            }

            else
            {
              if (status == EnumStatusUserFilter.Enabled)
              {
                total = servicePerson.CountNewVersion(p => p.StatusUser != EnumStatusUser.Disabled & p.User.Name.ToUpper().Contains(filter.ToUpper())).Result;
                return servicePerson.GetAllNewVersion(p => p.StatusUser != EnumStatusUser.Disabled & p.User.Name.ToUpper().Contains(filter.ToUpper()), count, count * (page - 1), "User.Name").Result
                .Select(x => new ViewListPersonCrud()
                {
                  _id = x._id,
                  Registration = x.Registration,
                  User = x.User,
                  Company = x.Company,
                  Establishment = x.Establishment,
                  StatusUser = x.StatusUser,
                  TypeJourney = x.TypeJourney,
                  TypeUser = x.TypeUser,
                  Occupation = x.Occupation?.Name,
                  Manager = x.Manager?.Name,
                  StatusFormOffBoardingStep1 = offboardings.Where(p => p.Person._id == x._id).FirstOrDefault() == null ? EnumStatusFormOffBoarding.Open : offboardings.Where(p => p.Person._id == x._id).FirstOrDefault().Step1.StatusFormOffBoarding,
                  StatusFormOffBoardingStep2 = offboardings.Where(p => p.Person._id == x._id).FirstOrDefault() == null ? EnumStatusFormOffBoarding.Open : offboardings.Where(p => p.Person._id == x._id).FirstOrDefault().Step2.StatusFormOffBoarding,
                }).OrderBy(p => p.User.Name).ToList();
              }
              else
              {
                total = servicePerson.CountNewVersion(p => p.User.Name.ToUpper().Contains(filter.ToUpper())).Result;
                return servicePerson.GetAllNewVersion(p => p.User.Name.ToUpper().Contains(filter.ToUpper()), count, count * (page - 1), "User.Name").Result
                .Select(x => new ViewListPersonCrud()
                {
                  _id = x._id,
                  Registration = x.Registration,
                  User = x.User,
                  Company = x.Company,
                  Establishment = x.Establishment,
                  StatusUser = x.StatusUser,
                  TypeJourney = x.TypeJourney,
                  TypeUser = x.TypeUser,
                  Occupation = x.Occupation?.Name,
                  Manager = x.Manager?.Name,
                  StatusFormOffBoardingStep1 = offboardings.Where(p => p.Person._id == x._id).FirstOrDefault() == null ? EnumStatusFormOffBoarding.Open : offboardings.Where(p => p.Person._id == x._id).FirstOrDefault().Step1.StatusFormOffBoarding,
                  StatusFormOffBoardingStep2 = offboardings.Where(p => p.Person._id == x._id).FirstOrDefault() == null ? EnumStatusFormOffBoarding.Open : offboardings.Where(p => p.Person._id == x._id).FirstOrDefault().Step2.StatusFormOffBoarding,
                }).OrderBy(p => p.User.Name).ToList();
              }
            }
          case EnumTypeUser.HR:
          case EnumTypeUser.ManagerHR:
            if (status == EnumStatusUserFilter.Enabled)
            {
              total = servicePerson.CountNewVersion(p => p.User.Name.ToUpper().Contains(filter.ToUpper()) && p.StatusUser != EnumStatusUser.Disabled && p.TypeUser != EnumTypeUser.Administrator && p.TypeUser != EnumTypeUser.Support).Result;
              return servicePerson.GetAllNewVersion(p => p.User.Name.ToUpper().Contains(filter.ToUpper()) && p.StatusUser != EnumStatusUser.Disabled && p.TypeUser != EnumTypeUser.Administrator && p.TypeUser != EnumTypeUser.Support, count, count * (page - 1), "User.Name").Result
              .Select(x => new ViewListPersonCrud()
              {
                _id = x._id,
                Registration = x.Registration,
                User = x.User,
                Company = x.Company,
                Establishment = x.Establishment,
                StatusUser = x.StatusUser,
                TypeJourney = x.TypeJourney,
                TypeUser = x.TypeUser,
                Occupation = x.Occupation?.Name,
                Manager = x.Manager?.Name,
                StatusFormOffBoardingStep1 = offboardings.Where(p => p.Person._id == x._id).FirstOrDefault() == null ? EnumStatusFormOffBoarding.Open : offboardings.Where(p => p.Person._id == x._id).FirstOrDefault().Step1.StatusFormOffBoarding,
                StatusFormOffBoardingStep2 = offboardings.Where(p => p.Person._id == x._id).FirstOrDefault() == null ? EnumStatusFormOffBoarding.Open : offboardings.Where(p => p.Person._id == x._id).FirstOrDefault().Step2.StatusFormOffBoarding,
              }).ToList();
            }
            else
            {
              total = servicePerson.CountNewVersion(p => p.User.Name.ToUpper().Contains(filter.ToUpper()) && p.TypeUser != EnumTypeUser.Administrator && p.TypeUser != EnumTypeUser.Support).Result;
              return servicePerson.GetAllNewVersion(p => p.User.Name.ToUpper().Contains(filter.ToUpper()) && p.TypeUser != EnumTypeUser.Administrator && p.TypeUser != EnumTypeUser.Support, count, count * (page - 1), "User.Name").Result
              .Select(x => new ViewListPersonCrud()
              {
                _id = x._id,
                Registration = x.Registration,
                User = x.User,
                Company = x.Company,
                Establishment = x.Establishment,
                StatusUser = x.StatusUser,
                TypeJourney = x.TypeJourney,
                TypeUser = x.TypeUser,
                Occupation = x.Occupation?.Name,
                Manager = x.Manager?.Name,
                StatusFormOffBoardingStep1 = offboardings.Where(p => p.Person._id == x._id).FirstOrDefault() == null ? EnumStatusFormOffBoarding.Open : offboardings.Where(p => p.Person._id == x._id).FirstOrDefault().Step1.StatusFormOffBoarding,
                StatusFormOffBoardingStep2 = offboardings.Where(p => p.Person._id == x._id).FirstOrDefault() == null ? EnumStatusFormOffBoarding.Open : offboardings.Where(p => p.Person._id == x._id).FirstOrDefault().Step2.StatusFormOffBoarding,
              }).ToList();
            }
              
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

    public BaseFields UpdateManager(Person person, string _idManager, string _idManagerOld)
    {
      try
      {
        //var person = servicePerson.GetNewVersion(p => p._id == _idPerson).Result;

        var manager = servicePerson.GetAllNewVersion(p => p._id == _idManager).Result.
          Select(p => new BaseFields()
          {
            _id = p._id,
            Name = p.User.Name,
            Mail = p.User.Mail
          }).FirstOrDefault();

        if (_idManager != _idManagerOld)
          Task.Run(() => SendQueue(_idManager, person._id, person.User?.Name));

        person.Manager = manager;
        var result = servicePerson.Update(person, null);

        hubConnection = new HubConnectionBuilder()
            .WithUrl(pathSignalr + "messagesHub")
            .Build();

        hubConnection.StartAsync();
        hubConnection.InvokeAsync("GetFilterPersons", person.Manager?._id, person._idAccount);

        return manager;
      }
      catch (Exception e)
      {
        throw e;
      }
    }


    public string Delete(string idperson)
    {
      try
      {
        var person = servicePerson.GetNewVersion(p => p._id == idperson).Result;
        person.Status = EnumStatus.Disabled;
        var i = servicePerson.Update(person, null);
        return "deleted";
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
        var view = new ViewCrudPerson()
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
          Occupation = person.Occupation,
          Registration = person.Registration,
          Salary = person.Salary,
          StatusUser = person.StatusUser,
          TypeJourney = person.TypeJourney,
          TypeUser = person.TypeUser,
          Workload = person.Workload,
          SalaryScales = person.SalaryScales == null ? null : new Views.BusinessNew.ViewSalaryScalePerson()
          {
            _idSalaryScale = person.SalaryScales._idSalaryScale,
            NameSalaryScale = person.SalaryScales.NameSalaryScale
          },
          User = person.User
        };
        if (view.User != null)
          view.User.PhotoUrl = serviceUser.GetNewVersion(p => p._id == view.User._id).Result.PhotoUrl;

        return view;
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
        //var exists = serviceUser.CountFreeNewVersion(p => p.Mail == view.User.Mail).Result;
        //if ((exists > 0) && (view.User._id == null))
        //  throw new Exception("existsmailornickname");
        if (view.User._id == null)
        {
          var exists = serviceUser.CountFreeNewVersion(p => p.Document == view.User.Document).Result;
          if (exists > 0)
            throw new Exception("existsdocument");
        }


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
          Schooling = view.User.Schooling,
          Sex = view.User.Sex,
          ChangePassword = EnumChangePassword.AccessFirst,
          UserAdmin = false,
          ShowSalary = view.User.ShowSalary
        };

        if ((user.Mail != null)&&(user.Mail != string.Empty))
        {
          if (user.Mail.IndexOf("@maristas.org.br") != -1 || user.Mail.IndexOf("@pucrs.br") != -1)
            user.ChangePassword = EnumChangePassword.No;
        }

        if (view.User._id == null)
          user = serviceUser.InsertNewVersion(user).Result;
        else
          user = serviceUser.GetNewVersion(p => p._id == view.User._id).Result;

        BaseFields manager = null;
        if (view.Manager != null)
        {
          manager = servicePerson.GetAllNewVersion(p => p._id == view.Manager._id).Result.
           Select(p => new BaseFields()
           {
             _id = p._id,
             Name = p.User.Name,
             Mail = p.User.Mail
           }).FirstOrDefault();
        }
        SalaryScalePerson salaryScale = null;
        if (view.SalaryScales != null)
          salaryScale = serviceSalaryScale.GetAllNewVersion(p => p._id == view.SalaryScales._idSalaryScale).Result
            .Select(p => new SalaryScalePerson() { _idSalaryScale = p._id, NameSalaryScale = p.Name })
            .FirstOrDefault();

        Person person = new Person()
        {
          Company = view.Company,
          DateLastOccupation = view.DateLastOccupation,
          DateLastReadjust = view.DateLastReadjust,
          DateResignation = view.DateResignation,
          Establishment = view.Establishment,
          HolidayReturn = view.HolidayReturn,
          Manager = manager,
          MotiveAside = view.MotiveAside,
          Occupation = view.Occupation,
          Registration = view.Registration,
          Salary = view.Salary,
          DocumentManager = null,
          StatusUser = view.StatusUser,
          TypeJourney = view.TypeJourney,
          TypeUser = view.TypeUser,
          User = user.GetViewCrud(),
          SalaryScales = salaryScale,
          Workload = view.Workload
        };

        if (person.StatusUser == EnumStatusUser.Disabled)
        {
          person.TypeJourney = EnumTypeJourney.OutOfJourney;
        }
        if (person.TypeUser == EnumTypeUser.Administrator || person.TypeUser == EnumTypeUser.Support || person.TypeUser == EnumTypeUser.Anonymous)
        {
          person.TypeJourney = EnumTypeJourney.OutOfJourney;
        }
        if (person.Occupation == null)
        {
          person.TypeJourney = EnumTypeJourney.OutOfJourney;
        }
        person = servicePerson.InsertNewVersion(person).Result;

        #region Registrar os históricos da pessoa nova
        EnumTypeHistory typeHistory = EnumTypeHistory.OnboardingPlataform;
        if (user.DateAdm != null)
        {
          TimeSpan admission = DateTime.Now - (DateTime)user.DateAdm;
          typeHistory = admission.Days <= 90 ? EnumTypeHistory.Admission : EnumTypeHistory.OnboardingPlataform;
        }
        PersonHistory personHistory = new PersonHistory()
        {
          Person = person.GetViewList(),
          TypeHistory = typeHistory,
          TypeChange = EnumTypeHistoryChange.Establishment,
          Register = DateTime.UtcNow,
          OldKey = null,
          OldValue = null,
          NewKey = person.Establishment?._id,
          NewValue = person.Establishment?.Name
        };
        personHistory = servicePersonHistory.InsertNewVersion(personHistory).Result;
        personHistory = new PersonHistory()
        {
          Person = person.GetViewList(),
          TypeHistory = typeHistory,
          TypeChange = EnumTypeHistoryChange.StatusUser,
          Register = DateTime.UtcNow,
          OldKey = null,
          OldValue = null,
          NewKey = ((int)person.StatusUser).ToString(),
          NewValue = person.StatusUser.ToString()
        };
        personHistory = servicePersonHistory.InsertNewVersion(personHistory).Result;
        personHistory = new PersonHistory()
        {
          Person = person.GetViewList(),
          TypeHistory = typeHistory,
          TypeChange = EnumTypeHistoryChange.Occupation,
          Register = DateTime.UtcNow,
          OldKey = null,
          OldValue = null,
          NewKey = person.Occupation?._id,
          NewValue = person.Occupation?.Name
        };
        personHistory = servicePersonHistory.InsertNewVersion(personHistory).Result;
        personHistory = new PersonHistory()
        {
          Person = person.GetViewList(),
          TypeHistory = typeHistory,
          TypeChange = EnumTypeHistoryChange.Salary,
          Register = DateTime.UtcNow,
          OldKey = null,
          OldValue = null,
          NewKey = null,
          NewValue = person.Salary.ToString()
        };
        personHistory = servicePersonHistory.InsertNewVersion(personHistory).Result;
        personHistory = new PersonHistory()
        {
          Person = person.GetViewList(),
          TypeHistory = typeHistory,
          TypeChange = EnumTypeHistoryChange.Manager,
          Register = DateTime.UtcNow,
          OldKey = null,
          OldValue = null,
          NewKey = person.Manager?._id,
          NewValue = person.Manager?.Name
        };
        personHistory = servicePersonHistory.InsertNewVersion(personHistory).Result;
        personHistory = new PersonHistory()
        {
          Person = person.GetViewList(),
          TypeHistory = typeHistory,
          TypeChange = EnumTypeHistoryChange.Jorney,
          Register = DateTime.UtcNow,
          OldKey = null,
          OldValue = null,
          NewKey = ((int)person.TypeJourney).ToString(),
          NewValue = person.TypeJourney.ToString()
        };
        personHistory = servicePersonHistory.InsertNewVersion(personHistory).Result;
        #endregion

        return person.GetViewCrud();
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
        bool modifyManager = false;
        var exists = serviceUser.CountFreeNewVersion(p => p._id != view.User._id && (p.Document == view.User.Document)).Result;
        if (exists > 0)
          throw new Exception("existsdocument");

        Person person = servicePerson.GetNewVersion(p => p._id == view._id).Result;

        #region Variáveis para salvar dados antigos
        ViewListEstablishment saveEstablishment = person.Establishment;
        EnumStatusUser saveStatusUser = person.StatusUser;
        ViewListOccupationResume saveOccupation = person.Occupation;
        decimal saveSalary = person.Salary;
        BaseFields saveManager = person.Manager;
        EnumTypeJourney saveTypeJourney = person.TypeJourney;
        #endregion

        User user = null;
        if (view.User != null)
        {
          user = serviceUser.GetNewVersion(p => p._id == view.User._id).Result;
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
          user.Schooling = view.User.Schooling;
          user.Sex = view.User.Sex;
          user.ShowSalary = view.User.ShowSalary;
          var xy = serviceUser.Update(user, null);

        }

        BaseFields manager = null;
        if (view.Manager != null)
          modifyManager = true;
        
        SalaryScalePerson salaryScale = null;
        if (view.SalaryScales != null)
          salaryScale = serviceSalaryScale.GetAllNewVersion(p => p._id == view.SalaryScales._idSalaryScale)
            .Result.Select(p => new SalaryScalePerson() { _idSalaryScale = p._id, NameSalaryScale = p.Name })
            .FirstOrDefault();

        person.Workload = view.Workload;
        person.Company = view.Company;
        person.DateLastOccupation = view.DateLastOccupation;
        person.DateLastReadjust = view.DateLastReadjust;
        person.DateResignation = view.DateResignation;
        person.Establishment = view.Establishment;
        person.HolidayReturn = view.HolidayReturn;
        person.Manager = manager;
        person.MotiveAside = view.MotiveAside;
        person.Occupation = view.Occupation;
        person.Registration = view.Registration;
        person.Salary = view.Salary;
        person.DocumentManager = null;
        person.StatusUser = view.StatusUser;
        person.TypeJourney = view.TypeJourney;
        person.TypeUser = view.TypeUser;
        person.User = user.GetViewCrud();
        person.SalaryScales = salaryScale;

        #region Ajustes na manutenção da pessoa
        if (person.StatusUser == EnumStatusUser.Disabled)
        {
          person.TypeJourney = EnumTypeJourney.OutOfJourney;
          var persons = servicePerson.CountNewVersion(p => p.User._id == person.User._id &&
          p.StatusUser != EnumStatusUser.Disabled).Result;
          if (persons == 0)
          {
            user.Status = EnumStatus.Disabled;
            var i = serviceUser.Update(user, null);
          }
        }
        if (person.TypeUser == EnumTypeUser.Administrator || person.TypeUser == EnumTypeUser.Support || person.TypeUser == EnumTypeUser.Anonymous)
        {
          person.TypeJourney = EnumTypeJourney.OutOfJourney;
        }
        if (person.Occupation == null)
        {
          person.TypeJourney = EnumTypeJourney.OutOfJourney;
        }
        if (saveOccupation?._id != view.Occupation?._id && (view.DateLastOccupation == null || view.DateLastOccupation < DateTime.UtcNow.AddDays(-15)))
        {
          person.DateLastOccupation = DateTime.UtcNow;
        }
        if (saveSalary != view.Salary && (view.DateLastReadjust == null || view.DateLastReadjust < DateTime.UtcNow.AddDays(-15)))
        {
          person.DateLastReadjust = DateTime.UtcNow;
        }
        #endregion

        var x = servicePerson.Update(person, null);

        if (modifyManager)
          manager = UpdateManager(person, view.Manager._id, person.Manager?._id);

        #region Registrar os históricos da pessoa nova
        PersonHistory personHistory = null;
        if (saveEstablishment?._id != person.Establishment?._id)
        {
          personHistory = new PersonHistory()
          {
            Person = person.GetViewList(),
            TypeHistory = person.StatusUser == EnumStatusUser.Disabled ? EnumTypeHistory.Demission : EnumTypeHistory.Change,
            TypeChange = EnumTypeHistoryChange.Establishment,
            Register = DateTime.UtcNow,
            OldKey = saveEstablishment?._id,
            OldValue = saveEstablishment?.Name,
            NewKey = person.Establishment?._id,
            NewValue = person.Establishment?.Name
          };
          personHistory = servicePersonHistory.InsertNewVersion(personHistory).Result;
        }
        if (saveStatusUser != person.StatusUser)
        {
          personHistory = new PersonHistory()
          {
            Person = person.GetViewList(),
            TypeHistory = person.StatusUser == EnumStatusUser.Disabled ? EnumTypeHistory.Demission : EnumTypeHistory.Change,
            TypeChange = EnumTypeHistoryChange.StatusUser,
            Register = DateTime.UtcNow,
            OldKey = ((int)saveStatusUser).ToString(),
            OldValue = saveStatusUser.ToString(),
            NewKey = ((int)person.StatusUser).ToString(),
            NewValue = person.StatusUser.ToString()
          };
          personHistory = servicePersonHistory.InsertNewVersion(personHistory).Result;
        }
        if (saveOccupation?._id != person.Occupation?._id)
        {
          personHistory = new PersonHistory()
          {
            Person = person.GetViewList(),
            TypeHistory = person.StatusUser == EnumStatusUser.Disabled ? EnumTypeHistory.Demission : EnumTypeHistory.Change,
            TypeChange = EnumTypeHistoryChange.Occupation,
            Register = DateTime.UtcNow,
            OldKey = saveOccupation?._id,
            OldValue = saveOccupation?.Name,
            NewKey = person.Occupation?._id,
            NewValue = person.Occupation?.Name
          };
          personHistory = servicePersonHistory.InsertNewVersion(personHistory).Result;
        }
        if (saveSalary != person.Salary)
        {
          personHistory = new PersonHistory()
          {
            Person = person.GetViewList(),
            TypeHistory = person.StatusUser == EnumStatusUser.Disabled ? EnumTypeHistory.Demission : EnumTypeHistory.Change,
            TypeChange = EnumTypeHistoryChange.Salary,
            Register = DateTime.UtcNow,
            OldKey = null,
            OldValue = saveSalary.ToString(),
            NewKey = null,
            NewValue = person.Salary.ToString()
          };
          personHistory = servicePersonHistory.InsertNewVersion(personHistory).Result;
        }
        if (view.Manager != null && saveManager?._id != person.Manager?._id)
        {
          personHistory = new PersonHistory()
          {
            Person = person.GetViewList(),
            TypeHistory = person.StatusUser == EnumStatusUser.Disabled ? EnumTypeHistory.Demission : EnumTypeHistory.Change,
            TypeChange = EnumTypeHistoryChange.Manager,
            Register = DateTime.UtcNow,
            OldKey = saveManager?._id,
            OldValue = saveManager?.Name,
            NewKey = person.Manager?._id,
            NewValue = person.Manager?.Name
          };
          personHistory = servicePersonHistory.InsertNewVersion(personHistory).Result;
        }
        if (saveTypeJourney != person.TypeJourney)
        {
          personHistory = new PersonHistory()
          {
            Person = person.GetViewList(),
            TypeHistory = person.StatusUser == EnumStatusUser.Disabled ? EnumTypeHistory.Demission : EnumTypeHistory.Change,
            TypeChange = EnumTypeHistoryChange.Jorney,
            Register = DateTime.UtcNow,
            OldKey = ((int)saveTypeJourney).ToString(),
            OldValue = saveTypeJourney.ToString(),
            NewKey = ((int)person.TypeJourney).ToString(),
            NewValue = person.TypeJourney.ToString()
          };
          personHistory = servicePersonHistory.InsertNewVersion(personHistory).Result;
        }
        #endregion

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
        var occupation = serviceOccupation.GetAllNewVersion(p => p._id == idoccupation).Result.FirstOrDefault();
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
        return servicePerson.GetAllNewVersion(p => p.Company._id == idcompany & p.StatusUser != EnumStatusUser.Disabled
       & p.TypeUser != EnumTypeUser.Administrator & p.User.Name.ToUpper().Contains(filter.ToUpper()))
         .Result.Select(item => item.GetViewList()).ToList();
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    #endregion

    #region journey
    public ViewListJourney ListJourney(string idmanager, string filter, int count, int page)
    {
      try
      {
        var list = servicePerson.GetAllNewVersion(p =>
        p.StatusUser != EnumStatusUser.Disabled &&
        p.Manager._id == idmanager && p.Occupation != null && p.User.Name.ToUpper().Contains(filter.ToUpper())).Result;
        int skip = (count * (page - 1));

        List<ViewListOnBoarding> listOnBoarding = new List<ViewListOnBoarding>();
        List<ViewListMonitoring> listMonitoring = new List<ViewListMonitoring>();
        List<ViewListCheckpoint> listCheckpoint = new List<ViewListCheckpoint>();

        foreach (var item in list)
        {
          if ((item.TypeJourney == EnumTypeJourney.OnBoarding) || (item.TypeJourney == EnumTypeJourney.OnBoardingOccupation))
          {
            var onboarding = serviceOnboarding.GetNewVersion(x => x.Person._id == item._id && x.StatusOnBoarding != EnumStatusOnBoarding.End).Result;
            var viewOnboarding = new ViewListOnBoarding()
            {
              Name = item.User?.Name,
              OccupationName = item.Occupation?.Name,
              _idPerson = item._id,
              StatusOnBoarding = EnumStatusOnBoarding.WaitBegin,
              Photo = item.User?.PhotoUrl
            };
            if (onboarding != null)
            {
              viewOnboarding._id = onboarding._id;
              viewOnboarding.StatusOnBoarding = onboarding.StatusOnBoarding;
            }
            listOnBoarding.Add(viewOnboarding);
          }
          else if (item.TypeJourney == EnumTypeJourney.Monitoring)
          {
            var monitoring = serviceMonitoring.GetNewVersion(x => x.Person._id == item._id && x.StatusMonitoring != EnumStatusMonitoring.End).Result;
            var viewMonitoring = new ViewListMonitoring()
            {
              Name = item.User?.Name,
              OccupationName = item.Occupation?.Name,
              idPerson = item._id,
              StatusMonitoring = EnumStatusMonitoring.Open,
              Photo = item.User?.PhotoUrl
            };
            if (monitoring != null)
            {
              viewMonitoring._id = monitoring._id;
              viewMonitoring.StatusMonitoring = monitoring.StatusMonitoring;
            }
            listMonitoring.Add(viewMonitoring);
          }
          else if (item.TypeJourney == EnumTypeJourney.Checkpoint)
          {
            var checkpoint = serviceCheckpoint.GetNewVersion(x => x.Person._id == item._id && x.StatusCheckpoint != EnumStatusCheckpoint.End).Result;
            var viewCheckpoint = new ViewListCheckpoint()
            {
              Name = item.User?.Name,
              OccupationName = item.Occupation?.Name,
              _idPerson = item._id,
              StatusCheckpoint = EnumStatusCheckpoint.Open,
              Photo = item.User?.PhotoUrl
            };
            if (checkpoint != null)
            {
              viewCheckpoint._id = checkpoint._id;
              viewCheckpoint.StatusCheckpoint = checkpoint.StatusCheckpoint;
            }
            listCheckpoint.Add(viewCheckpoint);
          }


        }


        var totalOnboarding = servicePerson.CountNewVersion(p => p.Manager._id == idmanager && p.Occupation != null &&
                                             (p.TypeJourney == EnumTypeJourney.OnBoarding || p.TypeJourney == EnumTypeJourney.OnBoardingOccupation) &&
                                             p.User.Name.ToUpper().Contains(filter.ToUpper())).Result;
        var totalMonitoring = servicePerson.CountNewVersion(p => p.Manager._id == idmanager && p.Occupation != null &&
                                             (p.TypeJourney == EnumTypeJourney.Monitoring) &&
                                             p.User.Name.ToUpper().Contains(filter.ToUpper())).Result;
        var totalCheckpoint = servicePerson.CountNewVersion(p => p.Manager._id == idmanager && p.Occupation != null &&
                                             (p.TypeJourney == EnumTypeJourney.Checkpoint) &&
                                             p.User.Name.ToUpper().Contains(filter.ToUpper())).Result;

        var result = new ViewListJourney();
        result.listOnBoarding = listOnBoarding.OrderBy(p => p.Name).Skip(skip).Take(count).ToList();
        result.listMonitoring = listMonitoring.OrderBy(p => p.Name).Skip(skip).Take(count).ToList();
        result.listCheckpoint = listCheckpoint.OrderBy(p => p.Name).Skip(skip).Take(count).ToList();
        result.totalOnBoarding = totalOnboarding;
        result.totalMonitoring = totalMonitoring;
        result.totalCheckpoint = totalCheckpoint;
        return result;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public ViewListTeam ListTeam_V3(string idmanager, IServiceAutoManager serviceAutoManager, string filter, int count, int page)
    {
      try
      {

        var datenow = DateTime.Parse(DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + " 00:00");
        var feelings = serviceFeelingDay.GetAllNewVersion(p => p.Date == datenow).Result;

        var list = servicePerson.GetAllNewVersion(p => 
        p.StatusUser != EnumStatusUser.Disabled && p.TypeJourney != EnumTypeJourney.OutOfJourney
        && p.Manager._id == idmanager && p.User.Name.ToUpper().Contains(filter.ToUpper())).Result;
        int skip = (count * (page - 1));

        List<ViewListPersonTeam> listTeam = new List<ViewListPersonTeam>();

        foreach (var item in list)
        {
          listTeam.Add(new ViewListPersonTeam()
          {
            Name = item.User?.Name,
            DataAdm = item.User?.DateAdm,
            Occupation = item.Occupation?.Name,
            _idPerson = item._id,
            Photo = item.User.PhotoUrl,
            Feeling = feelings.Where(p => p._idUser == item.User._id).FirstOrDefault()?.Feeling
          });

        }


        var totalTeam = servicePerson.CountNewVersion(p => p.Manager._id == idmanager && p.User.Name.ToUpper().Contains(filter.ToUpper())).Result;
        long totalAutoManager = 0;
        long totalApproved = 0;

        var result = new ViewListTeam();
        result.Team = listTeam.OrderBy(p => p.Name).Skip(skip).Take(count).ToList();
        result.AutoManager = serviceAutoManager.List(idmanager, ref totalAutoManager, count, page, filter);
        result.Approved = serviceAutoManager.ListApproved(idmanager);
        result.totalTeam = totalTeam;
        result.totalAutoManager = totalAutoManager;
        result.totalApproved = totalApproved;
        return result;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    #endregion

    #region Private

    private void SendMessageAsync(dynamic view)
    {
      try
      {
        var message = new Message(Encoding.UTF8.GetBytes(view));
        queueClient.SendAsync(message);
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    private void SendQueue(string idmanager, string idperson, string name)
    {
      try
      {
        var data = new ViewListStructManagerSend
        {
          _idPerson = idperson,
          _idManager = idmanager,
          _idAccount = _user._idAccount,
          Name = name
        };

        SendMessageAsync(JsonConvert.SerializeObject(data));

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
        var parameter = serviceParameter.GetAllNewVersion(p => p.Key == "typeregisterperson").Result.FirstOrDefault();
        if (parameter == null)
          serviceParameter.InsertNewVersion(new Parameter()
          {
            Name = "Tipo do cadastro da pessoa",
            Key = "typeregisterperson",
            Content = "0",
            Help = "Informe 0 para cadastro normal e 1 para cadastro de multicontratos.",
            Status = EnumStatus.Enabled
          }).Wait();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public List<Person> Load()
    {
      return servicePerson.GetAllFreeNewVersion().Result;
    }
    #endregion

  }
}
