using Manager.Core.Base;
using Manager.Core.BusinessModel;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Manager.Core.Business
{
  /// <summary>
  ///  Objeto persiste no banco de dados
  /// </summary>
  public class Person : BaseEntity
  {
    public EnumStatusUser StatusUser { get; set; }
    [BsonIgnoreIfNull]
    public ViewListCompany Company { get; set; }
    [BsonIgnoreIfNull]
    public ViewListOccupationResume Occupation { get; set; }
    [BsonIgnoreIfNull]
    public BaseFields Manager { get; set; }
    [BsonIgnoreIfNull]
    public string DocumentManager { get; set; }
    [BsonIgnoreIfNull]
    public DateTime? DateLastOccupation { get; set; }
    [BsonIgnoreIfNull]
    public decimal Salary { get; set; }
    [BsonIgnoreIfNull]
    public DateTime? DateLastReadjust { get; set; }
    [BsonIgnoreIfNull]
    public DateTime? DateResignation { get; set; }
    [BsonIgnoreIfNull]
    public SalaryScalePerson SalaryScales { get; set; }
    public EnumTypeJourney TypeJourney { get; set; }
    [BsonIgnoreIfNull]
    public ViewListEstablishment Establishment { get; set; }
    [BsonIgnoreIfNull]
    public DateTime? HolidayReturn { get; set; }
    [BsonIgnoreIfNull]
    public string MotiveAside { get; set; }
    public EnumTypeUser TypeUser { get; set; }
    [BsonIgnoreIfNull]
    public string Registration { get; set; }
    [BsonIgnoreIfNull]
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
        Name = User.Name
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
        Cbo = Occupation.Cbo
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
        _idManager = Manager._id,
        NameManager = Manager.Name
      };
    }
    public ViewListPersonInfo GetViewListPersonInfo()
    {
      return new ViewListPersonInfo()
      {
        _id = _id,
        Name = User.Name,
        TypeJourney = TypeJourney,
        Occupation = Occupation.Name,
        _idManager = Manager._id,
        Manager = Manager.Name
      };
    }
  }
}
