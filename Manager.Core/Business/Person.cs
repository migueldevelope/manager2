using Manager.Core.Base;
using Manager.Core.BusinessModel;
using Manager.Views.BusinessList;
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
        Company = Company.GetViewList(),
        Establishment = Establishment?.GetViewList(),
        Registration = Registration,
        User = User.GetViewList()
      };
    }
    public ViewListPersonResume GetViewListResume()
    {
      return new ViewListPersonResume()
      {
        _id = _id,
        Document = User.Document,
        Name = User.Name,
        Cbo = Occupation == null ? null : Occupation.CBO == null ? null : new ViewListCbo() { _id = Occupation.CBO._id, Name = Occupation.CBO.Name, Code = Occupation.CBO.Code }
      };
    }

    public ViewListPersonPlan GetViewListManager()
    {
      return new ViewListPersonPlan()
      {
        _id = _id,
        Company = Company.GetViewList(),
        Establishment = Establishment?.GetViewList(),
        Registration = Registration,
        User = User.GetViewList(),
        _idManager = Manager._id,
        NameManager = Manager.Name
      };
    }
  }
}
