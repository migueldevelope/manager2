using Manager.Core.Base;
using System;

namespace Manager.Core.Business
{
  public class DaysEvent : BaseEntity
  {
    public DateTime Begin { get; set; }
    public DateTime End { get; set; }
  }
}
