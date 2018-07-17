using Manager.Core.Base;
using System.Collections.Generic;

namespace Manager.Core.Business
{
  public class Company : BaseEntity
  {
    public string Name { get; set; }
    public string Logo { get; set; }
    public Career Career { get; set; }
    public List<Behavioral> Behavioral { get; set; }
    public List<Technique> Technique { get; set; }
    public List<Schooling> Schooling { get; set; }
    public List<string> Responsability { get; set; }
    public string Experience { get; set; }

  }
}
