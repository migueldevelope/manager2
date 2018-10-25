using Manager.Core.Base;

namespace Manager.Core.Business
{
  public class ProcessLevelTwo : BaseEntity
  {
    public string Name { get; set; }
    public ProcessLevelOne ProcessLevelOne { get; set; }
    public string Comments { get; set; }
    public long Order { get; set; }
  }
}
