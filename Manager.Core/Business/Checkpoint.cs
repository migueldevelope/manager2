using Manager.Core.Base;
using Manager.Core.Enumns;
using System;
using System.Collections.Generic;

namespace Manager.Core.Business
{
  public class Checkpoint : BaseEntity
  {
    public Person Person { get; set; }
    public DateTime? DateBegin { get; set; }
    public DateTime? DateEnd { get; set; }
    public string Comments { get; set; }
    public List<CheckpointQuestions> Questions { get; set; }
    public List<CheckpointSkill> Skill { get; set; }
    public EnumStatusCheckpoint StatusCheckpoint { get; set; }
    public EnumCheckpoint TypeCheckpoint { get; set; }
  }
}
