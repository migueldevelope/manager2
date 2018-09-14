using Manager.Core.Base;
using System.Collections.Generic;

namespace Manager.Core.Business
{
  public class Participant : BaseEntity
  {
    public Person Person { get; set; }
    public List<FrequencyEvent> FrequencyEvent { get; set; }
  }
}
