using Manager.Core.Base;
namespace Manager.Core.Business
{
  public class Establishment : BaseEntity
  {
    public Company Company { get; set; }
    public string Name { get; set; }
  }
}
