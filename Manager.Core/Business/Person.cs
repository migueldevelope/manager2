using Manager.Core.Base;
using Manager.Core.BusinessModel;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.BusinessNew;
using Manager.Views.Enumns;
using System;

namespace Manager.Core.Business
{
  /// <summary>
  ///  Objeto persiste no banco de dados
  /// </summary>
  public class Person : BaseEntity
  {
    public EnumStatusUser StatusUser { get; set; }
    public ViewListCompany Company { get; set; }
    public ViewListOccupationResume Occupation { get; set; }
    public BaseFields Manager { get; set; }
    public string DocumentManager { get; set; }
    public DateTime? DateLastOccupation { get; set; }
    public decimal Salary { get; set; }
    public DateTime? DateLastReadjust { get; set; }
    public DateTime? DateResignation { get; set; }
    public SalaryScalePerson SalaryScales { get; set; }
    public EnumTypeJourney TypeJourney { get; set; }
    public ViewListEstablishment Establishment { get; set; }
    public DateTime? HolidayReturn { get; set; }
    public string MotiveAside { get; set; }
    public EnumTypeUser TypeUser { get; set; }
    public string Registration { get; set; }
    public int Workload { get; set; }
    public ViewCrudUser User { get; set; }
    public ViewListPerson GetViewList()
    {
      return new ViewListPerson()
      {
        _id = _id,
        Company = Company,
        Establishment = Establishment,
        Registration = Registration,
        User = User
      };
    }
    public ViewBaseFields GetViewBaseFields()
    {
      return new ViewBaseFields()
      {
        _id = _id,
        Name = User.Name,
        Mail = User.Mail
      };
    }
    public ViewListPersonBase GetViewListBase()
    {
      return new ViewListPersonBase()
      {
        _id = _id,
        Name = User?.Name
      };
    }
    public ViewListPersonBaseManager GetViewListBaseManager()
    {
      var view = new ViewListPersonBaseManager()
      {
        _id = _id,
        Name = User.Name
      };
      if (Manager != null)
      {
        view.NameManager = Manager.Name;
        view._idManager = Manager._id;
      }

      return view;
    }
    public ViewListPersonResume GetViewListResume()
    {
      return new ViewListPersonResume()
      {
        _id = _id,
        Document = User.Document,
        Name = User.Name,
        Cbo = Occupation?.Cbo
      };
    }
    public ViewListPersonPlan GetViewListManager()
    {
      return new ViewListPersonPlan()
      {
        _id = _id,
        Company = Company,
        Establishment = Establishment,
        Registration = Registration,
        User = User,
        _idManager = Manager?._id,
        NameManager = Manager?.Name
      };
    }
    public ViewListPersonInfo GetViewListPersonInfo()
    {
      return new ViewListPersonInfo()
      {
        _id = _id,
        Name = User.Name,
        TypeJourney = TypeJourney,
        OccupationId = Occupation?._id,
        Occupation = Occupation?.Name,
        _idManager = Manager?._id,
        Manager = Manager?.Name,
        DateAdm = User?.DateAdm,
        Schooling = User.Schooling?.Name
      };
    }
    public ViewCrudPerson GetViewCrud()
    {
      return new ViewCrudPerson()
      {
        Company = Company,
        DateLastOccupation = DateLastOccupation,
        DateLastReadjust = DateLastReadjust,
        DateResignation = DateResignation,
        Establishment = Establishment,
        HolidayReturn = HolidayReturn,
        Manager = Manager == null ? null : new ViewBaseFields() { Mail = Manager.Mail, Name = Manager.Name, _id = Manager._id },
        MotiveAside = MotiveAside,
        Occupation = Occupation,
        Registration = Registration,
        Salary = Salary,
        SalaryScales = SalaryScales == null ? null : new ViewSalaryScalePerson() {_idSalaryScale = SalaryScales._idSalaryScale, NameSalaryScale = SalaryScales.NameSalaryScale },
        StatusUser = StatusUser,
        TypeJourney = TypeJourney,
        TypeUser = TypeUser,
        User = User,
        Workload = Workload,
        _id = _id
      };
    }
  }
}
