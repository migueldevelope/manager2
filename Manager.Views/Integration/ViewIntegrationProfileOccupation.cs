using System.Collections.Generic;

namespace Manager.Views.Integration
{
  public class ViewIntegrationProfileOccupation
  {
    public string _id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string NameGroup { get; set; }
    public List<string> Activities { get; set; }
    public List<string> Skills { get; set; }
    public List<string> Schooling { get; set; }
    public List<string> SchoolingComplement { get; set; }
    public string SpecificRequirements { get; set; }
    public List<string> Messages { get; set; }
    public bool Update { get; set; }
    public bool UpdateSkill { get; set; }
    public string Area { get; set; }
    public string Process { get; set; }
    public string SubProcess { get; set; }
  }
}
