using Manager.Core.Base;
using Manager.Core.BusinessModel;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using System;
using System.Collections.Generic;

namespace Manager.Core.Business
{
  /// <summary>
  /// Objeto persiste no banco de dados - histórico de treinamentos
  /// </summary>
  public class EventHistoric : BaseEntity
  {
    public ViewListPersonBase Person { get; set; }
    public ViewListCourse Course { get; set; }
    public ViewListEvent Event { get; set; }
    public decimal Workload { get; set; }
    public ViewCrudEntity Entity { get; set; }
    public DateTime? Begin { get; set; }
    public DateTime? End { get; set; }
    public string Name { get; set; }
    public List<ViewCrudAttachmentField> Attachments { get; set; }
  }
}
