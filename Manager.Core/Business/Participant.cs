using Manager.Core.Base;
using Manager.Core.Enumns;
using System.Collections.Generic;

namespace Manager.Core.Business
{
  public class Participant : BaseEntity
  {
    public Person Person { get; set; }
    public List<FrequencyEvent> FrequencyEvent { get; set; }
    public bool Approved { get; set; }
    public bool ApprovedGrade { get; set; }
    public decimal Grade { get; set; }
    public string Name { get; set; }
    public EnumTypeParticipant TypeParticipant { get; set; }

  }
}
