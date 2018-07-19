using Manager.Core.Base;
using System.Collections.Generic;

namespace Manager.Core.Business
{
  public class Occupation: BaseEntity
  {
    public string Name { get; set; }
    public OccupationGroup OccupationGroup { get; set; }
    public Area Area { get; set; }
    public Occupation Template { get; set; }
    public long Position { get; set; }
    public List<Behavioral> Behavioral { get; set; }
    public List<Technique> Technique { get; set; }
    public List<Schooling> Schooling { get; set; }
    public List<string> Responsability { get; set; }
    public string Experience { get; set; }

  }
}
