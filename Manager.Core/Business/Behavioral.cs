using Manager.Core.Base;

namespace Manager.Core.Business
{
  public class Behavioral : BaseEntity
  {
    public string Name { get; set; }
    public string Concept { get; set; }
    public Behavioral Template { get; set; }
  }
}
