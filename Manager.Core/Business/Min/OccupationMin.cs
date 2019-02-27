using System.Collections.Generic;

namespace Manager.Core.Business.Min
{
  public class OccupationMin : _BaseMin
  {
    public CompanyMin Company { get; set; }
    public GroupMin Group { get; set; }
    public long Line { get; set; }
    public List<ProcessLevelTwoMin> Process { get; set; }
    public CBO CBO { get; set; }
    public GradeMin Grade { get; set; }
  }
}
