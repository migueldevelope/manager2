using Manager.Core.Business;
using Manager.Core.Enumns;
using System;

namespace Manager.Core.Views
{
  public class ViewPersonsCrud
  {
    public string _id { get; set; }
    public string Name { get; set; }
    public string Mail { get; set; }
    public string Password { get; set; }
    public EnumChangePassword ChangePassword { get; set; }
    public Occupation Occupation { get; set; }
    public Company Company { get; set; }
    public string Phone { get; set; }
    public string Document { get; set; }
    public DateTime? DateBirth { get; set; }
    public DateTime? DateAdm { get; set; }
    public EnumStatusUser StatusUser { get; set; }
    public Person Manager { get; set; }
    public string Registration { get; set; }
    public EnumTypeUser TypeUser { get; set; }
    public EnumTypeJourney TypeJourney { get; set; }
    public Establishment Establishment { get; set; }
    public string PhoneFixed { get; set; }
    public string DocumentID { get; set; }
    public string DocumentCTPF { get; set; }
    public EnumSex Sex { get; set; }
    public DateTime? HolidayReturn { get; set; }
    public string MotiveAside { get; set; }
    public DateTime? DateLastOccupation { get; set; }
    public decimal Salary { get; set; }
    public DateTime? DateLastReadjust { get; set; }
    public DateTime? DateResignation { get; set; }
  }
}
