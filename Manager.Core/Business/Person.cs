using Manager.Core.Base;
using Manager.Core.BusinessModel;
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
    public Company Company { get; set; }
    public Occupation Occupation { get; set; }
    public BaseFields Manager { get; set; }
    public string DocumentManager { get; set; }
    public DateTime? DateLastOccupation { get; set; }
    public decimal Salary { get; set; }
    public DateTime? DateLastReadjust { get; set; }
    public DateTime? DateResignation { get; set; }
    public SalaryScalePerson SalaryScales { get; set; }
    public EnumTypeJourney TypeJourney { get; set; }
    public Establishment Establishment { get; set; }
    public DateTime? HolidayReturn { get; set; }
    public string MotiveAside { get; set; }
    public EnumTypeUser TypeUser { get; set; }
    public string Registration { get; set; }
    public User User { get; set; }
    public ViewListPerson GetViewList()
    {
      return new ViewListPerson()
      {
        _id = _id,
        Company = new ViewListCompany() { _id = Company._id, Name = Company.Name },
        Establishment = Establishment == null ? null : new ViewListEstablishment() { _id = Establishment._id, Name = Establishment.Name },
        Registration = Registration,
        User = new ViewListUser() { _id = User._id, Name = User.Name, Document = User.Document, Mail = User.Mail, Phone = User.Phone }
      };
    }
    public ViewListPersonPlan GetViewListManager()
    {
      return new ViewListPersonPlan()
      {
        _id = _id,
        Company = new ViewListCompany() { _id = Company._id, Name = Company.Name },
        Establishment = Establishment == null ? null : new ViewListEstablishment() { _id = Establishment._id, Name = Establishment.Name },
        Registration = Registration,
        User = new ViewListUser() { _id = User._id, Name = User.Name, Document = User.Document, Mail = User.Mail, Phone = User.Phone },
        _idManager = Manager._id,
        NameManager = Manager.Name
      };
    }
  }
}
