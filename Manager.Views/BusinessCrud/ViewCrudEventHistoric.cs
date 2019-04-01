using Manager.Views.BusinessList;
using System;
using System.Collections.Generic;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudEventHistoric: _ViewCrudBase
  {
    
    public string _idPerson { get; set; }
    public string NamePerson { get; set; }
    public ViewListCourse Course { get; set; }
    public ViewListEvent Event { get; set; }
    public decimal Workload { get; set; }
    public ViewCrudEntity Entity { get; set; }
    public DateTime Begin { get; set; }
    public DateTime End { get; set; }
    public List<ViewCrudAttachmentField> Attachments { get; set; }
  }
}
