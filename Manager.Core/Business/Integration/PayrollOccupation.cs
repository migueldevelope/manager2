using Manager.Core.Base;

namespace Manager.Core.Business.Integration
{
  public class PayrollOccupation : BaseEntity
  {
    public string Key { get; set; }
    public string Name { get; set; }
    public bool Split { get; set; }
  }
}
