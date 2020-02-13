using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using System;
using System.Collections.Generic;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudEventHistoricTemp: _ViewCrudBase
  {
    public string _idPerson { get; set; }
    public string NamePerson { get; set; }
    public ViewListEvent Event { get; set; }
    public decimal Workload { get; set; }
    public ViewCrudEntity Entity { get; set; }
    public DateTime? Begin { get; set; }
    public DateTime? End { get; set; }
    public List<ViewCrudAttachmentField> Attachments { get; set; }
    public EnumStatusEventHistoricTemp StatusEventHistoricTemp { get; set; }
    public string Observation { get; set; }
  }
}
