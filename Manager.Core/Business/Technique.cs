using Manager.Core.Base;

namespace Manager.Core.Business
{
  public class Technique: BaseEntity
  {
    public string Name { get; set; }
    public string Concept { get; set; }
    public Technique Template { get; set; }
  }
}
