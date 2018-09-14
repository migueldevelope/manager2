using Manager.Core.Base;
using System;

namespace Manager.Core.Business
{
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
  }
}
