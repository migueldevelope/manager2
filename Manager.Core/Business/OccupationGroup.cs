using Manager.Core.Base;
using Manager.Core.Enumns;
using MongoDB.Bson;
using System.Collections.Generic;

namespace Manager.Core.Business
{
  public class OccupationGroup : BaseEntity
  {
    public Career Career { get; set; }
    public string Name { get; set; }
    public Company Company { get; set; }
    public OccupationGroup Template { get; set; }
    public string ActionFocus { get; set; }
    public Sphere Sphere { get; set; }
    public Axis Axis { get; set; }
    public long Position { get; set; }
    public List<Behavioral> Behavioral { get; set; }
    public List<Technique> Technique { get; set; }
    public List<Schooling> Schooling { get; set; }
    public List<string> Responsability { get; set; }
    public string Experience { get; set; }
  }
}
