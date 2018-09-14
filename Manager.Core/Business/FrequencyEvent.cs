using Manager.Core.Base;

namespace Manager.Core.Business
{
  public class FrequencyEvent : BaseEntity
  {
    public DaysEvent DaysEvent { get; set; }
    public bool Present { get; set; }
  }
}
