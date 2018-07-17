using Manager.Core.Business;
using System;

namespace Manager.Core.Views
{
  public class ViewPersonTeam
  {
    public string IdPerson { get; set; }
    public string Name { get; set; }
    public Occupation Occupation { get; set; }
    public DateTime? DataAdm { get; set; }
  }
}
