using Manager.Views.BusinessList;
using System;
using System.Collections.Generic;

namespace Manager.Views.BusinessCrud
{
  public class ViewMapGroup: _ViewCrudBase
  {
    public ViewListCompany Company { get; set; }
    public ViewListAxis Axis { get; set; }
    public ViewListSphere Sphere { get; set; }
    public long Line { get; set; }
    public List<ViewListSkill> Skills { get; set; }
    public List<ViewListSkill> SkillsCompany { get; set; }
    public List<ViewCrudSchooling> Schooling { get; set; }
    public List<ViewListScope> Scope { get; set; }
  }
}
