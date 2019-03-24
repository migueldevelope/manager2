using Manager.Core.Base;
using Manager.Core.BusinessModel;
using System;
using System.Collections.Generic;

namespace Manager.Core.Business
{
  /// <summary>
  /// Objeto persiste no banco de dados - histórico de treinamentos
  /// </summary>
  public class EventHistoric : BaseEntity
  {
    public Person Person { get; set; }
    public Course Course { get; set; }
    public Event Event { get; set; }
    public decimal Workload { get; set; }
    public Entity Entity { get; set; }
    public DateTime Begin { get; set; }
    public DateTime End { get; set; }
    public string Name { get; set; }
    public List<AttachmentField> Attachments { get; set; }
  }
}
