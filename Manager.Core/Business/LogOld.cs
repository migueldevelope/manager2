using Manager.Core.Base;
using System;

namespace Manager.Core.Business
{
  public class LogOld : BaseEntity
  {
    public Person Person { get; set; }
    public string Description { get; set; }
    public DateTime? DataLog { get; set; }
    public string Local { get; set; }
  }
}
