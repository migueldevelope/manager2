using Manager.Views.Enumns;
using System;

namespace Manager.Views.BusinessList
{
  public class ViewListPersonTeam
  {
    public string _idPerson { get; set; }
    public string Name { get; set; }
    public string Occupation { get; set; }
    public DateTime? DataAdm { get; set; }
    public string Photo { get; set; }
    public EnumFeeling? Feeling { get; set; }
  }
}
