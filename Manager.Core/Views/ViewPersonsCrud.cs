using Manager.Core.Business;
using Manager.Core.Enumns;
using System;

namespace Manager.Core.Views
{
  public class ViewPersonsCrud
  {
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
    public long Registration { get; set; }
    public EnumTypeUser TypeUser { get; set; }
  }
}
