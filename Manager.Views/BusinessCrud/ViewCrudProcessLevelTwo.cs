using Manager.Views.BusinessList;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudProcessLevelTwo : _ViewCrudBase
  {
    public ViewListProcessLevelOne ProcessLevelOne { get; set; }
    public string Comments { get; set; }
    public long Order { get; set; }
  }
}