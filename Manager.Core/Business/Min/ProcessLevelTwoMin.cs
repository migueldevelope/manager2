namespace Manager.Core.Business.Min
{
  public class ProcessLevelTwoMin : _BaseMin
  {
    public ProcessLevelOneMin ProcessLevelOne { get; set; }
    public string Comments { get; set; }
    public long Order { get; set; }
  }
}
