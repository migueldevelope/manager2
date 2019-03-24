using Manager.Core.Base;
using Manager.Core.BusinessModel;
using Manager.Core.Enumns;
using System;
using System.Collections.Generic;

namespace Manager.Core.Business
{
  /// <summary>
  ///  Objeto persiste no banco de dados e também uma coleção do monitoring
  /// </summary>
  public class Plan : BaseEntity
  {
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime? Deadline { get; set; }
    public List<Skill> Skills { get; set; }
    public Person UserInclude { get; set; }
    public DateTime? DateInclude { get; set; }
    public EnumTypePlan TypePlan { get; set; }
    public EnumSourcePlan SourcePlan { get; set; }
    public EnumTypeAction TypeAction { get; set; }
    public EnumStatusPlan StatusPlan { get; set; }
    public string TextEnd { get; set; }
    public string TextEndManager { get; set; }
    public DateTime? DateEnd { get; set; }
    public byte Evaluation { get; set; }
    public string Result { get; set; }
    public EnumStatusPlanApproved StatusPlanApproved { get; set; }
    public List<AttachmentField> Attachments { get; set; }
    public EnumNewAction NewAction { get; set; }
    public List<StructPlan> StructPlans { get; set; }
  }
}
