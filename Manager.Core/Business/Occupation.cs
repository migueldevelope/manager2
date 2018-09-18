using Manager.Core.Base;
using System.Collections.Generic;

namespace Manager.Core.Business
{
  public class Occupation : BaseEntity
  {
    public string Name { get; set; }
    public Group Group { get; set; }
    public Area Area { get; set; }
    public long Line { get; set; }
    public List<Skill> Skills { get; set; }
    public List<Schooling> Schooling { get; set; }
    public List<Activitie> Activities { get; set; }
    public Occupation Template { get; set; }
    public ProcessLevelTwo ProcessLevelTwo { get; set; }
    public CBO CBO { get; set; }
    public string SpecificRequirements { get; set; }
  }
}
