using System.Collections.Generic;

namespace IntegrationServer.Views
{
    public class ViewIntegrationOccupation
    {
    public string IdOccupation { get; set; }
    public string Name { get; set; }
    public string NameGroup { get; set; }
    public string IdCompany { get; set; }
    public string NameCompany { get; set; }
    public string IdArea { get; set; }
    public string NameArea { get; set; }
    public string IdProcessLevelOne { get; set; }
    public string NameProcessLevelOne { get; set; }
    public string IdProcessLevelTwo { get; set; }
    public string NameProcessLevelTwo { get; set; }
    public List<string> Skills { get; set; }
    public List<string> Schooling { get; set; }
    public List<string> SchoolingComplement { get; set; }
    public List<string> Activities { get; set; }
    public string SpecificRequirements { get; set; }
  }
}
