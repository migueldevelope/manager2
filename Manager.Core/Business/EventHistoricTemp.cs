using Manager.Core.Base;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using System;
using System.Collections.Generic;

namespace Manager.Core.Business
{
  public class EventHistoricTemp: BaseEntity
  {
    public ViewListPersonBase Person { get; set; }
    public ViewListEvent Event { get; set; }
    public decimal Workload { get; set; }
    public ViewCrudEntity Entity { get; set; }
    public DateTime? Begin { get; set; }
    public DateTime? End { get; set; }
    public string Name { get; set; }
    public List<ViewCrudAttachmentField> Attachments { get; set; }
    public EnumStatusEventHistoricTemp StatusEventHistoricTemp { get; set; }
  }
}
