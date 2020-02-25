using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using System;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudUser : _ViewCrudBase 
  {
    public string Document { get; set; }
    public string Mail { get; set; }
    public string Phone { get; set; }
    public string Password { get; set; }
    public bool ShowSalary { get; set; }
    public DateTime? DateBirth { get; set; }
    public DateTime? DateAdm { get; set; }
    public ViewListSchooling Schooling { get; set; }
    public string PhotoUrl { get; set; }
    public string PhoneFixed { get; set; }
    public string DocumentID { get; set; }
    public string DocumentCTPF { get; set; }
    public EnumSex Sex { get; set; }
    public string Nickname { get; set; }
  }
}
