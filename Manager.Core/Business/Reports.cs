using Manager.Core.Base;
using Manager.Views.Enumns;
using System;

namespace Manager.Core.Business
{
  public class Reports: BaseEntity
  {
    public string Name { get; set; }
    public DateTime? Date { get; set; }
    public EnumStatusReport StatusReport { get; set; }
    public string Link { get; set; }
  }
}
