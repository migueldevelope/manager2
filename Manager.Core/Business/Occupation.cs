﻿using Manager.Core.Base;
using System.Collections.Generic;

namespace Manager.Core.Business
{
  public class Occupation: BaseEntity
  {
    public string Name { get; set; }
    public Group Group { get; set; }
    public Area Area { get; set; }
    public long Line { get; set; }
    public List<Skill> Skills { get; set; }
    public List<Schooling> Schooling { get; set; }
    public List<string> Activities { get; set; }
    public Occupation Template { get; set; }

  }
}
