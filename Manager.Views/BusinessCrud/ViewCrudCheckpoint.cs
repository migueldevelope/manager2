using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using System;
using System.Collections.Generic;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudCheckpoint : _ViewCrud
  {
    public ViewListPerson Person { get; set; }
    public ViewListOccupation Occupation { get; set; }
    public DateTime? DateBegin { get; set; }
    public DateTime? DateEnd { get; set; }
    public string Comments { get; set; }
    public string TextDefault { get; set; }
    public List<ViewCrudCheckpointQuestion> Questions { get; set; }
    public EnumStatusCheckpoint StatusCheckpoint { get; set; }
    public DateTime? DataAccess { get; set; }
    public EnumCheckpoint TypeCheckpoint { get; set; }
  }
}
