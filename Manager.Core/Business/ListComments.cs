using Manager.Core.Base;
using Manager.Core.Enumns;
using System;

namespace Manager.Core.Business
{
  public class ListComments : BaseEntity
  {
    public string Comments { get; set; }
    public DateTime? Date { get; set; }
    public EnumStatusView StatusView { get; set; }
    public EnumUserComment UserComment { get; set; }
  }
}
