using Manager.Views.Enumns;
using System;

namespace Manager.Views.BusinessList
{
  public class ViewListCheckpoint : _ViewListBase
  {
    public string _idPerson { get; set; }
    public string OccupationName { get; set; }
    public EnumStatusCheckpoint StatusCheckpoint { get; set; }
    public EnumCheckpoint TypeCheckpoint { get; set; }
    public string Photo { get; set; }
    public DateTime? Deadline { get; set; }
    public DateTime? DateAdm { get; set; }
  }
}
