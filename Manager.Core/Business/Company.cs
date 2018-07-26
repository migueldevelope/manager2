using Manager.Core.Base;
using System.Collections.Generic;

namespace Manager.Core.Business
{
  public class Company : BaseEntity
  {
    public string Name { get; set; }
    public string Logo { get; set; }
    public List<Skill> Skills { get; set; }

  }
}
