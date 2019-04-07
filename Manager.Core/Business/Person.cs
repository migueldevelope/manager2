using Manager.Core.Base;
using Manager.Core.BusinessModel;
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



    //public string Name { get; set; }
    //public long Registration { get; set; }
    //public DateTime? DateBirth { get; set; }
    //public DateTime? DateAdm { get; set; }
    //public Schooling Schooling { get; set; }
    //public string PhotoUrl { get; set; }
    //public long Coins { get; set; }
    //public EnumChangePassword ChangePassword { get; set; }
    //public string ForeignForgotPassword { get; set; }
    //public string PhoneFixed { get; set; }
    //public string DocumentID { get; set; }
    //public string DocumentCTPF { get; set; }
    //public EnumSex Sex { get; set; }
    //public string Document { get; set; }
    //public string Mail { get; set; }
    //public string Phone { get; set; }
    //public string Password { get; set; }

  }
}
