using Manager.Views.Enumns;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudGoal : _ViewCrudBase 
  {
    public string Concept { get; set; }
    public EnumTypeGoals TypeGoals { get; set; }
  }
}